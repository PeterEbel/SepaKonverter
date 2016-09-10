using System;
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
/*
        /// <summary>
        /// Invoked, because the user selected another tree view node.
        /// </summary>
        /// <param name="aMessageInfo"></param>
        /// <param name="aPaymentInitiation"></param>
        /// <param name="aPaymentInformation"></param>
        /// <param name="aTransactionInformation"></param>

        internal override void Update(
            SepaMessageInfo aMessageInfo,
            SepaPaymentInitiation aPaymentInitiation,
            SepaPaymentInformation aPaymentInformation,
            SepaTransactionInformation aTransactionInformation)
        {
            Update(aMessageInfo,
                (SepaDirectDebitPaymentInitiation)aPaymentInitiation,
                (SepaDirectDebitPaymentInformation)aPaymentInformation,
                (SepaDirectDebitTransactionInformation)aTransactionInformation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aMessageInfo"></param>
        /// <param name="aPaymentInitiation"></param>
        /// <param name="aPaymentInformation"></param>
        /// <param name="aTransactionInformation"></param>

        internal void Update(
            SepaMessageInfo aMessageInfo,
            SepaDirectDebitPaymentInitiation aPaymentInitiation,
            SepaDirectDebitPaymentInformation aPaymentInformation,
            SepaDirectDebitTransactionInformation aTransactionInformation)
        {
            m_aMessageInfo = aMessageInfo;
            m_aPaymentInitiation = aPaymentInitiation;
            m_aPaymentInformation = aPaymentInformation;
            m_aTransactionInformation = aTransactionInformation;

            // Fill the well known namespaces...
            namespaceLabel.Text = aMessageInfo.XmlNamespace;

                        namespaceComboBox.Items.Clear();
                        switch (aMessageInfo.Version)
                        {
                            case 1:
                                namespaceComboBox.Items.Add(SepaNamespace.ZKA_Pain_008_001_01);
                                namespaceComboBox.Items.Add(SepaNamespace.ZKA_Pain_008_002_01);
                                namespaceComboBox.Items.Add(SepaNamespace.AT_Pain_008_001_01);
                                break;
                            case 2:
                                namespaceComboBox.Items.Add(SepaNamespace.ZKA_Pain_008_002_02);
                                namespaceComboBox.Items.Add(SepaNamespace.ZKA_Pain_008_003_02);
                                namespaceComboBox.Items.Add(SepaNamespace.AT_Pain_008_001_02);
                                break;
                        }

            _FillForm(aMessageInfo);
            _FillForm(aPaymentInitiation);
            _FillForm(aPaymentInformation);
            _FillForm(aTransactionInformation);

            //

            // fill the model from the form to update error information.

            _TryFillData();
        }

        private void _FillForm(SepaMessageInfo aMessageInfo)
        {
            if (aMessageInfo != null)
            {
                namespaceLabel.Text = aMessageInfo.XmlNamespace;
            }
        }

        private void _FillForm(SepaDirectDebitPaymentInitiation aPaymentInitiation)
        {
            if (aPaymentInitiation != null)
            {
                paymentInformationCreditorNameTextBox.Text = aPaymentInitiation.InitiatingParty.Name;
            }
        }

        private void _FillForm(SepaDirectDebitPaymentInformation aPaymentInformation)
        {
            if (aPaymentInformation != null)
            {
                switch (aPaymentInformation.BatchBooking)
                {
                    case SepaTriState.Default:
                        batchBookingComboBox.SelectedIndex = 0;
                        break;
                    case SepaTriState.False:
                        batchBookingComboBox.SelectedIndex = 1;
                        break;
                    case SepaTriState.True:
                        batchBookingComboBox.SelectedIndex = 2;
                        break;
                }

                paymentInformationIdTextBox.Text = aPaymentInformation.PaymentInformationIdentification;
                paymentInformationCreditorNameTextBox.Text = aPaymentInformation.Creditor.Name;
                paymentInformationCreditorIbanTextBox.Text = aPaymentInformation.CreditorAccountIBAN.ToString();
                paymentInformationCreditorBicTextBox.Text = aPaymentInformation.CreditorAgentBIC.ToString();

                requestedCollectionDateTextBox.Text = aPaymentInformation.RequestedCollectionDate.ToString();

                if (aPaymentInformation.LocalInstrumentCode == null)
                {
                    localInstrumentComboBox.SelectedIndex = 0;
                }
                else
                {
                    localInstrumentComboBox.Text = aPaymentInformation.LocalInstrumentCode;
                }

                sequenceTypeComboBox.Text = aPaymentInformation.SequenceType;

                paymentInformationCreditorSchemeIdTextBox.Text = aPaymentInformation.CreditorSchemeIdentification;
            }
        }

        private void _FillForm(SepaDirectDebitTransactionInformation aTransactionInformation)
        {
            if (aTransactionInformation != null)
            {
                instructedAmountTextBox.Text = aTransactionInformation.Amount.ToString("N2");
                remittanceInfoTextBox.Text = aTransactionInformation.RemittanceInformation;
                endToEndReferenceTextBox.Text = aTransactionInformation.EndToEndId;

                if (string.IsNullOrEmpty(aTransactionInformation.PurposeCode))
                {
                    purposeCodeComboBox.SelectedIndex = 0;
                }
                else
                {
                    purposeCodeComboBox.Text = aTransactionInformation.PurposeCode;
                }

                //

                debtorNameTextBox.Text = aTransactionInformation.Debtor.Name;
                debtorIbanTextBox.Text = aTransactionInformation.DebtorAccountIBAN.ToString();
                debtorBicTextBox.Text = aTransactionInformation.DebtorAgentBIC.ToString();

                mandateIdentificationTextBox.Text = aTransactionInformation.MandateIdentification;

                mandateDateOfSignatureDateTimePicker.Value =
                    (aTransactionInformation.MandateDateOfSignature == DateTime.MinValue) ?
                    mandateDateOfSignatureDateTimePicker.MinDate :
                    aTransactionInformation.MandateDateOfSignature;

                // transactionInformationCreditorSchemeIdTextBox.Text = aTransactionInformation.CreditorSchemeIdentification;
            }
        }

        //

        private void _TryFillCreditorInformation()
        {
            _TryFillPaymentInformationCreditorName();
            _TryFillPaymentInformationCreditorAgentBIC();
            _TryFillPaymentInformationCreditorAccountIBAN();
            _TryFillPaymentInformationCreditorSchemeID();
        }

        private void _TryFillData()
        {
            _TryFillMessageInfoXmlNamespace();
            _TryFillPaymentInitiationData();
            _TryFillPaymentInformationData();
            _TryFillTransactionInformationData();
        }

        private bool _TryFillMessageInfoXmlNamespace()
        {
            if (m_aMessageInfo != null)
            {
                try
                {
                    m_aMessageInfo.XmlNamespace = namespaceLabel.Text;

                    RemoveError(namespaceLabel);

                    return true;
                }
                catch (Exception x)
                {
                    SetError(namespaceLabel, x);
                    return false;
                }
            }

            return true;
        }

        private void _TryFillPaymentInitiationData()
        {
            _TryFillPaymentInitiationInitiatingPartyName();
        }

        private bool _TryFillPaymentInitiationInitiatingPartyName()
        {
            if (m_aPaymentInitiation != null)
            {
                try
                {
                    m_aPaymentInitiation.InitiatingParty.Name = paymentInformationCreditorNameTextBox.Text;

                    RemoveError(paymentInformationCreditorNameTextBox);

                    return true;
                }
                catch (Exception x)
                {
                    SetError(paymentInformationCreditorNameTextBox, x);

                    return false;
                }
            }

            return true;
        }

        private void _TryFillPaymentInformationData()
        {
            _TryFillPaymentInformationID();
            _TryFillPaymentInformationBatchBooking();

            _TryFillPaymentInformationCreditorName();
            _TryFillPaymentInformationCreditorAccountIBAN();
            _TryFillPaymentInformationCreditorAgentBIC();

            _TryFillPaymentInformationRequestedCollectionDate();
            _TryFillPaymentInformationLocalInstrument();
            _TryFillPaymentInformationSequenceType();
            _TryFillPaymentInformationCreditorSchemeID();
        }

        private bool _TryFillPaymentInformationID()
        {
            if (m_aPaymentInformation != null)
            {
                try
                {
                    string sID = paymentInformationIdTextBox.Text;
                    if (sID == string.Empty)
                    {
                        sID = null;
                    }

                    m_aPaymentInformation.PaymentInformationIdentification = sID;

                    RemoveError(paymentInformationIdTextBox);

                    return true;
                }
                catch (Exception x)
                {
                    SetError(paymentInformationIdTextBox, x);

                    return false;
                }
            }

            return true;
        }

        private bool _TryFillPaymentInformationBatchBooking()
        {
            if (m_aPaymentInformation != null)
            {
                switch (batchBookingComboBox.SelectedIndex)
                {
                    case 0:
                        m_aPaymentInformation.BatchBooking = SepaTriState.Default;
                        break;
                    case 1:
                        m_aPaymentInformation.BatchBooking = SepaTriState.False;
                        break;
                    case 2:
                        m_aPaymentInformation.BatchBooking = SepaTriState.True;
                        break;
                }
            }

            return true;
        }

        private bool _TryFillPaymentInformationCreditorName()
        {
            if (m_aPaymentInformation != null)
            {
                try
                {
                    m_aPaymentInformation.Creditor.Name = paymentInformationCreditorNameTextBox.Text;

                    RemoveError(paymentInformationCreditorNameTextBox);

                    return true;
                }
                catch (Exception x)
                {
                    SetError(paymentInformationCreditorNameTextBox, x);

                    return false;
                }
            }

            return true;
        }

        private bool _TryFillPaymentInformationCreditorAccountIBAN()
        {
            if (m_aPaymentInformation != null)
            {
                try
                {
                    string sIBAN = paymentInformationCreditorIbanTextBox.Text;
                    if (sIBAN == string.Empty)
                    {
                        sIBAN = null;
                    }

                    //

                    m_aPaymentInformation.CreditorAccountIBAN = new SepaIBAN(SepaIBAN.Capture(sIBAN));

                    //

                    RemoveError(paymentInformationCreditorIbanTextBox);

                    return true;
                }
                catch (Exception x)
                {
                    SetError(paymentInformationCreditorIbanTextBox, x);

                    return false;
                }
            }

            return true;
        }

        private bool _TryFillPaymentInformationCreditorAgentBIC()
        {
            if (m_aPaymentInformation != null)
            {
                try
                {
                    string sBic = paymentInformationCreditorBicTextBox.Text.Trim();
                    if (sBic == string.Empty)
                    {
                        sBic = null;
                    }

                    m_aPaymentInformation.CreditorAgentBIC = new SepaBIC(sBic);

                    RemoveError(paymentInformationCreditorBicTextBox);

                    return true;
                }
                catch (Exception x)
                {
                    SetError(paymentInformationCreditorBicTextBox, x);

                    return false;
                }
            }

            return true;
        }

        private bool _TryFillPaymentInformationRequestedCollectionDate()
        {
            if (m_aPaymentInformation != null)
            {
                try
                {
                    DateTime dateValue;
                    DateTime.TryParse(requestedCollectionDateTextBox.Text, out dateValue);
                    m_aPaymentInformation.RequestedCollectionDate = dateValue;
                    RemoveError(requestedCollectionDateTextBox);
                    return true;
                }
                catch (Exception x)
                {
                    SetError(requestedCollectionDateTextBox, x);

                    return false;
                }
            }

            return true;
        }

        private bool _TryFillPaymentInformationLocalInstrument()
        {
            if (m_aPaymentInformation != null)
            {
                try
                {
                    m_aPaymentInformation.LocalInstrumentCode = (localInstrumentComboBox.SelectedIndex == 0) ? null : localInstrumentComboBox.Text;
                    RemoveError(localInstrumentComboBox);
                    return true;
                }
                catch (Exception x)
                {
                    SetError(localInstrumentComboBox, x);
                    return false;
                }
            }

            return true;
        }

        private bool _TryFillPaymentInformationSequenceType()
        {
            if (m_aPaymentInformation != null)
            {
                try
                {
                    m_aPaymentInformation.SequenceType = sequenceTypeComboBox.Text;
                    RemoveError(sequenceTypeComboBox);
                    return true;
                }
                catch (Exception x)
                {
                    SetError(sequenceTypeComboBox, x);
                    return false;
                }
            }

            return true;
        }

        private bool _TryFillPaymentInformationCreditorSchemeID()
        {
            if (m_aPaymentInformation != null)
            {
                try
                {
                    string sCreditorSchemeID = paymentInformationCreditorSchemeIdTextBox.Text;
                    if (sCreditorSchemeID == string.Empty)
                    {
                        sCreditorSchemeID = null;
                    }

                    m_aPaymentInformation.CreditorSchemeIdentification = sCreditorSchemeID;
                    RemoveError(paymentInformationCreditorSchemeIdTextBox);
                    return true;
                }
                catch (Exception x)
                {
                    SetError(paymentInformationCreditorSchemeIdTextBox, x);
                    return false;
                }
            }

            return true;
        }

        private void _TryFillTransactionInformationData()
        {
            _TryFillTransactionInformationInstructedAmount();
            _TryFillTransactionInformationRemittanceInfo();
            _TryFillTransactionInformationEndToEndReference();
            _TryFillTransactionInformationPurposeCode();

            _TryFillTransactionInformationDebtorName();
            _TryFillTransactionInformationDebtorAccountIBAN();
            _TryFillTransactionInformationDebtorAgentBIC();

            _TryFillTransactionInformationMandateIdentification();
            _TryFillTransactionInformationMandateDateOfSignature();
        }

        private bool _TryFillTransactionInformationInstructedAmount()
        {
            if (m_aTransactionInformation != null)
            {
                try
                {
                    m_aTransactionInformation.Amount = Decimal.Parse(this.instructedAmountTextBox.Text.Trim());
                    RemoveError(instructedAmountTextBox);
                    return true;
                }
                catch (Exception x)
                {
                    SetError(instructedAmountTextBox, x);
                    return false;
                }
            }

            return true;
        }

        private bool _TryFillTransactionInformationRemittanceInfo()
        {
            if (m_aTransactionInformation != null)
            {
                try
                {
                    string sRemittanceInfo = remittanceInfoTextBox.Text;
                    if (sRemittanceInfo == string.Empty)
                    {
                        sRemittanceInfo = null;
                    }

                    m_aTransactionInformation.RemittanceInformation = sRemittanceInfo;
                    RemoveError(remittanceInfoTextBox);
                    return true;
                }
                catch (Exception x)
                {
                    SetError(remittanceInfoTextBox, x);
                    return false;
                }
            }

            return true;
        }

        private bool _TryFillTransactionInformationEndToEndReference()
        {
            if (m_aTransactionInformation != null)
            {
                try
                {
                    m_aTransactionInformation.EndToEndId = endToEndReferenceTextBox.Text;
                    RemoveError(endToEndReferenceTextBox);
                    return true;
                }
                catch (Exception x)
                {
                    SetError(endToEndReferenceTextBox, x);
                    return false;
                }
            }
            return true;
        }

        private bool _TryFillTransactionInformationPurposeCode()
        {
            if (m_aTransactionInformation != null)
            {
                try
                {
                    m_aTransactionInformation.PurposeCode = (purposeCodeComboBox.SelectedIndex == 0) ? null : purposeCodeComboBox.Text;
                    RemoveError(purposeCodeComboBox);
                    return true;
                }
                catch (Exception x)
                {
                    SetError(purposeCodeComboBox, x);
                    return false;
                }
            }
            return true;
        }

        private bool _TryFillTransactionInformationDebtorName()
        {
            if (m_aTransactionInformation != null)
            {
                try
                {
                    m_aTransactionInformation.Debtor.Name = debtorNameTextBox.Text;
                    RemoveError(debtorNameTextBox);
                    return true;
                }
                catch (Exception x)
                {
                    SetError(debtorNameTextBox, x);
                    return false;
                }
            }

            return true;
        }

        private bool _TryFillTransactionInformationDebtorAccountIBAN()
        {
            if (m_aTransactionInformation != null)
            {
                try
                {
                    string sIBAN = debtorIbanTextBox.Text;
                    if (sIBAN == string.Empty)
                    {
                        sIBAN = null;
                    }

                    m_aTransactionInformation.DebtorAccountIBAN = new SepaIBAN(SepaIBAN.Capture(sIBAN));
                    RemoveError(debtorIbanTextBox);
                    return true;
                }
                catch (Exception x)
                {
                    SetError(debtorIbanTextBox, x);
                    return false;
                }
            }

            return true;
        }

        private bool _TryFillTransactionInformationDebtorAgentBIC()
        {
            if (m_aTransactionInformation != null)
            {
                try
                {
                    string sBic = debtorBicTextBox.Text.Trim();
                    if (sBic == string.Empty)
                    {
                        sBic = null;
                    }

                    m_aTransactionInformation.DebtorAgentBIC = new SepaBIC(sBic);
                    RemoveError(debtorBicTextBox);
                    return true;
                }
                catch (Exception x)
                {
                    SetError(debtorBicTextBox, x);
                    return false;
                }
            }

            return true;
        }

        private bool _TryFillTransactionInformationMandateIdentification()
        {
            if (m_aTransactionInformation != null)
            {
                try
                {
                    m_aTransactionInformation.MandateIdentification = mandateIdentificationTextBox.Text;
                    RemoveError(mandateIdentificationTextBox);
                    return true;
                }
                catch (Exception x)
                {
                    SetError(mandateIdentificationTextBox, x);
                    return false;
                }
            }

            return true;
        }

        private bool _TryFillTransactionInformationMandateDateOfSignature()
        {
            if (m_aTransactionInformation != null)
            {
                try
                {
                    DateTime mandateDateOfSignature = (mandateDateOfSignatureDateTimePicker.Value == mandateDateOfSignatureDateTimePicker.MinDate) ? DateTime.MinValue : mandateDateOfSignatureDateTimePicker.Value;
                    m_aTransactionInformation.MandateDateOfSignature = mandateDateOfSignature;
                    RemoveError(mandateDateOfSignatureDateTimePicker);
                    return true;
                }
                catch (Exception x)
                {
                    SetError(mandateDateOfSignatureDateTimePicker, x);
                    return false;
                }
            }

            return true;
        }

        private void initiatingPartyNameTextBox_TextChanged(object sender, EventArgs e)
        {
            _TryFillPaymentInitiationInitiatingPartyName();
        }

        private void paymentInformationIdTextBox_TextChanged(object sender, EventArgs e)
        {
            _TryFillPaymentInformationID();
        }

        private void batchBookingComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _TryFillPaymentInformationBatchBooking();
        }

        private void creditorNameTextBox_TextChanged(object sender, EventArgs e)
        {
            _TryFillPaymentInformationCreditorName();
        }

        private void creditorIbanTextBox_TextChanged(object sender, EventArgs e)
        {
            _TryFillPaymentInformationCreditorAccountIBAN();
        }

        private void creditorBicTextBox_TextChanged(object sender, EventArgs e)
        {
            _TryFillPaymentInformationCreditorAgentBIC();
        }

        private void dateOfExecutionDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            _TryFillPaymentInformationRequestedCollectionDate();
        }

        private void localInstrumentCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            _TryFillPaymentInformationLocalInstrument();
        }

        private void sequenceTypeComboBox_TextChanged(object sender, EventArgs e)
        {
            _TryFillPaymentInformationSequenceType();
        }

        private void paymentInformationCreditorSchemeIdTextBox_TextChanged(object sender, EventArgs e)
        {
            _TryFillPaymentInformationCreditorSchemeID();
        }

        private void instructedAmountTextBox_TextChanged(object sender, EventArgs e)
        {
            _TryFillTransactionInformationInstructedAmount();
        }

        private void remittanceInfoTextBox_TextChanged(object sender, EventArgs e)
        {
            _TryFillTransactionInformationRemittanceInfo();
        }

        private void endToEndReferenceTextBox_TextChanged(object sender, EventArgs e)
        {
            _TryFillTransactionInformationEndToEndReference();
        }

        private void purposeCodeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _TryFillTransactionInformationPurposeCode();
        }

        private void debtorNameTextBox_TextChanged(object sender, EventArgs e)
        {
            _TryFillTransactionInformationDebtorName();
        }

        private void debtorIbanTextBox_TextChanged(object sender, EventArgs e)
        {
            _TryFillTransactionInformationDebtorAccountIBAN();
        }

        private void debtorBicTextBox_TextChanged(object sender, EventArgs e)
        {
            _TryFillTransactionInformationDebtorAgentBIC();
        }

        private void mandateIdentificationTextBox_TextChanged(object sender, EventArgs e)
        {
            _TryFillTransactionInformationMandateIdentification();
        }

        private void mandateDateOfSignatureDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            _TryFillTransactionInformationMandateDateOfSignature();
        }

        SepaDocument NewPaymentInitiation(SepaWellKnownMessageInfos nWellKnownMessageInfo)
        {
            return NewPaymentInitiation(SepaMessageInfo.Create(nWellKnownMessageInfo));
        }

        SepaDocument NewPaymentInitiation(SepaMessageInfo aMessageInfo)
        {
            Debug.Assert(aMessageInfo != null);
            SepaPaymentInitiation aPaymentInitiation = (SepaPaymentInitiation)aMessageInfo.NewMessage();
            Debug.Assert(aPaymentInitiation != null);
            SepaTransactionInformation aTransactionInformation = _AddNewTransactionInformation(_AddNewPaymentInformation(aPaymentInitiation));
            SepaDocument aSepaDoc = new SepaDocument(aMessageInfo, aPaymentInitiation);
            return aSepaDoc;
        }

        SepaPaymentInformation _AddNewPaymentInformation(SepaPaymentInitiation aPaymentInitiation)
        {
            Debug.Assert(aPaymentInitiation != null);
            SepaPaymentInformation aPaymentInformation = aPaymentInitiation.NewPaymentInformation();
            aPaymentInitiation.PaymentInformations.Add(aPaymentInformation);
            return aPaymentInformation;
        }

        SepaTransactionInformation _AddNewTransactionInformation(SepaPaymentInformation aPaymentInformation)
        {
            Debug.Assert(aPaymentInformation != null);
            SepaTransactionInformation aTransactionInformation = aPaymentInformation.NewTransactionInformation();
            aPaymentInformation.TransactionInformations.Add(aTransactionInformation);
            return aTransactionInformation;
        }

        void _WriteDocument(Stream aStream)
        {
            Debug.Assert(aStream != null);
            this.Document.WriteDocument(aStream);
        }

        private void paymentInformationCreditorNameTextBox_TextChanged(object sender, EventArgs e)
        {
            m_aPaymentInitiation.InitiatingParty.Name = paymentInformationCreditorNameTextBox.Text;
        }

        private void paymentInformationCreditorBicTextBox_TextChanged(object sender, EventArgs e)
        {
            m_aPaymentInitiation.InitiatingParty.BIC = new SepaBIC(paymentInformationCreditorBicTextBox.Text);
        }

        private void paymentInformationCreditorIbanTextBox_TextChanged(object sender, EventArgs e)
        {
            m_aPaymentInitiation.InitiatingParty.CreditorSchemeIdentification = paymentInformationCreditorIbanTextBox.Text;
        }
*/
    }
}
