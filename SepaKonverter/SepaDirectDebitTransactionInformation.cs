
namespace SepaKonverter
{
    using System;
    using System.Xml;
    
    public sealed class SepaDirectDebitTransactionInformation
    {
        private string m_aDbtr;
        private string m_aUltmtDbtr;
        private DateTime m_dtMndtDtOfSgntr;
        private bool m_fSMNDA;
        private string m_sCdtrSchmeIdRead;
        private string m_sMndtId;
        private string m_sOrgnlCdtrNm;
        private string m_sOrgnlCdtrSchmeId;
        private string m_sOrgnlMndtId;
        private SepaIBAN m_tDbtrAcctIBAN;
        private SepaBIC m_tDbtrAgtBIC;
        private SepaIBAN m_tOrgnlDbtrAcctIBAN;
        
        public SepaDirectDebitTransactionInformation() : base("DrctDbtTxInf")
        {
            this.m_aDbtr = new SepaPartyIdentification("Dbtr", SepaPartyIdentification.Fields.Name, SepaPartyIdentification.Fields.Name);
            this.m_aUltmtDbtr = new SepaPartyIdentification("UltmtDbtr", SepaPartyIdentification.Fields.Name, SepaPartyIdentification.Fields.None);
        }
        
        public override void Clear()
        {
            base.Clear();
            this.m_aDbtr.Clear();
            this.m_aUltmtDbtr.Clear();
            this.m_tDbtrAcctIBAN = SepaIBAN.NullIBAN;
            this.m_tDbtrAgtBIC = SepaBIC.NullBIC;
            this.m_sMndtId = null;
            this.m_dtMndtDtOfSgntr = DateTime.MinValue;
            this.m_sOrgnlMndtId = null;
            this.m_sOrgnlCdtrNm = null;
            this.m_sOrgnlCdtrSchmeId = null;
            this.m_tOrgnlDbtrAcctIBAN = SepaIBAN.NullIBAN;
            this.m_fSMNDA = false;
            this.m_sCdtrSchmeIdRead = null;
        }
        
        public override bool IsValid()
        {
            return ((((base.IsValid() && this.m_aDbtr.IsValid()) && (this.m_aUltmtDbtr.IsValid() && !this.m_tDbtrAcctIBAN.IsNull)) && ((this.m_sMndtId != null) && (this.m_sMndtId != ""))) && (this.m_dtMndtDtOfSgntr != DateTime.MinValue));
        }
        
