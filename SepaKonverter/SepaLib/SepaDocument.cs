
namespace SepaLib
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;
    
    public class SepaDocument
    {
        private SepaMessage m_aMessage;
        private readonly SepaMessageInfo m_aMessageInfo;
        private Encoding m_aXmlEncoding;
        private const string XMLSCHEMAINSTANCENAMESPACE = "http://www.w3.org/2001/XMLSchema-instance";
        
        public SepaDocument(SepaMessageInfo aMessageInfo) : this(aMessageInfo, null)
        {
        }
        
        public SepaDocument(SepaMessageInfo aMessageInfo, SepaMessage aMessage)
        {
            this.m_aXmlEncoding = new UTF8Encoding(false);
            if (aMessageInfo == null)
            {
                throw new ArgumentNullException();
            }
            this.m_aMessageInfo = aMessageInfo;
            this.Message = aMessage;
        }
        
        private static string[] _Split(string s)
        {
            List<string> list = new List<string>();
            s = s.Trim() + " ";
            int startIndex = 0;
            bool flag = true;
            for (int i = 0; i < s.Length; i++)
            {
                char ch = s[i];
                bool flag2 = (((ch == ' ') || (ch == '\t')) || (ch == '\r')) || (ch == '\n');
                if (flag)
                {
                    if (flag2)
                    {
                        list.Add(s.Substring(startIndex, i - startIndex));
                        flag = false;
                    }
                }
                else if (!flag2)
                {
                    startIndex = i;
                    flag = true;
                }
            }
            return list.ToArray();
        }
        
        public static SepaDocument NewDocument(Stream aStream)
        {
            if (aStream == null)
            {
                throw new ArgumentNullException();
            }
            XmlReaderSettings settings = new XmlReaderSettings {
                IgnoreComments = true,
                IgnoreProcessingInstructions = true,
                IgnoreWhitespace = true
            };
            return NewDocumentXml(XmlReader.Create(aStream, settings));
        }
        
        public static SepaDocument NewDocument(string sFileName)
        {
            if (sFileName == null)
            {
                throw new ArgumentNullException();
            }
            if (sFileName == "")
            {
                throw new ArgumentException();
            }
            using (Stream stream = File.OpenRead(sFileName))
            {
                return NewDocument(stream);
            }
        }
        
        public static SepaDocument NewDocumentXml(XmlReader aXmlReader)
        {
            if (aXmlReader == null)
            {
                throw new ArgumentNullException();
            }
            aXmlReader.MoveToContent();
            if (aXmlReader.Name != "Document")
            {
                throw new XmlException("No SEPA Document", null);
            }
            string namespaceURI = aXmlReader.NamespaceURI;
            string s = null;
            if ((namespaceURI != null) && (namespaceURI != ""))
            {
                s = aXmlReader.GetAttribute("schemaLocation", "http://www.w3.org/2001/XMLSchema-instance");
                if ((s != null) && (s != ""))
                {
                    string[] strArray = _Split(s);
                    for (int i = 1; i < strArray.Length; i++)
                    {
                        if (strArray[i - 1] == namespaceURI)
                        {
                            s = strArray[i];
                            break;
                        }
                    }
                }
            }
            aXmlReader.ReadStartElement("Document");
            SepaMessageInfo aMessageInfo = SepaMessageInfo.Create(aXmlReader.LocalName, namespaceURI);
            if (aMessageInfo == null)
            {
                throw new NotSupportedException();
            }
            aMessageInfo.XmlSchemaLocation = s;
            SepaMessage aMessage = aMessageInfo.NewMessage();
            aMessage.ReadXml(aXmlReader, aMessageInfo);
            aXmlReader.ReadEndElement();
            return new SepaDocument(aMessageInfo, aMessage);
        }
        
        public void WriteDocument(Stream aStream)
        {
            if (aStream == null)
            {
                throw new ArgumentNullException();
            }
            XmlWriterSettings settings = new XmlWriterSettings {
                Encoding = this.m_aXmlEncoding
            };
            XmlWriter aXmlWriter = XmlWriter.Create(aStream, settings);
            this.WriteDocumentXml(aXmlWriter);
        }
        
        public void WriteDocument(string sFileName)
        {
            if (sFileName == null)
            {
                throw new ArgumentNullException();
            }
            if (sFileName == string.Empty)
            {
                throw new ArgumentException();
            }
            using (FileStream stream = File.Create(sFileName))
            {
                this.WriteDocument(stream);
                stream.Close();
            }
        }
        
        public void WriteDocumentXml(XmlWriter aXmlWriter)
        {
            aXmlWriter.WriteStartDocument();
            this.WriteXml(aXmlWriter);
            aXmlWriter.WriteEndDocument();
            aXmlWriter.Flush();
        }
        
        public void WriteXml(XmlWriter aXmlWriter)
        {
            if (this.m_aMessage == null)
            {
                throw new InvalidOperationException();
            }
            string xmlNamespace = this.MessageInfo.XmlNamespace;
            string xmlSchemaLocation = this.MessageInfo.XmlSchemaLocation;
            aXmlWriter.WriteStartElement("Document", xmlNamespace);
            if (xmlNamespace != null)
            {
                aXmlWriter.WriteAttributeString("xmlns", xmlNamespace);
                aXmlWriter.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                if (xmlSchemaLocation != null)
                {
                    aXmlWriter.WriteAttributeString("schemaLocation", "http://www.w3.org/2001/XMLSchema-instance", xmlNamespace + " " + xmlSchemaLocation);
                }
            }
            this.m_aMessage.WriteXml(aXmlWriter, this.m_aMessageInfo);
            aXmlWriter.WriteEndElement();
        }
        
        public string EbicsOrderType
        {
            get
            {
                if ((this.m_aMessage == null) || (this.m_aMessageInfo == null))
                {
                    return null;
                }
                string str = null;
                switch (this.m_aMessageInfo.MessageType)
                {
                    case SepaMessageType.CreditTransferPaymentInitiation:
                    {
                        SepaCreditTransferPaymentInitiation aMessage = (SepaCreditTransferPaymentInitiation) this.m_aMessage;
                        if (aMessage.PaymentInformations.Count != 1)
                        {
                            break;
                        }
                        SepaCreditTransferPaymentInformation information = (SepaCreditTransferPaymentInformation) aMessage.PaymentInformations[0];
                        if (!(information.ServiceLevelCode == "URGP"))
                        {
                            break;
                        }
                        return "CCU";
                    }
                    case SepaMessageType.PaymentStatusReport:
                    {
                        SepaPaymentStatusReport report = (SepaPaymentStatusReport) this.m_aMessage;
                        if (report.OriginalMessageType != SepaMessageType.CreditTransferPaymentInitiation)
                        {
                            if (report.OriginalMessageType == SepaMessageType.DirectDebitPaymentInitiation)
                            {
                                str = (this.m_aMessageInfo.Version >= 3) ? "CDZ" : "CDR";
                            }
                            return str;
                        }
                        return ((this.m_aMessageInfo.Version >= 3) ? "CRZ" : "CRJ");
                    }
                    case SepaMessageType.DirectDebitPaymentInitiation:
                    {
                        if (this.m_aMessageInfo.XmlNamespace == "urn:sepade:xsd:pain.008.001.01")
                        {
                            return "CDM";
                        }
                        SepaDirectDebitPaymentInitiation initiation2 = (SepaDirectDebitPaymentInitiation) this.m_aMessage;
                        if (initiation2.PaymentInformations.Count != 1)
                        {
                            return str;
                        }
                        SepaDirectDebitPaymentInformation information2 = (SepaDirectDebitPaymentInformation) initiation2.PaymentInformations[0];
                        string localInstrumentCode = information2.LocalInstrumentCode;
                        if (localInstrumentCode == null)
                        {
                            return str;
                        }
                        if (!(localInstrumentCode == "CORE"))
                        {
                            if (localInstrumentCode == "COR1")
                            {
                                return "CD1";
                            }
                            if (localInstrumentCode != "B2B")
                            {
                                return str;
                            }
                            return "CDB";
                        }
                        return "CDD";
                    }
                    case SepaMessageType.BankToCustomerAccountReport:
                        return "C52";
                    
                    case SepaMessageType.BankToCustomerStatement:
                        return "C53";
                    
                    case SepaMessageType.BankToCustomerDebitCreditNotification:
                        return "C54";
                    
                    default:
                        return str;
                }
                return (((this.m_aMessageInfo.Version >= 3) || (this.m_aMessageInfo.XmlNamespace == "urn:swift:xsd:$pain.001.002.02")) ? "CCT" : "CCM");
            }
        }
        
        public string HbciSegmentType
        {
            get
            {
                string str3;
                if ((this.m_aMessage == null) || (this.m_aMessageInfo == null))
                {
                    return null;
                }
                string str = null;
                SepaMessageType messageType = this.m_aMessageInfo.MessageType;
                if (messageType != SepaMessageType.CreditTransferPaymentInitiation)
                {
                    if (messageType != SepaMessageType.DirectDebitPaymentInitiation)
                    {
                        if (messageType != SepaMessageType.BankToCustomerAccountReport)
                        {
                            return str;
                        }
                        return "HKCAZ";
                    }
                }
                else
                {
                    SepaCreditTransferPaymentInitiation initiation = (SepaCreditTransferPaymentInitiation) this.m_aMessage;
                    if (initiation.PaymentInformations.Count != 1)
                    {
                        return str;
                    }
                    SepaCreditTransferPaymentInformation information = (SepaCreditTransferPaymentInformation) initiation.PaymentInformations[0];
                    bool flag = information.RequestedExecutionDate > DateTime.Now;
                    if (information.NumberOfTransactions == 1)
                    {
                        if (information.ServiceLevelCode == "URGP")
                        {
                            return "HKCSU";
                        }
                        if (flag)
                        {
                            return "HKCSE";
                        }
                        return "HKCCS";
                    }
                    if (information.NumberOfTransactions <= 1)
                    {
                        return str;
                    }
                    if (information.ServiceLevelCode == "URGP")
                    {
                        return "HKCMU";
                    }
                    if (flag)
                    {
                        return "HKCME";
                    }
                    return "HKCCM";
                }
                SepaDirectDebitPaymentInitiation aMessage = (SepaDirectDebitPaymentInitiation) this.m_aMessage;
                if (aMessage.PaymentInformations.Count != 1)
                {
                    return str;
                }
                SepaDirectDebitPaymentInformation information2 = (SepaDirectDebitPaymentInformation) aMessage.PaymentInformations[0];
                if (information2.NumberOfTransactions == 1)
                {
                    string localInstrumentCode = information2.LocalInstrumentCode;
                    if (localInstrumentCode == null)
                    {
                        return str;
                    }
                    if (!(localInstrumentCode == "CORE"))
                    {
                        if (localInstrumentCode != "COR1")
                        {
                            if (localInstrumentCode != "B2B")
                            {
                                return str;
                            }
                            return "HKBSE";
                        }
                    }
                    else
                    {
                        return "HKDSE";
                    }
                    return "HKDSC";
                }
                if ((information2.NumberOfTransactions <= 1) || ((str3 = information2.LocalInstrumentCode) == null))
                {
                    return str;
                }
                if (!(str3 == "CORE"))
                {
                    if (str3 != "COR1")
                    {
                        if (str3 != "B2B")
                        {
                            return str;
                        }
                        return "HKBME";
                    }
                }
                else
                {
                    return "HKDME";
                }
                return "HKDMC";
            }
        }
        
        public SepaMessage Message
        {
            get
            {
                return this.m_aMessage;
            }
            set
            {
                if ((value != null) && (value.MessageType != this.MessageType))
                {
                    throw new ArgumentException();
                }
                this.m_aMessage = value;
            }
        }
        
        public SepaMessageInfo MessageInfo
        {
            get
            {
                return this.m_aMessageInfo;
            }
        }
        
        public SepaMessageType MessageType
        {
            get
            {
                return this.m_aMessageInfo.MessageType;
            }
        }
        
        public Encoding XmlEncoding
        {
            get
            {
                return this.m_aXmlEncoding;
            }
            set
            {
                this.m_aXmlEncoding = value;
            }
        }
    }
}
