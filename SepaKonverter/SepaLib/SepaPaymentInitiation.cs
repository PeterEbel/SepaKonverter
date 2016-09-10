
namespace SepaLib
{
    using System;
    using System.Xml;
    
    public abstract class SepaPaymentInitiation : SepaMessage
    {
        private SepaPartyIdentification m_aInitgPty;
        private decimal m_dCtrlSumRead;
        private SepaTriState m_nBtchBookgRead;
        private int m_nNbOfTxsRead;
        private SepaPaymentInformations m_vPmtInfs;
        
        public SepaPaymentInitiation(string sPainTag, SepaMessageType nMessageType) : base(sPainTag, nMessageType)
        {
            this.m_aInitgPty = new SepaPartyIdentification("InitgPty", SepaPartyIdentification.Fields.BIC | SepaPartyIdentification.Fields.Name, SepaPartyIdentification.Fields.None);
            this.m_vPmtInfs = new SepaPaymentInformations(this);
        }
        
        private SepaTriState _GetBatchBooking()
        {
            SepaTriState batchBooking = SepaTriState.Default;
            if (this.m_vPmtInfs.Count > 0)
            {
                batchBooking = this.m_vPmtInfs[0].BatchBooking;
                foreach (SepaPaymentInformation information in this.m_vPmtInfs)
                {
                    if (information.BatchBooking != batchBooking)
                    {
                        return SepaTriState.Mixed;
                    }
                }
            }
            return batchBooking;
        }
        
        private void _ReadGroupHeader(XmlReader aXmlReader, SepaMessageInfo aMessageInfo)
        {
            aXmlReader.ReadStartElement("GrpHdr");
            base.ReadGroupHeaderFields(aXmlReader, aMessageInfo);
            if (aXmlReader.IsStartElement("BtchBookg"))
            {
                bool flag = XmlConvert.ToBoolean(aXmlReader.ReadElementString());
                this.m_nBtchBookgRead = flag ? SepaTriState.True : SepaTriState.False;
            }
            this.m_nNbOfTxsRead = XmlConvert.ToInt32(aXmlReader.ReadElementString("NbOfTxs"));
            if (aXmlReader.IsStartElement("CtrlSum"))
            {
                this.m_dCtrlSumRead = XmlConvert.ToDecimal(aXmlReader.ReadElementString());
            }
            if (aXmlReader.IsStartElement("Grpg"))
            {
                aXmlReader.ReadElementString();
            }
            this.m_aInitgPty.ReadXml(aXmlReader, aMessageInfo);
            aXmlReader.ReadEndElement();
        }
        
        private void _WriteGroupHeader(XmlWriter aXmlWriter, SepaMessageInfo aMessageInfo)
        {
            aXmlWriter.WriteStartElement("GrpHdr");
            base.WriteGroupHeaderFields(aXmlWriter, aMessageInfo);
            if (this.GroupHeader_HasBtchBookg(aMessageInfo))
            {
                SepaTriState state = this._GetBatchBooking();
                if (state == SepaTriState.Mixed)
                {
                    throw new ApplicationException("Mixed BtchBookg indicators!");
                }
                if (state != SepaTriState.Default)
                {
                    aXmlWriter.WriteElementString("BtchBookg", XmlConvert.ToString(state != SepaTriState.False));
                }
            }
            aXmlWriter.WriteElementString("NbOfTxs", XmlConvert.ToString(this.NumberOfTransactions));
            aXmlWriter.WriteElementString("CtrlSum", XmlConvert.ToString(this.ControlSum));
            string str = this.GroupHeader_GetGrpg(aMessageInfo);
            if (str != null)
            {
                aXmlWriter.WriteElementString("Grpg", str);
            }
            this.m_aInitgPty.WriteXml(aXmlWriter, aMessageInfo);
            aXmlWriter.WriteEndElement();
        }
        
        public override void Clear()
        {
            base.Clear();
            this.m_aInitgPty.Clear();
            this.m_vPmtInfs.Clear();
            this.m_nBtchBookgRead = SepaTriState.Default;
            this.m_nNbOfTxsRead = 0;
            this.m_dCtrlSumRead = 0M;
        }
        
        internal string GetDefaultGroupingCode()
        {
            if (this.m_vPmtInfs.Count == 1)
            {
                return "GRPD";
            }
            foreach (SepaPaymentInformation information in this.m_vPmtInfs)
            {
                if (information.TransactionInformations.Count > 1)
                {
                    return "MIXD";
                }
            }
            return "SNGL";
        }
        
        internal abstract string GroupHeader_GetGrpg(SepaMessageInfo aMessageInfo);
        internal abstract bool GroupHeader_HasBtchBookg(SepaMessageInfo aMessageInfo);
        public override bool IsValid()
        {
            if (!base.IsValid())
            {
                return false;
            }
            if (!this.m_aInitgPty.IsValid() || (this.m_vPmtInfs.Count == 0))
            {
                return false;
            }
            if (!this.m_vPmtInfs.IsValid())
            {
                return false;
            }
            return true;
        }
        
        public abstract SepaPaymentInformation NewPaymentInformation();
        protected override void OnAfterXmlRead(SepaMessageInfo aMessageInfo)
        {
            if ((this.m_dCtrlSumRead != 0M) && (this.m_dCtrlSumRead != this.ControlSum))
            {
                throw new InvalidOperationException("Read CtrlSum differs from calculated CtrlSum.");
            }
            if ((this.m_nNbOfTxsRead != 0) && (this.m_nNbOfTxsRead != this.NumberOfTransactions))
            {
                throw new InvalidOperationException("Read NbOfTxs differs from calculated NbOfTxs.");
            }
        }
        
        protected override void OnReadXml(XmlReader aXmlReader, SepaMessageInfo aMessageInfo)
        {
            if (base.MessageType != aMessageInfo.MessageType)
            {
                throw new ArgumentException();
            }
            this._ReadGroupHeader(aXmlReader, aMessageInfo);
            while (aXmlReader.IsStartElement("PmtInf"))
            {
                SepaPaymentInformation item = this.NewPaymentInformation();
                item.ReadXml(aXmlReader, aMessageInfo);
                if (this.m_nBtchBookgRead != SepaTriState.Default)
                {
                    item.BatchBooking = this.m_nBtchBookgRead;
                }
                this.m_vPmtInfs.Add(item);
            }
        }
        
        protected override void OnWriteXml(XmlWriter aXmlWriter, SepaMessageInfo aMessageInfo)
        {
            if (base.MessageType != aMessageInfo.MessageType)
            {
                throw new ArgumentException();
            }
            this._WriteGroupHeader(aXmlWriter, aMessageInfo);
            this.m_vPmtInfs.WriteXml(aXmlWriter, aMessageInfo);
        }
        
        public decimal ControlSum
        {
            get
            {
                decimal num = 0M;
                foreach (SepaPaymentInformation information in this.PaymentInformations)
                {
                    num += information.TransactionInformations.ControlSum;
                }
                return num;
            }
        }
        
        public SepaPartyIdentification InitiatingParty
        {
            get
            {
                return this.m_aInitgPty;
            }
        }
        
        public int NumberOfTransactions
        {
            get
            {
                int num = 0;
                foreach (SepaPaymentInformation information in this.PaymentInformations)
                {
                    num += information.TransactionInformations.Count;
                }
                return num;
            }
        }
        
        public SepaPaymentInformations PaymentInformations
        {
            get
            {
                return this.m_vPmtInfs;
            }
        }
    }
}
