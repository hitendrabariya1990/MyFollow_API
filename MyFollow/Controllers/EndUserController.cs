using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyFollow.DAL;
using System.Net;
using MyFollow.Models;

namespace MyFollow.Controllers
{
    public class EndUserController : Controller
    {
        private MyFollowContext db = new MyFollowContext();

       [Authorize(Roles = "User")]
        public ActionResult ViewPages(string email)
        {
            ViewBag.Email = email;
            return View();
        }
       
        public ActionResult Index()
        {
            //ViewBag.Email = email;
            return View();
        }

        [Authorize(Roles = "User")]
        public ActionResult ProductsList()
        {
            return View(db.Productses.ToList());
        }

         [Authorize(Roles = "User")]
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
            ViewBag.ImageList = db.UploadImageses.ToList().Where(a => a.ProductId == products.Id);
            return View(products);
        }
    }
}