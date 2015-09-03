using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MyFollow.DAL;
using MyFollow.Models;
using System.Web;
using System.Threading.Tasks;

namespace MyFollow.APIControllers
{
    public class ProductsAPIController : ApiController
    {

        private MyFollowContext db = new MyFollowContext();
        
        // GET: api/ProductsAPI
        [HttpGet]
        [Route("api/ProductsAPI/GetProductsList")]
        [Authorize(Roles = "ProductOwner")]
       // [ResponseType(typeof(Products))]
        public IHttpActionResult GetProductses(int id)
        {
            List<ProductsList> proList=new List<ProductsList>();
            var productOwner = db.ProductOwners.FirstOrDefault(x=>x.Id == id);
            if(productOwner != null)
            { 
                var product = productOwner.Products.ToList();
                
                foreach(var item in product)
                {
                    ProductsList productList = new ProductsList();
                    productList.Id = item.Id;
                    productList.Poid = item.Poid;
                    productList.ProductName = item.ProductName;
                    productList.Introduction = item.Introduction;
                    productList.Details = item.Details;
                    proList.Add(productList);
                }
                return Ok(proList);
            }
            return Ok(proList);
        }

        // GET: api/ProductsAPI/5
        [HttpGet]
        [Route("api/ProductsAPI/GetProducts")]
        [ResponseType(typeof(Products))]
        [Authorize(Roles = "ProductOwner")]
        public IHttpActionResult GetProducts(int id)
        {
            //Products products = db.Productses.FirstOrDefault(x=>x.Id == id);
            var productses = db.Productses.Include(p => p.UploadImages);
            var products=productses.FirstOrDefault(x=>x.Id == id);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        // PUT: api/ProductsAPI/5
         [Route("api/ProductsAPI/EditProducts")]
        [ResponseType(typeof(void))]
        [Authorize(Roles = "ProductOwner")]
        public IHttpActionResult PutProducts(int id, Products products)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != products.Id)
            {
                return BadRequest();
            }

            db.Entry(products).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ProductsAPI
        [HttpPost]
        [Route("api/ProductsAPI")]
        [ResponseType(typeof(Products))]
        [Authorize(Roles = "ProductOwner")]
        public IHttpActionResult PostProducts(ProductsList productsList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Products productes = new Products();
            productes.Poid = productsList.Poid;
            productes.ProductName = productsList.ProductName;
            productes.Introduction = productsList.Introduction;
            productes.Details = productsList.Details;
            productes.VideoLink = productsList.VideoLink;
            db.Productses.Add(productes);
            db.SaveChanges();
            var images = productsList.UploadImagesName.Split(',');
            UploadImages ui = new UploadImages();
            for (int i = 0; i < images.Length; i++)
            {
                if (images[i] == "" || images[i] == null)
                {
                }else
                {
                    ui.ProductId = productes.Id;
                    ui.ImageName = images[i];
                    db.UploadImageses.Add(ui);
                    db.SaveChanges();
                }
            }

           // return CreatedAtRoute("api/ProductsAPI/GetProductses", new { id = productes.Id }, productes);
            return  Ok(productes);
        }

        // DELETE: api/ProductsAPI/5
         [Route("api/ProductsAPI/DeleteProducts")]
        [ResponseType(typeof(Products))]
        [Authorize(Roles = "ProductOwner")]
        public IHttpActionResult DeleteProducts(int id)
        {
            Products products = db.Productses.Find(id);
            if (products == null)
            {
                return NotFound();
            }
            var poid = products.Poid;
            db.Productses.Remove(products);
            db.SaveChanges();
            return Ok(poid);
        }

        [Route("api/ProductsAPI/DeleteImage")]
        [ResponseType(typeof(UploadImages))]
        [Authorize(Roles = "ProductOwner")]
        public IHttpActionResult DeleteImage(int id)
        {
            UploadImages images = db.UploadImageses.Find(id);
            if (images == null)
            {
                return NotFound();
            }
            var pid = images.ProductId;
            db.UploadImageses.Remove(images);
            db.SaveChanges();

            var productses = db.Productses.Include(p => p.UploadImages);
            var products = productses.FirstOrDefault(x => x.Id == pid);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductsExists(int id)
        {
            return db.Productses.Count(e => e.Id == id) > 0;
        }
    }
}