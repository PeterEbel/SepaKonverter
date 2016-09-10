
namespace SepaLib
{
    using System;
    using System.Xml;
    
    public sealed class SepaFinancialInstitutionIdentification : SepaObject
    {
        private string m_sClrSysMmbId;
        private string m_sNm;
        private string m_sVatId;
        private SepaBIC m_tBIC;
        
        public SepaFinancialInstitutionIdentification() : base("FinInstnId")
        {
        }
        
        public override void Clear()
        {
            this.m_tBIC = SepaBIC.NullBIC;
            this.m_sClrSysMmbId = null;
            this.m_sNm = null;
            this.m_sVatId = null;
        }
        
        public bool IsEmpty()
        {
            if ((this.m_tBIC.IsNull && ((this.m_sClrSysMmbId == null) || (this.m_sClrSysMmbId == ""))) && ((this.m_sNm == null) || (this.m_sNm == "")))
            {
                if (this.m_sVatId != null)
                {
                    return (this.m_sVatId == "");
                }
                return true;
            }
            return false;
        }
        
        public override bool IsValid()
        {
            return true;
        }
        
        protected override void OnReadXml(XmlReader aXmlReader, SepaMessageInfo aMessageInfo)
        {
            if (aXmlReader.IsStartElement("BIC"))
            {
                this.m_tBIC = new SepaBIC(aXmlReader.ReadElementString("BIC"));
            }
            if (aXmlReader.IsStartElement("ClrSysMmbId"))
            {
                aXmlReader.ReadStartElement();
                if (aXmlReader.IsStartElement("ClrSysId"))
                {
                    aXmlReader.Skip();
                }
                this.m_sClrSysMmbId = aXmlReader.ReadElementString("MmbId");
                aXmlReader.ReadEndElement();
            }
            if (aXmlReader.IsStartElement("Nm"))
            {
                this.m_sNm = aXmlReader.ReadElementString();
            }
            if (aXmlReader.IsStartElement("PstlAdr"))
            {
                aXmlReader.Skip();
            }
            if (aXmlReader.IsStartElement("Othr"))
            {
                this.m_sVatId = SepaUtil.ReadOthrId(aXmlReader, "UmsStId");
            }
        }
        
        protected override void OnWriteXml(XmlWriter aXmlWriter, SepaMessageInfo aMessageInfo)
        {
            if (!this.m_tBIC.IsNull)
            {
                aXmlWriter.WriteElementString("BIC", this.m_tBIC.BIC);
            }
            if ((this.m_sClrSysMmbId != null) && (this.m_sClrSysMmbId != ""))
            {
                aXmlWriter.WriteStartElement("ClrSysMmbId");
                aXmlWriter.WriteStartElement("ClrSysId");
                aXmlWriter.WriteElementString("Cd", "DEBLZ");
                aXmlWriter.WriteEndElement();
                aXmlWriter.WriteElementString("MmbId", this.m_sClrSysMmbId);
                aXmlWriter.WriteEndElement();
            }
            if ((this.m_sNm != null) && (this.m_sNm != ""))
            {
                aXmlWriter.WriteElementString("Nm", this.m_sNm);
            }
            if ((this.m_sVatId != null) && (this.m_sVatId != ""))
            {
                SepaUtil.WriteOthrId(aXmlWriter, this.m_sVatId, "UmsStId");
            }
        }
        
        public SepaBIC BIC
        {
            get
            {
                return this.m_tBIC;
            }
            set
            {
                this.m_tBIC = value;
            }
        }
        
        public string ClearingSystemMemberIdentification
        {
            get
            {
                return this.m_sClrSysMmbId;
            }
            set
            {
                string sText = SepaUtil.Trim(value);
                if (((sText != null) && (sText != "")) && ((sText.Length > 0x23) || !SepaUtil.CheckCharset(sText)))
                {
                    throw new ArgumentException();
                }
                this.m_sClrSysMmbId = sText;
            }
        }
        
        public string Name
        {
            get
            {
                return this.m_sNm;
            }
            set
            {
                string s = SepaUtil.Trim(value);
                if ((s != null) && (s != ""))
                {
                    if (s.Length > 70)
                    {
                        throw new ArgumentException();
                    }
                    if (!SepaUtil.IsLatin1(s))
                    {
                        throw new ArgumentException();
                    }
                }
                this.m_sNm = s;
            }
        }
        
        public string VatID
        {
            get
            {
                return this.m_sVatId;
            }
            set
            {
                string str = SepaUtil.Trim(value);
                if (str == null)
                {
                    bool flag1 = str != "";
                }
                this.m_sVatId = str;
            }
        }
    }
}
