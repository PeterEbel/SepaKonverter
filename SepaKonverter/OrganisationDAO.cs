using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SepaKonverter
{
    public class OrganisationDAO
    {
        private DatabaseConnector conn;

        public OrganisationDAO()
        {
            conn = new DatabaseConnector();
        }

        public DataTable searchById(string OrganisationsNr)
        {
            string query = "select OrganisationsNr, Name, CreditorSchemeID, BIC, IBAN from dbo.Organisationen where OrganisationsNr = " + "'" + OrganisationsNr + "'" ;
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("OrganisationsNr", SqlDbType.VarChar);
            sqlParameters[0].Value = Convert.ToString(OrganisationsNr);
            return conn.executeSelectQuery(query, sqlParameters);
        }

        public int AnzahlOrganisationen()
        {
            string query = "select distinct count(Organisationen) from dbo.Organisationen";
            return conn.executeCountQuery(query, null);
        }

        public DataTable OrganisationenDetails()
        {
            string query = "select OrganisationsNr, Name, CreditorSchemeID, BIC, IBAN from dbo.Organisationen";
            return conn.executeSelectQuery(query);
        }

        public DataTable Organisationsnummern()
        {
            string query = "select distinct OrganisationsNr from dbo.Organisationen order by OrganisationsNr";
            return conn.executeSelectQuery(query);
        }

        public DataTable DatumLastschriftenById(string OrganisationsNr)
        {
            string query = "select distinct DatumAbrechnung, LastschriftStatus from dbo.Lastschriften where OrganisationsNr = " + "'" + OrganisationsNr + "' " + "order by DatumAbrechnung";
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("OrganisationsNr", SqlDbType.VarChar);
            sqlParameters[0].Value = Convert.ToString(OrganisationsNr);
            return conn.executeSelectQuery(query, sqlParameters);
        }
    }
}
