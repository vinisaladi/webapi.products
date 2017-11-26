using DataAccess;
using Newtonsoft.Json;
using ProductsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProductsApp.Controllers
{
    [RoutePrefix("api/Products")]
    public class ProductsController : ApiController
    {
        IDataAccess _dataAccess;
        public ProductsController()
        {
            _dataAccess = new DataAcccess();
        }
        //public List<Product> ReadFile()
        //{
        //    List<Product> prod = new List<Product>();
        //    //Step1 : Read file

        //    string filePath = @"C:\Users\vinis\Documents\ProductsData.txt";
        //    if (filePath != null)
        //    {
        //        string[] Lines = System.IO.File.ReadAllLines(filePath);
        //        foreach (string text in Lines)
        //        {
        //            Product p = new Product();
        //            string[] word = text.Split(',');
        //            foreach (string finalVal in word)
        //            {
        //                string[] dataVal = finalVal.Split('=');
        //                string propertyVal = dataVal[0].Trim();
        //                switch (propertyVal)
        //                {
        //                    case "Id":
        //                        p.Id = Convert.ToInt32(dataVal[1]);
        //                        break;
        //                    case "Name":
        //                        p.Name = dataVal[1];
        //                        break;
        //                    case "Category":
        //                        p.Category = dataVal[1];
        //                        break;
        //                    case "Price":
        //                        p.Price = Convert.ToDecimal(dataVal[1]);
        //                        break;

        //                }
        //            }
        //            prod.Add(p);
        //        }



        //        //Step2: Split line based on ,
        //        //Step3: Split above array elemetns with = to array of strings
        //        //step4 loop thru above array 
        //        // Step5: Switch and assign to product object
        //        //Step6: Add to list
        //        //return list

        //    }
        //    return prod;
        //}

        [Route("GetProduct")]
        public IEnumerable<Product> GetProducts()
        {
            var query = "select * from dbo.Product";
            _dataAccess.Connect();
            var result = _dataAccess.ExecuteQuery(query);
            var prods = result.Select(x => new Product()
            {
                Id = Convert.ToInt32(x["prod_id"]),
                Name = x["prod_name"].ToString(),
                Price = Convert.ToDecimal(x["prod_price"]),
                Category = x["prod_category"].ToString()
            }).ToList();
            return prods;
            //return ReadFile();
        }

        //public IEnumerable<Product> PostProduct(Array sentData)
        //{
        //    var query = "insert into Product(prod_id, prod_name, prod_category, prod_price) Values(@Id, @Name, @Category, @120) ";
        //    _dataAccess.Connect();
        //    var result = _dataAccess.ExecuteQuery(query, Array sentData);
        //    var prods = result.Select(x => new Product()
        //    {
        //        Id = Convert.ToInt32(x["prod_id"]),
        //        Name = x["prod_name"].ToString(),
        //        Price = Convert.ToDecimal(x["prod_price"]),
        //        Category = x["prod_category"].ToString()
        //    }).ToList();
        //    return prods;
        //    //return ReadFile();
        //}
        [HttpGet]
        [Route("GetProduct/id")]
        public IHttpActionResult GetProductById( [FromUri]int id)
        {
            var query = "select * from dbo.Product where prod_id =" + id;
            _dataAccess.Connect();

            var result = _dataAccess.ExecuteQuery(query).SingleOrDefault();
            if (result == null || result.Count == 0)
            {
                return NotFound();
            }
            Product p = new Product();
            p.Id = Convert.ToInt32(result["prod_id"]);
            p.Name = result["prod_name"].ToString();
            p.Price = Convert.ToDecimal(result["prod_price"]);
            p.Category = result["prod_category"].ToString();
            return Ok(p);
        }

        [HttpPost]
        [Route("GetProduct/Prod")]
        public HttpResponseMessage PostProduct([FromBody] Product p)
        {      
            try
            {
                var query = string.Format("insert into Product(prod_id, prod_name, prod_category, prod_price) Values({0},'{1}','{2}',{3})", p.Id, p.Name, p.Category, p.Price);
                _dataAccess.Connect();
                _dataAccess.ExecuteSetQuery(query);
                return Request.CreateResponse(HttpStatusCode.Created);
            } 
            
            catch (SystemException error)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
            }     
            
        }

        [HttpDelete]
        [Route("DeleteProduct")]
        public HttpResponseMessage DeleteProduct([FromUri]int id)
        {
           // try
            //{
                var query = "delete from dbo.Product where prod_id =" + id;
                _dataAccess.Connect();
                _dataAccess.ExecuteSetQuery(query);
                return Request.CreateResponse(HttpStatusCode.Accepted);
            //}
            //catch(SystemException error)
            //{
            //    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
            //}

      
            
        }

        [HttpPut]
        [Route("UpdateProduct/Prod")]
        public HttpResponseMessage UpdateProduct([FromBody] Product p)
        {
            try
            {
                var query = string.Format("Update Product SET  prod_name = '{0}', prod_category = '{1}', prod_price ='{2}' where prod_id = '{3}'", p.Name, p.Category, p.Price, p.Id);
                _dataAccess.Connect();
                _dataAccess.ExecuteSetQuery(query);
                return Request.CreateResponse(HttpStatusCode.Created);
            }

            catch (SystemException error)
            {
            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, error.Message);
            }

        }
    }
}
