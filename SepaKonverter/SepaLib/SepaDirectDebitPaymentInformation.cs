
namespace SepaLib
{
    using System;
    using System.Xml;
    
    public sealed class SepaDirectDebitPaymentInformation : SepaPaymentInformation
    {
        private SepaPartyIdentification m_aCdtr = new SepaPartyIdentification("Cdtr", SepaPartyIdentification.Fields.Name, SepaPartyIdentification.Fields.Name);
        private SepaPartyIdentification m_aUltmtCdtr = new SepaPartyIdentification("UltmtCdtr", SepaPartyIdentification.Fields.Name, SepaPartyIdentification.Fields.None);
        private DateTime m_dtReqdColltnDt = DateTime.Today;
        private string m_sCdtrAcctCcy;
        private string m_sCdtrSchmeId;
        private string m_sLclInstrmCd = "CORE";
        private string m_sSeqTp = "OOFF";
        private SepaIBAN m_tCdtrAcctIBAN;
        private SepaBIC m_tCdtrAgtBIC;
        
        public override void Clear()
        {
            base.Clear();
            this.m_sLclInstrmCd = null;
            this.m_sSeqTp = null;
            this.m_dtReqdColltnDt = DateTime.MinValue;
            this.m_aCdtr.Clear();
            this.m_aUltmtCdtr.Clear();
            this.m_tCdtrAcctIBAN = SepaIBAN.NullIBAN;
            this.m_sCdtrAcctCcy = null;
            this.m_tCdtrAgtBIC = SepaBIC.NullBIC;
            this.m_sCdtrSchmeId = null;
        }
        
        public override bool IsValid()
        {
            return ((((base.IsValid() && (this.m_sLclInstrmCd != null)) && ((this.m_sSeqTp != null) && (this.m_dtReqdColltnDt != DateTime.MinValue))) && ((this.m_aCdtr.IsValid() && this.m_aUltmtCdtr.IsValid()) && !this.m_tCdtrAcctIBAN.IsNull)) && (this.m_sCdtrSchmeId != null));
        }
        
        public override SepaTransactionInformation NewTransactionInformation()
        {
            return new SepaDirectDebitTransactionInformation();
        }
        
        protected override void OnReadXml(XmlReader aXmlReader, SepaMessageInfo aMessageInfo)
        {
            if (aMessageInfo.MessageType != SepaMessageType.DirectDebitPaymentInitiation)
            {
                throw new ArgumentException();
            }
            base.ReadPmtInfIdXml(aXmlReader);
            base.ReadPmtMtdXml(aXmlReader);
            if (aMessageInfo.Version >= 2)
            {
                base.ReadBtchBookg(aXmlReader);
                base.ReadNbOfTxs(aXmlReader);
                base.ReadCtrlSum(aXmlReader);
            }
            aXmlReader.ReadStartElement("PmtTpInf");
            aXmlReader.ReadStartElement("SvcLvl");
            aXmlReader.ReadElementString("Cd");
            aXmlReader.ReadEndElement();
            if (aXmlReader.IsStartElement("LclInstrm"))
            {
                aXmlReader.ReadStartElement();
                this.m_sLclInstrmCd = aXmlReader.ReadElementString("Cd");
                aXmlReader.ReadEndElement();
            }
            this.m_sSeqTp = aXmlReader.ReadElementString("SeqTp");
            base.ReadCtgyPurp(aXmlReader, aMessageInfo.Version >= 2);
            aXmlReader.ReadEndElement();
            this.m_dtReqdColltnDt = SepaUtil.ReadDtXml(aXmlReader, "ReqdColltnDt");
            this.m_aCdtr.ReadXml(aXmlReader, aMessageInfo);
            this.m_tCdtrAcctIBAN = SepaUtil.ReadAcctXml(aXmlReader, "CdtrAcct", out this.m_sCdtrAcctCcy);
            this.m_tCdtrAgtBIC = SepaUtil.ReadAgtBIC(aXmlReader, "CdtrAgt");
            if (aXmlReader.IsStartElement("UltmtCdtr"))
            {
                this.m_aUltmtCdtr.ReadXml(aXmlReader, aMessageInfo);
            }
            if (aXmlReader.IsStartElement("ChrgBr"))
            {
                aXmlReader.ReadElementString("ChrgBr");
            }
            if (aXmlReader.IsStartElement("CdtrSchmeId"))
            {
                this.m_sCdtrSchmeId = SepaUtil.ReadCdtrSchmeIdXml(aXmlReader);
            }
            while (aXmlReader.IsStartElement("DrctDbtTxInf"))
            {
                SepaDirectDebitTransactionInformation item = (SepaDirectDebitTransactionInformation) this.NewTransactionInformation();
                item.ReadXml(aXmlReader, aMessageInfo);
                base.TransactionInformations.Add(item);
                string creditorSchemeIdentificationRead = item.CreditorSchemeIdentificationRead;
                if (creditorSchemeIdentificationRead != null)
                {
                    if (this.m_sCdtrSchmeId == null)
                    {
                        this.m_sCdtrSchmeId = creditorSchemeIdentificationRead;
                    }
                    else if (this.m_sCdtrSchmeId != creditorSchemeIdentificationRead)
                    {
                        throw new XmlException();
                    }
                }
            }
        }
        