        protected override void OnReadXml(XmlReader aXmlReader, SepaMessageInfo aMessageInfo)
        {
            string str2;
            if (aMessageInfo.MessageType != SepaMessageType.DirectDebitPaymentInitiation)
            {
                throw new ArgumentException();
            }
            base.ReadPmtIdXml(aXmlReader);
            base.ReadInstdAmtXml(aXmlReader);
            if (aXmlReader.IsStartElement("ChrgBr"))
            {
                aXmlReader.Skip();
            }
            aXmlReader.ReadStartElement("DrctDbtTx");
            aXmlReader.ReadStartElement("MndtRltdInf");
            this.m_sMndtId = aXmlReader.ReadElementString("MndtId");
            this.m_dtMndtDtOfSgntr = SepaUtil.ReadDtXml(aXmlReader, "DtOfSgntr");
            bool flag = false;
            if (aXmlReader.IsStartElement("AmdmntInd"))
            {
                flag = XmlConvert.ToBoolean(aXmlReader.ReadElementString());
            }
            if (flag)
            {
                aXmlReader.ReadStartElement("AmdmntInfDtls");
                if (aXmlReader.IsStartElement("OrgnlMndtId"))
                {
                    this.m_sOrgnlMndtId = aXmlReader.ReadElementString();
                }
                if (aXmlReader.IsStartElement("OrgnlCdtrSchmeId"))
                {
                    aXmlReader.ReadStartElement();
                    if (aXmlReader.IsStartElement("Nm"))
                    {
                        this.m_sOrgnlCdtrNm = aXmlReader.ReadElementString();
                    }
                    if (aXmlReader.IsStartElement("Id"))
                    {
                        this.m_sOrgnlCdtrSchmeId = SepaUtil.ReadSepaIdXml(aXmlReader);
                    }
                    aXmlReader.ReadEndElement();
                }
                if (aXmlReader.IsStartElement("OrgnlDbtrAcct"))
                {
                    string str;
                    this.m_tOrgnlDbtrAcctIBAN = SepaUtil.ReadAcctXml(aXmlReader, "OrgnlDbtrAcct", out str);
                }
                if (aXmlReader.IsStartElement("OrgnlDbtrAgt"))
                {
                    aXmlReader.Skip();
                    this.m_fSMNDA = true;
                }
                aXmlReader.ReadEndElement();
            }
            if (aXmlReader.IsStartElement("ElctrncSgntr"))
            {
                aXmlReader.Skip();
            }
            aXmlReader.ReadEndElement();
            if (aXmlReader.IsStartElement("CdtrSchmeId"))
            {
                this.m_sCdtrSchmeIdRead = SepaUtil.ReadCdtrSchmeIdXml(aXmlReader);
            }
            aXmlReader.ReadEndElement();
            if (aXmlReader.IsStartElement("UltmtCdtr"))
            {
                aXmlReader.Skip();
            }
            this.m_tDbtrAgtBIC = SepaUtil.ReadAgtBIC(aXmlReader, "DbtrAgt");
            this.m_aDbtr.ReadXml(aXmlReader, aMessageInfo);
            this.m_tDbtrAcctIBAN = SepaUtil.ReadAcctXml(aXmlReader, "DbtrAcct", out str2);
            if (aXmlReader.IsStartElement("UltmtDbtr"))
            {
                this.m_aUltmtDbtr.ReadXml(aXmlReader, aMessageInfo);
            }
            base.ReadPurpXml(aXmlReader);
            base.ReadRmtInfXml(aXmlReader);
        }
        
