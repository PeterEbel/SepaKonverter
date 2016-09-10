using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SepaKonverter
{
    public class SepaDTARawDataDAO
    {

        private DatabaseConnector conn;

        public SepaDTARawDataDAO()
        {
            conn = new DatabaseConnector();
        }

        public int getNumberOfTransactions()
        {
            string query = @"SELECT COUNT(*) FROM DELL.Mitgliederverwaltung.dbo.DTA";

            return conn.executeCountQuery(query, null);
        }
        
        public decimal getTotalAmount()
        {
            string query = @"SELECT SUM(CAST(BetragInEuro AS DECIMAL (10,2))) FROM DELL.Mitgliederverwaltung.dbo.DTA";

            return conn.executeSumQuery(query);
        }

        public DataTable readDTA()
        {
            string query = @"SELECT Bankleitzahl,
                                    Kontonummer,
                                    InterneKundennummer,
                                    GegenkontoBankleitzahl,
                                    GegenkontoNr,
                                    BetragInEuro,
                                    Valutadatum,
                                    Kontoinhaber,
                                    GegenkontoInhaber,
                                    Verwendungszweck
                             FROM   DELL.Mitgliederverwaltung.dbo.DTA";

            return conn.executeSelectQuery(query);
        }
    }
}
