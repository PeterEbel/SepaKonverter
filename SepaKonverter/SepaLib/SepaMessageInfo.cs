
namespace SepaLib
{
    using System;
    using System.Text.RegularExpressions;
    
    public sealed class SepaMessageInfo : ICloneable
    {
        private readonly SepaMessageType m_nMessageType;
        private readonly int m_nVersion;
        private readonly string m_sMessageTag;
        private string m_sXmlNamespace;
        private string m_sXmlSchemaLocation;
        
        public SepaMessageInfo(SepaMessageInfo aMessageInfo)
        {
            if (aMessageInfo == null)
            {
                throw new ArgumentNullException();
            }
            this.m_nMessageType = aMessageInfo.MessageType;
            this.m_nVersion = aMessageInfo.Version;
            this.m_sMessageTag = aMessageInfo.MessageTag;
            this.XmlNamespace = aMessageInfo.XmlNamespace;
            this.XmlSchemaLocation = aMessageInfo.XmlSchemaLocation;
        }
        
        public SepaMessageInfo(SepaMessageType nMessageType, int nVersion)
        {
            if (nVersion <= 0)
            {
                throw new ArgumentException();
            }
            this.m_nMessageType = nMessageType;
            this.m_nVersion = nVersion;
            switch (nMessageType)
            {
                case SepaMessageType.CreditTransferPaymentInitiation:
                    if (nVersion < 2)
                    {
                        throw new NotSupportedException();
                    }
                    if (nVersion == 2)
                    {
                        this.m_sMessageTag = "pain.001.001.02";
                        return;
                    }
                    this.m_sMessageTag = "CstmrCdtTrfInitn";
                    return;
                
                case SepaMessageType.PaymentStatusReport:
                    if (nVersion < 3)
                    {
                        throw new NotSupportedException();
                    }
                    this.m_sMessageTag = "CstmrPmtStsRpt";
                    return;
                
                case SepaMessageType.DirectDebitPaymentInitiation:
                    if (nVersion == 1)
                    {
                        this.m_sMessageTag = "pain.008.001.01";
                        return;
                    }
                    if (nVersion == 2)
                    {
//                      this.m_sMessageTag = "pain.008.001.02";
                        this.m_sMessageTag = "pain.008.003.02";
                        return;
                    }
                    this.m_sMessageTag = "CstmrDrctDbtInitn";
                    return;
                
                case SepaMessageType.BankToCustomerAccountReport:
                    if (nVersion < 2)
                    {
                        throw new NotSupportedException();
                    }
                    this.m_sMessageTag = "BkToCstmrAcctRpt";
                    return;
                
                case SepaMessageType.BankToCustomerStatement:
                    if (nVersion < 2)
                    {
                        throw new NotSupportedException();
                    }
                    this.m_sMessageTag = "BkToCstmrStmt";
                    return;
                
                case SepaMessageType.BankToCustomerDebitCreditNotification:
                    if (nVersion < 2)
                    {
                        throw new NotSupportedException();
                    }
                    this.m_sMessageTag = "BkToCstmrDbtCdtNtfctn";
                    return;
            }
            throw new ArgumentException();
        }
        
        public SepaMessageInfo(SepaMessageType nMessageType, int nVersion, string sMessageTag, string sXmlNamespace, string sXmlSchemaLocation)
        {
            if (sMessageTag == null)
            {
                throw new ArgumentNullException();
            }
            if (!SepaUtil.CheckTagName(sMessageTag))
            {
                throw new ArgumentException();
            }
            if (nVersion <= 0)
            {
                throw new ArgumentException();
            }
            if (sXmlNamespace == "")
            {
                sXmlNamespace = null;
            }
            if (sXmlSchemaLocation == "")
            {
                sXmlSchemaLocation = null;
            }
            this.m_nMessageType = nMessageType;
            this.m_nVersion = nVersion;
            this.m_sMessageTag = sMessageTag;
            this.m_sXmlNamespace = sXmlNamespace;
            this.m_sXmlSchemaLocation = sXmlSchemaLocation;
        }
        
