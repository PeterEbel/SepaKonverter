
namespace SepaLib
{
    using System;
    using System.Xml;
    
    public sealed class SepaCreditTransferTransactionInformation : SepaTransactionInformation
    {
        private SepaPartyIdentification m_aCdtr;
        private SepaIBAN m_tCdtrAcctIBAN;
        private SepaBIC m_tCdtrAgtBIC;
        
        public SepaCreditTransferTransactionInformation() : base("CdtTrfTxInf")
        {
            this.m_aCdtr = new SepaPartyIdentification("Cdtr", SepaPartyIdentification.Fields.Name, SepaPartyIdentification.Fields.Name);
        }
        
        public override void Clear()
        {
            base.Clear();
            this.m_aCdtr.Clear();
            this.m_tCdtrAcctIBAN = SepaIBAN.NullIBAN;
            this.m_tCdtrAgtBIC = SepaBIC.NullBIC;
        }
        
        public override bool IsValid()
        {
            return ((base.IsValid() && this.m_aCdtr.IsValid()) && !this.m_tCdtrAcctIBAN.IsNull);
        }
        
        protected override void OnReadXml(XmlReader aXmlReader, SepaMessageInfo aMessageInfo)
        {
            string str;
            if (aMessageInfo.MessageType != SepaMessageType.CreditTransferPaymentInitiation)
            {
                throw new ArgumentException();
            }
            base.ReadPmtIdXml(aXmlReader);
            if (aXmlReader.IsStartElement("PmtTpInf"))
            {
                aXmlReader.Skip();
            }
            aXmlReader.ReadStartElement("Amt");
            base.ReadInstdAmtXml(aXmlReader);
            aXmlReader.ReadEndElement();
            if (aXmlReader.IsStartElement("ChrgBr"))
            {
                aXmlReader.Skip();
            }
            if (aXmlReader.IsStartElement("UltmtDbtr"))
            {
                aXmlReader.Skip();
            }
            if (aXmlReader.IsStartElement("CdtrAgt"))
            {
                this.m_tCdtrAgtBIC = SepaUtil.ReadAgtBIC(aXmlReader, "CdtrAgt");
            }
            this.m_aCdtr.ReadXml(aXmlReader, aMessageInfo);
            this.m_tCdtrAcctIBAN = SepaUtil.ReadAcctXml(aXmlReader, "CdtrAcct", out str);
            if (aXmlReader.IsStartElement("UltmtCdtr"))
            {
                aXmlReader.Skip();
            }
            while (aXmlReader.IsStartElement("InstrForCdtrAgt"))
            {
                aXmlReader.Skip();
            }
            base.ReadPurpXml(aXmlReader);
            base.ReadRmtInfXml(aXmlReader);
        }
        
        protected override void OnWriteXml(XmlWriter aXmlWriter, SepaMessageInfo aMessageInfo)
        {
            if (aMessageInfo.MessageType != SepaMessageType.CreditTransferPaymentInitiation)
            {
                throw new ArgumentException();
            }
            base.WritePmtIdXml(aXmlWriter);
            aXmlWriter.WriteStartElement("Amt");
            base.WriteInstdAmtXml(aXmlWriter);
            aXmlWriter.WriteEndElement();
            if (!this.m_tCdtrAgtBIC.IsNull)
            {
                SepaUtil.WriteAgtBIC(aXmlWriter, "CdtrAgt", this.m_tCdtrAgtBIC);
            }
            this.m_aCdtr.WriteXml(aXmlWriter, aMessageInfo);
            SepaUtil.WriteAcctXml(aXmlWriter, "CdtrAcct", this.m_tCdtrAcctIBAN, null);
            if (aMessageInfo.XmlNamespace != "urn:sepade:xsd:pain.001.001.02")
            {
                base.WritePurpXml(aXmlWriter);
            }
            base.WriteRmtInfXml(aXmlWriter);
        }
        
        public SepaPartyIdentification Creditor
        {
            get
            {
                return this.m_aCdtr;
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
    }
}
