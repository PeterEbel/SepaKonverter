
namespace SepaLib
{
    using System;
    using System.Xml;
    
    public abstract class SepaMessage : SepaObject
    {
        private DateTime m_dtCreDtTm;
        private readonly SepaMessageType m_nMessageType;
        private string m_sMsgId;
        
        protected SepaMessage(string sMessageTagName, SepaMessageType nMessageType) : base(sMessageTagName)
        {
            this.m_nMessageType = nMessageType;
            this.m_sMsgId = SepaUtil.GenerateIdentification("M");
            this.m_dtCreDtTm = DateTime.Now;
        }
        
        public override void Clear()
        {
            this.m_sMsgId = null;
            this.m_dtCreDtTm = DateTime.MinValue;
        }
        
        public override bool IsValid()
        {
            return (((this.m_sMsgId != null) && (this.m_sMsgId != "")) && (this.m_dtCreDtTm > DateTime.MinValue));
        }
        
        protected void ReadGroupHeaderFields(XmlReader aXmlReader, SepaMessageInfo aMessageInfo)
        {
            this.m_sMsgId = aXmlReader.ReadElementString("MsgId");
            this.m_dtCreDtTm = SepaUtil.ToLocalDateTime(aXmlReader.ReadElementString("CreDtTm"));
        }
        
        protected void WriteGroupHeaderFields(XmlWriter aXmlWriter, SepaMessageInfo aMessageInfo)
        {
            string str;
            aXmlWriter.WriteElementString("MsgId", this.m_sMsgId);
            if (((aMessageInfo.XmlNamespace == "APC:STUZZA:payments:ISO:pain:001:001:02:austrian:002") || (aMessageInfo.XmlNamespace == "ISO:pain.001.001.03:APC:STUZZA:payments:001")) || ((aMessageInfo.XmlNamespace == "APC:STUZZA:payments:ISO:pain:008:001:01:austrian:002") || (aMessageInfo.XmlNamespace == "ISO:pain.008.001.02:APC:STUZZA:payments:001")))
            {
                str = XmlConvert.ToString(this.m_dtCreDtTm, "yyyy-MM-ddTHH:mm:ss");
            }
            else if (((aMessageInfo.MessageType == SepaMessageType.CreditTransferPaymentInitiation) || (aMessageInfo.MessageType == SepaMessageType.DirectDebitPaymentInitiation)) || (aMessageInfo.MessageType == SepaMessageType.PaymentStatusReport))
            {
                str = XmlConvert.ToString(this.m_dtCreDtTm, XmlDateTimeSerializationMode.Utc);
            }
            else
            {
                str = XmlConvert.ToString(this.m_dtCreDtTm, XmlDateTimeSerializationMode.Local);
            }
            aXmlWriter.WriteElementString("CreDtTm", str);
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
                    this.m_dtCreDtTm = DateTime.MinValue;
                }
                else
                {
                    this.m_dtCreDtTm = value.ToLocalTime();
                }
            }
        }
        
        public string MessageIdentification
        {
            get
            {
                return this.m_sMsgId;
            }
            set
            {
                string sText = SepaUtil.Trim(value);
                if ((sText != null) && ((sText.Length > 0x23) || !SepaUtil.CheckCharset(sText)))
                {
                    throw new ArgumentException();
                }
                this.m_sMsgId = sText;
            }
        }
        
        public SepaMessageType MessageType
        {
            get
            {
                return this.m_nMessageType;
            }
        }
    }
}
