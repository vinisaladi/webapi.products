using System.Collections.Generic;

namespace DataAccess
{
    public interface IDataAccess
    {
        void Connect();
        void Close();
        List<Dictionary<string,object>> ExecuteQuery(string query);
        void ExecuteSetQuery(string query);
        //List<Dictionary<string, object>> ExecuteQuery(string query, int Id);
        //List<Dictionary<string, object>> InsertQuery(string query);
    }
}