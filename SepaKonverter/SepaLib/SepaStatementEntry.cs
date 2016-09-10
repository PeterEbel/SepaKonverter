
namespace SepaLib
{
    using System;
    using System.Xml;
    
    public sealed class SepaStatementEntry : SepaObject
    {
        private SepaAmount m_aAmt;
        private DateTime m_dtBookgDt;
        private DateTime m_dtValDt;
        private bool m_fRvslInd;
        private string m_sAcctSvcrRef;
        private string m_sAddtlInfIndMsgId;
        private string m_sAddtlInfIndMsgNmId;
        private string m_sAddtlNtryInf;
        private string m_sBkTxCd;
        private string m_sBkTxCdIssr;
        private string m_sBtchPmtInfId;
        private string m_sNtryRef;
        private string m_sSts;
        private SepaTransactionDetailsCollection m_vTxDtls;
        
        public SepaStatementEntry() : base("Ntry")
        {
            this.m_aAmt = new SepaAmount();
            this.m_aAmt.Currency = "EUR";
            this.m_sBkTxCdIssr = "ZKA";
            this.m_vTxDtls = new SepaTransactionDetailsCollection(this);
        }
        
        public override void Clear()
        {
            this.m_sNtryRef = null;
            this.m_aAmt.Clear();
            this.m_fRvslInd = false;
            this.m_sSts = null;
            this.m_dtBookgDt = DateTime.MinValue;
            this.m_dtValDt = DateTime.MinValue;
            this.m_sAcctSvcrRef = null;
            this.m_sBkTxCd = null;
            this.m_sBkTxCdIssr = null;
            this.m_sAddtlInfIndMsgNmId = null;
            this.m_sAddtlInfIndMsgId = null;
            this.m_sBtchPmtInfId = null;
            this.m_vTxDtls.Clear();
            this.m_sAddtlNtryInf = null;
        }
        
        public override bool IsValid()
        {
            if ((this.m_aAmt.Currency == null) || (this.m_aAmt.Currency == ""))
            {
                return false;
            }
            if ((this.m_sSts == null) || (this.m_sSts == ""))
            {
                return false;
            }
            if (!this.m_vTxDtls.IsValid())
            {
                return false;
            }
            if ((this.m_vTxDtls.Count > 0) && (this.m_vTxDtls[0].TransactionAmount.Amount != 0M))
            {
                decimal num = 0M;
                string currency = this.m_aAmt.Currency;
                foreach (SepaTransactionDetails details in this.m_vTxDtls)
                {
                    num += details.TransactionAmount.Amount;
                    if (details.TransactionAmount.Currency != currency)
                    {
                        return false;
                    }
                }
                if (this.m_aAmt.Amount != num)
                {
                    return false;
                }
            }
            return true;
        }
        
        protected override void OnReadXml(XmlReader aXmlReader, SepaMessageInfo aMessageInfo)
        {
            if (aXmlReader.IsStartElement("NtryRef"))
            {
                this.m_sNtryRef = aXmlReader.ReadElementString();
            }
            this.m_aAmt.ReadXml(aXmlReader, "Amt");
            if (aXmlReader.IsStartElement("RvslInd"))
            {
                this.m_fRvslInd = XmlConvert.ToBoolean(aXmlReader.ReadElementString());
            }
            this.m_sSts = aXmlReader.ReadElementString("Sts");
            aXmlReader.ReadStartElement("BookgDt");
            this.m_dtBookgDt = SepaUtil.ReadDtOrDtTmXml(aXmlReader);
            aXmlReader.ReadEndElement();
            aXmlReader.ReadStartElement("ValDt");
            this.m_dtValDt = SepaUtil.ReadDtOrDtTmXml(aXmlReader);
            aXmlReader.ReadEndElement();
            if (aXmlReader.IsStartElement("AcctSvcrRef"))
            {
                this.m_sAcctSvcrRef = aXmlReader.ReadElementString();
            }
            while (aXmlReader.IsStartElement("Avlbty"))
            {
                aXmlReader.Skip();
            }
            if (aXmlReader.IsStartElement("BkTxCd"))
            {
                this.m_sBkTxCd = SepaUtil.ReadBkTxCd(aXmlReader, out this.m_sBkTxCdIssr);
            }
            if (aXmlReader.IsStartElement("ComssnWvrInd"))
            {
                aXmlReader.Skip();
            }
            if (aXmlReader.IsStartElement("AddtlInfInd"))
            {
                aXmlReader.ReadStartElement();
                if (aXmlReader.IsStartElement("MsgNmId"))
                {
                    this.m_sAddtlInfIndMsgNmId = aXmlReader.ReadElementString();
                }
                if (aXmlReader.IsStartElement("MsgId"))
                {
                    this.m_sAddtlInfIndMsgId = aXmlReader.ReadElementString();
                }
                aXmlReader.ReadEndElement();
            }
            if (aXmlReader.IsStartElement("AmtDtls"))
            {
                aXmlReader.Skip();
            }
            while (aXmlReader.IsStartElement("Chrgs"))
            {
                aXmlReader.Skip();
            }
            if (aXmlReader.IsStartElement("TechInptChanl"))
            {
                aXmlReader.Skip();
            }
            while (aXmlReader.IsStartElement("Intrst"))
            {
                aXmlReader.Skip();
            }
            if (aXmlReader.IsStartElement("NtryDtls"))
            {
                aXmlReader.ReadStartElement();
                if (aXmlReader.IsStartElement("Btch"))
                {
                    if (aXmlReader.IsEmptyElement)
                    {
                        aXmlReader.Skip();
                    }
                    else
                    {
                        aXmlReader.ReadStartElement();
                        while (aXmlReader.IsStartElement())
                        {
                            if (aXmlReader.LocalName == "PmtInfId")
                            {
                                this.m_sBtchPmtInfId = aXmlReader.ReadElementString();
                            }
                            else
                            {
                                aXmlReader.Skip();
                            }
                        }
                        aXmlReader.ReadEndElement();
                    }
                }
                while (aXmlReader.IsStartElement("TxDtls"))
                {
                    SepaTransactionDetails item = new SepaTransactionDetails();
                    item.ReadXml(aXmlReader, aMessageInfo);
                    this.m_vTxDtls.Add(item);
                }
                aXmlReader.ReadEndElement();
            }
            if (aXmlReader.IsStartElement("AddtlNtryInf"))
            {
                this.m_sAddtlNtryInf = aXmlReader.ReadElementString();
            }
        }
        
