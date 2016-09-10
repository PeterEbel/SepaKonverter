
namespace SepaLib
{
    using System;
    using System.Runtime.InteropServices;
    using System.Xml;
    
    public sealed class SepaPartyIdentification : SepaObject
    {
        private Fields m_nRequiredFields;
        private Fields m_nSupportedFields;
        private string m_sCdtrSchmeId;
        private string m_sEBICSPartnerId;
        private string m_sNm;
        private SepaBIC m_tBIC;
        
        public SepaPartyIdentification(string sTagName, Fields nSupportedFields, Fields nRequiredFields = 0) : base(sTagName)
        {
            this.m_nSupportedFields = nSupportedFields;
            this.m_nRequiredFields = nRequiredFields;
        }
        
        public override void Clear()
        {
            this.m_sNm = null;
            this.m_tBIC = SepaBIC.NullBIC;
            this.m_sEBICSPartnerId = null;
            this.m_sCdtrSchmeId = null;
        }
        
        public bool IsEmpty()
        {
            bool flag = (this.m_sNm != null) && (this.m_sNm != "");
            bool flag2 = !this.m_tBIC.IsNull;
            bool flag3 = (this.m_sEBICSPartnerId != null) && (this.m_sEBICSPartnerId != "");
            bool flag4 = (this.m_sCdtrSchmeId != null) && (this.m_sCdtrSchmeId != "");
            if (((this.m_nSupportedFields & Fields.Name) != Fields.None) && flag)
            {
                return false;
            }
            if (((this.m_nSupportedFields & Fields.BIC) != Fields.None) && flag2)
            {
                return false;
            }
            if (((this.m_nSupportedFields & Fields.EBICS) != Fields.None) && flag3)
            {
                return false;
            }
            if (((this.m_nSupportedFields & Fields.CdtrId) != Fields.None) && flag4)
            {
                return false;
            }
            return true;
        }
        
        public override bool IsValid()
        {
            bool flag = (this.m_sNm != null) && (this.m_sNm != "");
            bool flag2 = !this.m_tBIC.IsNull;
            bool flag3 = (this.m_sEBICSPartnerId != null) && (this.m_sEBICSPartnerId != "");
            bool flag4 = (this.m_sCdtrSchmeId != null) && (this.m_sCdtrSchmeId != "");
            if (((this.m_nRequiredFields & Fields.Name) != Fields.None) && !flag)
            {
                return false;
            }
            if (((this.m_nRequiredFields & Fields.BIC) != Fields.None) && !flag2)
            {
                return false;
            }
            if (((this.m_nRequiredFields & Fields.EBICS) != Fields.None) && !flag3)
            {
                return false;
            }
            if (((this.m_nRequiredFields & Fields.CdtrId) != Fields.None) && !flag4)
            {
                return false;
            }
            return true;
        }
        
        protected override void OnReadXml(XmlReader aXmlReader, SepaMessageInfo aMessageInfo)
        {
            if (aXmlReader.IsStartElement("Nm"))
            {
                this.m_sNm = aXmlReader.ReadElementString();
            }
            if (aXmlReader.IsStartElement("PstlAdr"))
            {
                aXmlReader.Skip();
            }
            if (aXmlReader.IsStartElement("Id"))
            {
                aXmlReader.ReadStartElement();
                if (aXmlReader.IsStartElement("OrgId"))
                {
                    if (aXmlReader.IsEmptyElement)
                    {
                        aXmlReader.Skip();
                    }
                    else
                    {
                        aXmlReader.ReadStartElement();
                        while (aXmlReader.NodeType != XmlNodeType.EndElement)
                        {
                            if (aXmlReader.IsStartElement("BICOrBEI") || aXmlReader.IsStartElement("BkPtyId"))
                            {
                                this.m_tBIC = new SepaBIC(aXmlReader.ReadElementString());
                            }
                            else if (aXmlReader.IsStartElement("Othr"))
                            {
                                this.m_sEBICSPartnerId = SepaUtil.ReadOthrId(aXmlReader, "EBICS");
                            }
                            else
                            {
                                aXmlReader.Skip();
                            }
                        }
                        aXmlReader.ReadEndElement();
                    }
                }
                if (aXmlReader.IsStartElement("PrvtId"))
                {
                    if (aXmlReader.IsEmptyElement || ((this.m_nSupportedFields & Fields.CdtrId) == Fields.None))
                    {
                        aXmlReader.Skip();
                    }
                    else
                    {
                        aXmlReader.ReadStartElement();
                        if (aXmlReader.IsStartElement("DtAndPlcOfBirth"))
                        {
                            aXmlReader.Skip();
                        }
                        if (aXmlReader.IsStartElement("Othr"))
                        {
                            this.m_sCdtrSchmeId = SepaUtil.ReadOthrId(aXmlReader, null);
                        }
                        aXmlReader.ReadEndElement();
                    }
                }
                aXmlReader.ReadEndElement();
            }
            while (aXmlReader.NodeType != XmlNodeType.EndElement)
            {
                aXmlReader.Skip();
            }
        }
        