        private static string _ExtractPainIdentifier(string sNamespace)
        {
            string str = null;
            if (sNamespace != null)
            {
                Match match = new Regex(@"(pain\.\d{3}\.\d{3}\.\d{2})").Match(sNamespace);
                if (match.Success)
                {
                    str = match.Groups[1].Value;
                }
            }
            return str;
        }
        
        private static int _ExtractVersion(string sNamespace, string sMessageType, int nDefault)
        {
            if (sNamespace != null)
            {
                sMessageType = sMessageType.Replace(".", @"\.");
                Match match = new Regex(sMessageType + @"\.\d{3}\.(\d{2})").Match(sNamespace);
                if (match.Success)
                {
                    nDefault = int.Parse(match.Groups[1].Value);
                }
            }
            return nDefault;
        }
        
        public object Clone()
        {
            return new SepaMessageInfo(this);
        }
        
        public static SepaMessageInfo Create(SepaWellKnownMessageInfos nWellKnownMessage)
        {
            SepaMessageInfo info = null;
            switch (nWellKnownMessage)
            {
                case SepaWellKnownMessageInfos.Pain_001_001_02:
                case SepaWellKnownMessageInfos.ZKA_Pain_001_001_02:
                case SepaWellKnownMessageInfos.ZKA_Pain_001_002_02:
                case SepaWellKnownMessageInfos.AT_Pain_001_001_02:
                    info = new SepaMessageInfo(SepaMessageType.CreditTransferPaymentInitiation, 2);
                    break;
                
                case SepaWellKnownMessageInfos.Pain_001_001_03:
                case SepaWellKnownMessageInfos.ZKA_Pain_001_002_03:
                case SepaWellKnownMessageInfos.ZKA_Pain_001_003_03:
                case SepaWellKnownMessageInfos.AT_Pain_001_001_03:
                    info = new SepaMessageInfo(SepaMessageType.CreditTransferPaymentInitiation, 3);
                    break;
                
                case SepaWellKnownMessageInfos.Pain_008_001_01:
                case SepaWellKnownMessageInfos.ZKA_Pain_008_001_01:
                case SepaWellKnownMessageInfos.ZKA_Pain_008_002_01:
                case SepaWellKnownMessageInfos.AT_Pain_008_001_01:
                    info = new SepaMessageInfo(SepaMessageType.DirectDebitPaymentInitiation, 1);
                    break;
                
                case SepaWellKnownMessageInfos.Pain_008_001_02:
                case SepaWellKnownMessageInfos.ZKA_Pain_008_002_02:
                case SepaWellKnownMessageInfos.ZKA_Pain_008_003_02:
                case SepaWellKnownMessageInfos.AT_Pain_008_001_02:
                case SepaWellKnownMessageInfos.Pain_008_003_02:
                    info = new SepaMessageInfo(SepaMessageType.DirectDebitPaymentInitiation, 2);
                    break;
                
                case SepaWellKnownMessageInfos.ZKA_Pain_002_002_03:
                case SepaWellKnownMessageInfos.ZKA_Pain_002_003_03:
                    info = new SepaMessageInfo(SepaMessageType.PaymentStatusReport, 3);
                    break;
                
                case SepaWellKnownMessageInfos.ZKA_Camt_052_001_02:
                    info = new SepaMessageInfo(SepaMessageType.BankToCustomerAccountReport, 2);
                    break;
                
                case SepaWellKnownMessageInfos.ZKA_Camt_053_001_02:
                    info = new SepaMessageInfo(SepaMessageType.BankToCustomerStatement, 2);
                    break;
                
                case SepaWellKnownMessageInfos.ZKA_Camt_054_001_02:
                    info = new SepaMessageInfo(SepaMessageType.BankToCustomerDebitCreditNotification, 2);
                    break;
                
                default:
                    throw new ArgumentException();
            }
            info.m_sXmlNamespace = GetNamespace(nWellKnownMessage);
            info.m_sXmlSchemaLocation = GetSchemaLocation(nWellKnownMessage);
            return info;
        }
        
