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
    public class LendController : Controller
    {
        BookManagerEntities db = new BookManagerEntities();
        // GET: Lend
        public ActionResult Index()
        {
            var ui = (int)Session["uid"];
            var q = db.Books.Where(x => x.BookStatu.Status == "Lended" && x.UserId == ui).ToList();
            return View(q);
        }
        public ActionResult BookList()
        {
            var ui = (int)Session["uid"];
            var q = db.Books.Where(x => (x.Reading.ReadingStatus != "Running" || x.Reading.ReadingStatus != "Completed") && x.BookStatu.Status == "In Room" && x.UserId == ui).ToList();
            return View(q);
        }
        public ActionResult Create(int? id)
        {
            var ui = (int)Session["uid"];

            List<BookStatu> SatList = db.BookStatus.ToList();
            ViewBag.SatList = new SelectList(SatList, "BookStatusId", "Status");

            var book = db.Books.Where(m => m.BookId == id && m.UserId==ui).ToList().SingleOrDefault();
            if (book == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(book);
            }

        }

        [HttpPost]
        public ActionResult Create(Book b)
        {


            List<BookStatu> SatList = db.BookStatus.ToList();
            ViewBag.SatList = new SelectList(SatList, "BookStatusId", "Status");

            try
            {
                
                if(b.BookStatusId!= 2)
                {
                    TempData["msg"] = "Detail isn't saved!";
                    return RedirectToAction("Create", "Lend");
                }


                db.Entry(b).State = EntityState.Modified;

                db.SaveChanges();


                return RedirectToAction("Index", "Lend");
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Detail isn't saved!" + ex;
                return RedirectToAction("Create", "Lend");
            }

        }
        public ActionResult Edit(int? id)
        {
            List<BookStatu> SatList = db.BookStatus.ToList();
            ViewBag.SatList = new SelectList(SatList, "BookStatusId", "Status");
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
            try
            {

                if(b.BookStatusId== 1)
                {
                    b.BorrowerName = null;
                    b.BorrowDate = null;
                }

                db.Entry(b).State = EntityState.Modified;

                db.SaveChanges();


                return RedirectToAction("Index", "Lend");
            }
            catch
            {
                TempData["msg"] = "Detail isn't updated!";
                return RedirectToAction("Edit", "Lend");
            }
        }
    }
}