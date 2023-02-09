using System.Data.SqlClient;

namespace SimpleSQL
{
    class DBConnection : IDisposable
    {
        public SqlConnection Connection { get; set; }

        public DBConnection(string connectionstring)
        {
            try
            {
                Connection = new SqlConnection(connectionstring);
                Connection.Open();
            }
            catch (SqlException)
            {
                throw;
            }
            catch
            {
                throw;
            }
        }

        public void Dispose()
        {
            try
            {
                if (Connection != null && Connection.State != System.Data.ConnectionState.Closed) Connection.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
