using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyFollow.DAL;
using MyFollow.Models;

namespace MyFollow.Controllers
{
   [Authorize(Roles = "ProductOwner")]
    public class ProductsController : Controller
    {
        private MyFollowContext db = new MyFollowContext();

        // GET: Products
        public ActionResult Index()
        {
            string Email= Session["Email"].ToString();
            var productses = db.Productses.Include(p => p.ProductOwner);
            return View(productses.ToList().Where(a=>a.ProductOwner.EmailId==Email));
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Productses.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.ImageList = db.UploadImageses.ToList().Where(a=>a.ProductId == products.Id);
            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            string Email = Session["Email"].ToString();
            var productses = db.Productses.Include(p => p.ProductOwner);
            ProductOwner productOwner = db.ProductOwners.FirstOrDefault(x => x.EmailId == Email);
            TempData["OwnerId"] = productOwner.Id;
            ViewBag.Poid = new SelectList(db.ProductOwners.ToList().Where(a =>a.EmailId==Email), "Id", "CompanyName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Poid,Introduction,Details")] Products products, HttpPostedFileBase[] files)
        {
            if (ModelState.IsValid)
            {
                products.Poid =Convert.ToInt32( TempData["OwnerId"]);
                db.Productses.Add(products);
                db.SaveChanges();

                if (files != null)
                {
                    UploadImages ui=new UploadImages();
                    foreach (HttpPostedFileBase file in files)
                    {
                        string filename = System.IO.Path.GetFileName(file.FileName);
                        file.SaveAs(Server.MapPath("~/Images/" + filename));
                        string filepathtosave = "Images/" + filename;
                        //ui.ProductId = 1;
                        ui.ImageName = filename;
                        ui.ProductId = products.Id;
                        db.UploadImageses.Add(ui);
                        db.SaveChanges();
                    }

                  //  db.SaveChanges();
                    ViewBag.Message = "File Uploaded successfully.";
                }
                return RedirectToAction("Index");
            }

            ViewBag.Poid = new SelectList(db.ProductOwners, "Id", "CompanyName", products.Poid);
            return View(products);
        }
        
        
        //[ValidateAntiForgeryToken]
        public ActionResult Remove(int? id, int? Productid)
        {
            UploadImages uploadImages = db.UploadImageses.Find(id);
            db.UploadImageses.Remove(uploadImages);
            db.SaveChanges();
            return RedirectToAction("Edit", new { id= Productid });
        }
        // GET: Products/Edit/5

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Productses.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.Poid = new SelectList(db.ProductOwners, "Id", "CompanyName", products.Poid);
            ViewBag.ImageList = db.UploadImageses.ToList().Where(a => a.ProductId == products.Id);
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Poid,Introduction,Details")] Products products,UploadImages UploadImageses)
        {
            if (ModelState.IsValid)
            {
                db.Entry(products).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Poid = new SelectList(db.ProductOwners, "Id", "CompanyName", products.Poid);
            return View(products);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Productses.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.Productses.Find(id);
            db.Productses.Remove(products);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