        protected override void OnWriteXml(XmlWriter aXmlWriter, SepaMessageInfo aMessageInfo)
        {
            bool flag = false;
            bool flag2 = false;
            if (base.TagName == "InitgPty")
            {
                flag = (((aMessageInfo.XmlNamespace == "APC:STUZZA:payments:ISO:pain:001:001:02:austrian:002") || (aMessageInfo.XmlNamespace == "ISO:pain.001.001.03:APC:STUZZA:payments:001")) || (aMessageInfo.XmlNamespace == "APC:STUZZA:payments:ISO:pain:008:001:01:austrian:002")) || (aMessageInfo.XmlNamespace == "ISO:pain.008.001.02:APC:STUZZA:payments:001");
                flag2 = !flag;
            }
            bool flag3 = ((((this.m_nSupportedFields & Fields.Name) != Fields.None) && (this.m_sNm != null)) && (this.m_sNm != "")) && !flag;
            bool flag4 = (((this.m_nSupportedFields & Fields.BIC) != Fields.None) && !this.m_tBIC.IsNull) && !flag2;
            bool flag5 = (((this.m_nSupportedFields & Fields.EBICS) != Fields.None) && (this.m_sEBICSPartnerId != null)) && (this.m_sEBICSPartnerId != "");
            bool flag6 = (((this.m_nSupportedFields & Fields.CdtrId) != Fields.None) && (this.m_sCdtrSchmeId != null)) && (this.m_sCdtrSchmeId != "");
            if (flag3)
            {
                aXmlWriter.WriteElementString("Nm", this.m_sNm);
            }
            if ((flag4 || flag5) || flag6)
            {
                aXmlWriter.WriteStartElement("Id");
                if (flag4 || flag5)
                {
                    aXmlWriter.WriteStartElement("OrgId");
                    if (flag4)
                    {
                        if (flag)
                        {
                            aXmlWriter.WriteElementString("BkPtyId", this.m_tBIC.BIC);
                        }
                        else
                        {
                            aXmlWriter.WriteElementString("BICOrBEI", this.m_tBIC.BIC);
                        }
                    }
                    if (flag5)
                    {
                        SepaUtil.WriteOthrId(aXmlWriter, this.m_sEBICSPartnerId, "EBICS");
                    }
                    aXmlWriter.WriteEndElement();
                }
                if (flag6)
                {
                    aXmlWriter.WriteStartElement("PrvtId");
                    SepaUtil.WriteOthrId(aXmlWriter, this.m_sCdtrSchmeId, null);
                    aXmlWriter.WriteEndElement();
                }
                aXmlWriter.WriteEndElement();
            }
        }
        
        public string BankPartyIdentification
        {
            get
            {
                return this.m_tBIC.BIC;
            }
            set
            {
                this.m_tBIC = new SepaBIC(value);
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
        
        public string CreditorSchemeIdentification
        {
            get
            {
                return this.m_sCdtrSchmeId;
            }
            set
            {
                string sText = SepaUtil.Trim(value);
                if (((sText != null) && (sText != "")) && ((sText.Length > 0x23) || !SepaUtil.CheckCharset(sText)))
                {
                    throw new ArgumentException();
                }
                this.m_sCdtrSchmeId = sText;
            }
        }
        
        public string EBICSPartnerId
        {
            get
            {
                return this.m_sEBICSPartnerId;
            }
            set
            {
                this.m_sEBICSPartnerId = value;
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
        
        [Flags]
        public enum Fields
        {
            BIC = 2,
            CdtrId = 8,
            EBICS = 4,
            Name = 1,
            None = 0
        }
    }
}
