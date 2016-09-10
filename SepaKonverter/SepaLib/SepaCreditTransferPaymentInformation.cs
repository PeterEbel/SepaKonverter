
namespace SepaLib
{
    using System;
    using System.Xml;
    
    public sealed class SepaCreditTransferPaymentInformation : SepaPaymentInformation
    {
        private SepaPartyIdentification m_aDbtr = new SepaPartyIdentification("Dbtr", SepaPartyIdentification.Fields.Name, SepaPartyIdentification.Fields.Name);
        private SepaPartyIdentification m_aUltmtDbtr = new SepaPartyIdentification("UltmtDbtr", SepaPartyIdentification.Fields.Name, SepaPartyIdentification.Fields.None);
        private DateTime m_dtReqdExctnDt = DateTime.Today;
        private string m_sDbtrAcctCcy;
        private string m_sInstrPrty;
        private string m_sSvcLvlCd = "SEPA";
        private SepaIBAN m_tDbtrAcctIBAN;
        private SepaBIC m_tDbtrAgtBIC;
        
        public override void Clear()
        {
            base.Clear();
            this.m_sInstrPrty = null;
            this.m_sSvcLvlCd = null;
            this.m_dtReqdExctnDt = DateTime.MinValue;
            this.m_aDbtr.Clear();
            this.m_aUltmtDbtr.Clear();
            this.m_tDbtrAcctIBAN = SepaIBAN.NullIBAN;
            this.m_sDbtrAcctCcy = null;
            this.m_tDbtrAgtBIC = SepaBIC.NullBIC;
        }
        
        public override bool IsValid()
        {
            return ((((base.IsValid() && (this.m_sSvcLvlCd != null)) && ((this.m_sSvcLvlCd != "") && (this.m_dtReqdExctnDt != DateTime.MinValue))) && (this.m_aDbtr.IsValid() && this.m_aUltmtDbtr.IsValid())) && !this.m_tDbtrAcctIBAN.IsNull);
        }
        
        public override SepaTransactionInformation NewTransactionInformation()
        {
            return new SepaCreditTransferTransactionInformation();
        }
        
        protected override void OnReadXml(XmlReader aXmlReader, SepaMessageInfo aMessageInfo)
        {
            if (aMessageInfo.MessageType != SepaMessageType.CreditTransferPaymentInitiation)
            {
                throw new ArgumentException();
            }
            base.ReadPmtInfIdXml(aXmlReader);
            base.ReadPmtMtdXml(aXmlReader);
            if (aMessageInfo.Version >= 3)
            {
                base.ReadBtchBookg(aXmlReader);
                base.ReadNbOfTxs(aXmlReader);
                base.ReadCtrlSum(aXmlReader);
            }
            aXmlReader.ReadStartElement("PmtTpInf");
            if (aXmlReader.IsStartElement("InstrPrty"))
            {
                this.m_sInstrPrty = aXmlReader.ReadElementString();
            }
            aXmlReader.ReadStartElement("SvcLvl");
            this.m_sSvcLvlCd = aXmlReader.ReadElementString("Cd");
            aXmlReader.ReadEndElement();
            base.ReadCtgyPurp(aXmlReader, aMessageInfo.Version >= 3);
            aXmlReader.ReadEndElement();
            this.m_dtReqdExctnDt = SepaUtil.ReadDtXml(aXmlReader, "ReqdExctnDt");
            this.m_aDbtr.ReadXml(aXmlReader, aMessageInfo);
            this.m_tDbtrAcctIBAN = SepaUtil.ReadAcctXml(aXmlReader, "DbtrAcct", out this.m_sDbtrAcctCcy);
            this.m_tDbtrAgtBIC = SepaUtil.ReadAgtBIC(aXmlReader, "DbtrAgt");
            if (aXmlReader.IsStartElement("UltmtDbtr"))
            {
                this.m_aUltmtDbtr.ReadXml(aXmlReader, aMessageInfo);
            }
            if (aXmlReader.IsStartElement("ChrgBr"))
            {
                aXmlReader.ReadElementString();
            }
            while (aXmlReader.IsStartElement("CdtTrfTxInf"))
            {
                SepaCreditTransferTransactionInformation item = (SepaCreditTransferTransactionInformation) this.NewTransactionInformation();
                item.ReadXml(aXmlReader, aMessageInfo);
                base.TransactionInformations.Add(item);
            }
        }
        
