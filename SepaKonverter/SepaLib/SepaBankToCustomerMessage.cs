
namespace SepaLib
{
    using System;
    using System.Xml;
    
    public class SepaBankToCustomerMessage : SepaMessage
    {
        private SepaPartyIdentification m_aMsgRcpt;
        private SepaStatement m_aStmt;
        private string m_sAddtlInf;
        
        public SepaBankToCustomerMessage(string sMessageTagName, SepaMessageType nMessageType) : base(sMessageTagName, nMessageType)
        {
            this.m_aMsgRcpt = new SepaPartyIdentification("MsgRcpt", SepaPartyIdentification.Fields.EBICS | SepaPartyIdentification.Fields.BIC | SepaPartyIdentification.Fields.Name, SepaPartyIdentification.Fields.None);
            this.m_aStmt = new SepaStatement(this._GetContentTag());
        }
        
        private string _GetContentTag()
        {
            switch (base.MessageType)
            {
                case SepaMessageType.BankToCustomerAccountReport:
                    return "Rpt";
                
                case SepaMessageType.BankToCustomerStatement:
                    return "Stmt";
                
                case SepaMessageType.BankToCustomerDebitCreditNotification:
                    return "Ntfctn";
            }
            return null;
        }
        
        public override void Clear()
        {
            base.Clear();
            this.m_aMsgRcpt.Clear();
            this.m_sAddtlInf = null;
            this.m_aStmt.Clear();
        }
        
        public override bool IsValid()
        {
            return ((base.IsValid() && this.m_aMsgRcpt.IsValid()) && this.m_aStmt.IsValid());
        }
        
        protected override void OnReadXml(XmlReader aXmlReader, SepaMessageInfo aMessageInfo)
        {
            if (base.MessageType != aMessageInfo.MessageType)
            {
                throw new ArgumentException();
            }
            aXmlReader.ReadStartElement("GrpHdr");
            base.ReadGroupHeaderFields(aXmlReader, aMessageInfo);
            if (aXmlReader.IsStartElement("MsgRcpt"))
            {
                this.m_aMsgRcpt.ReadXml(aXmlReader, aMessageInfo);
            }
            if (aXmlReader.IsStartElement("MsgPgntn"))
            {
                aXmlReader.Skip();
            }
            if (aXmlReader.IsStartElement("AddtlInf"))
            {
                this.m_sAddtlInf = aXmlReader.ReadElementString();
            }
            aXmlReader.ReadEndElement();
            this.m_aStmt.ReadXml(aXmlReader, aMessageInfo);
        }
        
        protected override void OnWriteXml(XmlWriter aXmlWriter, SepaMessageInfo aMessageInfo)
        {
            if (base.MessageType != aMessageInfo.MessageType)
            {
                throw new ArgumentException();
            }
            aXmlWriter.WriteStartElement("GrpHdr");
            base.WriteGroupHeaderFields(aXmlWriter, aMessageInfo);
            if (!this.m_aMsgRcpt.IsEmpty())
            {
                this.m_aMsgRcpt.WriteXml(aXmlWriter, aMessageInfo);
            }
            aXmlWriter.WriteStartElement("MsgPgntn");
            aXmlWriter.WriteElementString("PgNb", "1");
            aXmlWriter.WriteElementString("LastPgInd", "true");
            aXmlWriter.WriteEndElement();
            if ((this.m_sAddtlInf != null) && (this.m_sAddtlInf != ""))
            {
                aXmlWriter.WriteElementString("AddtlInf", this.m_sAddtlInf);
            }
            aXmlWriter.WriteEndElement();
            this.m_aStmt.WriteXml(aXmlWriter, aMessageInfo);
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
        
        public SepaPartyIdentification MessageRecipient
        {
            get
            {
                return this.m_aMsgRcpt;
            }
        }
        
        public SepaStatement Statement
        {
            get
            {
                return this.m_aStmt;
            }
        }
    }
}
