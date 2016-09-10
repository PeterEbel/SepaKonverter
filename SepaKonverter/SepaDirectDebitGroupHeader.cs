using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SepaKonverter
{
    public class SepaDirectDebitGroupHeader
    {
        
        private string m_sMsgId;
        private string m_sCreDtTm;
        private int m_nNbOfTxs;
        private decimal m_dCtrlSum;
        private string m_sNm;
        private string m_sId;
        private string m_sIssr;

        public SepaDirectDebitGroupHeader()
        {
            m_sMsgId = null;
            m_sCreDtTm = DateTime.MinValue.ToString();
            m_nNbOfTxs = 0;
            m_dCtrlSum = 0;
            m_sNm = null;
            m_sId = null;
        }

        public String MessageIdentification
        {
            get
            {
                return m_sMsgId;
            }
            set
            {
                m_sMsgId = value;
            }
        }

        public string CreationDateTime
        {
            get
            {
                return m_sCreDtTm;
            }
            set
            {
                m_sCreDtTm = value;
            }
        }

        public int NumberOfTransactions
        {
            get
            {
                return m_nNbOfTxs;
            }
            set
            {
                m_nNbOfTxs = value;
            }
        }

        public decimal ControlSum
        {
            get
            {
                return m_dCtrlSum;
            }
            set
            {
                m_dCtrlSum = value;
            }
        }

        public String InitiatingPartyName
        {
            get
            {
                return m_sNm;
            }
            set
            {
                m_sNm = value;
            }
        }

        public String InitiatingPartyId
        {
            get
            {
                return m_sId;
            }
            set
            {
                m_sId = value;
            }
        }

    }
}