        protected override void OnWriteXml(XmlWriter aXmlWriter, SepaMessageInfo aMessageInfo)
        {
            if ((this.m_sNtryRef != null) && (this.m_sNtryRef != ""))
            {
                aXmlWriter.WriteElementString("NtryRef", this.m_sNtryRef);
            }
            this.m_aAmt.WriteXml(aXmlWriter, "Amt");
            if (this.m_fRvslInd)
            {
                aXmlWriter.WriteElementString("RvslInd", XmlConvert.ToString(this.m_fRvslInd));
            }
            aXmlWriter.WriteElementString("Sts", this.m_sSts);
            if (this.m_dtBookgDt > DateTime.MinValue)
            {
                aXmlWriter.WriteStartElement("BookgDt");
                SepaUtil.WriteDtXml(aXmlWriter, "Dt", this.m_dtBookgDt);
                aXmlWriter.WriteEndElement();
            }
            if (this.m_dtValDt > DateTime.MinValue)
            {
                aXmlWriter.WriteStartElement("ValDt");
                SepaUtil.WriteDtXml(aXmlWriter, "Dt", this.m_dtValDt);
                aXmlWriter.WriteEndElement();
            }
            if ((this.m_sAcctSvcrRef != null) && (this.m_sAcctSvcrRef != ""))
            {
                aXmlWriter.WriteElementString("AcctSvcrRef", this.m_sAcctSvcrRef);
            }
            SepaUtil.WriteBkTxCd(aXmlWriter, this.m_sBkTxCd, this.m_sBkTxCdIssr);
            if (((this.m_sAddtlInfIndMsgNmId != null) && (this.m_sAddtlInfIndMsgNmId != "")) || ((this.m_sAddtlInfIndMsgId != null) && (this.m_sAddtlInfIndMsgId != "")))
            {
                aXmlWriter.WriteStartElement("AddtlInfInd");
                if ((this.m_sAddtlInfIndMsgNmId != null) && (this.m_sAddtlInfIndMsgNmId != ""))
                {
                    aXmlWriter.WriteElementString("MsgNmId", this.m_sAddtlInfIndMsgNmId);
                }
                if ((this.m_sAddtlInfIndMsgId != null) && (this.m_sAddtlInfIndMsgId != ""))
                {
                    aXmlWriter.WriteElementString("MsgId", this.m_sAddtlInfIndMsgId);
                }
                aXmlWriter.WriteEndElement();
            }
            if (this.m_vTxDtls.Count > 0)
            {
                aXmlWriter.WriteStartElement("NtryDtls");
                if ((this.m_sBtchPmtInfId != null) && (this.m_sBtchPmtInfId != ""))
                {
                    aXmlWriter.WriteStartElement("Btch");
                    aXmlWriter.WriteElementString("PmtInfId", this.m_sBtchPmtInfId);
                    aXmlWriter.WriteEndElement();
                }
                this.m_vTxDtls.WriteXml(aXmlWriter, aMessageInfo);
                aXmlWriter.WriteEndElement();
            }
            if ((this.m_sAddtlNtryInf != null) && (this.m_sAddtlNtryInf != ""))
            {
                aXmlWriter.WriteElementString("AddtlNtryInf", this.m_sAddtlNtryInf);
            }
        }
        
