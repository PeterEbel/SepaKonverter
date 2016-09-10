using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SepaKonverter
{
    public class SepaDirectDebitRawDataVO
    {

        #region RawData Members

        private string m_sOrganisationsNr;
        private string m_sMessageID;
        private string m_sCreationDateTime;
        private string m_sNumberOfTransactions;
        private string m_sControlSum;
        private string m_sInitiatingPartyname;
        private string m_sPaymentInformationID;
        private string m_sPaymentMethod;
        private string m_sBatchBooking;
        private string m_sNumberOfTransactionsPaymentInfo;
        private string m_sControlSumPaymentInfo;
        private string m_sServiceLevelCode;
        private string m_sLocalInstrumentCode;
        private string m_sSequenceType;
        private string m_sCategoryPurpose;
        private string m_sRequestedCollectionDate;
        private string m_sCreditorName;
        private string m_sCreditorPostalAddressCountry;
        private string m_sCreditorPostalAddressAddressLine;
        private string m_sCreditorAccountIBAN;
        private string m_sCreditorAccountCurrency;
        private string m_sCreditorAgentBIC;
        private string m_sChargeBearer;
        private string m_sCreditorIdentification;
        private string m_sInstructionID;
        private string m_sEndToEndID;
        private string m_sMandateID;
        private string m_sDateOfSignature;
        private string m_sAmendmentIndicator;
        private string m_sOriginalMandateID;
        private string m_sOriginalCreditorName;
        private string m_sOriginalCreditorIdentification;
        private string m_sOriginalDebtorAccountIBAN;
        private string m_sOriginalDebtorAgentBIC;
        private string m_sCreditorIdentificationTransactionInfo;
        private string m_sDebtorName;
        private string m_sDebtorAgentBIC;
        private string m_sDebtorAccountIBAN;
        private string m_sInstructedAmount;
        private string m_sDebtorPostalAddressCountry;
        private string m_sDebtorPostalAddressAddressLine;
        private string m_sPurposeCode;
        private string m_sUnstructuredRemittanceInfo;
        private string m_sLastschriftStatus;
        private string m_sDatumImport;
        private string m_sDatumAbrechnung;

        #endregion


        public SepaDirectDebitRawDataVO()
        {

        }
        
        #region Properties

        public string OrganisationsNr
        {
            get
            {
                return m_sOrganisationsNr;
            }
            set
            {
                m_sOrganisationsNr = value;
            }
        }

        public string MessageID
        {
            get
            {
                return m_sMessageID;
            }
            set
            {
                m_sMessageID = value;
            }
        }

        public string CreationDateTime
        {
            get
            {
                return m_sCreationDateTime;
            }
            set
            {
                m_sCreationDateTime = value;
            }
        }

        public string NumberOfTransactions
        {
            get
            {
                return m_sNumberOfTransactions;
            }
            set
            {
                m_sNumberOfTransactions = value;
            }
        }

        public string ControlSum
        {
            get
            {
                return m_sControlSum;
            }
            set
            {
                m_sControlSum = value;
            }
        }

        public string InitiatingPartyname
        {
            get
            {
                return m_sInitiatingPartyname;
            }
            set
            {
                m_sInitiatingPartyname = value;
            }
        }

        public string PaymentInformationID
        {
            get
            {
                return m_sPaymentInformationID;
            }
            set
            {
                m_sPaymentInformationID = value;
            }
        }

        public string PaymentMethod
        {
            get
            {
                return m_sPaymentMethod;
            }
            set
            {
                m_sPaymentMethod = value;
            }
        }

        public string BatchBooking
        {
            get
            {
                return m_sBatchBooking;
            }
            set
            {
                m_sBatchBooking = value;
            }
        }

        public string NumberOfTransactionsPaymentInfo
        {
            get
            {
                return m_sNumberOfTransactionsPaymentInfo;
            }
            set
            {
                m_sNumberOfTransactionsPaymentInfo = value;
            }
        }

        public string ControlSumPaymentInfo
        {
            get
            {
                return m_sControlSumPaymentInfo;
            }
            set
            {
                m_sControlSumPaymentInfo = value;
            }
        }

        public string ServiceLevelCode
        {
            get
            {
                return m_sServiceLevelCode;
            }
            set
            {
                m_sServiceLevelCode = value;
            }
        }

        public string LocalInstrumentCode
        {
            get
            {
                return m_sLocalInstrumentCode;
            }
            set
            {
                m_sLocalInstrumentCode = value;
            }
        }

        public string SequenceType
        {
            get
            {
                return m_sSequenceType;
            }
            set
            {
                m_sSequenceType = value;
            }
        }

        public string CategoryPurpose
        {
            get
            {
                return m_sCategoryPurpose;
            }
            set
            {
                m_sCategoryPurpose = value;
            }
        }

        public string RequestedCollectionDate
        {
            get
            {
                return m_sRequestedCollectionDate;
            }
            set
            {
                m_sRequestedCollectionDate = value;
            }
        }

        public string CreditorName
        {
            get
            {
                return m_sCreditorName;
            }
            set
            {
                m_sCreditorName = value;
            }
        }

        public string CreditorPostalAddressCountry
        {
            get
            {
                return m_sCreditorPostalAddressCountry;
            }
            set
            {
                m_sCreditorPostalAddressCountry = value;
            }
        }

        public string CreditorPostalAddressAddressLine
        {
            get
            {
                return m_sCreditorPostalAddressAddressLine;
            }
            set
            {
                m_sCreditorPostalAddressAddressLine = value;
            }
        }

        public string CreditorAccountIBAN
        {
            get
            {
                return m_sCreditorAccountIBAN;
            }
            set
            {
                m_sCreditorAccountIBAN = value;
            }
        }

        public string CreditorAccountCurrency
        {
            get
            {
                return m_sCreditorAccountCurrency;
            }
            set
            {
                m_sCreditorAccountCurrency = value;
            }
        }

        public string CreditorAgentBIC
        {
            get
            {
                return m_sCreditorAgentBIC;
            }
            set
            {
                m_sCreditorAgentBIC = value;
            }
        }

        public string ChargeBearer
        {
            get
            {
                return m_sChargeBearer;
            }
            set
            {
                m_sChargeBearer = value;
            }
        }

        public string CreditorIdentification
        {
            get
            {
                return m_sCreditorIdentification;
            }
            set
            {
                m_sCreditorIdentification = value;
            }
        }

        public string InstructionID
        {
            get
            {
                return m_sInstructionID;
            }
            set
            {
                m_sInstructionID = value;
            }
        }

        public string EndToEndID
        {
            get
            {
                return m_sEndToEndID;
            }
            set
            {
                m_sEndToEndID = value;
            }
        }

        public string MandateID
        {
            get
            {
                return m_sMandateID;
            }
            set
            {
                m_sMandateID = value;
            }
        }

        public string DateOfSignature
        {
            get
            {
                return m_sDateOfSignature;
            }
            set
            {
                m_sDateOfSignature = value;
            }
        }

        public string AmendmentIndicator
        {
            get
            {
                return m_sAmendmentIndicator;
            }
            set
            {
                m_sAmendmentIndicator = value;
            }
        }

        public string OriginalMandateID
        {
            get
            {
                return m_sOriginalMandateID;
            }
            set
            {
                m_sOriginalMandateID = value;
            }
        }

        public string OriginalCreditorName
        {
            get
            {
                return m_sOriginalCreditorName;
            }
            set
            {
                m_sOriginalCreditorName = value;
            }
        }

        public string OriginalCreditorIdentification
        {
            get
            {
                return m_sOriginalCreditorIdentification;
            }
            set
            {
                m_sOriginalCreditorIdentification = value;
            }
        }

        public string OriginalDebtorAccountIBAN
        {
            get
            {
                return m_sOriginalDebtorAccountIBAN;
            }
            set
            {
                m_sOriginalDebtorAccountIBAN = value;
            }
        }

        public string OriginalDebtorAgentBIC
        {
            get
            {
                return m_sOriginalDebtorAgentBIC;
            }
            set
            {
                m_sOriginalDebtorAgentBIC = value;
            }
        }

        public string CreditorIdentificationTransactionInfo
        {
            get
            {
                return m_sCreditorIdentificationTransactionInfo;
            }
            set
            {
                m_sCreditorIdentificationTransactionInfo = value;
            }
        }

        public string DebtorName
        {
            get
            {
                return m_sDebtorName;
            }
            set
            {
                m_sDebtorName = value;
            }
        }

        public string DebtorAgentBIC
        {
            get
            {
                return m_sDebtorAgentBIC;
            }
            set
            {
                m_sDebtorAgentBIC = value;
            }
        }

        public string DebtorAccountIBAN
        {
            get
            {
                return m_sDebtorAccountIBAN;
            }
            set
            {
                m_sDebtorAccountIBAN = value;
            }
        }

        public string InstructedAmount
        {
            get
            {
                return m_sInstructedAmount;
            }
            set
            {
                m_sInstructedAmount = value;
            }
        }

        public string DebtorPostalAddressCountry
        {
            get
            {
                return m_sDebtorPostalAddressCountry;
            }
            set
            {
                m_sDebtorPostalAddressCountry = value;
            }
        }

        public string DebtorPostalAddressAddressLine
        {
            get
            {
                return m_sDebtorPostalAddressAddressLine;
            }
            set
            {
                m_sDebtorPostalAddressAddressLine = value;
            }
        }

        public string PurposeCode
        {
            get
            {
                return m_sPurposeCode;
            }
            set
            {
                m_sPurposeCode = value;
            }
        }

        public string UnstructuredRemittanceInfo
        {
            get
            {
                return m_sUnstructuredRemittanceInfo;
            }
            set
            {
                m_sUnstructuredRemittanceInfo = value;
            }
        }

        public string LastschriftStatus
        {
            get
            {
                return m_sLastschriftStatus;
            }
            set
            {
                m_sLastschriftStatus = value;
            }
        }

        public string DatumImport
        {
            get
            {
                return m_sDatumImport;
            }
            set
            {
                m_sDatumImport = value;
            }
        }

        public string DatumAbrechnung
        {
            get
            {
                return m_sDatumAbrechnung;
            }
            set
            {
                m_sDatumAbrechnung = value;
            }
        }

        #endregion

    }
}