        protected override void OnWriteXml(XmlWriter aXmlWriter, SepaMessageInfo aMessageInfo)
        {
            if (aMessageInfo.MessageType != SepaMessageType.DirectDebitPaymentInitiation)
            {
                throw new ArgumentException();
            }
            base.WritePmtIdXml(aXmlWriter);
            base.WriteInstdAmtXml(aXmlWriter);
            aXmlWriter.WriteStartElement("DrctDbtTx");
            aXmlWriter.WriteStartElement("MndtRltdInf");
            aXmlWriter.WriteElementString("MndtId", this.m_sMndtId);
            SepaUtil.WriteDtXml(aXmlWriter, "DtOfSgntr", this.m_dtMndtDtOfSgntr);
            if ((((this.m_sOrgnlMndtId != null) && (this.m_sOrgnlMndtId != "")) || ((this.m_sOrgnlCdtrNm != null) && (this.m_sOrgnlCdtrNm != ""))) || (((this.m_sOrgnlCdtrSchmeId != null) && (this.m_sOrgnlCdtrSchmeId != "")) || (!this.m_tOrgnlDbtrAcctIBAN.IsNull || this.m_fSMNDA)))
            {
                aXmlWriter.WriteElementString("AmdmntInd", "true");
                aXmlWriter.WriteStartElement("AmdmntInfDtls");
                if ((this.m_sOrgnlMndtId != null) && (this.m_sOrgnlMndtId != ""))
                {
                    aXmlWriter.WriteElementString("OrgnlMndtId", this.m_sOrgnlMndtId);
                }
                if (((this.m_sOrgnlCdtrNm != null) && (this.m_sOrgnlCdtrNm != "")) || ((this.m_sOrgnlCdtrSchmeId != null) && (this.m_sOrgnlCdtrSchmeId != "")))
                {
                    aXmlWriter.WriteStartElement("OrgnlCdtrSchmeId");
                    if ((this.m_sOrgnlCdtrNm != null) && (this.m_sOrgnlCdtrNm != ""))
                    {
                        aXmlWriter.WriteElementString("Nm", this.m_sOrgnlCdtrNm);
                    }
                    if ((this.m_sOrgnlCdtrSchmeId != null) && (this.m_sOrgnlCdtrSchmeId != ""))
                    {
                        SepaUtil.WriteSepaIdXml(aXmlWriter, this.m_sOrgnlCdtrSchmeId, aMessageInfo.Version == 1);
                    }
                    aXmlWriter.WriteEndElement();
                }
                if (!this.m_tOrgnlDbtrAcctIBAN.IsNull)
                {
                    SepaUtil.WriteAcctXml(aXmlWriter, "OrgnlDbtrAcct", this.m_tOrgnlDbtrAcctIBAN, null);
                }
                if (this.m_fSMNDA)
                {
                    aXmlWriter.WriteStartElement("OrgnlDbtrAgt");
                    aXmlWriter.WriteStartElement("FinInstnId");
                    aXmlWriter.WriteStartElement("Othr");
                    aXmlWriter.WriteElementString("Id", "SMNDA");
                    aXmlWriter.WriteEndElement();
                    aXmlWriter.WriteEndElement();
                    aXmlWriter.WriteEndElement();
                }
                aXmlWriter.WriteEndElement();
            }
            else
            {
                aXmlWriter.WriteElementString("AmdmntInd", "false");
            }
            aXmlWriter.WriteEndElement();
            if (aMessageInfo.Version == 1)
            {
                SepaDirectDebitPaymentInformation paymentInformation = (SepaDirectDebitPaymentInformation) base.PaymentInformation;
                if (paymentInformation == null)
                {
                    throw new InvalidOperationException();
                }
                string creditorSchemeIdentification = paymentInformation.CreditorSchemeIdentification;
                switch (creditorSchemeIdentification)
                {
                    case null:
                    case "":
                        throw new InvalidOperationException();
                }
                SepaUtil.WriteCdtrSchmeIdXml(aXmlWriter, creditorSchemeIdentification, true);
            }
            aXmlWriter.WriteEndElement();
            SepaUtil.WriteAgtBIC(aXmlWriter, "DbtrAgt", this.m_tDbtrAgtBIC);
            this.m_aDbtr.WriteXml(aXmlWriter, aMessageInfo);
            SepaUtil.WriteAcctXml(aXmlWriter, "DbtrAcct", this.m_tDbtrAcctIBAN, null);
            if (!this.m_aUltmtDbtr.IsEmpty())
            {
                this.m_aUltmtDbtr.WriteXml(aXmlWriter, aMessageInfo);
            }
            if (aMessageInfo.XmlNamespace != "urn:sepade:xsd:pain.008.001.01")
            {
                base.WritePurpXml(aXmlWriter);
            }
            base.WriteRmtInfXml(aXmlWriter);
        }
        
        internal string CreditorSchemeIdentificationRead
        {
            get
            {
                return this.m_sCdtrSchmeIdRead;
            }
        }
        
        public SepaPartyIdentification Debtor
        {
            get
            {
                return this.m_aDbtr;
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
        
        public DateTime MandateDateOfSignature
        {
            get
            {
                return this.m_dtMndtDtOfSgntr;
            }
            set
            {
                if (value == DateTime.MinValue)
                {
                    throw new ArgumentException();
                }
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
        
        public string OriginalCreditorName
        {
            get
            {
                return this.m_sOrgnlCdtrNm;
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
                this.m_sOrgnlCdtrNm = s;
            }
        }
        
        public string OriginalCreditorSchemeIdentification
        {
            get
            {
                return this.m_sOrgnlCdtrSchmeId;
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
                this.m_sOrgnlCdtrSchmeId = sText;
            }
        }
        
        public SepaIBAN OriginalDebtorAccountIBAN
        {
            get
            {
                return this.m_tOrgnlDbtrAcctIBAN;
            }
            set
            {
                this.m_tOrgnlDbtrAcctIBAN = value;
            }
        }
        
        public string OriginalMandateIdentification
        {
            get
            {
                return this.m_sOrgnlMndtId;
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
                this.m_sOrgnlMndtId = sText;
            }
        }
        
        public bool SameMandateNewDebtorAgent
        {
            get
            {
                return this.m_fSMNDA;
            }
            set
            {
                this.m_fSMNDA = value;
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
