using MyBookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBookStore.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        BookManagerEntities db = new BookManagerEntities();
        public ActionResult Index()
        {
            if (Session["username"] != null)
            {
                int ui = (int)Session["uid"];
                ViewBag.latest = db.Books.OrderBy(x => x.BuyingDate).Where(x => x.BuyingDate != null && x.UserId == ui).Take(10).ToList();
                ViewBag.AllBooks = db.Books.Where(x => x.BookStatu.Status != "Buyable" && x.Reading.ReadingStatus != null && x.UserId == ui).Count();
                ViewBag.BuyAble = db.Books.Where(a => a.BookStatu.Status == "Buyable" && a.UserId == ui).Count();
                ViewBag.Completed = db.Books.Where(a => a.Reading.ReadingStatus == "Completed" && a.UserId == ui).Count();
                ViewBag.Lended = db.Books.Where(a => a.BookStatu.Status == "Lended" && a.UserId == ui).Count();


                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}