using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SepaKonverter
{
    public class SepaPaymentInformationDAO
    {
        private DatabaseConnector conn;

        public SepaPaymentInformationDAO()
        {
            conn = new DatabaseConnector();
        }

        public DataTable searchById(string OrganisationsNr, string DatumAbrechnung)
        {
            string query = "select distinct PaymentInformationID, PaymentMethod, BatchBooking, NumberOfTransactionsPaymentInfo, ControlSumPaymentInfo, ServiceLevelCode, LocalInstrumentCode, SequenceType, RequestedCollectionDate, CreditorName, CreditorIdentification, CreditorAgentBIC, CreditorAccountIBAN, ChargeBearer from dbo.Lastschriften where OrganisationsNr = " + "'" + OrganisationsNr + "' and DatumAbrechnung = " + "'"  + DatumAbrechnung + "'";
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("OrganisationsNr", SqlDbType.VarChar);
            sqlParameters[0].Value = Convert.ToString(OrganisationsNr);
            sqlParameters[1] = new SqlParameter("DatumAbrechnung", SqlDbType.VarChar);
            sqlParameters[1].Value = Convert.ToString(DatumAbrechnung);
            return conn.executeSelectQuery(query, sqlParameters);
        }
    }
}
