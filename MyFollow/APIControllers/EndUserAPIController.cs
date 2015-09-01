using MyFollow.DAL;
using MyFollow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace MyFollow.APIControllers
{
    public class EndUserAPIController : ApiController
    {
        private MyFollowContext db = new MyFollowContext();
       
        [HttpGet]
        [Route("api/EndUserAPI/GetProductsList")]
        [ResponseType(typeof(Products))]
        public IEnumerable<Products> GetProductList()
        {
            return db.Productses.ToList();
        }


        [HttpGet]
        [Route("api/EndUserAPI/GetProductsDetails")]
        [ResponseType(typeof(Products))]
        public IHttpActionResult GetProducts(int id)
        {
            //Products products = db.Productses.FirstOrDefault(x=>x.Id == id);
            var productses = db.Productses.ToList();
            var products = productses.FirstOrDefault(x => x.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);
        }
    }

}
