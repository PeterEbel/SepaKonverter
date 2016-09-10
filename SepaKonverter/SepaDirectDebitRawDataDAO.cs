using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SepaKonverter
{
    public class SepaDirectDebitRawDataDAO
    {

        private DatabaseConnector conn;
        string query;

        public SepaDirectDebitRawDataDAO()
        {
            conn = new DatabaseConnector();
        }

        public int getNumberOfTransactions(string _sOrganisationsNr, string _sSequenceType, string _sDatumAbrechnung = "NULL")
        {

            if (_sDatumAbrechnung == "NULL")
            { 

                query = @"SELECT COUNT(*)
                          FROM   dbo.Lastschriften
                          WHERE  OrganisationsNr = " + "'" + _sOrganisationsNr + "' " + "and SequenceType = " + "'" + _sSequenceType + "' " + "and DatumAbrechnung is NULL";

                SqlParameter[] sqlParameters = new SqlParameter[2];
                sqlParameters[0] = new SqlParameter("OrganisationsNr", SqlDbType.VarChar);
                sqlParameters[0].Value = Convert.ToString(_sOrganisationsNr);
                sqlParameters[1] = new SqlParameter("SequenceType", SqlDbType.VarChar);
                sqlParameters[1].Value = Convert.ToString(_sSequenceType);

                return conn.executeCountQuery(query, sqlParameters);

            }
            else
            {
                
                query = @"SELECT COUNT(*)
                          FROM   dbo.Lastschriften
                          WHERE  OrganisationsNr = " + "'" + _sOrganisationsNr + "' " + "and SequenceType = " + "'" + _sSequenceType + "' " + "and DatumAbrechnung = " + "'" + _sDatumAbrechnung + "' ";
    
                SqlParameter[] sqlParameters = new SqlParameter[3];
                sqlParameters[0] = new SqlParameter("OrganisationsNr", SqlDbType.VarChar);
                sqlParameters[0].Value = Convert.ToString(_sOrganisationsNr);
                sqlParameters[1] = new SqlParameter("SequenceType", SqlDbType.VarChar);
                sqlParameters[1].Value = Convert.ToString(_sSequenceType);
                sqlParameters[2] = new SqlParameter("DatumAbrechnung", SqlDbType.VarChar);
                sqlParameters[2].Value = Convert.ToString(_sDatumAbrechnung);

                return conn.executeCountQuery(query, sqlParameters);

            }
        }
        
        public DataTable searchById(string _sOrganisationsNr, string _sSequenceType, string _sDatumAbrechnung = "NULL")
        {

            if (_sDatumAbrechnung == "NULL")
            {
                query = @"SELECT OrganisationsNr,
                                        MessageID,
                                        CreationDateTime,
                                        NumberOfTransactions,
                                        ControlSum,
                                        InitiatingPartyname,
                                        PaymentInformationID,
                                        PaymentMethod,
                                        BatchBooking,
                                        NumberOfTransactionsPaymentInfo,
                                        ControlSumPaymentInfo,
                                        ServiceLevelCode,
                                        LocalInstrumentCode,
                                        SequenceType,
                                        CategoryPurpose,
                                        RequestedCollectionDate,
                                        CreditorName,
                                        CreditorPostalAddressCountry,
                                        CreditorPostalAddressAddressLine,
                                        CreditorAccountIBAN,
                                        CreditorAccountCurrency,
                                        CreditorAgentBIC,
                                        ChargeBearer,
                                        CreditorIdentification,
                                        InstructionID,
                                        EndToEndID,
                                        MandateID,
                                        DateOfSignature,
                                        AmendmentIndicator,
                                        OriginalMandateID,
                                        OriginalCreditorName,
                                        OriginalCreditorIdentification,
                                        OriginalDebtorAccountIBAN,
                                        OriginalDebtorAgentBIC,
                                        CreditorIdentificationTransactionInfo,
                                        DebtorName as 'Teilnehmer',
                                        DebtorAgentBIC as 'BIC',
                                        RTRIM(DebtorAccountIBAN) as 'IBAN',
                                        InstructedAmount as 'Betrag',
                                        DebtorPostalAddressCountry,
                                        DebtorPostalAddressAddressLine,
                                        PurposeCode,
                                        UnstructuredRemittanceInfo as 'Verwendungszweck',
                                        LastschriftStatus,
                                        DatumImport,
                                        DatumAbrechnung
                                 FROM   dbo.Lastschriften
                                 WHERE  OrganisationsNr = " + "'" + _sOrganisationsNr + "' " + "and SequenceType = " + "'" + _sSequenceType + "' " + "and DatumAbrechnung is NULL " + "order by DebtorName";

                SqlParameter[] sqlParameters = new SqlParameter[2];
                sqlParameters[0] = new SqlParameter("OrganisationsNr", SqlDbType.VarChar);
                sqlParameters[0].Value = Convert.ToString(_sOrganisationsNr);
                sqlParameters[1] = new SqlParameter("SequenceType", SqlDbType.VarChar);
                sqlParameters[1].Value = Convert.ToString(_sSequenceType);

                return conn.executeSelectQuery(query, sqlParameters);

            }
            else
            {
                query = @"SELECT OrganisationsNr,
                                        MessageID,
                                        CreationDateTime,
                                        NumberOfTransactions,
                                        ControlSum,
                                        InitiatingPartyname,
                                        PaymentInformationID,
                                        PaymentMethod,
                                        BatchBooking,
                                        NumberOfTransactionsPaymentInfo,
                                        ControlSumPaymentInfo,
                                        ServiceLevelCode,
                                        LocalInstrumentCode,
                                        SequenceType,
                                        CategoryPurpose,
                                        RequestedCollectionDate,
                                        CreditorName,
                                        CreditorPostalAddressCountry,
                                        CreditorPostalAddressAddressLine,
                                        CreditorAccountIBAN,
                                        CreditorAccountCurrency,
                                        CreditorAgentBIC,
                                        ChargeBearer,
                                        CreditorIdentification,
                                        InstructionID,
                                        EndToEndID,
                                        MandateID,
                                        DateOfSignature,
                                        AmendmentIndicator,
                                        OriginalMandateID,
                                        OriginalCreditorName,
                                        OriginalCreditorIdentification,
                                        OriginalDebtorAccountIBAN,
                                        OriginalDebtorAgentBIC,
                                        CreditorIdentificationTransactionInfo,
                                        DebtorName as 'Teilnehmer',
                                        DebtorAgentBIC as 'BIC',
                                        RTRIM(DebtorAccountIBAN) as 'IBAN',
                                        InstructedAmount as 'Betrag',
                                        DebtorPostalAddressCountry,
                                        DebtorPostalAddressAddressLine,
                                        PurposeCode,
                                        UnstructuredRemittanceInfo as 'Verwendungszweck',
                                        LastschriftStatus,
                                        DatumImport,
                                        DatumAbrechnung
                                 FROM   dbo.Lastschriften
                                 WHERE  OrganisationsNr = " + "'" + _sOrganisationsNr + "' " + "and SequenceType = " + "'" + _sSequenceType + "' " +  "and DatumAbrechnung = " + "'" + _sDatumAbrechnung + "' " + "order by DebtorName";

                SqlParameter[] sqlParameters = new SqlParameter[3];
                sqlParameters[0] = new SqlParameter("OrganisationsNr", SqlDbType.VarChar);
                sqlParameters[0].Value = Convert.ToString(_sOrganisationsNr);
                sqlParameters[1] = new SqlParameter("SequenceType", SqlDbType.VarChar);
                sqlParameters[1].Value = Convert.ToString(_sSequenceType);
                sqlParameters[2] = new SqlParameter("DatumAbrechnung", SqlDbType.VarChar);
                sqlParameters[2].Value = Convert.ToString(_sDatumAbrechnung);

                return conn.executeSelectQuery(query, sqlParameters);

            }

        }
    }
}