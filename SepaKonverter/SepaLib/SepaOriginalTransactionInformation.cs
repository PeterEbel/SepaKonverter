
namespace SepaLib
{
    using System;
    using System.Xml;
    
    public class SepaOriginalTransactionInformation : SepaObject
    {
        private SepaAmount m_aAmt;
        private SepaPartyIdentification m_aCdtr;
        private SepaPartyIdentification m_aDbtr;
        private DateTime m_dtMndtDtOfSgntr;
        private DateTime m_dtReqdDt;
        private bool m_fRmtInfStrd;
        private string m_sCdtrAcctCcy;
        private string m_sCdtrSchmeId;
        private string m_sCtgyPurp;
        private string m_sDbtrAcctCcy;
        private string m_sInstrPrty;
        private string m_sLclInstrmCd;
        private string m_sMndtId;
        private string m_sOrgnlEndToEndId;
        private string m_sOrgnlInstrId;
        private string m_sRmtInf;
        private string m_sSeqTp;
        private string m_sStsId;
        private SepaIBAN m_tCdtrAcctIBAN;
        private SepaBIC m_tCdtrAgtBIC;
        private SepaIBAN m_tDbtrAcctIBAN;
        private SepaBIC m_tDbtrAgtBIC;
        private SepaStatusReasonInformations m_vStsRsnInfs;
        
        public SepaOriginalTransactionInformation() : base("TxInfAndSts")
        {
            this.m_vStsRsnInfs = new SepaStatusReasonInformations(this);
            this.m_aAmt = new SepaAmount();
            this.m_aAmt.Currency = "EUR";
            this.m_aDbtr = new SepaPartyIdentification("Dbtr", SepaPartyIdentification.Fields.Name, SepaPartyIdentification.Fields.None);
            this.m_aCdtr = new SepaPartyIdentification("Cdtr", SepaPartyIdentification.Fields.Name, SepaPartyIdentification.Fields.None);
        }
        
        public override void Clear()
        {
            this.m_sStsId = null;
            this.m_sOrgnlInstrId = null;
            this.m_sOrgnlEndToEndId = null;
            this.m_vStsRsnInfs.Clear();
            this.m_aAmt.Clear();
            this.m_dtReqdDt = DateTime.MinValue;
            this.m_sCdtrSchmeId = null;
            this.m_sInstrPrty = null;
            this.m_sLclInstrmCd = null;
            this.m_sSeqTp = null;
            this.m_sCtgyPurp = null;
            this.m_sMndtId = null;
            this.m_dtMndtDtOfSgntr = DateTime.MinValue;
            this.m_sRmtInf = null;
            this.m_fRmtInfStrd = false;
            this.m_aDbtr.Clear();
            this.m_tDbtrAcctIBAN = SepaIBAN.NullIBAN;
            this.m_sDbtrAcctCcy = null;
            this.m_tDbtrAgtBIC = SepaBIC.NullBIC;
            this.m_aCdtr.Clear();
            this.m_tCdtrAcctIBAN = SepaIBAN.NullIBAN;
            this.m_sCdtrAcctCcy = null;
            this.m_tCdtrAgtBIC = SepaBIC.NullBIC;
        }
        
        public override bool IsValid()
        {
            if (!this.m_vStsRsnInfs.IsValid())
            {
                return false;
            }
            if (!this.m_aDbtr.IsValid() || !this.m_aCdtr.IsValid())
            {
                return false;
            }
            SepaMessageType originalMessageType = this.OriginalPaymentInformation.PaymentStatusReport.OriginalMessageType;
            if (originalMessageType == SepaMessageType.CreditTransferPaymentInitiation)
            {
                if (((this.m_sCdtrSchmeId != null) || (this.m_sLclInstrmCd != null)) || (((this.m_sSeqTp != null) || (this.m_sMndtId != null)) || (this.m_dtMndtDtOfSgntr > DateTime.MinValue)))
                {
                    return false;
                }
            }
            else if ((originalMessageType == SepaMessageType.DirectDebitPaymentInitiation) && (this.m_sInstrPrty != null))
            {
                return false;
            }
            return true;
        }
        
