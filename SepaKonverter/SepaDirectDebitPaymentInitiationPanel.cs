using System;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Xml;
using System.IO;

using SepaKonverter;
using SepaLib;

namespace SepaKonverter.SepaPanel
{

    public partial class SepaDirectDebitPaymentInitiationPanel : SepaPaymentInitiationControl
    {

        SepaDocument m_aSepaDocument;

        private int m_iLastIndex = 0;
        private string m_CurrentOrganisation;
        private string m_CurrentDirectDebit;
        private string m_SequenceType;
        private decimal m_CurrentControlSumFromDBTable;

        public SepaDirectDebitPaymentInitiationPanel()
        {
            InitializeComponent();
            initializeOrganisationComboBox();
        }

        private void initializeOrganisationComboBox()
        {
            SqlDataReader myDataReader;
            SqlConnection myConnection;
            SqlCommand mySelectCommand;

            myConnection = new SqlConnection(ConfigurationManager.GetConnectionString());
            mySelectCommand = new SqlCommand("select OrganisationsNr from dbo.Organisationen order by OrganisationsNr", myConnection);

            try
            {
                myConnection.Open();
                myDataReader = mySelectCommand.ExecuteReader();
                while (myDataReader.Read())
                {
                    organisationIDComboBox.Items.Add(myDataReader[0].ToString().TrimEnd());
                    sourceOrganisationComboBox.Items.Add(myDataReader[0].ToString().TrimEnd());
                }
                myDataReader.Close();
                sourceOrganisationComboBox.SelectedIndex = 0;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Fetch Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            finally
            {
                myConnection.Close();
            }
        }

        private void initializeDirectDebitHistoryComboBox()
        {
            SqlDataReader myDataReader;
            SqlConnection myConnection;
            SqlCommand mySelectCommand;

            myConnection = new SqlConnection(ConfigurationManager.GetConnectionString());
            mySelectCommand = new SqlCommand("select distinct DatumAbrechnung from dbo.Lastschriften where OrganisationsNr = " + "'" + m_CurrentOrganisation + "'", myConnection);

            directDebitHistoryComboBox.Items.Clear();

            try
            {
                myConnection.Open();
                myDataReader = mySelectCommand.ExecuteReader();
                while (myDataReader.Read())
                {
                    directDebitHistoryComboBox.Items.Add(myDataReader[0].ToString());
                }
                myDataReader.Close();

                if (directDebitHistoryComboBox.Items.Count != 0)
                {
                    directDebitHistoryComboBox.Enabled = true;
                    directDebitHistoryComboBox.SelectedIndex = 0;
                }
                else
                {
                    directDebitHistoryComboBox.Enabled = false;
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Fetch Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            finally
            {
                myConnection.Close();
            }
        }

        private void organisationIDComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_CurrentOrganisation = organisationIDComboBox.SelectedItem.ToString();
            initializeDirectDebitHistoryComboBox();
            m_CurrentDirectDebit = directDebitHistoryComboBox.SelectedItem.ToString();
        }

        private void directDebitHistoryComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_CurrentDirectDebit = directDebitHistoryComboBox.SelectedItem.ToString();
          //CreateNewSepaDocument();
        }

        private void showDirectDebitDetailsList()
        {

            for (int i = 0; i < 46; i++)
            {
                DirectDebitDetailsDataGridView.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                DirectDebitDetailsDataGridView.Columns[i].Visible = false;
            }

            DirectDebitDetailsDataGridView.Size = new Size(675, 460);

            DirectDebitDetailsDataGridView.Columns[35].Visible = true;
            DirectDebitDetailsDataGridView.Columns[35].HeaderText = "Teilnehmer";
            DirectDebitDetailsDataGridView.Columns[35].Width = 203;

            DirectDebitDetailsDataGridView.Columns[36].Visible = true;
            DirectDebitDetailsDataGridView.Columns[36].HeaderText = "BIC";
            DirectDebitDetailsDataGridView.Columns[36].Width = 150;

            DirectDebitDetailsDataGridView.Columns[37].Visible = true;
            DirectDebitDetailsDataGridView.Columns[37].HeaderText = "IBAN";
            DirectDebitDetailsDataGridView.Columns[37].Width = 200;

            DirectDebitDetailsDataGridView.Columns[38].Visible = true;
            DirectDebitDetailsDataGridView.Columns[38].HeaderText = "Betrag";
            DirectDebitDetailsDataGridView.Columns[38].Width = 100;
            DirectDebitDetailsDataGridView.Columns[38].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void DirectDebitDetailsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SepaPaymentInformationVO m_myPaymentInformation;

            if (e.RowIndex != m_iLastIndex)
            {
                if (e.RowIndex != -1)
                {
                    m_iLastIndex = e.RowIndex;

                    m_myPaymentInformation = null;
                    m_myPaymentInformation = new SepaPaymentInformationVO();
                    m_myPaymentInformation.PaymentInformationIdentification= DirectDebitDetailsDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();

                    paymentInformationIdTextBox.Text = DirectDebitDetailsDataGridView.Rows[e.RowIndex].Cells[6].Value.ToString();
                    localInstrumentComboBox.Text = DirectDebitDetailsDataGridView.Rows[e.RowIndex].Cells[12].Value.ToString();
                    sequenceTypeComboBox.Text = DirectDebitDetailsDataGridView.Rows[e.RowIndex].Cells[13].Value.ToString();
                    requestedCollectionDateTextBox.Text = DirectDebitDetailsDataGridView.Rows[e.RowIndex].Cells[15].Value.ToString();
                    debtorNameTextBox.Text = DirectDebitDetailsDataGridView.Rows[e.RowIndex].Cells[35].Value.ToString();
                    debtorBicTextBox.Text = DirectDebitDetailsDataGridView.Rows[e.RowIndex].Cells[36].Value.ToString();
                    debtorIbanTextBox.Text = DirectDebitDetailsDataGridView.Rows[e.RowIndex].Cells[37].Value.ToString();
                    instructedAmountTextBox.Text = DirectDebitDetailsDataGridView.Rows[e.RowIndex].Cells[38].Value.ToString();

                    m_iLastIndex = DirectDebitDetailsDataGridView.CurrentRow.Index;
                }
            }
        }
        
        SepaDocument Document
        {
            get
            {
                return m_aSepaDocument;
            }
            set
            {
                if (value != m_aSepaDocument)
                {
                    m_aSepaDocument = value;
                }
            }
        }

        void CreateNewSepaDocument(string _sSequenceType)
        {
            SepaDirectDebitGroupHeader m_aGroupHeader;
            SepaMessageInfo m_aSepaMessageInfo;

            OrganisationVO myCurrentOrganisationVO;
            OrganisationBO myCurrentOrganisationBO;

            SepaPaymentInformationVO myPaymentInformationVO;
            SepaDirectDebitRawDataBO mySepaDirectDebitRawDataBO;
            SepaDirectDebitRawDataVO [] mySepaDirectDebitRawDataVO;
            SepaDirectDebitTransactionInformation mySepaDirectDebitTransactionInformation;

            myCurrentOrganisationBO = new OrganisationBO();
            myCurrentOrganisationVO = myCurrentOrganisationBO.getOrganisationById(m_CurrentOrganisation);

            mySepaDirectDebitRawDataBO = new SepaDirectDebitRawDataBO(m_CurrentOrganisation);
            mySepaDirectDebitRawDataVO = mySepaDirectDebitRawDataBO.getDirectDebitRawDataById(_sSequenceType);

          //Creation of the Sepa Document
          //Group Header
            m_aGroupHeader = new SepaDirectDebitGroupHeader();
            m_aGroupHeader.MessageIdentification = mySepaDirectDebitRawDataVO[1].MessageID;
            m_aGroupHeader.CreationDateTime = mySepaDirectDebitRawDataVO[1].CreationDateTime;
            m_aGroupHeader.NumberOfTransactions = int.Parse(mySepaDirectDebitRawDataBO.getNumberOfTransactions(_sSequenceType).ToString());

          //Kontrollsumme im Groupheader als Summe aus der Datenbank ermitteln  
            m_aGroupHeader.ControlSum =  (decimal) GetControlSumFromDBTable(_sSequenceType);
            
            m_aGroupHeader.InitiatingPartyName = mySepaDirectDebitRawDataVO[1].InitiatingPartyname;
            m_aGroupHeader.InitiatingPartyId = mySepaDirectDebitRawDataVO[1].CreditorIdentification;

            m_aSepaDocument = null;
            m_aSepaMessageInfo = SepaMessageInfo.Create(SepaWellKnownMessageInfos.Pain_008_003_02);

          //Payment Information
            myPaymentInformationVO = new SepaPaymentInformationVO();
            myPaymentInformationVO.PaymentInformationIdentification = mySepaDirectDebitRawDataVO[1].PaymentInformationID;
            myPaymentInformationVO.PaymentMethod = "DD";
            myPaymentInformationVO.BatchBooking = "true";
            myPaymentInformationVO.NumberOfTransactions = m_aGroupHeader.NumberOfTransactions;

          //Summe der Einzeltransaktionen ermitteln
            decimal dControlSum = 0;
            for (int i = 0; i < myPaymentInformationVO.NumberOfTransactions; i++)
            {
                dControlSum = dControlSum + Convert.ToDecimal(mySepaDirectDebitRawDataVO[i].InstructedAmount);
            }
          //Stimmt die Summe der Einzeltransaktionen mit der Kontrollsumme aus der Datenbank überein?
            Debug.Assert(m_aGroupHeader.ControlSum == dControlSum);

            myPaymentInformationVO.ControlSum = dControlSum;
            myPaymentInformationVO.ServiceLevelCode = "SEPA";
            myPaymentInformationVO.LocalInstrumentCode = "COR1";
            myPaymentInformationVO.SequenceType = mySepaDirectDebitRawDataVO[1].SequenceType;
            m_SequenceType = myPaymentInformationVO.SequenceType;

            myPaymentInformationVO.RequestedCollectionDate = requestedCollectionDateDateTimePicker.Text;
            myPaymentInformationVO.CreditorName = mySepaDirectDebitRawDataVO[1].CreditorName;
            myPaymentInformationVO.CreditorAccountIBAN = new SepaIBAN(mySepaDirectDebitRawDataVO[1].CreditorAccountIBAN.TrimEnd());
            myPaymentInformationVO.CreditorAccountCurrency = "EUR";
            myPaymentInformationVO.CreditorAgentBIC = new SepaBIC(mySepaDirectDebitRawDataVO[1].CreditorAgentBIC.TrimEnd());
            myPaymentInformationVO.ChargeBearer = "SLEV";
            myPaymentInformationVO.CreditorSchemeIdentification = mySepaDirectDebitRawDataVO[1].CreditorIdentification.ToString();

            #region BuildXmlStructure

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineChars = "\n";

            using (XmlWriter writer = XmlWriter.Create("c:\\tmp\\test.xml", settings))
            {

              //Namespace and Schema location
                writer.WriteStartDocument();
                    writer.WriteStartElement("Document", m_aSepaMessageInfo.XmlNamespace);
                        writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                        writer.WriteAttributeString("schemaLocation", "http://www.w3.org/2001/XMLSchema-instance", m_aSepaMessageInfo.XmlNamespace + " " + m_aSepaMessageInfo.XmlSchemaLocation);
//                      writer.WriteAttributeString("schemaLocation", "http://www.w3.org/2001/XMLSchema-instance", m_aSepaMessageInfo.XmlNamespace + " " + "C:\\Users\\Peter\\Documents\\SEPA\\pain.008.003.02.xsd");
                        writer.WriteStartElement("CstmrDrctDbtInitn");
                          //Groupheader
                            writer.WriteStartElement("GrpHdr");
                                writer.WriteElementString("MsgId", m_aGroupHeader.MessageIdentification);
                                writer.WriteElementString("CreDtTm", m_aGroupHeader.CreationDateTime.ToString());
                                writer.WriteElementString("NbOfTxs", m_aGroupHeader.NumberOfTransactions.ToString());
                                writer.WriteElementString("CtrlSum", GetControlSumFromDBTable(_sSequenceType).ToString().Replace(',','.'));
                                writer.WriteStartElement("InitgPty");
                                    writer.WriteElementString("Nm", m_aGroupHeader.InitiatingPartyName);
                                writer.WriteEndElement();
                            writer.WriteEndElement(); //End Groupheader
                          //PaymentInformation
                            writer.WriteStartElement("PmtInf");
                                writer.WriteElementString("PmtInfId", myPaymentInformationVO.PaymentInformationIdentification);
                                writer.WriteElementString("PmtMtd", myPaymentInformationVO.PaymentMethod);
                                writer.WriteElementString("BtchBookg", myPaymentInformationVO.BatchBooking);
                                writer.WriteElementString("NbOfTxs", myPaymentInformationVO.NumberOfTransactions.ToString());
                                writer.WriteElementString("CtrlSum", myPaymentInformationVO.ControlSum.ToString().Replace(',','.'));
                                writer.WriteStartElement("PmtTpInf");
                                    writer.WriteStartElement("SvcLvl");
                                        writer.WriteElementString("Cd", myPaymentInformationVO.ServiceLevelCode);
                                    writer.WriteEndElement();
                                    writer.WriteStartElement("LclInstrm");
                                        writer.WriteElementString("Cd", myPaymentInformationVO.LocalInstrumentCode);
                                    writer.WriteEndElement();
                                    writer.WriteElementString("SeqTp", myPaymentInformationVO.SequenceType);
                                writer.WriteEndElement();
                                writer.WriteElementString("ReqdColltnDt", myPaymentInformationVO.RequestedCollectionDate.ToString());
                                writer.WriteStartElement("Cdtr");
                                    writer.WriteElementString("Nm", myPaymentInformationVO.CreditorName);
                                writer.WriteEndElement();
                                writer.WriteStartElement("CdtrAcct");
                                    writer.WriteStartElement("Id");
                                        writer.WriteElementString("IBAN", myPaymentInformationVO.CreditorAccountIBAN.ToString());
                                    writer.WriteEndElement();
                                    writer.WriteElementString("Ccy", myPaymentInformationVO.CreditorAccountCurrency);
                                writer.WriteEndElement();
                                writer.WriteStartElement("CdtrAgt");
                                    writer.WriteStartElement("FinInstnId");
                                        writer.WriteElementString("BIC", myPaymentInformationVO.CreditorAgentBIC.ToString());
                                    writer.WriteEndElement();
                                writer.WriteEndElement();
                                writer.WriteElementString("ChrgBr", myPaymentInformationVO.ChargeBearer);
                                writer.WriteStartElement("CdtrSchmeId");
                                    writer.WriteStartElement("Id");
                                        writer.WriteStartElement("PrvtId");
                                            writer.WriteStartElement("Othr");
                                                writer.WriteElementString("Id", myPaymentInformationVO.CreditorSchemeIdentification);
                                                writer.WriteStartElement("SchmeNm");
                                                    writer.WriteElementString("Prtry", "SEPA");
                                                writer.WriteEndElement();
                                            writer.WriteEndElement();
                                        writer.WriteEndElement();
                                    writer.WriteEndElement();
                                writer.WriteEndElement();

                              //DirectDebitTransactionInformation
                                for (int i = 0; i < myPaymentInformationVO.NumberOfTransactions; i++)
                                {
                                    writer.WriteStartElement("DrctDbtTxInf");
                                        writer.WriteStartElement("PmtId");
                                            writer.WriteElementString("InstrId", mySepaDirectDebitRawDataVO[i].InstructionID.ToString());
                                            writer.WriteElementString("EndToEndId", mySepaDirectDebitRawDataVO[i].EndToEndID.ToString());
                                        writer.WriteEndElement();
                                        writer.WriteStartElement("InstdAmt");
                                            writer.WriteAttributeString("Ccy", "EUR");
                                            writer.WriteString(mySepaDirectDebitRawDataVO[i].InstructedAmount.ToString().Replace(',','.'));
                                        writer.WriteEndElement();
                                        writer.WriteStartElement("DrctDbtTx");
                                            writer.WriteStartElement("MndtRltdInf");
                                                writer.WriteElementString("MndtId", mySepaDirectDebitRawDataVO[i].MandateID);
                                                writer.WriteElementString("DtOfSgntr", mySepaDirectDebitRawDataVO[i].DateOfSignature.TrimEnd());
                                            writer.WriteEndElement();
                                        writer.WriteEndElement();
                                        writer.WriteStartElement("DbtrAgt");
                                            writer.WriteStartElement("FinInstnId");
                                                writer.WriteElementString("BIC", mySepaDirectDebitRawDataVO[i].DebtorAgentBIC);
                                            writer.WriteEndElement();
                                        writer.WriteEndElement();
                                        writer.WriteStartElement("Dbtr");
                                            writer.WriteElementString("Nm", mySepaDirectDebitRawDataVO[i].DebtorName);
                                        writer.WriteEndElement();
                                        writer.WriteStartElement("DbtrAcct");
                                            writer.WriteStartElement("Id");
                                                writer.WriteElementString("IBAN", mySepaDirectDebitRawDataVO[i].DebtorAccountIBAN);
                                            writer.WriteEndElement();
                                        writer.WriteEndElement();
                                    writer.WriteEndElement(); //End DirectDebitTransactionInformation
                                }

                            writer.WriteEndElement(); //End PaymentInformation
                        writer.WriteEndElement(); //End CstmrDrctDbtInitn
                    writer.WriteEndElement(); //End Document
                writer.WriteEndDocument(); //End Namespace and Schema location
            }

            #endregion BuildXmlStructure

            for (int i = 0; i < mySepaDirectDebitRawDataVO.Length; i++)
            {
                mySepaDirectDebitTransactionInformation = new SepaDirectDebitTransactionInformation();
                mySepaDirectDebitTransactionInformation.EndToEndId = mySepaDirectDebitRawDataVO[i].EndToEndID;
                mySepaDirectDebitTransactionInformation.Amount = decimal.Parse(mySepaDirectDebitRawDataVO[i].InstructedAmount);
                mySepaDirectDebitTransactionInformation.MandateIdentification = mySepaDirectDebitRawDataVO[i].MandateID;
            }

            m_CurrentControlSumFromDBTable = GetControlSumFromDBTable(_sSequenceType);
            
            DirectDebitDetailsDataGridView.DataSource = mySepaDirectDebitRawDataVO;
            showDirectDebitDetailsList();

          //Dialog-Controls aktualisieren
//          msgIDLabel.Text = m_aSepaDoc.Message.MessageIdentification;
            paymentInformationCreditorNameTextBox.Text = myCurrentOrganisationVO.Name;
            paymentInformationCreditorBicTextBox.Text = myCurrentOrganisationVO.BIC;
            paymentInformationCreditorIbanTextBox.Text = myCurrentOrganisationVO.IBAN;
            paymentInformationCreditorSchemeIdTextBox.Text = myCurrentOrganisationVO.GläubigerID;
            controlSumOutputLabel.Text = m_CurrentControlSumFromDBTable.ToString();

        }

        #region CODE_ARCHIVE

        void CreateNewSepaDocument_SAVE(string _sSequenceType)
        {
            SepaDirectDebitGroupHeader m_aGroupHeader;
            SepaMessageInfo m_aSepaMessageInfo;

            OrganisationVO myCurrentOrganisationVO;
            OrganisationBO myCurrentOrganisationBO;

            SepaPaymentInformationVO myPaymentInformationVO;
            SepaDirectDebitRawDataBO mySepaDirectDebitRawDataBO;
            SepaDirectDebitRawDataVO [] mySepaDirectDebitRawDataVO;
            SepaDirectDebitTransactionInformation mySepaDirectDebitTransactionInformation;

            myCurrentOrganisationBO = new OrganisationBO();
            myCurrentOrganisationVO = myCurrentOrganisationBO.getOrganisationById(m_CurrentOrganisation);

            mySepaDirectDebitRawDataBO = new SepaDirectDebitRawDataBO(m_CurrentOrganisation);
            mySepaDirectDebitRawDataVO = mySepaDirectDebitRawDataBO.getDirectDebitRawDataById(_sSequenceType);

          //Creation of the Sepa Document
          //Group Header
            m_aGroupHeader = new SepaDirectDebitGroupHeader();
            m_aGroupHeader.MessageIdentification = mySepaDirectDebitRawDataVO[1].MessageID;
            m_aGroupHeader.CreationDateTime = mySepaDirectDebitRawDataVO[1].CreationDateTime;
            m_aGroupHeader.NumberOfTransactions = int.Parse(mySepaDirectDebitRawDataBO.getNumberOfTransactions(_sSequenceType).ToString());

          //Kontrollsumme im Groupheader als Summe aus der Datenbank ermitteln  
            m_aGroupHeader.ControlSum =  (decimal) GetControlSumFromDBTable(_sSequenceType);
            
            m_aGroupHeader.InitiatingPartyName = mySepaDirectDebitRawDataVO[1].InitiatingPartyname;
            m_aGroupHeader.InitiatingPartyId = mySepaDirectDebitRawDataVO[1].CreditorIdentification;

            m_aSepaDocument = null;
            m_aSepaMessageInfo = SepaMessageInfo.Create(SepaWellKnownMessageInfos.Pain_008_003_02);

          //Payment Information
          //myPaymentInformationBO = new SepaPaymentInformationBO(m_CurrentOrganisation, m_CurrentDirectDebit);
          //myPaymentInformationVO = myPaymentInformationBO.getPaymentInformationById(m_CurrentOrganisation);
            myPaymentInformationVO = new SepaPaymentInformationVO();
            myPaymentInformationVO.PaymentInformationIdentification = mySepaDirectDebitRawDataVO[1].PaymentInformationID;
            myPaymentInformationVO.PaymentMethod = "DD";
            myPaymentInformationVO.BatchBooking = "true";
            myPaymentInformationVO.NumberOfTransactions = m_aGroupHeader.NumberOfTransactions;

          //Summe der Einzeltransaktionen ermitteln
            decimal dControlSum = 0;
            for (int i = 0; i < myPaymentInformationVO.NumberOfTransactions; i++)
            {
                dControlSum = dControlSum + Convert.ToDecimal(mySepaDirectDebitRawDataVO[i].InstructedAmount);
            }
          //Stimmt die Summe der Einzeltransaktionen mit der Kontrollsumme aus der Datenbank überein?
            Debug.Assert(m_aGroupHeader.ControlSum == dControlSum);

            myPaymentInformationVO.ControlSum = dControlSum;
            myPaymentInformationVO.ServiceLevelCode = "SEPA";
            myPaymentInformationVO.LocalInstrumentCode = "COR1";
            myPaymentInformationVO.SequenceType = "RCUR";

          //myPaymentInformationVO.RequestedCollectionDate = mySepaDirectDebitRawDataVO[1].RequestedCollectionDate;
          //Datenfeld RequestedCollectionDate fehlt im DTA-Input
            myPaymentInformationVO.RequestedCollectionDate = mySepaDirectDebitRawDataVO[1].CreationDateTime; 
            myPaymentInformationVO.CreditorName = mySepaDirectDebitRawDataVO[1].CreditorName;
            myPaymentInformationVO.CreditorAccountIBAN = new SepaIBAN(mySepaDirectDebitRawDataVO[1].CreditorAccountIBAN.TrimEnd());
            myPaymentInformationVO.CreditorAccountCurrency = "EUR";
            myPaymentInformationVO.CreditorAgentBIC = new SepaBIC(mySepaDirectDebitRawDataVO[1].CreditorAgentBIC.TrimEnd());
            myPaymentInformationVO.ChargeBearer = "SLEV";
            myPaymentInformationVO.CreditorSchemeIdentification = mySepaDirectDebitRawDataVO[1].CreditorIdentification.ToString();

//          SchemeName/Priority fehlen noch

            #region BuildXmlStructure

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineChars = "\n";

            using (XmlWriter writer = XmlWriter.Create("c:\\tmp\\test.xml", settings))
            {

              //Namespace and Schema location
                writer.WriteStartDocument();
                    writer.WriteStartElement("Document", m_aSepaMessageInfo.XmlNamespace);
                        writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
//                      writer.WriteAttributeString("schemaLocation", "http://www.w3.org/2001/XMLSchema-instance", m_aSepaMessageInfo.XmlNamespace + " " + m_aSepaMessageInfo.XmlSchemaLocation);
                        writer.WriteAttributeString("schemaLocation", "http://www.w3.org/2001/XMLSchema-instance", m_aSepaMessageInfo.XmlNamespace + " " + "C:\\Users\\Peter\\Documents\\SEPA\\pain.008.003.02.xsd");
                        writer.WriteStartElement("CstmrDrctDbtInitn");
                          //Groupheader
                            writer.WriteStartElement("GrpHdr");
                                writer.WriteElementString("MsgId", m_aGroupHeader.MessageIdentification);
                                writer.WriteElementString("CreDtTm", m_aGroupHeader.CreationDateTime.ToString());
                                writer.WriteElementString("NbOfTxs", m_aGroupHeader.NumberOfTransactions.ToString());
                                writer.WriteElementString("CtrlSum", GetControlSumFromDBTable(_sSequenceType).ToString().Replace(',','.'));
                                writer.WriteStartElement("InitgPty");
                                    writer.WriteElementString("Nm", m_aGroupHeader.InitiatingPartyName);
                                writer.WriteEndElement();
                            writer.WriteEndElement(); //End Groupheader
                          //PaymentInformation
                            writer.WriteStartElement("PmtInf");
                                writer.WriteElementString("PmtInfId", myPaymentInformationVO.PaymentInformationIdentification);
                                writer.WriteElementString("PmtMtd", myPaymentInformationVO.PaymentMethod);
                                writer.WriteElementString("BtchBookg", myPaymentInformationVO.BatchBooking);
                                writer.WriteElementString("NbOfTxs", myPaymentInformationVO.NumberOfTransactions.ToString());
                                writer.WriteElementString("CtrlSum", myPaymentInformationVO.ControlSum.ToString().Replace(',','.'));
                                writer.WriteStartElement("PmtTpInf");
                                    writer.WriteStartElement("SvcLvl");
                                        writer.WriteElementString("Cd", myPaymentInformationVO.ServiceLevelCode);
                                    writer.WriteEndElement();
                                    writer.WriteStartElement("LclInstrm");
                                        writer.WriteElementString("Cd", myPaymentInformationVO.LocalInstrumentCode);
                                    writer.WriteEndElement();
                                    writer.WriteElementString("SeqTp", myPaymentInformationVO.SequenceType);
                                writer.WriteEndElement();
                                writer.WriteElementString("ReqdColltnDt", myPaymentInformationVO.RequestedCollectionDate.ToString());
                                writer.WriteStartElement("Cdtr");
                                    writer.WriteElementString("Nm", myPaymentInformationVO.CreditorName);
                                writer.WriteEndElement();
                                writer.WriteStartElement("CdtrAcct");
                                    writer.WriteStartElement("Id");
                                        writer.WriteElementString("IBAN", myPaymentInformationVO.CreditorAccountIBAN.ToString());
                                    writer.WriteEndElement();
                                    writer.WriteElementString("Ccy", myPaymentInformationVO.CreditorAccountCurrency);
                                writer.WriteEndElement();
                                writer.WriteStartElement("CdtrAgt");
                                    writer.WriteStartElement("FinInstnId");
                                        writer.WriteElementString("BIC", myPaymentInformationVO.CreditorAgentBIC.ToString());
                                    writer.WriteEndElement();
                                writer.WriteEndElement();
                                writer.WriteElementString("ChrgBr", myPaymentInformationVO.ChargeBearer);
                                writer.WriteStartElement("CdtrSchmeId");
                                    writer.WriteStartElement("Id");
                                        writer.WriteStartElement("PrvtId");
                                            writer.WriteStartElement("Othr");
                                                writer.WriteElementString("Id", myPaymentInformationVO.CreditorSchemeIdentification);
                                            writer.WriteEndElement();
                                        writer.WriteEndElement();
                                    writer.WriteEndElement();
                                writer.WriteEndElement();
                            writer.WriteEndElement(); //End PaymentInformation

                          //DirectDebitTransactionInformation
                            writer.WriteStartElement("DrctDbtTxInf");
                            for (int i = 0; i < myPaymentInformationVO.NumberOfTransactions; i++)
                            {
                                writer.WriteStartElement("PmtId");
                                    writer.WriteElementString("InstrId", mySepaDirectDebitRawDataVO[i].InstructionID.ToString());
                                    writer.WriteElementString("EndToEndId", mySepaDirectDebitRawDataVO[i].EndToEndID.ToString());
                                writer.WriteEndElement();
                                writer.WriteStartElement("InstrAmt");
                                    writer.WriteAttributeString("Ccy", "EUR");
                                    writer.WriteString(mySepaDirectDebitRawDataVO[i].InstructedAmount.ToString().Replace(',','.'));
                                writer.WriteEndElement();
                                    writer.WriteElementString("ChrgBr", "SLEV");
                                writer.WriteStartElement("DrctDbtTx");
                                    writer.WriteStartElement("MndtRltdInf");
                                        writer.WriteElementString("MndtId", mySepaDirectDebitRawDataVO[i].MandateID);
                                        writer.WriteElementString("DtOfSgntr", mySepaDirectDebitRawDataVO[i].DateOfSignature);
                                    writer.WriteEndElement();
                                    writer.WriteStartElement("CdtrSchmeId");
                                        writer.WriteStartElement("Id");
                                            writer.WriteStartElement("PrvtId");
                                                writer.WriteStartElement("Othr");
                                                    writer.WriteElementString("Id", myPaymentInformationVO.CreditorSchemeIdentification);
                                                writer.WriteEndElement();
                                            writer.WriteEndElement();
                                        writer.WriteEndElement();
                                    writer.WriteEndElement();
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement(); //End DirectDebitTransactionInformation

                        writer.WriteEndElement(); //End CstmrDrctDbtInitn
                    writer.WriteEndElement(); //End Document
                writer.WriteEndDocument(); //End Namespace and Schema location
            }

            #endregion BuildXmlStructure

            for (int i = 0; i < mySepaDirectDebitRawDataVO.Length; i++)
            {
                mySepaDirectDebitTransactionInformation = new SepaDirectDebitTransactionInformation();
                mySepaDirectDebitTransactionInformation.EndToEndId = mySepaDirectDebitRawDataVO[i].EndToEndID;
                mySepaDirectDebitTransactionInformation.Amount = decimal.Parse(mySepaDirectDebitRawDataVO[i].InstructedAmount);
                mySepaDirectDebitTransactionInformation.MandateIdentification = mySepaDirectDebitRawDataVO[i].MandateID;
            }

            m_CurrentControlSumFromDBTable = GetControlSumFromDBTable(_sSequenceType);
            
            DirectDebitDetailsDataGridView.DataSource = mySepaDirectDebitRawDataVO;
            showDirectDebitDetailsList();

          //Dialog-Controls aktualisieren
//          msgIDLabel.Text = m_aSepaDoc.Message.MessageIdentification;
            paymentInformationCreditorNameTextBox.Text = myCurrentOrganisationVO.Name;
            paymentInformationCreditorBicTextBox.Text = myCurrentOrganisationVO.BIC;
            paymentInformationCreditorIbanTextBox.Text = myCurrentOrganisationVO.IBAN;
            paymentInformationCreditorSchemeIdTextBox.Text = myCurrentOrganisationVO.GläubigerID;
            controlSumOutputLabel.Text = m_CurrentControlSumFromDBTable.ToString();

          //bindDirectDebitGridView(m_CurrentOrganisation, directDebitHistoryComboBox.SelectedItem.ToString());
        }

        #endregion

        
        decimal GetControlSumFromDBTable(string _sSequenceType, string _sDatumAbrechnung = "NULL")
        {
          //Kontrollsumme aus der Tabelle ermitteln
            SqlConnection connection = new SqlConnection(ConfigurationManager.GetConnectionString());
            SqlCommand mySelectCommand;

            connection.Open();
            mySelectCommand = new SqlCommand();
            mySelectCommand.Parameters.Add("@OrganisationsNr", SqlDbType.Char);
            mySelectCommand.Parameters[0].Value = m_CurrentOrganisation;
            mySelectCommand.Parameters.Add("@SequenceType", SqlDbType.Char);
            mySelectCommand.Parameters[1].Value = _sSequenceType;

            if (_sDatumAbrechnung == "NULL")
            {
                mySelectCommand.CommandText = "select SUM(CONVERT(decimal(15,2), InstructedAmount)) from dbo.LASTSCHRIFTEN where OrganisationsNr = @OrganisationsNr and SequenceType = @SequenceType and DatumAbrechnung is NULL group by OrganisationsNr";
            }
            else
            {
                mySelectCommand.Parameters.Add("@DatumAbrechnung", SqlDbType.Char);
                mySelectCommand.Parameters[2].Value = _sDatumAbrechnung;
                mySelectCommand.CommandText = "select SUM(CONVERT(decimal(15,2), InstructedAmount)) from dbo.LASTSCHRIFTEN where OrganisationsNr = @OrganisationsNr and SequenceType = @SequenceType and  DatumAbrechnung = @DatumAbrechnung group by OrganisationsNr";
            }

            mySelectCommand.Connection = connection;
            decimal m_aControlSum = (decimal) mySelectCommand.ExecuteScalar();
            connection.Close();

            return (m_aControlSum);
        }

        public decimal? CustomParse(string incomingValue)
        {
            decimal val;
            if (!decimal.TryParse(incomingValue.Replace(",", "."), NumberStyles.Number, CultureInfo.InvariantCulture, out val))
                return null;
            return val;
        }

        private void connectButton_Click(object sender, EventArgs e)
        {

            SepaDTARawDataVO [] mySepaDTARawDataVO;
            SepaDTARawDataBO mySepaDTARawDataBO;

            mySepaDTARawDataBO = new SepaDTARawDataBO();
            mySepaDTARawDataVO = mySepaDTARawDataBO.Import();
            countDTARecords1OutputLabel.Text = mySepaDTARawDataBO.getNumberOfTransactions().ToString();
            countDTARecords2OutputLabel.Text = mySepaDTARawDataBO.NumberOfTransactions.ToString();
            summe1DTAOutputLabel.Text = mySepaDTARawDataBO.getTotalAmount().ToString();
            summe2DTAOutputLabel.Text = Math.Round(mySepaDTARawDataBO.ControlSum, 2).ToString();
            DTADetailsDatagridView.DataSource = mySepaDTARawDataVO;
            sourceOrganisationComboBox.Enabled = true;

        }

//Todo: Prüfen, ob für einen SequenceType am gleichen Tag bereits ein Lauf stattgefunden hat

        private void copyDTARecordsButton_Click(object sender, EventArgs e)
        {

            int nCount;

            SqlConnection myConnection;
            SqlCommand myCommand;

            this.Cursor = Cursors.WaitCursor;

            myConnection = new SqlConnection(ConfigurationManager.GetConnectionString());
            myCommand = new SqlCommand("sp_DTAConverter", myConnection);
            myCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter IdIn = myCommand.Parameters.Add("@OrganisationsID", SqlDbType.Char, 5);
            SqlParameter SeqTypeIn = myCommand.Parameters.Add("@_SequenceType", SqlDbType.Char, 4);
            IdIn.Direction = ParameterDirection.Input;
            IdIn.Value = m_CurrentOrganisation;


//Todo!!!!

            SeqTypeIn.Direction = ParameterDirection.Input;
          //  SeqTypeIn.Value = "FRST";
          SeqTypeIn.Value = "RCUR";

            try
            {
                myConnection.Open();
                nCount = myCommand.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Fetch Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            finally
            {
                myConnection.Close();
            }

//Todo!!!
          //  CreateNewSepaDocument("FRST");
          CreateNewSepaDocument("RCUR");

            this.Cursor = Cursors.Default;

            MessageBox.Show("Lastschrift-Datei erfolgreich erstellt.");

        }

        private void sourceOrganisationComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_CurrentOrganisation = sourceOrganisationComboBox.SelectedItem.ToString();
            copyDTARecordsButton.Enabled = true;
        }

        private void saveFileButton_Click(object sender, EventArgs e)
        {

        }

    }
}
