using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MyFollow.ClassFile;
using MyFollow.DAL;
using MyFollow.Models;
using System.Net;
using System.Data.Entity;

namespace MyFollow.Controllers
{
    [Authorize(Roles="ProductOwner")]
    public class ProductOwnersController : Controller
    {
        private MyFollowContext db = new MyFollowContext();

        public ProductOwnersController()
        {
        }
        public ProductOwnersController(ApplicationUserManager userManager,  ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }


        // GET: ProductOwners
        public ActionResult Index(string email)
        {
            Session["Email"] = email;
            var product = db.ProductOwners.FirstOrDefault(x => x.EmailId == email);
            if(product != null)
            { 
                ViewBag.Id = product.Id;
            }
            return View();
        }

        // GET: ProductOwners/Details/5
        public ActionResult Details(string email)
        {
            if (email == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session["Email"] = email;
            ProductOwner productOwner = db.ProductOwners.FirstOrDefault(x => x.EmailId == email);
            if (productOwner == null)
            {
                return HttpNotFound();
            }
            return View(productOwner);
        }

        // GET: ProductOwners/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: ProductOwners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "Id,CompanyName,EmailId,Description,DateofJoin,FoundedIn,Street1,Street2,City,State,Country,Pincode,ContactNumber,Website,Twitter,Facebook,Password,ApprovalFlag")] ProductOwner productOwner)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.ProductOwners.Add(productOwner);
        //        db.SaveChanges();
        //        var user = new ApplicationUser { UserName = productOwner.EmailId, Email = productOwner.EmailId };
        //        var result = await UserManager.CreateAsync(user, productOwner.Password);
        //        if (result.Succeeded)
        //        {
        //            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
        //            await UserManager.AddToRolesAsync(user.Id, "ProductOwner");
        //        }

        //        return RedirectToAction("Index");
        //    }

        //    return View(productOwner);
        //}

        // GET: ProductOwners/Edit/5
        public ActionResult Edit()
        {
            if (Session["Email"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string email = Session["Email"].ToString();
            ProductOwner productOwner = db.ProductOwners.FirstOrDefault(x => x.EmailId == email);
            if (productOwner == null)
            {
                return HttpNotFound();
            }
            return View(productOwner);
        }

        // POST: ProductOwners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( ProductOwner productOwner)
        {
            if (ModelState.IsValid)
            {
                productOwner.EmailId = Session["Email"].ToString();
                db.Entry(productOwner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new {Email=Session["Email"] });
            }
            return View(productOwner);
        }

        // GET: ProductOwners/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ProductOwner productOwner = db.ProductOwners.Find(id);
        //    if (productOwner == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(productOwner);
        //}

        //// POST: ProductOwners/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    ProductOwner productOwner = db.ProductOwners.Find(id);
        //    db.ProductOwners.Remove(productOwner);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
