
namespace SepaLib
{
    using System;
    using System.Xml;
    
    public sealed class SepaAccount : SepaObject
    {
        private SepaPartyIdentification m_aOwnr;
        private SepaFinancialInstitutionIdentification m_aSvcr;
        private string m_sCcy;
        private string m_sNm;
        private string m_sOthrId;
        private string m_sOthrSchmeNmCd;
        private string m_sTpCd;
        private SepaIBAN m_tIBAN;
        
        public SepaAccount(string sTagName) : base(sTagName)
        {
            this.m_aOwnr = new SepaPartyIdentification("Ownr", SepaPartyIdentification.Fields.Name, SepaPartyIdentification.Fields.None);
            this.m_aSvcr = new SepaFinancialInstitutionIdentification();
        }
        
        public override void Clear()
        {
            this.m_tIBAN = SepaIBAN.NullIBAN;
            this.m_sOthrId = null;
            this.m_sOthrSchmeNmCd = null;
            this.m_sTpCd = null;
            this.m_sCcy = null;
            this.m_sNm = null;
            this.m_aOwnr.Clear();
            this.m_aSvcr.Clear();
        }
        
        public bool IsEmpty()
        {
            return ((((this.m_tIBAN.IsNull && ((this.m_sOthrId == null) || (this.m_sOthrId == ""))) && ((this.m_sTpCd == null) || (this.m_sTpCd == ""))) && ((((this.m_sCcy == null) || (this.m_sCcy == "")) && ((this.m_sNm == null) || (this.m_sNm == ""))) && this.m_aOwnr.IsEmpty())) && this.m_aSvcr.IsEmpty());
        }
        
        public override bool IsValid()
        {
            if (this.m_tIBAN.IsNull && ((this.m_sOthrId == null) || (this.m_sOthrId == "")))
            {
                return false;
            }
            return (this.m_aOwnr.IsValid() && this.m_aSvcr.IsValid());
        }
        
        protected override void OnReadXml(XmlReader aXmlReader, SepaMessageInfo aMessageInfo)
        {
            aXmlReader.ReadStartElement("Id");
            if (aXmlReader.IsStartElement("IBAN"))
            {
                this.m_tIBAN = new SepaIBAN(aXmlReader.ReadElementString());
            }
            else
            {
                aXmlReader.ReadStartElement("Othr");
                this.m_sOthrId = aXmlReader.ReadElementString("Id");
                if (aXmlReader.IsStartElement("SchmeNm"))
                {
                    aXmlReader.ReadStartElement();
                    if (aXmlReader.IsStartElement("Cd"))
                    {
                        this.m_sOthrSchmeNmCd = aXmlReader.ReadElementString();
                    }
                    else
                    {
                        aXmlReader.Skip();
                    }
                    aXmlReader.ReadEndElement();
                }
                if (aXmlReader.IsStartElement("Issr"))
                {
                    aXmlReader.Skip();
                }
                aXmlReader.ReadEndElement();
            }
            aXmlReader.ReadEndElement();
            if (aXmlReader.IsStartElement("Tp"))
            {
                aXmlReader.ReadStartElement();
                if (aXmlReader.IsStartElement("Cd"))
                {
                    this.m_sTpCd = aXmlReader.ReadElementString();
                }
                else if (aXmlReader.IsStartElement())
                {
                    aXmlReader.Skip();
                }
                aXmlReader.ReadEndElement();
            }
            if (aXmlReader.IsStartElement("Ccy"))
            {
                this.m_sCcy = aXmlReader.ReadElementString();
            }
            if (aXmlReader.IsStartElement("Nm"))
            {
                this.m_sNm = aXmlReader.ReadElementString();
            }
            if (aXmlReader.IsStartElement("Ownr"))
            {
                this.m_aOwnr.ReadXml(aXmlReader, aMessageInfo);
            }
            if (aXmlReader.IsStartElement("Svcr"))
            {
                aXmlReader.ReadStartElement();
                this.m_aSvcr.ReadXml(aXmlReader, aMessageInfo);
                aXmlReader.ReadEndElement();
            }
        }
        
        protected override void OnWriteXml(XmlWriter aXmlWriter, SepaMessageInfo aMessageInfo)
        {
            aXmlWriter.WriteStartElement("Id");
            if (!this.m_tIBAN.IsNull)
            {
                aXmlWriter.WriteElementString("IBAN", this.m_tIBAN.IBAN);
            }
            else
            {
                aXmlWriter.WriteStartElement("Othr");
                aXmlWriter.WriteElementString("Id", this.m_sOthrId);
                if ((this.m_sOthrSchmeNmCd != null) && (this.m_sOthrSchmeNmCd != ""))
                {
                    aXmlWriter.WriteStartElement("SchmeNm");
                    aXmlWriter.WriteElementString("Cd", this.m_sOthrSchmeNmCd);
                    aXmlWriter.WriteEndElement();
                }
                aXmlWriter.WriteEndElement();
            }
            aXmlWriter.WriteEndElement();
            if ((this.m_sTpCd != null) && (this.m_sTpCd != ""))
            {
                aXmlWriter.WriteStartElement("Tp");
                aXmlWriter.WriteElementString("Cd", this.m_sTpCd);
                aXmlWriter.WriteEndElement();
            }
            if ((this.m_sCcy != null) && (this.m_sCcy != ""))
            {
                aXmlWriter.WriteElementString("Ccy", this.m_sCcy);
            }
            if ((this.m_sNm != null) && (this.m_sNm != ""))
            {
                aXmlWriter.WriteElementString("Nm", this.m_sNm);
            }
            if (!this.m_aOwnr.IsEmpty())
            {
                this.m_aOwnr.WriteXml(aXmlWriter, aMessageInfo);
            }
            if (!this.m_aSvcr.IsEmpty())
            {
                aXmlWriter.WriteStartElement("Svcr");
                this.m_aSvcr.WriteXml(aXmlWriter, aMessageInfo);
                aXmlWriter.WriteEndElement();
            }
        }
        
        public string Currency
        {
            get
            {
                return this.m_sCcy;
            }
            set
            {
                string s = SepaUtil.Trim(value);
                if (((s != null) && (s != "")) && ((s.Length != 3) || !SepaUtil.IsAlpha(s)))
                {
                    throw new ArgumentException();
                }
                this.m_sCcy = s;
            }
        }
        
        public SepaIBAN IBAN
        {
            get
            {
                return this.m_tIBAN;
            }
            set
            {
                this.m_tIBAN = value;
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
                if (((s != null) && (s != "")) && ((s.Length > 70) || !SepaUtil.IsLatin1(s)))
                {
                    throw new ArgumentException();
                }
                this.m_sNm = s;
            }
        }
        
        public string OtherId
        {
            get
            {
                return this.m_sOthrId;
            }
            set
            {
                string sText = SepaUtil.Trim(value);
                if ((sText != null) && ((sText.Length > 0x22) || !SepaUtil.CheckCharset(sText)))
                {
                    throw new ArgumentException();
                }
                this.m_sOthrId = sText;
            }
        }
        
        public string OtherSchemeNameCode
        {
            get
            {
                return this.m_sOthrSchmeNmCd;
            }
            set
            {
                this.m_sOthrSchmeNmCd = value;
            }
        }
        
        public SepaPartyIdentification Owner
        {
            get
            {
                return this.m_aOwnr;
            }
        }
        
        public SepaFinancialInstitutionIdentification Servicer
        {
            get
            {
                return this.m_aSvcr;
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
