using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SepaKonverter
{
    class PaymentInformation
    {

        private decimal m_dCtrlSumRead;
        private SepaLib.SepaTriState m_nBtchBookg;
        private int m_nNbOfTxsRead;
        private string m_sCtgyPurp;
        private string m_sPmtInfId;

        public PaymentInformation()
        {

        }

        public PaymentInformation(decimal _dCtrlSumRead, SepaLib.SepaTriState _nBtchBookg, int _nNbOfTxsRead, string _sCtgyPurp, string _sPmtInfId)
        {
            m_dCtrlSumRead = _dCtrlSumRead;
            m_nBtchBookg = _nBtchBookg;
            m_nNbOfTxsRead = _nNbOfTxsRead;
            m_sCtgyPurp = _sCtgyPurp;
            m_sPmtInfId = _sPmtInfId;
        }

        public decimal ControlSum
        {
            get
            {
                return m_dCtrlSumRead;
            }
            set
            {
                m_dCtrlSumRead = value;
            }
        }

        public SepaLib.SepaTriState BatchBookings
        {
            get
            {
                return m_nBtchBookg;
            }
            set
            {
                m_nBtchBookg = value;
            }
        }

        public int NumberOfTransactions
        {
            get
            {
                return m_nNbOfTxsRead;
            }
            set
            {
                m_nNbOfTxsRead = value;
            }
        }

        public String CategoryPurpose
        {
            get
            {
                return m_sCtgyPurp;
            }
            set
            {
                m_sCtgyPurp = value;
            }
        }

        public String PaymentInformationID
        {
            get
            {
                return m_sPmtInfId;
            }
            set
            {
                m_sPmtInfId = value;
            }
        }
    }
}
