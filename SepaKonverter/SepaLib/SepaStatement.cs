
namespace SepaLib
{
    using System;
    using System.Xml;
    
    public class SepaStatement : SepaObject
    {
        private SepaAccount m_aAcct;
        private DateTime m_dtCreDtTm;
        private DateTime m_dtFrDtTm;
        private DateTime m_dtToDtTm;
        private int m_nElctrncSeqNb;
        private int m_nLglSeqNb;
        private string m_sAddtlInf;
        private string m_sId;
        private SepaBalances m_vBals;
        private SepaStatementEntries m_vNtrys;
        
        public SepaStatement(string sTagName) : base(sTagName)
        {
            this.m_sId = SepaUtil.GenerateIdentification("S");
            this.m_dtCreDtTm = DateTime.Now;
            this.m_aAcct = new SepaAccount("Acct");
            this.m_vBals = new SepaBalances(this);
            this.m_vNtrys = new SepaStatementEntries(this);
        }
        
        private string _GetAddtlInfTagName()
        {
            return ("Addtl" + base.TagName + "Inf");
        }
        
        public override void Clear()
        {
            this.m_sId = null;
            this.m_nElctrncSeqNb = 0;
            this.m_nLglSeqNb = 0;
            this.m_dtCreDtTm = DateTime.MinValue;
            this.m_dtFrDtTm = DateTime.MinValue;
            this.m_dtToDtTm = DateTime.MinValue;
            this.m_aAcct.Clear();
            this.m_vBals.Clear();
            this.m_vNtrys.Clear();
            this.m_sAddtlInf = null;
        }
        
        public override bool IsValid()
        {
            if (((this.m_sId != null) && (this.m_sId != "")) && (this.m_dtCreDtTm != DateTime.MinValue))
            {
                if (((this.m_dtFrDtTm != DateTime.MinValue) || (this.m_dtToDtTm != DateTime.MinValue)) && (((this.m_dtFrDtTm == DateTime.MinValue) || (this.m_dtToDtTm == DateTime.MinValue)) || (this.m_dtFrDtTm > this.m_dtToDtTm)))
                {
                    return false;
                }
                if (!this.m_aAcct.IsValid() || !this.m_vBals.IsValid())
                {
                    return false;
                }
                string tagName = base.TagName;
                if (tagName == null)
                {
                    goto Label_010C;
                }
                if (!(tagName == "Stmt"))
                {
                    if (tagName == "Rpt")
                    {
                        if (this.m_nElctrncSeqNb == 0)
                        {
                            return false;
                        }
                    }
                    else if ((tagName == "Ntfctn") && (this.m_vBals.Count > 0))
                    {
                        return false;
                    }
                    goto Label_010C;
                }
                if ((this.m_nElctrncSeqNb != 0) && (this.m_vBals.Count >= 2))
                {
                    goto Label_010C;
                }
            }
            return false;
        Label_010C:
            if (!this.m_vNtrys.IsValid())
            {
                return false;
            }
            return true;
        }
        
        protected override void OnReadXml(XmlReader aXmlReader, SepaMessageInfo aMessageInfo)
        {
            this.m_sId = aXmlReader.ReadElementString("Id");
            if (aXmlReader.IsStartElement("ElctrncSeqNb"))
            {
                this.m_nElctrncSeqNb = XmlConvert.ToInt32(aXmlReader.ReadElementString());
            }
            if (aXmlReader.IsStartElement("LglSeqNb"))
            {
                this.m_nLglSeqNb = XmlConvert.ToInt32(aXmlReader.ReadElementString());
            }
            this.m_dtCreDtTm = SepaUtil.ToLocalDateTime(aXmlReader.ReadElementString("CreDtTm"));
            if (aXmlReader.IsStartElement("FrToDt"))
            {
                aXmlReader.ReadStartElement();
                this.m_dtFrDtTm = SepaUtil.ToLocalDateTime(aXmlReader.ReadElementString("FrDtTm"));
                this.m_dtToDtTm = SepaUtil.ToLocalDateTime(aXmlReader.ReadElementString("ToDtTm"));
                aXmlReader.ReadEndElement();
            }
            if (aXmlReader.IsStartElement("CpyDplctInd"))
            {
                aXmlReader.Skip();
            }
            if (aXmlReader.IsStartElement("RptgSrc"))
            {
                aXmlReader.Skip();
            }
            this.m_aAcct.ReadXml(aXmlReader, aMessageInfo);
            if (aXmlReader.IsStartElement("RltdAcct"))
            {
                aXmlReader.Skip();
            }
            while (aXmlReader.IsStartElement("Intrst"))
            {
                aXmlReader.Skip();
            }
            while (aXmlReader.IsStartElement("Bal"))
            {
                SepaBalance item = new SepaBalance();
                item.ReadXml(aXmlReader, aMessageInfo);
                this.m_vBals.Add(item);
            }
            while (aXmlReader.IsStartElement("TxsSummry"))
            {
                aXmlReader.Skip();
            }
            while (aXmlReader.IsStartElement("Ntry"))
            {
                SepaStatementEntry entry = new SepaStatementEntry();
                entry.ReadXml(aXmlReader, aMessageInfo);
                this.m_vNtrys.Add(entry);
            }
            if (aXmlReader.IsStartElement(this._GetAddtlInfTagName()))
            {
                this.m_sAddtlInf = aXmlReader.ReadElementString();
            }
        }
        
