
namespace SepaLib
{
    using System;
    using System.Xml;
    
    public sealed class SepaBalance : SepaObject
    {
        private SepaAmount m_aAmt;
        private DateTime m_dtDt;
        private string m_sTpCd;
        
        public SepaBalance() : base("Bal")
        {
            this.m_aAmt = new SepaAmount();
        }
        
        public override void Clear()
        {
            this.m_sTpCd = null;
            this.m_aAmt.Clear();
            this.m_dtDt = DateTime.MinValue;
        }
        
        public override bool IsValid()
        {
            return (((((this.m_sTpCd != null) && (this.m_sTpCd != "")) && ((this.m_aAmt.Currency != null) && (this.m_aAmt.Currency != ""))) && (this.m_aAmt.CreditDebitIndicator != SepaCreditDebitIndicator.Undefined)) && (this.m_dtDt > DateTime.MinValue));
        }
        
        protected override void OnReadXml(XmlReader aXmlReader, SepaMessageInfo aMessageInfo)
        {
            aXmlReader.ReadStartElement("Tp");
            aXmlReader.ReadStartElement("CdOrPrtry");
            this.m_sTpCd = aXmlReader.ReadElementString("Cd");
            aXmlReader.ReadEndElement();
            if (aXmlReader.IsStartElement("SubTp"))
            {
                aXmlReader.Skip();
            }
            aXmlReader.ReadEndElement();
            if (aXmlReader.IsStartElement("CdtLine"))
            {
                aXmlReader.Skip();
            }
            this.m_aAmt.ReadXml(aXmlReader, "Amt");
            aXmlReader.ReadStartElement("Dt");
            this.m_dtDt = SepaUtil.ReadDtOrDtTmXml(aXmlReader);
            aXmlReader.ReadEndElement();
            while (aXmlReader.IsStartElement("Avlbty"))
            {
                aXmlReader.Skip();
            }
        }
        
        protected override void OnWriteXml(XmlWriter aXmlWriter, SepaMessageInfo aMessageInfo)
        {
            aXmlWriter.WriteStartElement("Tp");
            aXmlWriter.WriteStartElement("CdOrPrtry");
            aXmlWriter.WriteElementString("Cd", this.m_sTpCd);
            aXmlWriter.WriteEndElement();
            aXmlWriter.WriteEndElement();
            this.m_aAmt.WriteXml(aXmlWriter, "Amt");
            aXmlWriter.WriteStartElement("Dt");
            SepaUtil.WriteDtXml(aXmlWriter, "Dt", this.m_dtDt);
            aXmlWriter.WriteEndElement();
        }
        
        public SepaAmount Amount
        {
            get
            {
                return this.m_aAmt;
            }
        }
        
        public DateTime Date
        {
            get
            {
                return this.m_dtDt;
            }
            set
            {
                this.m_dtDt = value;
            }
        }
        
        public string TypeCode
        {
            get
            {
                return this.m_sTpCd;
            }
            set
            {
                string s = SepaUtil.Trim(value);
                if (((s != null) && (s != "")) && ((s.Length != 4) || !SepaUtil.IsAlpha(s)))
                {
                    throw new ArgumentException();
                }
                this.m_sTpCd = s;
            }
        }
    }
}
