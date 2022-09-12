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
    public class RunBookController : Controller
    {
        BookManagerEntities db = new BookManagerEntities();
        // GET: RunBook
        public ActionResult Index()
        {
            var ui = (int)Session["uid"];
            var q = db.Books.Where(x => x.Reading.ReadingStatus == "Running" && x.UserId == ui).ToList();

            return View(q);
        }
        public ActionResult BookList()
        {
            var ui = (int)Session["uid"];
            var q = db.Books.Where(x => (x.Reading.ReadingStatus != "Running"|| x.Reading.ReadingStatus != "Completed") && x.BookStatu.Status=="In Room" && x.UserId == ui).ToList();
            return View(q);
        }
        public ActionResult Create(int? id)
        {
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
        public ActionResult Create(Book b)
        {
            List<Reading> ReadList = db.Readings.ToList();
            ViewBag.ReadList = new SelectList(ReadList, "ReadingId", "ReadingStatus");
            try
            {
                b.StartDate = DateTime.Now;
                db.Entry(b).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "RunBook");
            }
            catch
            {
                TempData["msg"] = "Detail isn't updated!";
                return RedirectToAction("Create", "RunBook");
            }
        }

        public ActionResult Edit(int? id)
        {
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
                TempData["msg"] = "Detail isn't updated!"; 
                return RedirectToAction("Edit", "RunBook");
            }
        }
    }
}