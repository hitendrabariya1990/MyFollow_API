using System.Data.Entity;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using Microsoft.AspNet.Identity;
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
        public MyFollowContext Db = new MyFollowContext();
        public ProductsList ProductsList;
        public List<ProductsList> ProductsLists;
        
        [HttpGet]
        [Route("api/EndUserAPI/GetProductsList")]
        [ResponseType(typeof(Products))]
        [Authorize(Roles = "User")]
        public IHttpActionResult GetProductList()
        {
            var userid = User.Identity.GetUserId();
            ProductsLists = new List<ProductsList>();
            var productses = Db.Productses.ToList();
            if (productses == null)
            {
                return NotFound();
            }
            foreach (var item in productses)
            {
                ProductsList = new ProductsList();
                int user = Convert.ToInt32(userid);
                var follower = Db.FollowProducts.FirstOrDefault(x => x.ProductId == item.Id && x.Euid == user);
                if (follower != null)
                {
                    ProductsList.Flag = true;
                }
                else
                {
                    ProductsList.Flag = false;
                }
                ProductsList.Id = item.Id;
                ProductsList.CompanyName = item.ProductOwner.CompanyName;
                ProductsList.ProductName = item.ProductName;
                ProductsLists.Add(ProductsList);
            }
            
            return Ok(ProductsLists);
        }


        [HttpGet]
        [Route("api/EndUserAPI/GetFollowProductList")]
        [ResponseType(typeof(Products))]
        [Authorize(Roles = "User")]
        public IHttpActionResult GetFollowProductList()
        {
            var userId = User.Identity.GetUserId();
            int userid = Convert.ToInt32(userId);
            var productses = (from x in Db.FollowProducts
                                  join p in Db.Productses on x.ProductId equals p.Id
                                  where x.Euid == userid
                                  select new
                                  {
                                      ProductId=x.ProductId,
                                      CompanyName=p.ProductOwner.CompanyName,
                                      ProductName=p.ProductName,
                                      Introduction=p.Introduction,
                                      Details=p.Details

                                  });
            if (productses == null)
            {
                return NotFound();
            }
            return Ok(productses.ToList());
        }


        [HttpGet]
        [Route("api/EndUserAPI/GetFollowProductsDetails")]
        [ResponseType(typeof(Products))]
        [Authorize(Roles = "User")]
        public IHttpActionResult GetFollowProducts(int id)
        {
            //Products products = db.Productses.FirstOrDefault(x=>x.Id == id);
            var productses = Db.Productses.Include(a=>a.UploadImages);
            var products = productses.FirstOrDefault(x => x.Id == id);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }


        [HttpPost]
        [Route("api/EndUserAPI")]
        [Authorize(Roles = "User")]
       // [ResponseType(typeof(Products))]
        public IHttpActionResult PostProducts(ProductsList productsList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userid = User.Identity.GetUserId();
            var followProducts = new FollowProducts();
            followProducts.Euid = Convert.ToInt32(userid);
            followProducts.ProductId = productsList.Id;
            Db.FollowProducts.Add(followProducts);
            Db.SaveChanges();
           
            // return CreatedAtRoute("api/ProductsAPI/GetProductses", new { id = productes.Id }, productes);
            return Ok();
        }

        [Route("api/EndUserAPI/UnFollowProducts")]
        [ResponseType(typeof(Products))]
        [Authorize(Roles = "User")]
        public IHttpActionResult DeleteProducts(int id)
        {
            var userid = User.Identity.GetUserId();
            int user = Convert.ToInt32(userid);
            var follower = Db.FollowProducts.FirstOrDefault(x => x.ProductId == id && x.Euid == user);
            if (follower == null)
            {
                return NotFound();
            }
           // var poid = followproducts.Poid;
            Db.FollowProducts.Remove(follower);
            Db.SaveChanges();
            return Ok();
        }

    }

}
