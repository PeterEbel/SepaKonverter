
namespace SepaLib
{
    using System;
    using System.Xml;
    
    public abstract class SepaTransactionInformation : SepaObject
    {
        private SepaAmount m_aAmt;
        private bool m_fRmtInfStrd;
        private string m_sEndToEndId;
        private string m_sPurpCd;
        private string m_sRmtInf;
        
        public SepaTransactionInformation(string sTagName) : base(sTagName)
        {
            this.m_aAmt = new SepaAmount();
            this.m_aAmt.Currency = "EUR";
        }
        
        public override void Clear()
        {
            this.m_sEndToEndId = null;
            this.m_aAmt.Clear();
            this.m_sPurpCd = null;
            this.m_sRmtInf = null;
            this.m_fRmtInfStrd = false;
        }
        
        public override bool IsValid()
        {
            return ((((this.m_sEndToEndId != "") && (this.m_aAmt.Amount > 0M)) && (this.m_aAmt.Currency != null)) && (this.m_aAmt.Currency != ""));
        }
        
        internal void ReadInstdAmtXml(XmlReader aXmlReader)
        {
            this.m_aAmt.ReadXml(aXmlReader, "InstdAmt");
        }
        
        internal void ReadPmtIdXml(XmlReader aXmlReader)
        {
            aXmlReader.ReadStartElement("PmtId");
            if (aXmlReader.IsStartElement("InstrId"))
            {
                aXmlReader.Skip();
            }
            string str = aXmlReader.ReadElementString("EndToEndId");
            aXmlReader.ReadEndElement();
            if (str == "NOTPROVIDED")
            {
                str = null;
            }
            this.m_sEndToEndId = str;
        }
        
        internal void ReadPurpXml(XmlReader aXmlReader)
        {
            if (aXmlReader.IsStartElement("Purp"))
            {
                aXmlReader.ReadStartElement();
                this.m_sPurpCd = aXmlReader.ReadElementString("Cd");
                aXmlReader.ReadEndElement();
            }
        }
        
        internal void ReadRmtInfXml(XmlReader aXmlReader)
        {
            if (aXmlReader.IsStartElement("RmtInf"))
            {
                if (aXmlReader.IsEmptyElement)
                {
                    aXmlReader.Skip();
                }
                else
                {
                    aXmlReader.ReadStartElement();
                    if (aXmlReader.IsStartElement("Ustrd"))
                    {
                        this.m_sRmtInf = aXmlReader.ReadElementString();
                    }
                    else if (aXmlReader.IsStartElement("Strd"))
                    {
                        this.m_sRmtInf = aXmlReader.ReadInnerXml();
                        this.m_fRmtInfStrd = true;
                    }
                    aXmlReader.ReadEndElement();
                }
            }
        }
        
        internal void WriteInstdAmtXml(XmlWriter aXmlWriter)
        {
            this.m_aAmt.WriteXml(aXmlWriter, "InstdAmt");
        }
        
        internal void WritePmtIdXml(XmlWriter aXmlWriter)
        {
            string sEndToEndId = this.m_sEndToEndId;
            if (sEndToEndId == null)
            {
                sEndToEndId = "NOTPROVIDED";
            }
            aXmlWriter.WriteStartElement("PmtId");
            aXmlWriter.WriteElementString("EndToEndId", sEndToEndId);
            aXmlWriter.WriteEndElement();
        }
        
        internal void WritePurpXml(XmlWriter aXmlWriter)
        {
            if ((this.m_sPurpCd != null) && (this.m_sPurpCd != ""))
            {
                aXmlWriter.WriteStartElement("Purp");
                aXmlWriter.WriteElementString("Cd", this.m_sPurpCd);
                aXmlWriter.WriteEndElement();
            }
        }
        
        internal void WriteRmtInfXml(XmlWriter aXmlWriter)
        {
            if ((this.m_sRmtInf != null) && (this.m_sRmtInf != ""))
            {
                aXmlWriter.WriteStartElement("RmtInf");
                if (this.m_fRmtInfStrd)
                {
                    aXmlWriter.WriteStartElement("Strd");
                    aXmlWriter.WriteRaw(this.m_sRmtInf);
                    aXmlWriter.WriteEndElement();
                }
                else
                {
                    aXmlWriter.WriteElementString("Ustrd", this.m_sRmtInf);
                }
                aXmlWriter.WriteEndElement();
            }
        }
        
        public decimal Amount
        {
            get
            {
                return this.m_aAmt.Amount;
            }
            set
            {
                this.m_aAmt.Amount = value;
            }
        }
        
        public string AmountCurrency
        {
            get
            {
                return this.m_aAmt.Currency;
            }
            set
            {
                this.m_aAmt.Currency = value;
            }
        }
        
        public string EndToEndId
        {
            get
            {
                return this.m_sEndToEndId;
            }
            set
            {
                string sText = SepaUtil.Trim(value);
                if (sText == "NOTPROVIDED")
                {
                    sText = null;
                }
                if ((sText != null) && (((sText == "") || (sText.Length > 0x23)) || !SepaUtil.CheckCharset(sText)))
                {
                    throw new ArgumentException();
                }
                this.m_sEndToEndId = sText;
            }
        }
        
        public SepaPaymentInformation PaymentInformation
        {
            get
            {
                return (SepaPaymentInformation) base.Parent;
            }
        }
        
        public SepaPaymentInitiation PaymentInitiation
        {
            get
            {
                SepaPaymentInformation paymentInformation = this.PaymentInformation;
                if (paymentInformation == null)
                {
                    return null;
                }
                return paymentInformation.PaymentInitiation;
            }
        }
        
        public string PurposeCode
        {
            get
            {
                return this.m_sPurpCd;
            }
            set
            {
                string s = SepaUtil.Trim(value);
                if (((s != null) && (s != "")) && ((s.Length != 4) || !SepaUtil.IsAlpha(s)))
                {
                    throw new ArgumentException();
                }
                this.m_sPurpCd = s;
            }
        }
        
        public string RemittanceInformation
        {
            get
            {
                return this.m_sRmtInf;
            }
            set
            {
                string s = SepaUtil.Trim(value);
                if ((s != null) && (s != ""))
                {
                    if (s.Length > 140)
                    {
                        throw new ArgumentException();
                    }
                    if (!SepaUtil.IsLatin1(s))
                    {
                        throw new ArgumentException();
                    }
                }
                this.m_sRmtInf = s;
                this.m_fRmtInfStrd = false;
            }
        }
    }
}
