using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DataAcccess : IDataAccess, IDisposable
    {
        private string _connectionString { get; set; }
        private SqlConnection _conn { get; set; }

        public DataAcccess()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ProductDataConnectionString"].ConnectionString;

        }

        public void Connect()
        {
            _conn = new SqlConnection(_connectionString);
            _conn.Open();
        }

        public void Close()
        {
            _conn.Close();
        }

        public List<Dictionary<string, object>> ExecuteQuery(string query)
        {
            var lstRows = new List<Dictionary<string, object>>();
            using (var command = new SqlCommand(query, _conn))
            {
                //changed from var to SqlDataReader
                var dataReader = command.ExecuteReader();

                //var ct = dataReader.GetSchemaTable().Columns.Count;
                var ct = dataReader.GetSchemaTable().Rows.Count;
                while (dataReader.Read())
                {
                    var objColValues = new Dictionary<string, object>();
                    for (int i = 0; i < ct; i++)
                    {
                        objColValues.Add(dataReader.GetName(i), dataReader.GetValue(i));
                    }
                    lstRows.Add(objColValues);
                }
            }
            return lstRows;
        }

            public void ExecuteSetQuery(string query)
        {
            try
            {
                using (var command = new SqlCommand(query, _conn))
                {
                    var dataReader = command.ExecuteNonQuery();

                }
            }
            
            catch(SystemException err)
            {
                throw err;
            }
        //    return lstRows;
        }

        public void Dispose()
        {
            Close();
        }
    }
}