        public static SepaMessageInfo Create(string sMessageTag, string sXmlNamespace)
        {
            if (sMessageTag == null)
            {
                throw new ArgumentNullException();
            }
            if (!SepaUtil.CheckTagName(sMessageTag))
            {
                throw new ArgumentException();
            }
            SepaMessageInfo info = null;
            switch (sMessageTag)
            {
                case "CstmrCdtTrfInitn":
                    info = new SepaMessageInfo(SepaMessageType.CreditTransferPaymentInitiation, _ExtractVersion(sXmlNamespace, "pain.001", 3));
                    break;
                
                case "CstmrDrctDbtInitn":
                    info = new SepaMessageInfo(SepaMessageType.DirectDebitPaymentInitiation, _ExtractVersion(sXmlNamespace, "pain.008", 2));
                    break;
                
                case "pain.001.001.02":
                    info = new SepaMessageInfo(SepaMessageType.CreditTransferPaymentInitiation, 2);
                    break;
                
                case "pain.008.001.01":
                    info = new SepaMessageInfo(SepaMessageType.DirectDebitPaymentInitiation, 1);
                    break;

                case "pain.008.001.02":
                    info = new SepaMessageInfo(SepaMessageType.DirectDebitPaymentInitiation, 2);
                    break;

                case "pain.008.003.02":
                    info = new SepaMessageInfo(SepaMessageType.DirectDebitPaymentInitiation, 2);
                    break;

                case "CstmrPmtStsRpt":
                    info = new SepaMessageInfo(SepaMessageType.PaymentStatusReport, _ExtractVersion(sXmlNamespace, "pain.002", 3));
                    break;
                
                case "BkToCstmrAcctRpt":
                    info = new SepaMessageInfo(SepaMessageType.BankToCustomerAccountReport, _ExtractVersion(sXmlNamespace, "camt.052", 2));
                    break;
                
                case "BkToCstmrStmt":
                    info = new SepaMessageInfo(SepaMessageType.BankToCustomerStatement, _ExtractVersion(sXmlNamespace, "camt.053", 2));
                    break;
                
                case "BkToCstmrDbtCdtNtfctn":
                    info = new SepaMessageInfo(SepaMessageType.BankToCustomerDebitCreditNotification, _ExtractVersion(sXmlNamespace, "camt.054", 2));
                    break;
                
                default:
                    return null;
            }
            info.XmlNamespace = sXmlNamespace;
            return info;
        }
        
        public static string GetNamespace(SepaWellKnownMessageInfos nWellKnownMessage)
        {
            switch (nWellKnownMessage)
            {
                case SepaWellKnownMessageInfos.ZKA_Pain_001_001_02:
                    return "urn:sepade:xsd:pain.001.001.02";
                
                case SepaWellKnownMessageInfos.ZKA_Pain_001_002_02:
                    return "urn:swift:xsd:$pain.001.002.02";
                
                case SepaWellKnownMessageInfos.ZKA_Pain_001_002_03:
                    return "urn:iso:std:iso:20022:tech:xsd:pain.001.002.03";
                
                case SepaWellKnownMessageInfos.ZKA_Pain_001_003_03:
                    return "urn:iso:std:iso:20022:tech:xsd:pain.001.003.03";
                
                case SepaWellKnownMessageInfos.ZKA_Pain_002_002_03:
                    return "urn:iso:std:iso:20022:tech:xsd:pain.002.002.03";
                
                case SepaWellKnownMessageInfos.ZKA_Pain_002_003_03:
                    return "urn:iso:std:iso:20022:tech:xsd:pain.002.003.03";

                case SepaWellKnownMessageInfos.Pain_008_001_01:
                    return "urn:iso:std:iso:20022:tech:xsd:pain.008.001.01";

                case SepaWellKnownMessageInfos.Pain_008_001_02:
                    return "urn:iso:std:iso:20022:tech:xsd:pain.008.001.02";

                case SepaWellKnownMessageInfos.ZKA_Pain_008_001_01:
                    return "urn:sepade:xsd:pain.008.001.01";
                
                case SepaWellKnownMessageInfos.ZKA_Pain_008_002_01:
                    return "urn:swift:xsd:$pain.008.002.01";
                
                case SepaWellKnownMessageInfos.ZKA_Pain_008_002_02:
                    return "urn:iso:std:iso:20022:tech:xsd:pain.008.002.02";

                case SepaWellKnownMessageInfos.Pain_008_003_02:
                    return "urn:iso:std:iso:20022:tech:xsd:pain.008.003.02";

                case SepaWellKnownMessageInfos.ZKA_Pain_008_003_02:
                    return "urn:iso:std:iso:20022:tech:xsd:pain.008.003.02";
                
                case SepaWellKnownMessageInfos.ZKA_Camt_052_001_02:
                    return "urn:iso:std:iso:20022:tech:xsd:camt.052.001.02";
                
                case SepaWellKnownMessageInfos.ZKA_Camt_053_001_02:
                    return "urn:iso:std:iso:20022:tech:xsd:camt.053.001.02";
                
                case SepaWellKnownMessageInfos.ZKA_Camt_054_001_02:
                    return "urn:iso:std:iso:20022:tech:xsd:camt.054.001.02";
                
                case SepaWellKnownMessageInfos.AT_Pain_001_001_02:
                    return "APC:STUZZA:payments:ISO:pain:001:001:02:austrian:002";
                
                case SepaWellKnownMessageInfos.AT_Pain_001_001_03:
                    return "ISO:pain.001.001.03:APC:STUZZA:payments:001";
                
                case SepaWellKnownMessageInfos.AT_Pain_008_001_01:
                    return "APC:STUZZA:payments:ISO:pain:008:001:01:austrian:002";
                
                case SepaWellKnownMessageInfos.AT_Pain_008_001_02:
                    return "ISO:pain.008.001.02:APC:STUZZA:payments:001";
            }
            return null;
        }
        
