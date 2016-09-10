
namespace SepaLib
{
    using System;
    using System.Xml;
    
    public abstract class SepaPaymentInformation : SepaObject
    {
        private decimal m_dCtrlSumRead;
        private SepaTriState m_nBtchBookg;
        private int m_nNbOfTxsRead;
        private string m_sCtgyPurp;
        private string m_sPmtInfId;
        private SepaTransactionInformations m_vTxInfs;
        
        public SepaPaymentInformation() : base("PmtInf")
        {
            this.m_sPmtInfId = SepaUtil.GenerateIdentification("P");
            this.m_vTxInfs = new SepaTransactionInformations(this);
        }
        
        public override void Clear()
        {
            this.m_sPmtInfId = null;
            this.m_nBtchBookg = SepaTriState.Default;
            this.m_nNbOfTxsRead = 0;
            this.m_dCtrlSumRead = 0M;
            this.m_sCtgyPurp = null;
            this.m_vTxInfs.Clear();
        }
        
        public override bool IsValid()
        {
            if (this.m_vTxInfs.Count == 0)
            {
                return false;
            }
            if (!this.m_vTxInfs.IsValid())
            {
                return false;
            }
            return true;
        }
        
        public abstract SepaTransactionInformation NewTransactionInformation();
        protected override void OnAfterXmlRead(SepaMessageInfo aMessageInfo)
        {
            base.OnAfterXmlRead(aMessageInfo);
            if ((this.m_dCtrlSumRead != 0M) && (this.m_dCtrlSumRead != this.ControlSum))
            {
                throw new ApplicationException("Read CtrlSum differs from calculated CtrlSum.");
            }
            if ((this.m_nNbOfTxsRead != 0) && (this.m_nNbOfTxsRead != this.NumberOfTransactions))
            {
                throw new ApplicationException("Read NbOfTxs differs from calculated NbOfTxs.");
            }
        }
        
        internal void ReadBtchBookg(XmlReader aXmlReader)
        {
            if (aXmlReader.IsStartElement("BtchBookg"))
            {
                string s = aXmlReader.ReadElementString();
                switch (s)
                {
                    case null:
                    case "":
                        throw new XmlException("Empty BtchBookg element.");
                }
                bool flag = XmlConvert.ToBoolean(s);
                this.m_nBtchBookg = flag ? SepaTriState.True : SepaTriState.False;
            }
        }
        
        internal void ReadCtgyPurp(XmlReader aXmlReader, bool fWithCd)
        {
            if (aXmlReader.IsStartElement("CtgyPurp"))
            {
                if (fWithCd)
                {
                    aXmlReader.ReadStartElement("CtgyPurp");
                    this.m_sCtgyPurp = aXmlReader.ReadElementString("Cd");
                    aXmlReader.ReadEndElement();
                }
                else
                {
                    this.m_sCtgyPurp = aXmlReader.ReadElementString();
                }
            }
        }
        
        internal void ReadCtrlSum(XmlReader aXmlReader)
        {
            if (aXmlReader.IsStartElement("CtrlSum"))
            {
                string s = aXmlReader.ReadElementString();
                switch (s)
                {
                    case null:
                    case "":
                        throw new XmlException("Empty CtrlSum element.");
                }
                this.m_dCtrlSumRead = XmlConvert.ToDecimal(s);
            }
        }
        
        internal void ReadNbOfTxs(XmlReader aXmlReader)
        {
            if (aXmlReader.IsStartElement("NbOfTxs"))
            {
                string s = aXmlReader.ReadElementString();
                switch (s)
                {
                    case null:
                    case "":
                        throw new XmlException("Empty NbOfTxs element.");
                }
                this.m_nNbOfTxsRead = XmlConvert.ToInt32(s);
            }
        }
        
        internal void ReadPmtInfIdXml(XmlReader aXmlReader)
        {
            if (aXmlReader.IsStartElement("PmtInfId"))
            {
                this.m_sPmtInfId = aXmlReader.ReadElementString();
            }
        }
        
        internal void ReadPmtMtdXml(XmlReader aXmlReader)
        {
            if (aXmlReader.ReadElementString("PmtMtd") != this.PaymentMethod)
            {
                throw new ApplicationException("PmtMtd not compatible with PmtInf.");
            }
        }
        
        internal void WriteBtchBookg(XmlWriter aXmlWriter)
        {
            if (this.m_nBtchBookg != SepaTriState.Default)
            {
                aXmlWriter.WriteElementString("BtchBookg", XmlConvert.ToString(this.m_nBtchBookg != SepaTriState.False));
            }
        }
        
        internal void WriteCtgyPurp(XmlWriter aXmlWriter, bool fWithCd)
        {
            if ((this.m_sCtgyPurp != null) && (this.m_sCtgyPurp != ""))
            {
                if (fWithCd)
                {
                    aXmlWriter.WriteStartElement("CtgyPurp");
                    aXmlWriter.WriteElementString("Cd", this.m_sCtgyPurp);
                    aXmlWriter.WriteEndElement();
                }
                else
                {
                    aXmlWriter.WriteElementString("CtgyPurp", this.m_sCtgyPurp);
                }
            }
        }
        
        internal void WriteCtrlSum(XmlWriter aXmlWriter)
        {
            aXmlWriter.WriteElementString("CtrlSum", XmlConvert.ToString(this.ControlSum));
        }
        
        internal void WriteNbOfTxs(XmlWriter aXmlWriter)
        {
            aXmlWriter.WriteElementString("NbOfTxs", XmlConvert.ToString(this.NumberOfTransactions));
        }
        
        internal void WritePmtInfIdXml(XmlWriter aXmlWriter)
        {
            if ((this.m_sPmtInfId != null) && (this.m_sPmtInfId != ""))
            {
                aXmlWriter.WriteElementString("PmtInfId", this.m_sPmtInfId);
            }
        }
        
        internal void WritePmtMtdXml(XmlWriter aXmlWriter)
        {
            aXmlWriter.WriteElementString("PmtMtd", this.PaymentMethod);
        }
        
        internal void WriteTxInfs(XmlWriter aXmlWriter, SepaMessageInfo aMessageInfo)
        {
            this.m_vTxInfs.WriteXml(aXmlWriter, aMessageInfo);
        }
        
        public SepaTriState BatchBooking
        {
            get
            {
                return this.m_nBtchBookg;
            }
            set
            {
                this.m_nBtchBookg = value;
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
        
        public decimal ControlSum
        {
            get
            {
                return this.m_vTxInfs.ControlSum;
            }
        }
        
        public abstract SepaMessageType MessageType { get; }
        
        public int NumberOfTransactions
        {
            get
            {
                return this.m_vTxInfs.Count;
            }
        }

        public string PaymentInformationIdentification
        {
            get
            {
                return this.m_sPmtInfId;
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
                this.m_sPmtInfId = sText;
            }
        }
        
        public SepaPaymentInitiation PaymentInitiation
        {
            get
            {
                return (SepaPaymentInitiation) base.Parent;
            }
        }
        
        public abstract string PaymentMethod { get; }
        
        public SepaTransactionInformations TransactionInformations
        {
            get
            {
                return this.m_vTxInfs;
            }
        }
    }
}
