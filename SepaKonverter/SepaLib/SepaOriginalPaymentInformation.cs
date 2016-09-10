
namespace SepaLib
{
    using System;
    using System.Xml;
    
    public class SepaOriginalPaymentInformation : SepaObject
    {
        private string m_sOrgnlPmtInfId;
        private SepaStatusReasonInformations m_vStsRsnInfs;
        private SepaOriginalTransactionInformations m_vTxInfAndSts;
        
        public SepaOriginalPaymentInformation() : base("OrgnlPmtInfAndSts")
        {
            this.m_vStsRsnInfs = new SepaStatusReasonInformations(this);
            this.m_vTxInfAndSts = new SepaOriginalTransactionInformations(this);
        }
        
        public override void Clear()
        {
            this.m_sOrgnlPmtInfId = null;
            this.m_vStsRsnInfs.Clear();
            this.m_vTxInfAndSts.Clear();
        }
        
        public override bool IsValid()
        {
            return (((this.m_sOrgnlPmtInfId != null) && this.m_vStsRsnInfs.IsValid()) && this.m_vTxInfAndSts.IsValid());
        }
        
        protected override void OnReadXml(XmlReader aXmlReader, SepaMessageInfo aMessageInfo)
        {
            this.m_sOrgnlPmtInfId = aXmlReader.ReadElementString("OrgnlPmtInfId");
            if (aXmlReader.IsStartElement("OrgnlNbOfTxs"))
            {
                aXmlReader.Skip();
            }
            if (aXmlReader.IsStartElement("OrgnlCtrlSum"))
            {
                aXmlReader.Skip();
            }
            if (aXmlReader.IsStartElement("PmtInfSts"))
            {
                if (aXmlReader.ReadElementString() != "RJCT")
                {
                    throw new XmlException("Unsupported PmtInfSts");
                }
                while (aXmlReader.IsStartElement("StsRsnInf"))
                {
                    SepaStatusReasonInformation item = new SepaStatusReasonInformation();
                    item.ReadXml(aXmlReader, aMessageInfo);
                    this.m_vStsRsnInfs.Add(item);
                }
            }
            while (aXmlReader.IsStartElement("TxInfAndSts"))
            {
                SepaOriginalTransactionInformation information2 = new SepaOriginalTransactionInformation();
                information2.ReadXml(aXmlReader, aMessageInfo);
                this.m_vTxInfAndSts.Add(information2);
            }
        }
        
        protected override void OnWriteXml(XmlWriter aXmlWriter, SepaMessageInfo aMessageInfo)
        {
            aXmlWriter.WriteElementString("OrgnlPmtInfId", this.m_sOrgnlPmtInfId);
            if (this.m_vStsRsnInfs.Count > 0)
            {
                aXmlWriter.WriteElementString("PmtInfSts", "RJCT");
                this.m_vStsRsnInfs.WriteXml(aXmlWriter, aMessageInfo);
            }
            this.m_vTxInfAndSts.WriteXml(aXmlWriter, aMessageInfo);
        }
        
        public string OriginalPaymentInformationIdentification
        {
            get
            {
                return this.m_sOrgnlPmtInfId;
            }
            set
            {
                string sText = SepaUtil.Trim(value);
                if ((sText != null) && (sText != ""))
                {
                    if (sText.Length > 0x23)
                    {
                        throw new ArgumentException();
                    }
                    if (!SepaUtil.CheckCharset(sText))
                    {
                        throw new ArgumentException();
                    }
                }
                this.m_sOrgnlPmtInfId = sText;
            }
        }
        
        public SepaOriginalTransactionInformations OriginalTransactionInformations
        {
            get
            {
                return this.m_vTxInfAndSts;
            }
        }
        
        public SepaPaymentStatusReport PaymentStatusReport
        {
            get
            {
                return (SepaPaymentStatusReport) base.Parent;
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