        public static string GetSchemaLocation(SepaWellKnownMessageInfos nWellKnownMessage)
        {
            switch (nWellKnownMessage)
            {
                case SepaWellKnownMessageInfos.ZKA_Pain_001_001_02:
                    return "pain.001.001.02.xsd";
                
                case SepaWellKnownMessageInfos.ZKA_Pain_001_002_02:
                    return "pain.001.002.02.xsd";
                
                case SepaWellKnownMessageInfos.ZKA_Pain_001_002_03:
                    return "pain.001.002.03.xsd";
                
                case SepaWellKnownMessageInfos.ZKA_Pain_001_003_03:
                    return "pain.001.003.03.xsd";
                
                case SepaWellKnownMessageInfos.ZKA_Pain_002_002_03:
                    return "pain.002.002.03.xsd";
                
                case SepaWellKnownMessageInfos.ZKA_Pain_002_003_03:
                    return "pain.002.003.03.xsd";

                case SepaWellKnownMessageInfos.Pain_008_001_01:
                    return "pain.008.001.01.xsd";

                case SepaWellKnownMessageInfos.Pain_008_001_02:
                    return "pain.008.001.02.xsd";

                case SepaWellKnownMessageInfos.Pain_008_003_02:
                    return "pain.008.003.02.xsd";

                case SepaWellKnownMessageInfos.ZKA_Pain_008_001_01:
                    return "pain.008.001.01.xsd";
                
                case SepaWellKnownMessageInfos.ZKA_Pain_008_002_01:
                    return "pain.008.002.01.xsd";
                
                case SepaWellKnownMessageInfos.ZKA_Pain_008_002_02:
                    return "pain.008.002.02.xsd";
                
                case SepaWellKnownMessageInfos.ZKA_Pain_008_003_02:
                    return "pain.008.003.02.xsd";
                
                case SepaWellKnownMessageInfos.ZKA_Camt_052_001_02:
                    return "camt.052.001.02.xsd";
                
                case SepaWellKnownMessageInfos.ZKA_Camt_053_001_02:
                    return "camt.053.001.02.xsd";
                
                case SepaWellKnownMessageInfos.ZKA_Camt_054_001_02:
                    return "camt.054.001.02.xsd";
                
                case SepaWellKnownMessageInfos.AT_Pain_001_001_02:
                case SepaWellKnownMessageInfos.AT_Pain_001_001_03:
                case SepaWellKnownMessageInfos.AT_Pain_008_001_01:
                case SepaWellKnownMessageInfos.AT_Pain_008_001_02:
                    return null;
            }
            return null;
        }
        
