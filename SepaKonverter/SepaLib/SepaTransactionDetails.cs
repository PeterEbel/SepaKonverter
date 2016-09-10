
namespace SepaLib
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    
    public sealed class SepaTransactionDetails : SepaObject
    {
        private SepaPartyIdentification m_aCdtr;
        private SepaAccount m_aCdtrAcct;
        private SepaFinancialInstitutionIdentification m_aCdtrAgt;
        private SepaPartyIdentification m_aDbtr;
        private SepaAccount m_aDbtrAcct;
        private SepaFinancialInstitutionIdentification m_aDbtrAgt;
        private SepaAmount m_aTxAmt;
        private string m_sBkTxCd;
        private string m_sBkTxCdIssr;
        private string m_sChqNb;
        private string m_sEndToEndId;
        private string m_sMndtId;
        private string m_sPurpCd;
        private string m_sRtrInfRsnCd;
        private List<string> m_vsRmtInf;
        
        public SepaTransactionDetails() : base("TxDtls")
        {
            this.m_aTxAmt = new SepaAmount();
            this.m_sBkTxCdIssr = "ZKA";
            this.m_aDbtr = new SepaPartyIdentification("Dbtr", SepaPartyIdentification.Fields.Name, SepaPartyIdentification.Fields.None);
            this.m_aDbtrAcct = new SepaAccount("DbtrAcct");
            this.m_aCdtr = new SepaPartyIdentification("Cdtr", SepaPartyIdentification.Fields.CdtrId | SepaPartyIdentification.Fields.Name, SepaPartyIdentification.Fields.None);
            this.m_aCdtrAcct = new SepaAccount("CdtrAcct");
            this.m_aDbtrAgt = new SepaFinancialInstitutionIdentification();
            this.m_aCdtrAgt = new SepaFinancialInstitutionIdentification();
        }
        
        private void _AddRmtInf(string sRmtInf)
        {
        }
        
        public override void Clear()
        {
            this.m_sEndToEndId = null;
            this.m_sMndtId = null;
            this.m_sChqNb = null;
            this.m_aTxAmt.Clear();
            this.m_sBkTxCd = null;
            this.m_sBkTxCdIssr = null;
            this.m_aDbtr.Clear();
            this.m_aDbtrAcct.Clear();
            this.m_aCdtr.Clear();
            this.m_aCdtrAcct.Clear();
            this.m_aDbtrAgt.Clear();
            this.m_aCdtrAgt.Clear();
            this.m_sPurpCd = null;
            this.m_vsRmtInf = null;
            this.m_sRtrInfRsnCd = null;
        }
        
        public override bool IsValid()
        {
            return true;
        }
        
        protected override void OnReadXml(XmlReader aXmlReader, SepaMessageInfo aMessageInfo)
        {
            if (aXmlReader.IsStartElement("Refs"))
            {
                if (aXmlReader.IsEmptyElement)
                {
                    aXmlReader.Skip();
                }
                else
                {
                    aXmlReader.ReadStartElement();
                    while (aXmlReader.IsStartElement())
                    {
                        string localName = aXmlReader.LocalName;
                        if (localName == null)
                        {
                            goto Label_0085;
                        }
                        if (!(localName == "EndToEndId"))
                        {
                            if (localName == "MndtId")
                            {
                                goto Label_0069;
                            }
                            if (localName == "ChqNb")
                            {
                                goto Label_0077;
                            }
                            goto Label_0085;
                        }
                        this.m_sEndToEndId = aXmlReader.ReadElementString();
                        continue;
                    Label_0069:
                        this.m_sMndtId = aXmlReader.ReadElementString();
                        continue;
                    Label_0077:
                        this.m_sChqNb = aXmlReader.ReadElementString();
                        continue;
                    Label_0085:
                        aXmlReader.Skip();
                    }
                    aXmlReader.ReadEndElement();
                }
            }
            if (aXmlReader.IsStartElement("AmtDtls"))
            {
                aXmlReader.ReadStartElement();
                while (aXmlReader.IsStartElement())
                {
                    string str2;
                    if (((str2 = aXmlReader.LocalName) != null) && (str2 == "TxAmt"))
                    {
                        aXmlReader.ReadStartElement();
                        this.m_aTxAmt.ReadXml(aXmlReader, "Amt");
                        if (aXmlReader.IsStartElement("CcyXchg"))
                        {
                            aXmlReader.Skip();
                        }
                        aXmlReader.ReadEndElement();
                    }
                    else
                    {
                        aXmlReader.Skip();
                    }
                }
                aXmlReader.ReadEndElement();
            }
            while (aXmlReader.IsStartElement("Avlbty"))
            {
                aXmlReader.Skip();
            }
            if (aXmlReader.IsStartElement("BkTxCd"))
            {
                this.m_sBkTxCd = SepaUtil.ReadBkTxCd(aXmlReader, out this.m_sBkTxCdIssr);
            }
            while (aXmlReader.IsStartElement("Chrgs"))
            {
                aXmlReader.Skip();
            }
            while (aXmlReader.IsStartElement("Intrst"))
            {
                aXmlReader.Skip();
            }
            if (aXmlReader.IsStartElement("RltdPties"))
            {
                aXmlReader.ReadStartElement();
                if (aXmlReader.IsStartElement("InitgPty"))
                {
                    aXmlReader.Skip();
                }
                if (aXmlReader.IsStartElement("Dbtr"))
                {
                    this.m_aDbtr.ReadXml(aXmlReader, aMessageInfo);
                }
                if (aXmlReader.IsStartElement("DbtrAcct"))
                {
                    this.m_aDbtrAcct.ReadXml(aXmlReader, aMessageInfo);
                }
                if (aXmlReader.IsStartElement("UltmtDbtr"))
                {
                    aXmlReader.Skip();
                }
                if (aXmlReader.IsStartElement("Cdtr"))
                {
                    this.m_aCdtr.ReadXml(aXmlReader, aMessageInfo);
                }
                if (aXmlReader.IsStartElement("CdtrAcct"))
                {
                    this.m_aCdtrAcct.ReadXml(aXmlReader, aMessageInfo);
                }
                if (aXmlReader.IsStartElement("UltmtCdtr"))
                {
                    aXmlReader.Skip();
                }
                while (aXmlReader.IsStartElement())
                {
                    aXmlReader.Skip();
                }
                aXmlReader.ReadEndElement();
            }
            if (aXmlReader.IsStartElement("RltdAgts"))
            {
                aXmlReader.ReadStartElement();
                if (aXmlReader.IsStartElement("DbtrAgt"))
                {
                    aXmlReader.ReadStartElement();
                    this.m_aDbtrAgt.ReadXml(aXmlReader, aMessageInfo);
                    aXmlReader.ReadEndElement();
                }
                if (aXmlReader.IsStartElement("CdtrAgt"))
                {
                    aXmlReader.ReadStartElement();
                    this.m_aCdtrAgt.ReadXml(aXmlReader, aMessageInfo);
                    aXmlReader.ReadEndElement();
                }
                while (aXmlReader.IsStartElement())
                {
                    aXmlReader.Skip();
                }
                aXmlReader.ReadEndElement();
            }
            if (aXmlReader.IsStartElement("Purp"))
            {
                aXmlReader.ReadStartElement();
                if (aXmlReader.IsStartElement("Cd"))
                {
                    this.m_sPurpCd = aXmlReader.ReadElementString();
                }
                else
                {
                    aXmlReader.Skip();
                }
                aXmlReader.ReadEndElement();
            }
            while (aXmlReader.IsStartElement("RltdRmtInf"))
            {
                aXmlReader.Skip();
            }
            if (aXmlReader.IsStartElement("RmtInf"))
            {
                this.m_vsRmtInf = new List<string>();
                aXmlReader.ReadStartElement();
                while (aXmlReader.IsStartElement("Ustrd"))
                {
                    this.m_vsRmtInf.Add(aXmlReader.ReadElementString());
                }
                while (aXmlReader.IsStartElement("Strd"))
                {
                    aXmlReader.Skip();
                }
                aXmlReader.ReadEndElement();
            }
            while (aXmlReader.IsStartElement())
            {
                if (aXmlReader.LocalName == "RtrInf")
                {
                    aXmlReader.ReadStartElement();
                    if (aXmlReader.IsStartElement("OrgnlBkTxCd"))
                    {
                        aXmlReader.Skip();
                    }
                    if (aXmlReader.IsStartElement("Orgtr"))
                    {
                        aXmlReader.Skip();
                    }
                    if (aXmlReader.IsStartElement("Rsn"))
                    {
                        aXmlReader.ReadStartElement();
                        if (aXmlReader.IsStartElement("Cd"))
                        {
                            this.m_sRtrInfRsnCd = aXmlReader.ReadElementString();
                        }
                        else
                        {
                            aXmlReader.Skip();
                        }
                        aXmlReader.ReadEndElement();
                    }
                    while (aXmlReader.IsStartElement("AddtlInf"))
                    {
                        aXmlReader.Skip();
                    }
                    aXmlReader.ReadEndElement();
                }
                else
                {
                    aXmlReader.Skip();
                }
            }
        }
        
        protected override void OnWriteXml(XmlWriter aXmlWriter, SepaMessageInfo aMessageInfo)
        {
            if ((((this.m_sEndToEndId != null) && (this.m_sEndToEndId != "")) || ((this.m_sMndtId != null) && (this.m_sMndtId != ""))) || ((this.m_sChqNb != null) && (this.m_sChqNb != "")))
            {
                aXmlWriter.WriteStartElement("Refs");
                if ((this.m_sEndToEndId != null) && (this.m_sEndToEndId != ""))
                {
                    aXmlWriter.WriteElementString("EndToEndId", this.m_sEndToEndId);
                }
                if ((this.m_sMndtId != null) && (this.m_sMndtId != ""))
                {
                    aXmlWriter.WriteElementString("MndtId", this.m_sMndtId);
                }
                if ((this.m_sChqNb != null) && (this.m_sChqNb != ""))
                {
                    aXmlWriter.WriteElementString("ChqNb", this.m_sChqNb);
                }
                aXmlWriter.WriteEndElement();
            }
            if (this.m_aTxAmt.Amount != 0M)
            {
                aXmlWriter.WriteStartElement("AmtDtls");
                aXmlWriter.WriteStartElement("TxAmt");
                this.m_aTxAmt.WriteXml(aXmlWriter, "Amt");
                aXmlWriter.WriteEndElement();
                aXmlWriter.WriteEndElement();
            }
            if ((this.m_sBkTxCd != null) && (this.m_sBkTxCd != ""))
            {
                SepaUtil.WriteBkTxCd(aXmlWriter, this.m_sBkTxCd, this.m_sBkTxCdIssr);
            }
            if ((!this.m_aDbtr.IsEmpty() || !this.m_aDbtrAcct.IsEmpty()) || (!this.m_aCdtr.IsEmpty() || !this.m_aCdtrAcct.IsEmpty()))
            {
                aXmlWriter.WriteStartElement("RltdPties");
                if (!this.m_aDbtr.IsEmpty())
                {
                    this.m_aDbtr.WriteXml(aXmlWriter, aMessageInfo);
                }
                if (!this.m_aDbtrAcct.IsEmpty())
                {
                    this.m_aDbtrAcct.WriteXml(aXmlWriter, aMessageInfo);
                }
                if (!this.m_aCdtr.IsEmpty())
                {
                    this.m_aCdtr.WriteXml(aXmlWriter, aMessageInfo);
                }
                if (!this.m_aCdtrAcct.IsEmpty())
                {
                    this.m_aCdtrAcct.WriteXml(aXmlWriter, aMessageInfo);
                }
                aXmlWriter.WriteEndElement();
            }
            if (!this.m_aDbtrAgt.IsEmpty() || !this.m_aCdtrAgt.IsEmpty())
            {
                aXmlWriter.WriteStartElement("RltdAgts");
                if (!this.m_aDbtrAgt.IsEmpty())
                {
                    aXmlWriter.WriteStartElement("DbtrAgt");
                    this.m_aDbtrAgt.WriteXml(aXmlWriter, aMessageInfo);
                    aXmlWriter.WriteEndElement();
                }
                if (!this.m_aCdtrAgt.IsEmpty())
                {
                    aXmlWriter.WriteStartElement("CdtrAgt");
                    this.m_aCdtrAgt.WriteXml(aXmlWriter, aMessageInfo);
                    aXmlWriter.WriteEndElement();
                }
                aXmlWriter.WriteEndElement();
            }
            if ((this.m_sPurpCd != null) && (this.m_sPurpCd != ""))
            {
                aXmlWriter.WriteStartElement("Purp");
                aXmlWriter.WriteElementString("Cd", this.m_sPurpCd);
                aXmlWriter.WriteEndElement();
            }
            if ((this.m_vsRmtInf != null) && (this.m_vsRmtInf.Count > 0))
            {
                aXmlWriter.WriteStartElement("RmtInf");
                foreach (string str in this.m_vsRmtInf)
                {
                    aXmlWriter.WriteElementString("Ustrd", str);
                }
                aXmlWriter.WriteEndElement();
            }
            if ((this.m_sRtrInfRsnCd != null) && (this.m_sRtrInfRsnCd != ""))
            {
                aXmlWriter.WriteStartElement("RtrInf");
                aXmlWriter.WriteStartElement("Rsn");
                aXmlWriter.WriteElementString("Cd", this.m_sRtrInfRsnCd);
                aXmlWriter.WriteEndElement();
                aXmlWriter.WriteEndElement();
            }
        }
        
        public string BankTransactionCode
        {
            get
            {
                return this.m_sBkTxCd;
            }
            set
            {
                this.m_sBkTxCd = value;
            }
        }
        
        public string BankTransactionCodeIssuer
        {
            get
            {
                return this.m_sBkTxCdIssr;
            }
            set
            {
                string sText = SepaUtil.Trim(value);
                if (((sText != null) && (sText != "")) && ((sText.Length > 0x23) || !SepaUtil.CheckCharset(sText)))
                {
                    throw new ArgumentException();
                }
                this.m_sBkTxCdIssr = sText;
            }
        }
        
        public string ChequeNumber
        {
            get
            {
                return this.m_sChqNb;
            }
            set
            {
                string sText = SepaUtil.Trim(value);
                if (((sText != null) && (sText != "")) && ((sText.Length > 0x23) || !SepaUtil.CheckCharset(sText)))
                {
                    throw new ArgumentException();
                }
                this.m_sChqNb = sText;
            }
        }
        
        public SepaPartyIdentification Creditor
        {
            get
            {
                return this.m_aCdtr;
            }
        }
        
        public SepaAccount CreditorAccount
        {
            get
            {
                return this.m_aCdtrAcct;
            }
        }
        
        public SepaFinancialInstitutionIdentification CreditorAgent
        {
            get
            {
                return this.m_aCdtrAgt;
            }
        }
        
        public SepaPartyIdentification Debtor
        {
            get
            {
                return this.m_aDbtr;
            }
        }
        
        public SepaAccount DebtorAccount
        {
            get
            {
                return this.m_aDbtrAcct;
            }
        }
        
        public SepaFinancialInstitutionIdentification DebtorAgent
        {
            get
            {
                return this.m_aDbtrAgt;
            }
        }
        
        public string EndToEndId
        {
            get
            {
                return this.m_sEndToEndId;
            }
            set
            {
                string sText = SepaUtil.Trim(value);
                if (((sText != null) && (sText != "")) && ((sText.Length > 0x23) || !SepaUtil.CheckCharset(sText)))
                {
                    throw new ArgumentException();
                }
                this.m_sEndToEndId = sText;
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
                if (((sText != null) && (sText != "")) && ((sText.Length > 0x23) || !SepaUtil.CheckCharset(sText)))
                {
                    throw new ArgumentException();
                }
                this.m_sMndtId = sText;
            }
        }
        
        public string PurposeCode
        {
            get
            {
                return this.m_sPurpCd;
            }
            set
            {
                string s = SepaUtil.Trim(value);
                if (((s != null) && (s != "")) && ((s.Length != 4) || !SepaUtil.IsAlpha(s)))
                {
                    throw new ArgumentException();
                }
                this.m_sPurpCd = s;
            }
        }
        
        public string RemittanceInformation
        {
            get
            {
                if (this.m_vsRmtInf == null)
                {
                    return null;
                }
                return string.Join(Environment.NewLine, this.m_vsRmtInf.ToArray());
            }
            set
            {
                string str = SepaUtil.Trim(value);
                if (str == null)
                {
                    this.m_vsRmtInf = null;
                }
                else
                {
                    string[] collection = str.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    for (int i = 0; i < collection.Length; i++)
                    {
                        string s = collection[i];
                        if (s.Length > 140)
                        {
                            throw new ArgumentException();
                        }
                        if (!SepaUtil.IsLatin1(s))
                        {
                            throw new ArgumentException();
                        }
                    }
                    this.m_vsRmtInf = new List<string>(collection);
                }
            }
        }
        
        public string ReturnInformationReasonCode
        {
            get
            {
                return this.m_sRtrInfRsnCd;
            }
            set
            {
                if ((value != null) && ((value.Length != 4) || !SepaUtil.IsAlphaNumeric(value)))
                {
                    throw new ArgumentException();
                }
                this.m_sRtrInfRsnCd = value;
            }
        }
        
        public SepaAmount TransactionAmount
        {
            get
            {
                return this.m_aTxAmt;
            }
        }
    }
}
