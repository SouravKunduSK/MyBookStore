using MyBookStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBookStore.Controllers
{
    [Authorize]
    public class UnBookController : Controller
    {
        BookManagerEntities db = new BookManagerEntities();
        // GET: UnBook
        public ActionResult Index()
        {
            var ui = (int)Session["uid"];
            var q = db.Books.Where(x => x.Reading.ReadingStatus == "Incomplete" || x.Reading.ReadingStatus == "New" && x.BookStatu.Status!="Buyable" && x.UserId == ui).ToList();
            return View(q);
        }

        public ActionResult Edit(int? id)
        {
            List<BookStatu> SatList = db.BookStatus.ToList();
            ViewBag.SatList = new SelectList(SatList, "BookStatusId", "Status");
            List<Reading> ReadList = db.Readings.ToList();
            ViewBag.ReadList = new SelectList(ReadList, "ReadingId", "ReadingStatus");
            var query = db.Books.Where(m => m.BookId == id).ToList().SingleOrDefault();

            if (query == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(query);
            }
        }
        [HttpPost]
        public ActionResult Edit(Book b)
        {
            List<BookStatu> SatList = db.BookStatus.ToList();
            ViewBag.SatList = new SelectList(SatList, "BookStatusId", "Status");
            List<Reading> ReadList = db.Readings.ToList();
            ViewBag.ReadList = new SelectList(ReadList, "ReadingId", "ReadingStatus");
            try
            {
                db.Entry(b).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "RunBook");
            }
            catch
            {
                TempData["msg"] = "Detail isn't updated!" ;
                return RedirectToAction("Edit", "RunBook");
            }
        }

    }
}