using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SepaLib;

namespace SepaKonverter
{
    public class SepaPaymentInformationBO
    {
        private SepaPaymentInformationDAO paymentInformationDAO;
        private string m_sCurrentOrganisation;
        private string m_sCurrentDirectDebit;

        public SepaPaymentInformationBO(string _id, string _cdd)
        {
            m_sCurrentOrganisation = _id;
            m_sCurrentDirectDebit = _cdd;
            paymentInformationDAO = new SepaPaymentInformationDAO();
        }

/*
        public SepaPaymentInformationVO getPaymentInformationById(string _id)
        {
            SepaPaymentInformationVO paymentInformationVO = new SepaPaymentInformationVO();
            dataTable = new DataTable();
            dataTable = paymentInformationDAO.searchById(_id, m_sCurrentDirectDebit);

            foreach (DataRow dr in dataTable.Rows)
            {
                paymentInformationVO.PaymentInformationIdentification = 
                paymentInformationVO.PaymentMethod = "DD";
                paymentInformationVO.BatchBooking = "false";
                paymentInformationVO.ServiceLevelCode = "SEPA";
                paymentInformationVO.LocalInstrumentCode = "COR1";
                paymentInformationVO.SequenceType = dr[4].ToString();
             // paymentInformationVO.RequestedCollectionDate = Convert.ToDateTime(dr[5].ToString());
                paymentInformationVO.CreditorName = dr[6].ToString();
                paymentInformationVO.CreditorSchemeIdentification = dr[7].ToString();
                paymentInformationVO.CreditorAgentBIC = new SepaBIC(dr[8].ToString());
                paymentInformationVO.CreditorAccountIBAN = new SepaIBAN(dr[9].ToString().TrimEnd());
                paymentInformationVO.ChargeBearer = "SLEV";
            }
            paymentInformationVO.NumberOfTransactions = GetNumberOfTransactions();

            return paymentInformationVO;
        }
*/

        private int GetNumberOfTransactions()
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.GetConnectionString());
            SqlCommand mySelectCommand;
            int m_nNumberOfTransactions;

            connection.Open();

            mySelectCommand = new SqlCommand();
            mySelectCommand.Parameters.Add("@ONR", SqlDbType.Char);
            mySelectCommand.Parameters.Add("@CDD", SqlDbType.Char);
            mySelectCommand.Parameters[0].Value = m_sCurrentOrganisation.ToString().TrimEnd();
            mySelectCommand.Parameters[1].Value = m_sCurrentDirectDebit.ToString().TrimEnd();
            mySelectCommand.CommandText = "SELECT count(*) FROM dbo.LASTSCHRIFTEN WHERE ORGANISATIONSNR = @ONR AND DatumAbrechnung = @CDD";
            mySelectCommand.Connection = connection;

            m_nNumberOfTransactions = (int)mySelectCommand.ExecuteScalar();

            connection.Close();

            return (m_nNumberOfTransactions);
        }
    }
}
