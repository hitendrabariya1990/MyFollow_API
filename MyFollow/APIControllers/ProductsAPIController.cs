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
    public class ProductsapiController : ApiController
    {
        MyFollowContext db = new MyFollowContext();
       
        [HttpGet]
        [Route("api/ProductsAPI/GetProductsList")]
        [Authorize(Roles = "ProductOwner")]
       // [ResponseType(typeof(Products))]
        public IHttpActionResult GetProductses(int id)
        {
           var productlist=(from x in db.MainProducts
                                 join p in db.Productses on x.Id equals p.MProductId
                                 where x.Poid == id
                                 select new
                                 {
                                     x.Id,x.ProductName,p.Introduction,p.Details,
                                     ProductId=p.Id
                                 });
            return Ok(productlist.ToList());
        }

        [HttpGet]
        [Route("api/ProductsAPI/GetProducts")]
        [ResponseType(typeof(Products))]
        [Authorize(Roles = "ProductOwner")]
        public IHttpActionResult GetProducts(int id)
        {
            var productses = db.Productses.Include(p => p.UploadImages);
            var products=productses.FirstOrDefault(x=>x.Id == id);
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }

        // PUT
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
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST
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
            //productes. = productsList.Poid;
            productes.MProductId = productsList.MProductId;
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
            return  Ok(productes);
        }

        // DELETE
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
           // var poid = products.Poid;
            db.Productses.Remove(products);
            db.SaveChanges();
            //return Ok(poid);
            return Ok();
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

        [HttpPost]
        [Route("api/ProductsAPI/PostMainProduct")]
        [ResponseType(typeof(Products))]
        [Authorize(Roles = "ProductOwner")]
        public IHttpActionResult PostMainProduct(ProductsList productsList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            MainProduct mproductes = new MainProduct();
            mproductes.Poid = productsList.Poid;
            mproductes.ProductName = productsList.ProductName;
            db.MainProducts.Add(mproductes);
            db.SaveChanges();
           
            return Ok(mproductes);
        }

        [HttpGet]
        [Route("api/ProductsAPI/GetMainProductsList")]
        [Authorize(Roles = "ProductOwner")]
        // [ResponseType(typeof(Products))]
        public IHttpActionResult GetMainProductses(int id)
        {
            var mainproduct = db.MainProducts.ToList().Where(x=>x.Poid == id);
            return Ok(mainproduct);
        }

        //[Route("api/ProductsAPI/DeleteMainProducts")]
        //[ResponseType(typeof(Products))]
        //[Authorize(Roles = "ProductOwner")]
        //public IHttpActionResult DeleteMianProduct(int id)
        //{
        //    MainProduct products = db.MainProducts.Find(id);
        //    if (products == null)
        //    {
        //        return NotFound();
        //    } 
        //    db.MainProducts.Remove(products);
        //    db.SaveChanges();
        //    return Ok();
        //}
    }
}