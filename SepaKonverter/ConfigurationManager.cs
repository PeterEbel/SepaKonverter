using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SepaKonverter
{
    public static class ConfigurationManager
    {
        public static string GetConnectionString()
        {
            SqlConnectionStringBuilder sConnectionStringBuilder;

            sConnectionStringBuilder = new SqlConnectionStringBuilder();
            sConnectionStringBuilder["Data Source"] = "Peter-PC\\SQLEXPRESS";
#if DEV
            sConnectionStringBuilder["Initial Catalog"] = "SEPA_DEV";
#else
            sConnectionStringBuilder["Initial Catalog"] = "SEPA";
#endif

            sConnectionStringBuilder["Integrated Security"] = true;

            return sConnectionStringBuilder.ConnectionString;
        }

        public static string GetDELLConnectionString()
        {
            SqlConnectionStringBuilder sConnectionStringBuilder;

            sConnectionStringBuilder = new SqlConnectionStringBuilder();
            sConnectionStringBuilder["Data Source"] = "(local)\\SQLEXPRESS";
            sConnectionStringBuilder["Initial Catalog"] = "Mitgliederverwaltung";
            sConnectionStringBuilder["Integrated Security"] = true;

            return sConnectionStringBuilder.ConnectionString;
        }
    }
}
