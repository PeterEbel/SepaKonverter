
namespace SepaLib
{
    using System;
    using System.Runtime.InteropServices;
    using System.Xml;
    
    public class SepaAmount
    {
        private decimal m_nAmt;
        private SepaCreditDebitIndicator m_nCdtDbtInd;
        private string m_sCcy;
        
        public void Clear()
        {
            this.m_nAmt = 0M;
            this.m_sCcy = null;
            this.m_nCdtDbtInd = SepaCreditDebitIndicator.Undefined;
        }
        
        internal void ReadXml(XmlReader aXmlReader, string sTagName = "Amt")
        {
            this.Clear();
            if (!aXmlReader.IsStartElement(sTagName))
            {
                throw new XmlException();
            }
            this.m_sCcy = aXmlReader.GetAttribute("Ccy");
            this.m_nAmt = XmlConvert.ToDecimal(aXmlReader.ReadElementString());
            if (aXmlReader.IsStartElement("CdtDbtInd"))
            {
                this.CreditDebitIndicatorCode = aXmlReader.ReadElementString();
            }
        }
        
        public override string ToString()
        {
            return this.Value.ToString("N2");
        }
        
        internal void WriteXml(XmlWriter aXmlWriter, string sTagName = "Amt")
        {
            aXmlWriter.WriteStartElement(sTagName);
            if ((this.m_sCcy != null) && (this.m_sCcy != ""))
            {
                aXmlWriter.WriteAttributeString("Ccy", this.m_sCcy);
            }
            aXmlWriter.WriteString(XmlConvert.ToString(this.m_nAmt));
            aXmlWriter.WriteEndElement();
            if (this.m_nCdtDbtInd != SepaCreditDebitIndicator.Undefined)
            {
                aXmlWriter.WriteElementString("CdtDbtInd", this.CreditDebitIndicatorCode);
            }
        }
        
        public decimal Amount
        {
            get
            {
                return this.m_nAmt;
            }
            set
            {
                if ((value < 0M) || (value > 999999999.99M))
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (SepaUtil.DecimalPlaces(value) > 2)
                {
                    throw new ArgumentException();
                }
                this.m_nAmt = value;
            }
        }
        
        public SepaCreditDebitIndicator CreditDebitIndicator
        {
            get
            {
                return this.m_nCdtDbtInd;
            }
            set
            {
                this.m_nCdtDbtInd = value;
            }
        }
        
        public string CreditDebitIndicatorCode
        {
            get
            {
                switch (this.m_nCdtDbtInd)
                {
                    case SepaCreditDebitIndicator.Credit:
                        return "CRDT";
                    
                    case SepaCreditDebitIndicator.Debit:
                        return "DBIT";
                }
                return null;
            }
            set
            {
                string str = value;
                if (str != null)
                {
                    if (!(str == "CRDT"))
                    {
                        if (str != "DBIT")
                        {
                            if (str != "")
                            {
                                throw new ArgumentException();
                            }
                            goto Label_003E;
                        }
                    }
                    else
                    {
                        this.m_nCdtDbtInd = SepaCreditDebitIndicator.Credit;
                        return;
                    }
                    this.m_nCdtDbtInd = SepaCreditDebitIndicator.Debit;
                    return;
                }
            Label_003E:
                this.m_nCdtDbtInd = SepaCreditDebitIndicator.Undefined;
            }
        }
        
        public string Currency
        {
            get
            {
                return this.m_sCcy;
            }
            set
            {
                string s = SepaUtil.Trim(value);
                if (((s != null) && (s != "")) && ((s.Length != 3) || !SepaUtil.IsAlpha(s)))
                {
                    throw new ArgumentException();
                }
                this.m_sCcy = s;
            }
        }
        
        public decimal Value
        {
            get
            {
                if (this.m_nCdtDbtInd != SepaCreditDebitIndicator.Debit)
                {
                    return this.m_nAmt;
                }
                return -this.m_nAmt;
            }
        }
    }
}
