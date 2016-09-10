using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using SepaLib;
namespace SepaKonverter
{

    public class SepaPaymentInformationVO
    {
        private string m_sPmtInfId;
		private string m_sPmtMthd;
		private string m_nBtchBookg;
		private int m_nNbOfTxs;
        private decimal m_dCtrlSum;
		private string m_SvcLvlCd;
		private string m_sLclInstrmCd;
		private string m_SeqTp;
		private string m_sCtgyPurp;
		private string m_sReqColltnDt;
		private string m_sCdtrNm;
		private string m_sCdtrPstlAdrCtry;
		private string m_sCdtrPstlAdrAdrLine;
		private SepaIBAN m_sCdtrAcctIdIBAN;
		private string m_sCdtrAcctCcy;
		private SepaBIC m_sCdtrAgtBIC;
		private string m_sUltmtCdrNm;
		private string m_sUltmtCdrId;
		private string m_sUltmtCdtrIdBICOrBEI;
		private string m_sUltmtCdtrIdOthrId;
		private string m_sUltmtCdtrIdOthrIssr;
		private string m_sChrgBr;
		private string m_CdtrSchmeId;
		private string m_CdtrSchmeIdOthrId;
		private string m_CdtrSchmeIdOthrSchmeNm;
		private string m_CdtrSchmeIdOthrSchmePrtry;
		
		public SepaPaymentInformationVO()
        {
            this.m_sPmtInfId = null;
            this.m_sPmtMthd = null;
            this.m_nBtchBookg = null;
            this.m_nNbOfTxs = 0;
            this.m_dCtrlSum = 0;
            this.m_SvcLvlCd = null;
            this.m_sLclInstrmCd = null;
            this.m_SeqTp = null;
            this.m_sCtgyPurp = null;
            this.m_sReqColltnDt = DateTime.MinValue.ToString();
            this.m_sCdtrNm = null;
            this.m_sCdtrPstlAdrCtry = null;
            this.m_sCdtrPstlAdrAdrLine = null;
          //this.m_sCdtrAcctIdIBAN = null;
            this.m_sCdtrAcctCcy = null;
          //this.m_sCdtrAgtBIC  = ;
            this.m_sUltmtCdrNm = null;
            this.m_sUltmtCdrId = null;
            this.m_sUltmtCdtrIdBICOrBEI = null;
            this.m_sUltmtCdtrIdOthrId = null;
            this.m_sUltmtCdtrIdOthrIssr = null;
            this.m_sChrgBr = null;
            this.m_CdtrSchmeId = null;
            this.m_CdtrSchmeIdOthrId = null;
            this.m_CdtrSchmeIdOthrSchmeNm = null;
            this.m_CdtrSchmeIdOthrSchmePrtry = null;
        }
        
        public String PaymentInformationIdentification
        {
            get
            {
                return m_sPmtInfId;
            }
            set
            {
                m_sPmtInfId = value;
            }
        }
        
        public String PaymentMethod
        {
            get
            {
                return m_sPmtMthd;
            }
            set
            {
                m_sPmtMthd = value;
            }
        }
        
        public String BatchBooking
        {
            get
            {
                return m_nBtchBookg;
            }
            set
            {
                m_nBtchBookg = value;
            }
        }
        
        public int NumberOfTransactions
        {
            get
            {
                return m_nNbOfTxs;
            }
            set
            {
                m_nNbOfTxs = value;
            }
        }
        
        public decimal ControlSum
        {
            get
            {
                return m_dCtrlSum;
            }
            set
            {
                m_dCtrlSum = value;
            }
        }
        
        public String ServiceLevelCode
        {
            get
            {
                return m_SvcLvlCd;
            }
            set
            {
                m_SvcLvlCd = value;
            }
        }
        
        public String LocalInstrumentCode
        {
            get
            {
                return m_sLclInstrmCd;
            }
            set
            {
                m_sLclInstrmCd = value;
            }
        }
        
        public String SequenceType
        {
            get
            {
                return m_SeqTp;
            }
            set
            {
                m_SeqTp = value;
            }
        }
        
        public String CategoryPurpose
        {
            get
            {
                return m_sCtgyPurp;
            }
            set
            {
                m_sCtgyPurp = value;
            }
        }
        
        public string RequestedCollectionDate
        {
            get
            {
                return m_sReqColltnDt;
            }
            set
            {
                m_sReqColltnDt = value;
            }
        }
        
        public String CreditorName
        {
            get
            {
                return m_sCdtrNm;
            }
            set
            {
                m_sCdtrNm = value;
            }
        }
        
        public String CreditorPostalAddressCountry
        {
            get
            {
                return m_sCdtrPstlAdrCtry;
            }
            set
            {
                m_sCdtrPstlAdrCtry = value;
            }
        }
        
        public String CreditorPostalAddressAddressLine
        {
            get
            {
                return m_sCdtrPstlAdrAdrLine;
            }
            set
            {
                m_sCdtrPstlAdrAdrLine = value;
            }
        }
        
        public SepaIBAN CreditorAccountIBAN
        {
            get
            {
                return m_sCdtrAcctIdIBAN;
            }
            set
            {
                m_sCdtrAcctIdIBAN = value;
            }
        }
        
        public String CreditorAccountCurrency
        {
            get
            {
                return m_sCdtrAcctCcy;
            }
            set
            {
                m_sCdtrAcctCcy = value;
            }
        }

        public SepaLib.SepaBIC CreditorAgentBIC
        {
            get
            {
                return m_sCdtrAgtBIC;
            }
            set
            {
                m_sCdtrAgtBIC = value;
            }
        }
        
        public String UltimateCreditorName
        {
            get
            {
                return m_sUltmtCdrNm;
            }
            set
            {
                m_sUltmtCdrNm = value;
            }
        }
        
        public String UltimateCreditorIdentificationBICOrBEI
        {
            get
            {
                return m_sUltmtCdtrIdBICOrBEI;
            }
            set
            {
                m_sUltmtCdtrIdBICOrBEI = value;
            }
        }
        
        public String UltimateCreditorIdentificationOtherIdentification
        {
            get
            {
                return m_sUltmtCdtrIdOthrId;
            }
            set
            {
                m_sUltmtCdtrIdOthrId = value;
            }
        }
        
        public String UltimateCreditorIdentificationOtherIssuer
        {
            get
            {
                return m_sUltmtCdtrIdOthrIssr;
            }
            set
            {
                m_sUltmtCdtrIdOthrIssr = value;
            }
        }
        
        public String ChargeBearer
        {
            get
            {
                return m_sChrgBr;
            }
            set
            {
                m_sChrgBr = value;
            }
        }
        
        public String CreditorSchemeIdentification
        {
            get
            {
                return m_CdtrSchmeId;
            }
            set
            {
                m_CdtrSchmeId = value;
            }
        }

        public String CreditorSchemeOtherIdentification
        {
            get
            {
                return m_CdtrSchmeIdOthrId;
            }
            set
            {
                m_CdtrSchmeIdOthrId = value;
            }
        }
        
        public String CreditorSchemeOtherSchemeName
        {
            get
            {
                return m_CdtrSchmeIdOthrSchmeNm;
            }
            set
            {
                m_CdtrSchmeIdOthrSchmeNm = value;
            }
        }
        
        public String CreditorSchemeOtherSchemeProprietary
        {
            get
            {
                return m_CdtrSchmeIdOthrSchmePrtry;
            }
            set
            {
                m_CdtrSchmeIdOthrSchmePrtry = value;
            }
        }
    }
}
