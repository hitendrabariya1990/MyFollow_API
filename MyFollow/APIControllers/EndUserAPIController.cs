using System.Data.Entity;
using Microsoft.AspNet.Identity;
using MyFollow.DAL;
using MyFollow.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace MyFollow.APIControllers
{
    public class EndUserapiController : ApiController
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
            var productses = Db.MainProducts.ToList();
            if (productses.FirstOrDefault() == null)
            {
                return NotFound();
            }
            foreach (var item in productses)
            {
                ProductsList = new ProductsList();
                int user = Convert.ToInt32(userid);
                var follower = Db.FollowProducts.FirstOrDefault(x => x.MProductId == item.Id && x.Euid == user);
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
                              join p in Db.MainProducts on x.MProductId equals p.Id
                              join p1 in Db.Productses on p.Id equals p1.MProductId
                              where x.Euid == userid
                              select new
                              {
                                  p1.Id,
                                  p1.MainProduct.ProductOwner.CompanyName,
                                  p.ProductName,
                                  p1.Introduction,
                                  p1.Details

                              });
            if (productses.FirstOrDefault() == null)
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
        public IHttpActionResult PostProducts(ProductsList productsList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userid = User.Identity.GetUserId();
            var followProducts = new FollowProducts();
            followProducts.Euid = Convert.ToInt32(userid);
            followProducts.MProductId = productsList.Id;
            
            Db.FollowProducts.Add(followProducts);
            Db.SaveChanges();
            return Ok();
        }

        [Route("api/EndUserAPI/UnFollowProducts")]
        [ResponseType(typeof(Products))]
        [Authorize(Roles = "User")]
        public IHttpActionResult DeleteProducts(int id)
        {
            var userid = User.Identity.GetUserId();
            int user = Convert.ToInt32(userid);
            var follower = Db.FollowProducts.FirstOrDefault(x => x.MProductId == id && x.Euid == user);
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