        public static SepaWellKnownMessageInfos GetZkaWellKnownMessageInfo(string sMessageFormat)
        {
            switch (sMessageFormat)
            {
                case "pain.001.001.02":
                    return SepaWellKnownMessageInfos.ZKA_Pain_001_001_02;
                
                case "pain.001.002.02":
                    return SepaWellKnownMessageInfos.ZKA_Pain_001_002_02;
                
                case "pain.001.002.03":
                    return SepaWellKnownMessageInfos.ZKA_Pain_001_002_03;
                
                case "pain.001.003.03":
                    return SepaWellKnownMessageInfos.ZKA_Pain_001_003_03;
                
                case "pain.002.002.03":
                    return SepaWellKnownMessageInfos.ZKA_Pain_002_002_03;
                
                case "pain.002.003.03":
                    return SepaWellKnownMessageInfos.ZKA_Pain_002_003_03;
                
                case "pain.008.001.01":
                    return SepaWellKnownMessageInfos.ZKA_Pain_008_001_01;
                
                case "pain.008.002.01":
                    return SepaWellKnownMessageInfos.ZKA_Pain_008_002_01;
                
                case "pain.008.002.02":
                    return SepaWellKnownMessageInfos.ZKA_Pain_008_002_02;
                
                case "pain.008.003.02":
                    return SepaWellKnownMessageInfos.ZKA_Pain_008_003_02;
                
                case "camt.052.001.02":
                    return SepaWellKnownMessageInfos.ZKA_Camt_052_001_02;
                
                case "camt.053.001.02":
                    return SepaWellKnownMessageInfos.ZKA_Camt_053_001_02;
                
                case "camt.054.001.02":
                    return SepaWellKnownMessageInfos.ZKA_Camt_054_001_02;
            }
            return SepaWellKnownMessageInfos.Null;
        }
        
        public SepaMessage NewMessage()
        {
            switch (this.m_nMessageType)
            {
                case SepaMessageType.CreditTransferPaymentInitiation:
                    return new SepaCreditTransferPaymentInitiation(this.m_sMessageTag);
                
                case SepaMessageType.PaymentStatusReport:
                    return new SepaPaymentStatusReport(this.m_sMessageTag);
                
                case SepaMessageType.DirectDebitPaymentInitiation:
                    return new SepaDirectDebitPaymentInitiation(this.m_sMessageTag);
                
                case SepaMessageType.BankToCustomerAccountReport:
                    return new SepaBankToCustomerAccountReport(this.m_sMessageTag);
                
                case SepaMessageType.BankToCustomerStatement:
                    return new SepaBankToCustomerStatement(this.m_sMessageTag);
                
                case SepaMessageType.BankToCustomerDebitCreditNotification:
                    return new SepaBankToCustomerDebitCreditNotification(this.m_sMessageTag);
            }
            throw new NotSupportedException();
        }
        
        public string MessageTag
        {
            get
            {
                return this.m_sMessageTag;
            }
        }
        
        public SepaMessageType MessageType
        {
            get
            {
                return this.m_nMessageType;
            }
        }
        
        public string PainIdentifier
        {
            get
            {
                if (((this.m_nMessageType != SepaMessageType.CreditTransferPaymentInitiation) && (this.m_nMessageType != SepaMessageType.DirectDebitPaymentInitiation)) && (this.m_nMessageType != SepaMessageType.PaymentStatusReport))
                {
                    return null;
                }
                string str = _ExtractPainIdentifier(this.m_sXmlNamespace);
                if (str == null)
                {
                    str = string.Format("pain.{0:D3}.001.{1:D2}", (int) this.m_nMessageType, this.m_nVersion);
                }
                return str;
            }
        }
        
        public int Version
        {
            get
            {
                return this.m_nVersion;
            }
        }
        
        public string XmlNamespace
        {
            get
            {
                return this.m_sXmlNamespace;
            }
            set
            {
                this.m_sXmlNamespace = (value == "") ? null : value;
            }
        }
        
        public string XmlSchemaLocation
        {
            get
            {
                return this.m_sXmlSchemaLocation;
            }
            set
            {
                this.m_sXmlSchemaLocation = (value == "") ? null : value;
            }
        }
    }
}