        protected override void OnReadXml(XmlReader aXmlReader, SepaMessageInfo aMessageInfo)
        {
            if (aXmlReader.IsStartElement("StsId"))
            {
                this.m_sStsId = aXmlReader.ReadElementString();
            }
            if (aXmlReader.IsStartElement("OrgnlInstrId"))
            {
                this.m_sOrgnlInstrId = aXmlReader.ReadElementString();
            }
            if (aXmlReader.IsStartElement("OrgnlEndToEndId"))
            {
                this.m_sOrgnlEndToEndId = aXmlReader.ReadElementString();
            }
            if (aXmlReader.IsStartElement("TxSts"))
            {
                if (aXmlReader.ReadElementString() != "RJCT")
                {
                    throw new XmlException("Unsupported TxSts");
                }
                while (aXmlReader.IsStartElement("StsRsnInf"))
                {
                    SepaStatusReasonInformation item = new SepaStatusReasonInformation();
                    item.ReadXml(aXmlReader, aMessageInfo);
                    this.m_vStsRsnInfs.Add(item);
                }
            }
            aXmlReader.ReadStartElement("OrgnlTxRef");
            if (aXmlReader.IsStartElement("Amt"))
            {
                aXmlReader.ReadStartElement();
                this.m_aAmt.ReadXml(aXmlReader, "InstdAmt");
                aXmlReader.ReadEndElement();
            }
            if (aXmlReader.IsStartElement("ReqdExctnDt"))
            {
                this.m_dtReqdDt = SepaUtil.ReadDtXml(aXmlReader, "ReqdExctnDt");
            }
            if (aXmlReader.IsStartElement("ReqdColltnDt"))
            {
                if (this.m_dtReqdDt > DateTime.MinValue)
                {
                    throw new XmlException();
                }
                this.m_dtReqdDt = SepaUtil.ReadDtXml(aXmlReader, "ReqdColltnDt");
            }
            if (aXmlReader.IsStartElement("CdtrSchmeId"))
            {
                this.m_sCdtrSchmeId = SepaUtil.ReadCdtrSchmeIdXml(aXmlReader);
            }
            if (aXmlReader.IsStartElement("PmtTpInf"))
            {
                aXmlReader.ReadStartElement();
                if (aXmlReader.IsStartElement("InstrPrty"))
                {
                    this.m_sInstrPrty = aXmlReader.ReadElementString();
                }
                if (aXmlReader.IsStartElement("SvcLvl"))
                {
                    aXmlReader.ReadStartElement();
                    string str2 = aXmlReader.ReadElementString("Cd");
                    aXmlReader.ReadEndElement();
                    if (str2 != "SEPA")
                    {
                        throw new XmlException();
                    }
                }
                if (aXmlReader.IsStartElement("LclInstrm"))
                {
                    aXmlReader.ReadStartElement();
                    this.m_sLclInstrmCd = aXmlReader.ReadElementString("Cd");
                    aXmlReader.ReadEndElement();
                }
                if (aXmlReader.IsStartElement("SeqTp"))
                {
                    this.m_sSeqTp = aXmlReader.ReadElementString();
                }
                if (aXmlReader.IsStartElement("CtgyPurp"))
                {
                    aXmlReader.ReadStartElement();
                    this.m_sCtgyPurp = aXmlReader.ReadElementString("Cd");
                    aXmlReader.ReadEndElement();
                }
                aXmlReader.ReadEndElement();
            }
            if (aXmlReader.IsStartElement("MndtRltdInf"))
            {
                aXmlReader.ReadStartElement();
                if (aXmlReader.IsStartElement("MndtId"))
                {
                    this.m_sMndtId = aXmlReader.ReadElementString();
                }
                if (aXmlReader.IsStartElement("DtOfSgntr"))
                {
                    this.m_dtMndtDtOfSgntr = SepaUtil.ReadDtXml(aXmlReader, "DtOfSgntr");
                }
                if (aXmlReader.IsStartElement("AmdmntInd"))
                {
                    aXmlReader.Skip();
                }
                if (aXmlReader.IsStartElement("AmdmntInfDtls"))
                {
                    aXmlReader.Skip();
                }
                if (aXmlReader.IsStartElement("ElctrncSgntr"))
                {
                    aXmlReader.Skip();
                }
                aXmlReader.ReadEndElement();
            }
            if (aXmlReader.IsStartElement("RmtInf"))
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
            if (aXmlReader.IsStartElement("UltmtDbtr"))
            {
                aXmlReader.Skip();
            }
            if (aXmlReader.IsStartElement("Dbtr"))
            {
                this.m_aDbtr.ReadXml(aXmlReader, aMessageInfo);
            }
            if (aXmlReader.IsStartElement("DbtrAcct"))
            {
                this.m_tDbtrAcctIBAN = SepaUtil.ReadAcctXml(aXmlReader, "DbtrAcct", out this.m_sDbtrAcctCcy);
            }
            if (aXmlReader.IsStartElement("DbtrAgt"))
            {
                this.m_tDbtrAgtBIC = SepaUtil.ReadAgtBIC(aXmlReader, "DbtrAgt");
            }
            if (aXmlReader.IsStartElement("CdtrAgt"))
            {
                this.m_tCdtrAgtBIC = SepaUtil.ReadAgtBIC(aXmlReader, "CdtrAgt");
            }
            if (aXmlReader.IsStartElement("Cdtr"))
            {
                this.m_aCdtr.ReadXml(aXmlReader, aMessageInfo);
            }
            if (aXmlReader.IsStartElement("CdtrAcct"))
            {
                this.m_tCdtrAcctIBAN = SepaUtil.ReadAcctXml(aXmlReader, "CdtrAcct", out this.m_sCdtrAcctCcy);
            }
            if (aXmlReader.IsStartElement("UltmtCdtr"))
            {
                aXmlReader.Skip();
            }
            aXmlReader.ReadEndElement();
        }
        
