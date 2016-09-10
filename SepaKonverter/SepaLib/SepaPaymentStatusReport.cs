
namespace SepaLib
{
    using System;
    using System.Xml;
    
    public class SepaPaymentStatusReport : SepaMessage
    {
        private SepaMessageType m_nOriginalMessageType;
        private string m_sOrgnlMsgId;
        private SepaBIC m_tAgtBIC;
        private SepaOriginalPaymentInformations m_vOrgnlPmtInfs;
        
        public SepaPaymentStatusReport(string sMessageTag) : base(sMessageTag, SepaMessageType.PaymentStatusReport)
        {
            this.m_vOrgnlPmtInfs = new SepaOriginalPaymentInformations(this);
        }
        
        public override void Clear()
        {
            base.Clear();
            this.m_nOriginalMessageType = SepaMessageType.Null;
            this.m_sOrgnlMsgId = null;
            this.m_tAgtBIC = SepaBIC.NullBIC;
            this.m_vOrgnlPmtInfs.Clear();
        }
        
        public override bool IsValid()
        {
            if (!base.IsValid())
            {
                return false;
            }
            if (((this.m_nOriginalMessageType == SepaMessageType.Null) || this.m_tAgtBIC.IsNull) || (this.m_sOrgnlMsgId == null))
            {
                return false;
            }
            if (!this.m_vOrgnlPmtInfs.IsValid())
            {
                return false;
            }
            return true;
        }
        
        protected override void OnReadXml(XmlReader aXmlReader, SepaMessageInfo aMessageInfo)
        {
            if (base.MessageType != aMessageInfo.MessageType)
            {
                throw new ArgumentException();
            }
            aXmlReader.ReadStartElement("GrpHdr");
            base.ReadGroupHeaderFields(aXmlReader, aMessageInfo);
            if (aXmlReader.IsStartElement("DbtrAgt"))
            {
                this.m_tAgtBIC = SepaUtil.ReadAgtBIC(aXmlReader, "DbtrAgt");
                this.m_nOriginalMessageType = SepaMessageType.CreditTransferPaymentInitiation;
            }
            else
            {
                this.m_tAgtBIC = SepaUtil.ReadAgtBIC(aXmlReader, "CdtrAgt");
                this.m_nOriginalMessageType = SepaMessageType.DirectDebitPaymentInitiation;
            }
            aXmlReader.ReadEndElement();
            aXmlReader.ReadStartElement("OrgnlGrpInfAndSts");
            this.m_sOrgnlMsgId = aXmlReader.ReadElementString("OrgnlMsgId");
            if (aXmlReader.ReadElementString("OrgnlMsgNmId") != this.OriginalMessageNameIdentification)
            {
                throw new XmlException("Inconsistent OrgnlMsgNmId");
            }
            while (aXmlReader.NodeType != XmlNodeType.EndElement)
            {
                aXmlReader.Skip();
            }
            aXmlReader.ReadEndElement();
            while (aXmlReader.IsStartElement("OrgnlPmtInfAndSts"))
            {
                SepaOriginalPaymentInformation item = new SepaOriginalPaymentInformation();
                item.ReadXml(aXmlReader, aMessageInfo);
                this.m_vOrgnlPmtInfs.Add(item);
            }
        }
        
        protected override void OnWriteXml(XmlWriter aXmlWriter, SepaMessageInfo aMessageInfo)
        {
            if (base.MessageType != aMessageInfo.MessageType)
            {
                throw new ArgumentException();
            }
            aXmlWriter.WriteStartElement("GrpHdr");
            base.WriteGroupHeaderFields(aXmlWriter, aMessageInfo);
            switch (this.m_nOriginalMessageType)
            {
                case SepaMessageType.CreditTransferPaymentInitiation:
                    SepaUtil.WriteAgtBIC(aXmlWriter, "DbtrAgt", this.m_tAgtBIC);
                    break;
                
                case SepaMessageType.DirectDebitPaymentInitiation:
                    SepaUtil.WriteAgtBIC(aXmlWriter, "CdtrAgt", this.m_tAgtBIC);
                    break;
            }
            aXmlWriter.WriteEndElement();
            aXmlWriter.WriteStartElement("OrgnlGrpInfAndSts");
            aXmlWriter.WriteElementString("OrgnlMsgId", this.m_sOrgnlMsgId);
            aXmlWriter.WriteElementString("OrgnlMsgNmId", this.OriginalMessageNameIdentification);
            aXmlWriter.WriteEndElement();
            this.m_vOrgnlPmtInfs.WriteXml(aXmlWriter, aMessageInfo);
        }
        
        public SepaBIC AgentBIC
        {
            get
            {
                return this.m_tAgtBIC;
            }
            set
            {
                this.m_tAgtBIC = value;
            }
        }
        
        public string OriginalMessageIdentification
        {
            get
            {
                return this.m_sOrgnlMsgId;
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
                this.m_sOrgnlMsgId = sText;
            }
        }
        
        public string OriginalMessageNameIdentification
        {
            get
            {
                SepaMessageType nOriginalMessageType = this.m_nOriginalMessageType;
                if (nOriginalMessageType != SepaMessageType.CreditTransferPaymentInitiation)
                {
                    if (nOriginalMessageType == SepaMessageType.DirectDebitPaymentInitiation)
                    {
                        return "pain.008";
                    }
                    return null;
                }
                return "pain.001";
            }
        }
        
        public SepaMessageType OriginalMessageType
        {
            get
            {
                return this.m_nOriginalMessageType;
            }
            set
            {
                if ((value != SepaMessageType.CreditTransferPaymentInitiation) && (value != SepaMessageType.DirectDebitPaymentInitiation))
                {
                    throw new ArgumentException();
                }
            }
        }
        
        public SepaOriginalPaymentInformations OriginalPaymentInformations
        {
            get
            {
                return this.m_vOrgnlPmtInfs;
            }
        }
    }
}
