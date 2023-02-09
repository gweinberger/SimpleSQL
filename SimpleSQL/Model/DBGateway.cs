using System.Collections.Generic;
using System.Data.SqlClient;

namespace SimpleSQL
{
    internal class DBGateway
    {
        private string connectionString = "";

        internal DBGateway(string connectionString)
        {
            this.connectionString = connectionString;
        }

        internal string GetDBVersion()
        {
            try
            {
                string? result = "";
                using (DBConnection db = new DBConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand($"select @@version", db.Connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows && reader.Read() && Convert.IsDBNull(reader[0]) == false) result = reader[0].ToString();
                            reader.Close();
                        }
                    }
                }
                return result != null ? result : "";
            }
            catch
            {
                throw;
            }
        }

        internal List<string> GetDatabaseList()
        {
            try
            {
                List<string> result = new List<string>();
                using (DBConnection db = new DBConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand($"select name from sys.databases", db.Connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.HasRows && reader.Read())
                            {
                                string? data = reader["name"].ToString();
                                if (data != null && data.Trim() != "") result.Add(data);
                            }
                            reader.Close();
                        }
                    }
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        internal List<List<string>> ExecuteReader(string command, bool addHeader = true)
        {
            try
            {
                List<List<string>> result = new List<List<string>>();
                using (DBConnection db = new DBConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(command, db.Connection))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.HasRows && reader.Read())
                        {
                            //add header
                            if (addHeader)
                            {
                                List<string> headerList = new List<string>();
                                for (int i = 0; i < reader.FieldCount; i++) { headerList.Add(reader.GetName(i).ToUpper()); }
                                if (headerList.Count > 0) result.Add(headerList);
                                addHeader = false;
                            }
                            //add fields
                            List<string> fieldList = new List<string>();
                            for (int i = 0; i < reader.FieldCount; i++) { fieldList.Add(reader[i] != null ? reader[i].ToString()! : ""); }
                            result.Add(fieldList);
                        }
                        reader.Close();
                        reader.Dispose();
                    }
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        internal int ExecuteNonQuery(string command)
        {
            try
            {
                int result = 0;
                using (DBConnection db = new DBConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(command, db.Connection))
                    {
                        result = cmd.ExecuteNonQuery();
                    }
                }
                return result;
            }
            catch
            {
                throw;
            }
        }
    }
}
