using DataAccess;
using ProductsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductRepository
{
    public class ProdRepository
    {
        IDataAccess _dataAccess;
        public ProdRepository()
        {
            _dataAccess = new DataAcccess();
        }

        public List<Product> GetAll()
        {
            var query = "select * from Product";
            _dataAccess.Connect();
            var result = _dataAccess.ExecuteQuery(query);
            var prods = result.Select(x => new Product() {
                Id = Convert.ToInt32(x["Id"]),
                Name = x["Name"].ToString(),
                Price = Convert.ToDecimal(x["Price"]),
                Category = x["Cateroy"].ToString()
            }).ToList();
            return prods;
        }

        //public List<Product> GetById(int id)
        //{
        //    var query = "select * from Products where Id = @id;";
        //    _dataAccess.Connect();
        //    var result = _dataAccess.ExecuteQuery(query);
        //    if(result == null)
        //    {
        //        return ;
        //    }
        //    return result[];
        //}
    }
}
