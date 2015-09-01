using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
        [ResponseType(typeof(Products))]
        public IEnumerable<Products> GetProductses(int id)
        {
         
            //var productOwner = db.ProductOwners.FirstOrDefault(x=>x.Id == id);
            //var product = productOwner.Products.ToList();
            return db.Productses.ToList();
        }

        // GET: api/ProductsAPI/5
        [HttpGet]
        [Route("api/ProductsAPI/GetProducts")]
        [ResponseType(typeof(Products))]
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
        public IHttpActionResult PostProducts(ProductsList productsList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Products productes = new Products();
            productes.Poid = productsList.Poid;
            productes.Introduction = productsList.Introduction;
            productes.Details = productsList.Details;
           // productes.UploadImages = productsList.UploadImagesName;
            db.Productses.Add(productes);
            db.SaveChanges();
            var Images = productsList.UploadImagesName.Split(',');
            UploadImages ui = new UploadImages();
            for (int i = 0; i < Images.Length;i++ )
            {
                if (Images[i] == "" || Images[i] == null)
                {
                }else
                {
                    ui.ProductId = productes.Id;
                    ui.ImageName = Images[i];
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
        public IHttpActionResult DeleteProducts(int id)
        {
            Products products = db.Productses.Find(id);
            var poid = products.Poid;
            if (products == null)
            {
                return NotFound();
            }
           
            db.Productses.Remove(products);
            db.SaveChanges();
            var product1 = db.Productses.ToList().Where(x=>x.Poid==poid);
            return Ok(product1);
        }

        [Route("api/ProductsAPI/DeleteImage")]
        [ResponseType(typeof(UploadImages))]
        public IHttpActionResult DeleteImage(int id)
        {
            UploadImages Images = db.UploadImageses.Find(id);
            if (Images == null)
            {
                return NotFound();
            }

            db.UploadImageses.Remove(Images);
            db.SaveChanges();

            return Ok(Images);
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