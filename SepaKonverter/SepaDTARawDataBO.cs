using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SepaKonverter
{
    public class SepaDTARawDataBO
    {
        private SepaDTARawDataDAO dtaRawDataDAO;
        private DataTable dataTable;

        int i = 0;
        private decimal m_nControlsum;
        private int m_nNumberOfTransactions;

        public SepaDTARawDataBO()
        {
            dtaRawDataDAO = new SepaDTARawDataDAO();
            m_nControlsum = 0;
            m_nNumberOfTransactions = 0;
        }

        public SepaDTARawDataVO[] Import()
        {
            dataTable = new DataTable();
            dataTable = dtaRawDataDAO.readDTA();
            SepaDTARawDataVO [] dtaRawDataVO = new SepaDTARawDataVO [dataTable.Rows.Count];

            foreach (DataRow dr in dataTable.Rows)
            {
                dtaRawDataVO[i] = new SepaDTARawDataVO();

                dtaRawDataVO[i].Bankleitzahl = dr[0].ToString();
                dtaRawDataVO[i].Kontonummer = dr[1].ToString();
                dtaRawDataVO[i].InterneKundennummer = dr[2].ToString();
                dtaRawDataVO[i].GegenkontoBankleitzahl = dr[3].ToString();
                dtaRawDataVO[i].GegenkontoKontonummer = dr[4].ToString();
                dtaRawDataVO[i].BetragInEuro = dr[5].ToString();
                dtaRawDataVO[i].Valutadatum = dr[6].ToString();
                dtaRawDataVO[i].Kontoinhaber = dr[7].ToString();
                dtaRawDataVO[i].GegenkontoInhaber = dr[8].ToString();
                dtaRawDataVO[i].Verwendungszweck = dr[9].ToString();

                m_nControlsum += Decimal.Parse(dtaRawDataVO[i].BetragInEuro); 

                i++;
            }

            m_nNumberOfTransactions = i;

            return dtaRawDataVO;
        }

        public int getNumberOfTransactions()
        {
            return dtaRawDataDAO.getNumberOfTransactions();
        }

        public decimal getTotalAmount()
        {
            return dtaRawDataDAO.getTotalAmount();
        }

        public decimal ControlSum
        {
            get
            {
              return m_nControlsum;
            }
        }

        public int NumberOfTransactions
        {
            get
            {
              return m_nNumberOfTransactions;
            }
        }
    }
}