        protected override void OnWriteXml(XmlWriter aXmlWriter, SepaMessageInfo aMessageInfo)
        {
            if (aMessageInfo.MessageType != SepaMessageType.CreditTransferPaymentInitiation)
            {
                throw new ArgumentException();
            }
            base.WritePmtInfIdXml(aXmlWriter);
            base.WritePmtMtdXml(aXmlWriter);
            if (aMessageInfo.Version >= 3)
            {
                base.WriteBtchBookg(aXmlWriter);
                base.WriteNbOfTxs(aXmlWriter);
                base.WriteCtrlSum(aXmlWriter);
            }
            aXmlWriter.WriteStartElement("PmtTpInf");
            if (((aMessageInfo.XmlNamespace != "urn:sepade:xsd:pain.001.001.02") && (this.m_sInstrPrty != null)) && (this.m_sInstrPrty != ""))
            {
                aXmlWriter.WriteElementString("InstrPrty", this.m_sInstrPrty);
            }
            aXmlWriter.WriteStartElement("SvcLvl");
            aXmlWriter.WriteElementString("Cd", this.m_sSvcLvlCd);
            aXmlWriter.WriteEndElement();
            if (aMessageInfo.XmlNamespace != "urn:sepade:xsd:pain.001.001.02")
            {
                base.WriteCtgyPurp(aXmlWriter, aMessageInfo.Version >= 3);
            }
            aXmlWriter.WriteEndElement();
            SepaUtil.WriteDtXml(aXmlWriter, "ReqdExctnDt", this.m_dtReqdExctnDt);
            this.m_aDbtr.WriteXml(aXmlWriter, aMessageInfo);
            string sCcy = (aMessageInfo.XmlNamespace != "urn:sepade:xsd:pain.001.001.02") ? this.m_sDbtrAcctCcy : null;
            SepaUtil.WriteAcctXml(aXmlWriter, "DbtrAcct", this.m_tDbtrAcctIBAN, sCcy);
            SepaUtil.WriteAgtBIC(aXmlWriter, "DbtrAgt", this.m_tDbtrAgtBIC);
            if (!this.m_aUltmtDbtr.IsEmpty())
            {
                this.m_aUltmtDbtr.WriteXml(aXmlWriter, aMessageInfo);
            }
            aXmlWriter.WriteElementString("ChrgBr", "SLEV");
            base.WriteTxInfs(aXmlWriter, aMessageInfo);
        }
        
        public SepaPartyIdentification Debtor
        {
            get
            {
                return this.m_aDbtr;
            }
        }
        
        public string DebtorAccountCurrency
        {
            get
            {
                return this.m_sDbtrAcctCcy;
            }
            set
            {
                string s = SepaUtil.Trim(value);
                if (((s != null) && (s != "")) && ((s.Length != 3) || !SepaUtil.IsAlpha(s)))
                {
                    throw new ArgumentException();
                }
                this.m_sDbtrAcctCcy = s;
            }
        }
        
        public SepaIBAN DebtorAccountIBAN
        {
            get
            {
                return this.m_tDbtrAcctIBAN;
            }
            set
            {
                this.m_tDbtrAcctIBAN = value;
            }
        }
        
        public SepaBIC DebtorAgentBIC
        {
            get
            {
                return this.m_tDbtrAgtBIC;
            }
            set
            {
                this.m_tDbtrAgtBIC = value;
            }
        }
        
        public string InstructionPriority
        {
            get
            {
                return this.m_sInstrPrty;
            }
            set
            {
                string s = SepaUtil.Trim(value);
                if (((s != null) && (s != "")) && ((s.Length != 4) || !SepaUtil.IsAlpha(s)))
                {
                    throw new ArgumentException();
                }
                this.m_sInstrPrty = s;
            }
        }
        
        public override SepaMessageType MessageType
        {
            get
            {
                return SepaMessageType.CreditTransferPaymentInitiation;
            }
        }
        
        public override string PaymentMethod
        {
            get
            {
                return "TRF";
            }
        }
        
        public DateTime RequestedExecutionDate
        {
            get
            {
                return this.m_dtReqdExctnDt;
            }
            set
            {
                if (value == DateTime.MinValue)
                {
                    throw new ArgumentException();
                }
                this.m_dtReqdExctnDt = value;
            }
        }
        
        public string ServiceLevelCode
        {
            get
            {
                return this.m_sSvcLvlCd;
            }
            set
            {
                string s = SepaUtil.Trim(value);
                if (((s != null) && (s != "")) && ((s.Length != 4) || !SepaUtil.IsAlpha(s)))
                {
                    throw new ArgumentException();
                }
                this.m_sSvcLvlCd = s;
            }
        }
        
        public SepaPartyIdentification UltimateDebtor
        {
            get
            {
                return this.m_aUltmtDbtr;
            }
        }
    }
}
