using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SepaKonverter
{
    public class DatabaseConnector
    {
        private SqlDataAdapter myAdapter;
        private SqlConnection conn;

        /// <constructor>
        /// Initialize Connection
        /// </constructor>
        public DatabaseConnector()
        {
            myAdapter = new SqlDataAdapter();
            conn = new SqlConnection(ConfigurationManager.GetConnectionString());
        }

        public DatabaseConnector(string sServer)
        {
            myAdapter = new SqlDataAdapter();
            conn = new SqlConnection(ConfigurationManager.GetDELLConnectionString());
        }

        /// <method>
        /// Open Database Connection if closed or broken
        /// </method>
        private SqlConnection openConnection()
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
            {
                conn.Open();
            }
            return conn;
        }

        /// <method>
        /// Count Query
        /// </method>
        public int executeCountQuery(String _query, SqlParameter[] _sqlParameters = null)
        {

            int iCount;
            SqlCommand myCommand = new SqlCommand();

            try
            {
                myCommand.Connection = openConnection();

                if (_sqlParameters != null)
                {
                    myCommand.Parameters.AddRange(_sqlParameters);
                }

                myCommand.CommandText = _query;
                iCount = (int)myCommand.ExecuteScalar();

            }
            catch (SqlException e)
            {
                Console.Write("Error - Connection.executeCountQuery - Query:" + _query + " \nException: " + e.StackTrace.ToString());
                return 0;
            }
            finally
            {

            }
            return iCount;
        }

        public decimal executeSumQuery(String _query, SqlParameter[] _sqlParameters = null)
        {

            decimal dSum;

            SqlCommand myCommand = new SqlCommand();

            try
            {
                myCommand.Connection = openConnection();

                if (_sqlParameters != null)
                {
                    myCommand.Parameters.AddRange(_sqlParameters);
                }

                myCommand.CommandText = _query;
                dSum = (decimal) myCommand.ExecuteScalar();

            }
            catch (SqlException e)
            {
                Console.Write("Error - Connection.executeCountQuery - Query:" + _query + " \nException: " + e.StackTrace.ToString());
                return 0;
            }
            finally
            {

            }
            return dSum;
        }

        /// <method>
        /// Select Query
        /// </method>
        public DataTable executeSelectQuery(String _query, SqlParameter[] _sqlParameters = null)
        {
            SqlCommand myCommand = new SqlCommand();
            DataTable dataTable = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                myCommand.Connection = openConnection();
                myCommand.CommandText = _query;

                if (_sqlParameters != null)
                {
                    myCommand.Parameters.AddRange(_sqlParameters);
                }

                myCommand.ExecuteNonQuery();                
                myAdapter.SelectCommand = myCommand;
                myAdapter.Fill(ds);
                dataTable = ds.Tables[0];
            }
            catch (SqlException e)
            {
                Console.Write("Error - Connection.executeSelectQuery - Query:" + _query + " \nException: " + e.StackTrace.ToString());
                return null;
            }
            finally
            {

            }
            return dataTable;
        }

        /// <method>
        /// Select Query ohne Parameter
        /// </method>
        public DataTable executeSelectQuery(String _query)
        {
            SqlCommand myCommand = new SqlCommand();
            DataTable dataTable = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                myCommand.Connection = openConnection();
                myCommand.CommandText = _query;
                myCommand.ExecuteNonQuery();
                myAdapter.SelectCommand = myCommand;
                myAdapter.Fill(ds);
                dataTable = ds.Tables[0];
            }
            catch (SqlException e)
            {
                Console.Write("Error - Connection.executeSelectQuery - Query:" + _query + " \nException: " + e.StackTrace.ToString());
                return null;
            }
            finally
            {

            }
            return dataTable;
        }

        /// <method>
        /// Insert Query
        /// </method>
        public bool executeInsertQuery(String _query, SqlParameter[] _sqlParameter)
        {
            SqlCommand myCommand = new SqlCommand();
            try
            {
                myCommand.Connection = openConnection();
                myCommand.CommandText = _query;
                myCommand.Parameters.AddRange(_sqlParameter);
                myAdapter.InsertCommand = myCommand;
                myCommand.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Console.Write("Error - Connection.executeInsertQuery - Query:" + _query + " \nException: \n" + e.StackTrace.ToString());
                return false;
            }
            finally
            {
            }
            return true;
        }

        /// <method>
        /// Update Query
        /// </method>
        public bool executeUpdateQuery(String _query, SqlParameter[] _sqlParameter)
        {
            SqlCommand myCommand = new SqlCommand();
            try
            {
                myCommand.Connection = openConnection();
                myCommand.CommandText = _query;
                myCommand.Parameters.AddRange(_sqlParameter);
                myAdapter.UpdateCommand = myCommand;
                myCommand.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Console.Write("Error - Connection.executeUpdateQuery - Query:" + _query + " \nException: " + e.StackTrace.ToString());
                return false;
            }
            finally
            {
            }
            return true;
        }
    }
}