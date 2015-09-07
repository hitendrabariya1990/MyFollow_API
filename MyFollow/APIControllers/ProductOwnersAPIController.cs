using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using MyFollow.DAL;
using MyFollow.Models;



namespace MyFollow.APIControllers
{
    public class ProductOwnersapiController : ApiController
    {
        private MyFollowContext db = new MyFollowContext();
      
        // GET: api/ProductOwnersAPI
        public IEnumerable<ProductOwner> GetProductOwners()
        {
            return db.ProductOwners;
        }

        //[HttpGet]
        //[Route("api/ProductOwnersAPI/GetProductOwnerByEmail")]
        //[ResponseType(typeof(ProductOwner))]
        //public IHttpActionResult GetProductOwnerByEmail(string Email)
        //{
        //    ProductOwner productOwner = new ProductOwner();
        //    productOwner.EmailId = Email;
        //    if (productOwner == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(productOwner);
        //}


        [HttpGet]
        [Route("api/ProductOwnersAPI/GetProductOwner")]
        [ResponseType(typeof(ProductOwner))]
        [Authorize(Roles = "ProductOwner")]
        public IHttpActionResult GetProductOwner(int id)
        {
            ProductOwner productOwner = db.ProductOwners.Find(id);
            if (productOwner == null)
            {
                return NotFound();
            }
            return Ok(productOwner);
        }

        [HttpPut]
        [Route("api/ProductOwnersAPI/PutProductOwner")]
        [ResponseType(typeof(void))]
        [Authorize(Roles = "ProductOwner")]
        public IHttpActionResult PutProductOwner(int id, ProductOwner productOwner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != productOwner.Id)
            {
                return BadRequest();
            }

            db.Entry(productOwner).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductOwnerExists(id))
                {
                    return NotFound();
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ProductOwnersAPI
        [HttpPost]
        [Route("api/ProductOwnersAPI")]
        [ResponseType(typeof(ProductOwner))]
        [Authorize(Roles = "ProductOwner")]
        public IHttpActionResult PostProductOwner(ProductOwner productOwner)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.ProductOwners.Add(productOwner);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = productOwner.Id }, productOwner);
        }

        // DELETE: api/ProductOwnersAPI/5
        [ResponseType(typeof(ProductOwner))]
        [Authorize(Roles = "ProductOwner")]
        public IHttpActionResult DeleteProductOwner(int id)
        {
            ProductOwner productOwner = db.ProductOwners.Find(id);
            if (productOwner == null)
            {
                return NotFound();
            }

            db.ProductOwners.Remove(productOwner);
            db.SaveChanges();

            return Ok(productOwner);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductOwnerExists(int id)
        {
            return db.ProductOwners.Count(e => e.Id == id) > 0;
        }
    }
}