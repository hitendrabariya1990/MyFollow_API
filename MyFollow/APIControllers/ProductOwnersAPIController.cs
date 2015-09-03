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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace MyFollow.APIControllers
{
    public class ProductOwnersAPIController : ApiController
    {
        private MyFollowContext db = new MyFollowContext();
        public UserManager<IdentityUser> UserManager { get; private set; }
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

      
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
        //[ResponseType(typeof(ProductOwner))]
        // PUT: api/ProductOwnersAPI/5
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
                else
                {
                    throw;
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
            var queryVals = HttpContext.Current.Request.Url.AbsoluteUri; 
           //var message = queryVals["id"];
            //string id = "";
            //var user = await UserManager.FindByIdAsync(id);
            //await UserManager.AddToRolesAsync(user.Id, "ProductOwner");


           // var emailid= UserManager.FindByEmail(email);
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