        protected override void OnWriteXml(XmlWriter aXmlWriter, SepaMessageInfo aMessageInfo)
        {
            SepaMessageType originalMessageType = this.OriginalPaymentInformation.PaymentStatusReport.OriginalMessageType;
            if (this.m_sStsId != null)
            {
                aXmlWriter.WriteElementString("StsId", this.m_sStsId);
            }
            if (this.m_sOrgnlInstrId != null)
            {
                aXmlWriter.WriteElementString("OrgnlInstrId", this.m_sOrgnlInstrId);
            }
            if (this.m_sOrgnlEndToEndId != null)
            {
                aXmlWriter.WriteElementString("OrgnlEndToEndId", this.m_sOrgnlEndToEndId);
            }
            if (this.m_vStsRsnInfs.Count > 0)
            {
                aXmlWriter.WriteElementString("TxSts", "RJCT");
                this.m_vStsRsnInfs.WriteXml(aXmlWriter, aMessageInfo);
            }
            aXmlWriter.WriteStartElement("OrgnlTxRef");
            if (this.m_aAmt.Amount > 0M)
            {
                aXmlWriter.WriteStartElement("Amt");
                this.m_aAmt.WriteXml(aXmlWriter, "InstdAmt");
                aXmlWriter.WriteEndElement();
            }
            if (this.m_dtReqdDt > DateTime.MinValue)
            {
                switch (originalMessageType)
                {
                    case SepaMessageType.CreditTransferPaymentInitiation:
                        SepaUtil.WriteDtXml(aXmlWriter, "ReqdExctnDt", this.m_dtReqdDt);
                        break;
                    
                    case SepaMessageType.DirectDebitPaymentInitiation:
                        SepaUtil.WriteDtXml(aXmlWriter, "ReqdColltnDt", this.m_dtReqdDt);
                        break;
                }
            }
            if (this.m_sCdtrSchmeId != null)
            {
                SepaUtil.WriteCdtrSchmeIdXml(aXmlWriter, this.m_sCdtrSchmeId, false);
            }
            if (((this.m_sInstrPrty != null) || (this.m_sLclInstrmCd != null)) || ((this.m_sSeqTp != null) || (this.m_sCtgyPurp != null)))
            {
                aXmlWriter.WriteStartElement("PmtTpInf");
                if (this.m_sInstrPrty != null)
                {
                    aXmlWriter.WriteElementString("InstrPrty", this.m_sInstrPrty);
                }
                aXmlWriter.WriteStartElement("SvcLvl");
                aXmlWriter.WriteElementString("Cd", "SEPA");
                aXmlWriter.WriteEndElement();
                if (this.m_sLclInstrmCd != null)
                {
                    aXmlWriter.WriteStartElement("LclInstrm");
                    aXmlWriter.WriteElementString("Cd", this.m_sLclInstrmCd);
                    aXmlWriter.WriteEndElement();
                }
                if (this.m_sSeqTp != null)
                {
                    aXmlWriter.WriteElementString("SeqTp", this.m_sSeqTp);
                }
                if (this.m_sCtgyPurp != null)
                {
                    aXmlWriter.WriteStartElement("CtgyPurp");
                    aXmlWriter.WriteElementString("Cd", this.m_sCtgyPurp);
                    aXmlWriter.WriteEndElement();
                }
                aXmlWriter.WriteEndElement();
            }
            if ((this.m_sMndtId != null) || (this.m_dtMndtDtOfSgntr > DateTime.MinValue))
            {
                aXmlWriter.WriteStartElement("MndtRltdInf");
                if (this.m_sMndtId != null)
                {
                    aXmlWriter.WriteElementString("MndtId", this.m_sMndtId);
                }
                if (this.m_dtMndtDtOfSgntr > DateTime.MinValue)
                {
                    SepaUtil.WriteDtXml(aXmlWriter, "DtOfSgntr", this.m_dtMndtDtOfSgntr);
                }
                aXmlWriter.WriteEndElement();
            }
            if (this.m_sRmtInf != null)
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
            if (!this.m_aDbtr.IsEmpty())
            {
                this.m_aDbtr.WriteXml(aXmlWriter, aMessageInfo);
            }
            if (!this.m_tDbtrAcctIBAN.IsNull)
            {
                SepaUtil.WriteAcctXml(aXmlWriter, "DbtrAcct", this.m_tDbtrAcctIBAN, this.m_sDbtrAcctCcy);
            }
            if (!this.m_tDbtrAgtBIC.IsNull)
            {
                SepaUtil.WriteAgtBIC(aXmlWriter, "DbtrAgt", this.m_tDbtrAgtBIC);
            }
            if (!this.m_tCdtrAgtBIC.IsNull)
            {
                SepaUtil.WriteAgtBIC(aXmlWriter, "CdtrAgt", this.m_tCdtrAgtBIC);
            }
            if (!this.m_aCdtr.IsEmpty())
            {
                this.m_aCdtr.WriteXml(aXmlWriter, aMessageInfo);
            }
            if (!this.m_tCdtrAcctIBAN.IsNull)
            {
                SepaUtil.WriteAcctXml(aXmlWriter, "CdtrAcct", this.m_tCdtrAcctIBAN, this.m_sCdtrAcctCcy);
            }
            aXmlWriter.WriteEndElement();
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
        
        public string CategoryPurpose
        {
            get
            {
                return this.m_sCtgyPurp;
            }
            set
            {
                string s = SepaUtil.Trim(value);
                if ((s != null) && (s != ""))
                {
                    if (s.Length != 4)
                    {
                        throw new ArgumentException();
                    }
                    if (!SepaUtil.IsAlpha(s))
                    {
                        throw new ArgumentException();
                    }
                }
                this.m_sCtgyPurp = value;
            }
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
                if (sText != null)
                {
                    if ((sText == "") || (sText.Length > 0x23))
                    {
                        throw new ArgumentException();
                    }
                    if (!SepaUtil.CheckCharset(sText))
                    {
                        throw new ArgumentException();
                    }
                }
                this.m_sCdtrSchmeId = sText;
            }
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
        
        public DateTime MandateDateOfSignature
        {
            get
            {
                return this.m_dtMndtDtOfSgntr;
            }
            set
            {
                this.m_dtMndtDtOfSgntr = value;
            }
        }
        
        public string MandateIdentification
        {
            get
            {
                return this.m_sMndtId;
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
                this.m_sMndtId = sText;
            }
        }
        
        public string OriginalEndToEndId
        {
            get
            {
                return this.m_sOrgnlEndToEndId;
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
                this.m_sOrgnlEndToEndId = sText;
            }
        }
        
        public string OriginalInstructionIdentification
        {
            get
            {
                return this.m_sOrgnlInstrId;
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
                this.m_sOrgnlInstrId = sText;
            }
        }
        
        public SepaOriginalPaymentInformation OriginalPaymentInformation
        {
            get
            {
                return (SepaOriginalPaymentInformation) base.Parent;
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
        
        public DateTime RequestedDate
        {
            get
            {
                return this.m_dtReqdDt;
            }
            set
            {
                this.m_dtReqdDt = value;
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
        
        public string StatusIdentification
        {
            get
            {
                return this.m_sStsId;
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
                this.m_sStsId = sText;
            }
        }
        
        public SepaStatusReasonInformations StatusReasonInformations
        {
            get
            {
                return this.m_vStsRsnInfs;
            }
        }
    }
}
