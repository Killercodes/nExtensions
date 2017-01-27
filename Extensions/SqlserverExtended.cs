using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace nExtensions
{
    public static class SqlserverExtended
    {
        public static void SqlTrustedConnection (this SqlConnection sqc, string Data_Source, string Init_Catalog)
        {
            sqc.ConnectionString = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=SSPI;", Data_Source, Init_Catalog);
        }

        public static void SqlTrustedConnection (this SqlConnection sqc, string Server, string Database, bool Trusted_Connection)
        {
            sqc.ConnectionString = "Server=" + Server +
            ";Database=" + Database +
            ";Trusted_Connection=" + Trusted_Connection + ";";
        }

        public static void SqlStandardConnection (this SqlConnection sqc, string Data_Source, string Init_Catalog, string User_Id, string Password)
        {
            sqc.ConnectionString = "Data Source=" + Data_Source +
            ";Initial Catalog=" + Init_Catalog +
            ";User Id=" + User_Id +
            ";Password=" + Password + ";";
        }

        public static void SqlStandardConnection (this SqlConnection sqc, string Server, string Database, string User_ID, string Password, bool Trusted_Connection)
        {
            sqc.ConnectionString = "Server=" + Server +
            ";Database=" + Database +
            ";User Id=" + User_ID +
            ";Password=" + Password +
            ";Trusted_Connection=" + Trusted_Connection + ";";
        }

        public static void OleStandardConnection (this OleDbConnection ocs, string Your_Server_Name, string Your_Database_Name, string Your_Username, string Your_Password)
        {
            ocs.ConnectionString = "Provider=SQLOLEDB;Data Source=" + Your_Server_Name +
            ";Initial Catalog=" + Your_Database_Name +
            ";UserId=" + Your_Username +
            ";Password=" + Your_Password + ";";
        }

        public static void OleTrustedConnection (this OleDbConnection ocs, string Your_Server_Name, string Your_Database_Name)
        {
            ocs.ConnectionString = "Provider=SQLOLEDB;Data Source=" + Your_Server_Name +
            ";Initial Catalog=" + Your_Database_Name + ";Integrated Security=SSPI;";
        }
        public static SqlDataReader CallStoredProcedure (this SqlConnection sqlConnection,string storedProcedurename)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;
            
            cmd.CommandText = storedProcedurename;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection;

            sqlConnection.Open();

            reader = cmd.ExecuteReader();
            // Data is accessible through the DataReader object here.
            sqlConnection.Close();
            return reader;
        }

        /// <summary>
        /// Checks SqlConnection, returns true if connected
        /// </summary>
        /// <param name="sqlConnection"></param>
        /// <returns></returns>
        public static bool TestConnection (this SqlConnection sqlConnection)
        {
            try
            {
                sqlConnection.Open();
                sqlConnection.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            } 
        }

        public static List<string> AvailableDatabase (this SqlConnection sqlConnection)
        {
            List<string> databases = new List<string>();
            sqlConnection.Open();
            DataTable tblDatabases = sqlConnection.GetSchema("Databases");
            sqlConnection.Close();

            foreach (DataRow row in tblDatabases.Rows)
            {
                databases.Add(row["database_name"].ToString());
            }

            return databases;
        }
    }
}