        protected override void OnWriteXml(XmlWriter aXmlWriter, SepaMessageInfo aMessageInfo)
        {
            aXmlWriter.WriteElementString("Id", this.m_sId);
            if (this.m_nElctrncSeqNb > 0)
            {
                aXmlWriter.WriteElementString("ElctrncSeqNb", XmlConvert.ToString(this.m_nElctrncSeqNb));
            }
            if (this.m_nLglSeqNb > 0)
            {
                aXmlWriter.WriteElementString("LglSeqNb", XmlConvert.ToString(this.m_nLglSeqNb));
            }
            aXmlWriter.WriteElementString("CreDtTm", XmlConvert.ToString(this.m_dtCreDtTm, XmlDateTimeSerializationMode.Local));
            if (this.m_dtFrDtTm != DateTime.MinValue)
            {
                aXmlWriter.WriteStartElement("FrToDt");
                aXmlWriter.WriteElementString("FrDtTm", XmlConvert.ToString(this.m_dtFrDtTm, XmlDateTimeSerializationMode.Local));
                aXmlWriter.WriteElementString("ToDtTm", XmlConvert.ToString(this.m_dtToDtTm, XmlDateTimeSerializationMode.Local));
                aXmlWriter.WriteEndElement();
            }
            this.m_aAcct.WriteXml(aXmlWriter, aMessageInfo);
            this.m_vBals.WriteXml(aXmlWriter, aMessageInfo);
            this.m_vNtrys.WriteXml(aXmlWriter, aMessageInfo);
            if ((this.m_sAddtlInf != null) && (this.m_sAddtlInf != ""))
            {
                aXmlWriter.WriteElementString(this._GetAddtlInfTagName(), this.m_sAddtlInf);
            }
        }
        
        public SepaAccount Account
        {
            get
            {
                return this.m_aAcct;
            }
        }
        
        public string AdditionalInformation
        {
            get
            {
                return this.m_sAddtlInf;
            }
            set
            {
                if ((value != null) && (value.Length > 500))
                {
                    throw new ArgumentException();
                }
                this.m_sAddtlInf = value;
            }
        }
        
        public SepaBalances Balances
        {
            get
            {
                return this.m_vBals;
            }
        }
        
        public DateTime CreationDateTime
        {
            get
            {
                return this.m_dtCreDtTm;
            }
            set
            {
                if (value == DateTime.MinValue)
                {
                    throw new ArgumentException();
                }
                this.m_dtCreDtTm = value.ToLocalTime();
            }
        }
        
        public int ElectronicSequenceNumber
        {
            get
            {
                return this.m_nElctrncSeqNb;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this.m_nElctrncSeqNb = value;
            }
        }
        
        public SepaStatementEntries Entries
        {
            get
            {
                return this.m_vNtrys;
            }
        }
        
        public DateTime FromDateTime
        {
            get
            {
                return this.m_dtFrDtTm;
            }
            set
            {
                if (value == DateTime.MinValue)
                {
                    this.m_dtFrDtTm = DateTime.MinValue;
                }
                else
                {
                    this.m_dtFrDtTm = value.ToLocalTime();
                }
            }
        }
        
        public string Identification
        {
            get
            {
                return this.m_sId;
            }
            set
            {
                string sText = SepaUtil.Trim(value);
                if ((sText != null) && ((sText.Length > 0x23) || !SepaUtil.CheckCharset(sText)))
                {
                    throw new ArgumentException();
                }
                this.m_sId = sText;
            }
        }
        
        public int LegalSequenceNumber
        {
            get
            {
                return this.m_nLglSeqNb;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this.m_nLglSeqNb = value;
            }
        }
        
        public DateTime ToDateTime
        {
            get
            {
                return this.m_dtToDtTm;
            }
            set
            {
                if (value == DateTime.MinValue)
                {
                    this.m_dtToDtTm = DateTime.MinValue;
                }
                else
                {
                    this.m_dtToDtTm = value.ToLocalTime();
                }
            }
        }
    }
}