        protected override void OnWriteXml(XmlWriter aXmlWriter, SepaMessageInfo aMessageInfo)
        {
            if (aMessageInfo.MessageType != SepaMessageType.DirectDebitPaymentInitiation)
            {
                throw new ArgumentException();
            }
            base.WritePmtInfIdXml(aXmlWriter);
            base.WritePmtMtdXml(aXmlWriter);
            if (aMessageInfo.Version >= 2)
            {
                base.WriteBtchBookg(aXmlWriter);
                base.WriteNbOfTxs(aXmlWriter);
                base.WriteCtrlSum(aXmlWriter);
            }
            aXmlWriter.WriteStartElement("PmtTpInf");
            aXmlWriter.WriteStartElement("SvcLvl");
            aXmlWriter.WriteElementString("Cd", "SEPA");
            aXmlWriter.WriteEndElement();
            if (aMessageInfo.XmlNamespace != "urn:sepade:xsd:pain.008.001.01")
            {
                aXmlWriter.WriteStartElement("LclInstrm");
                aXmlWriter.WriteElementString("Cd", this.m_sLclInstrmCd);
                aXmlWriter.WriteEndElement();
            }
            aXmlWriter.WriteElementString("SeqTp", this.m_sSeqTp);
            if (aMessageInfo.XmlNamespace != "urn:sepade:xsd:pain.008.001.01")
            {
                base.WriteCtgyPurp(aXmlWriter, aMessageInfo.Version >= 2);
            }
            aXmlWriter.WriteEndElement();
            SepaUtil.WriteDtXml(aXmlWriter, "ReqdColltnDt", this.m_dtReqdColltnDt);
            this.m_aCdtr.WriteXml(aXmlWriter, aMessageInfo);
            string sCcy = (aMessageInfo.XmlNamespace != "urn:sepade:xsd:pain.008.001.01") ? this.m_sCdtrAcctCcy : null;
            SepaUtil.WriteAcctXml(aXmlWriter, "CdtrAcct", this.m_tCdtrAcctIBAN, sCcy);
            SepaUtil.WriteAgtBIC(aXmlWriter, "CdtrAgt", this.m_tCdtrAgtBIC);
            if (!this.m_aUltmtCdtr.IsEmpty())
            {
                this.m_aUltmtCdtr.WriteXml(aXmlWriter, aMessageInfo);
            }
            aXmlWriter.WriteElementString("ChrgBr", "SLEV");
            if (aMessageInfo.Version >= 2)
            {
                SepaUtil.WriteCdtrSchmeIdXml(aXmlWriter, this.m_sCdtrSchmeId, false);
            }
            base.WriteTxInfs(aXmlWriter, aMessageInfo);
        }
        
        public SepaPartyIdentification Creditor
        {
            get
            {
                return this.m_aCdtr;
            }
        }
        
        public string CreditorAccountCurrency
        {
            get
            {
                return this.m_sCdtrAcctCcy;
            }
            set
            {
                string s = SepaUtil.Trim(value);
                if (((s != null) && (s != "")) && ((s.Length != 3) || !SepaUtil.IsAlpha(s)))
                {
                    throw new ArgumentException();
                }
                this.m_sCdtrAcctCcy = s;
            }
        }
        
        public SepaIBAN CreditorAccountIBAN
        {
            get
            {
                return this.m_tCdtrAcctIBAN;
            }
            set
            {
                this.m_tCdtrAcctIBAN = value;
            }
        }
        
        public SepaBIC CreditorAgentBIC
        {
            get
            {
                return this.m_tCdtrAgtBIC;
            }
            set
            {
                this.m_tCdtrAgtBIC = value;
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
                if (sText == null)
                {
                    throw new ArgumentNullException();
                }
                if ((sText == "") || (sText.Length > 0x23))
                {
                    throw new ArgumentException();
                }
                if (!SepaUtil.CheckCharset(sText))
                {
                    throw new ArgumentException();
                }
                this.m_sCdtrSchmeId = sText;
            }
        }
        
        public string LocalInstrumentCode
        {
            get
            {
                return this.m_sLclInstrmCd;
            }
            set
            {
                string str = SepaUtil.Trim(value);
                if (str == null)
                {
                    throw new ArgumentNullException();
                }
                if ((str.Length == 0) || (str.Length > 0x23))
                {
                    throw new ArgumentException();
                }
                this.m_sLclInstrmCd = str;
            }
        }
        
        public override SepaMessageType MessageType
        {
            get
            {
                return SepaMessageType.DirectDebitPaymentInitiation;
            }
        }
        
        public override string PaymentMethod
        {
            get
            {
                return "DD";
            }
        }
        
        public DateTime RequestedCollectionDate
        {
            get
            {
                return this.m_dtReqdColltnDt;
            }
            set
            {
                if (value == DateTime.MinValue)
                {
                    throw new ArgumentException();
                }
                this.m_dtReqdColltnDt = value;
            }
        }
        
        public string SequenceType
        {
            get
            {
                return this.m_sSeqTp;
            }
            set
            {
                string str = SepaUtil.Trim(value);
                if (str == null)
                {
                    throw new ArgumentNullException();
                }
                if (((str != "FRST") && (str != "RCUR")) && ((str != "OOFF") && (str != "FNAL")))
                {
                    throw new ArgumentException();
                }
                this.m_sSeqTp = str;
            }
        }
        
        public SepaPartyIdentification UltimateCreditor
        {
            get
            {
                return this.m_aUltmtCdtr;
            }
        }
    }
}
