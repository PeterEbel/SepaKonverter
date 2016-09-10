
namespace SepaLib
{
    using System;
    using System.Xml;
    
    public class SepaStatusReasonInformation : SepaObject
    {
        private SepaPartyIdentification m_aOrgtr;
        private string m_sRsnCd;
        
        public SepaStatusReasonInformation() : base("StsRsnInf")
        {
            this.m_aOrgtr = new SepaPartyIdentification("Orgtr", SepaPartyIdentification.Fields.BIC | SepaPartyIdentification.Fields.Name, SepaPartyIdentification.Fields.None);
        }
        
        public override void Clear()
        {
            this.m_aOrgtr.Clear();
            this.m_sRsnCd = null;
        }
        
        public override bool IsValid()
        {
            if (!this.m_aOrgtr.IsValid())
            {
                return false;
            }
            return true;
        }
        
        protected override void OnReadXml(XmlReader aXmlReader, SepaMessageInfo aMessageInfo)
        {
            if (aXmlReader.IsStartElement("Orgtr"))
            {
                this.m_aOrgtr.ReadXml(aXmlReader, aMessageInfo);
            }
            if (aXmlReader.IsStartElement("Rsn"))
            {
                aXmlReader.ReadStartElement();
                this.m_sRsnCd = aXmlReader.ReadElementString("Cd");
                aXmlReader.ReadEndElement();
            }
        }
        
        protected override void OnWriteXml(XmlWriter aXmlWriter, SepaMessageInfo aMessageInfo)
        {
            if (!this.m_aOrgtr.IsEmpty())
            {
                this.m_aOrgtr.WriteXml(aXmlWriter, aMessageInfo);
            }
            if (this.m_sRsnCd != null)
            {
                aXmlWriter.WriteStartElement("Rsn");
                aXmlWriter.WriteElementString("Cd", this.m_sRsnCd);
                aXmlWriter.WriteEndElement();
            }
        }
        
        public SepaPartyIdentification StatusOriginator
        {
            get
            {
                return this.m_aOrgtr;
            }
        }
        
        public string StatusReasonCode
        {
            get
            {
                return this.m_sRsnCd;
            }
            set
            {
                if ((value != null) && ((value.Length != 4) || !SepaUtil.IsAlphaNumeric(value)))
                {
                    throw new ArgumentException();
                }
                this.m_sRsnCd = value;
            }
        }
    }
}
