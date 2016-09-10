using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SepaKonverter
{
    public class SepaDirectDebitRawDataBO
    {
        private SepaDirectDebitRawDataDAO sepaRawDataDAO;
        private DataTable dataTable;
        private string m_sCurrentOrganisation;

        int i = 0;

        public SepaDirectDebitRawDataBO(string _id)
        {
            m_sCurrentOrganisation = _id;
            sepaRawDataDAO = new SepaDirectDebitRawDataDAO();
        }

        public SepaDirectDebitRawDataVO[] getDirectDebitRawDataById(string _sSequenceType)
        {
            dataTable = new DataTable();
            dataTable = sepaRawDataDAO.searchById(m_sCurrentOrganisation, _sSequenceType);
            SepaDirectDebitRawDataVO [] sepaRawDataVO = new SepaDirectDebitRawDataVO [dataTable.Rows.Count];

            foreach (DataRow dr in dataTable.Rows)
            {
                sepaRawDataVO[i] = new SepaDirectDebitRawDataVO();

                sepaRawDataVO[i].OrganisationsNr = dr[0].ToString();
                sepaRawDataVO[i].MessageID = dr[1].ToString();
                sepaRawDataVO[i].CreationDateTime = dr[2].ToString();
                sepaRawDataVO[i].NumberOfTransactions = dr[3].ToString();
                sepaRawDataVO[i].ControlSum = dr[4].ToString();
                sepaRawDataVO[i].InitiatingPartyname = dr[5].ToString();
                sepaRawDataVO[i].PaymentInformationID = dr[6].ToString();
                sepaRawDataVO[i].PaymentMethod = dr[7].ToString();
                sepaRawDataVO[i].BatchBooking = dr[8].ToString();
                sepaRawDataVO[i].NumberOfTransactionsPaymentInfo = dr[9].ToString();
                sepaRawDataVO[i].ControlSumPaymentInfo = dr[10].ToString();
                sepaRawDataVO[i].ServiceLevelCode = dr[11].ToString();
                sepaRawDataVO[i].LocalInstrumentCode = dr[12].ToString();
                sepaRawDataVO[i].SequenceType = dr[13].ToString();
                sepaRawDataVO[i].CategoryPurpose = dr[14].ToString();
                sepaRawDataVO[i].RequestedCollectionDate = dr[15].ToString();
                sepaRawDataVO[i].CreditorName = dr[16].ToString();
                sepaRawDataVO[i].CreditorPostalAddressCountry = dr[17].ToString();
                sepaRawDataVO[i].CreditorPostalAddressAddressLine = dr[18].ToString();
                sepaRawDataVO[i].CreditorAccountIBAN = dr[19].ToString();
                sepaRawDataVO[i].CreditorAccountCurrency = dr[20].ToString();
                sepaRawDataVO[i].CreditorAgentBIC = dr[21].ToString();
                sepaRawDataVO[i].ChargeBearer = dr[22].ToString();
                sepaRawDataVO[i].CreditorIdentification = dr[23].ToString();
                sepaRawDataVO[i].InstructionID = dr[24].ToString();
                sepaRawDataVO[i].EndToEndID = dr[25].ToString();
                sepaRawDataVO[i].MandateID = dr[26].ToString();
                sepaRawDataVO[i].DateOfSignature = dr[27].ToString();
                sepaRawDataVO[i].AmendmentIndicator = dr[28].ToString();
                sepaRawDataVO[i].OriginalMandateID = dr[29].ToString();
                sepaRawDataVO[i].OriginalCreditorName = dr[30].ToString();
                sepaRawDataVO[i].OriginalCreditorIdentification = dr[31].ToString();
                sepaRawDataVO[i].OriginalDebtorAccountIBAN = dr[32].ToString();
                sepaRawDataVO[i].OriginalDebtorAgentBIC = dr[33].ToString();
                sepaRawDataVO[i].CreditorIdentificationTransactionInfo = dr[34].ToString();
                sepaRawDataVO[i].DebtorName = dr[35].ToString();
                sepaRawDataVO[i].DebtorAgentBIC = dr[36].ToString();
                sepaRawDataVO[i].DebtorAccountIBAN = dr[37].ToString();
                sepaRawDataVO[i].InstructedAmount = dr[38].ToString();
                sepaRawDataVO[i].DebtorPostalAddressCountry = dr[39].ToString();
                sepaRawDataVO[i].DebtorPostalAddressAddressLine = dr[40].ToString();
                sepaRawDataVO[i].PurposeCode = dr[41].ToString();
                sepaRawDataVO[i].UnstructuredRemittanceInfo = dr[42].ToString();
                sepaRawDataVO[i].LastschriftStatus = dr[43].ToString();
                sepaRawDataVO[i].DatumImport = dr[44].ToString();
                sepaRawDataVO[i].DatumAbrechnung = dr[45].ToString();

                i++;
            }

            return sepaRawDataVO;
        }

        public int getNumberOfTransactions(string _sSequenceType)
        {
            return sepaRawDataDAO.getNumberOfTransactions(m_sCurrentOrganisation, _sSequenceType);
        }

    }
}