        public string AccountServicerReference
        {
            get
            {
                return this.m_sAcctSvcrRef;
            }
            set
            {
                string sText = SepaUtil.Trim(value);
                if (((sText != null) && (sText != "")) && ((sText.Length > 0x23) || !SepaUtil.CheckCharset(sText)))
                {
                    throw new ArgumentException();
                }
                this.m_sAcctSvcrRef = sText;
            }
        }
        
        public string AdditionalEntryInformation
        {
            get
            {
                return this.m_sAddtlNtryInf;
            }
            set
            {
                if ((value != null) && (value.Length > 500))
                {
                    throw new ArgumentException();
                }
                this.m_sAddtlNtryInf = value;
            }
        }
        
        public string AdditionalInformationMessageIdentification
        {
            get
            {
                return this.m_sAddtlInfIndMsgId;
            }
            set
            {
                string sText = SepaUtil.Trim(value);
                if (((sText != null) && (sText != "")) && ((sText.Length > 0x23) || !SepaUtil.CheckCharset(sText)))
                {
                    throw new ArgumentException();
                }
                this.m_sAddtlInfIndMsgId = sText;
            }
        }
        
        public string AdditionalInformationMessageNameIdentification
        {
            get
            {
                return this.m_sAddtlInfIndMsgNmId;
            }
            set
            {
                string sText = SepaUtil.Trim(value);
                if (((sText != null) && (sText != "")) && ((sText.Length > 0x23) || !SepaUtil.CheckCharset(sText)))
                {
                    throw new ArgumentException();
                }
                this.m_sAddtlInfIndMsgNmId = sText;
            }
        }
        
        public SepaAmount Amount
        {
            get
            {
                return this.m_aAmt;
            }
        }
        
        public string BankTransactionCode
        {
            get
            {
                return this.m_sBkTxCd;
            }
            set
            {
                this.m_sBkTxCd = value;
            }
        }
        
        public string BankTransactionCodeIssuer
        {
            get
            {
                return this.m_sBkTxCdIssr;
            }
            set
            {
                string sText = SepaUtil.Trim(value);
                if (((sText != null) && (sText != "")) && ((sText.Length > 0x23) || !SepaUtil.CheckCharset(sText)))
                {
                    throw new ArgumentException();
                }
                this.m_sBkTxCdIssr = sText;
            }
        }
        
        public string BatchPaymentInformationIdentification
        {
            get
            {
                return this.m_sBtchPmtInfId;
            }
            set
            {
                string sText = SepaUtil.Trim(value);
                if (((sText != null) && (sText != "")) && ((sText.Length > 0x23) || !SepaUtil.CheckCharset(sText)))
                {
                    throw new ArgumentException();
                }
                this.m_sBtchPmtInfId = sText;
            }
        }
        
        public DateTime BookingDate
        {
            get
            {
                return this.m_dtBookgDt;
            }
            set
            {
                this.m_dtBookgDt = value;
            }
        }
        
        public string EntryReference
        {
            get
            {
                return this.m_sNtryRef;
            }
            set
            {
                string sText = SepaUtil.Trim(value);
                if (((sText != null) && (sText != "")) && ((sText.Length > 0x23) || !SepaUtil.CheckCharset(sText)))
                {
                    throw new ArgumentException();
                }
                this.m_sNtryRef = sText;
            }
        }
        
        public bool ReversalIndicator
        {
            get
            {
                return this.m_fRvslInd;
            }
            set
            {
                this.m_fRvslInd = value;
            }
        }
        
        public string Status
        {
            get
            {
                return this.m_sSts;
            }
            set
            {
                string s = SepaUtil.Trim(value);
                if (((s != null) && (s != "")) && ((s.Length != 4) || !SepaUtil.IsAlpha(s)))
                {
                    throw new ArgumentException();
                }
                this.m_sSts = s;
            }
        }
        
        public SepaTransactionDetailsCollection TransactionDetails
        {
            get
            {
                return this.m_vTxDtls;
            }
        }
        
        public DateTime ValueDate
        {
            get
            {
                return this.m_dtValDt;
            }
            set
            {
                this.m_dtValDt = value;
            }
        }
    }
}
