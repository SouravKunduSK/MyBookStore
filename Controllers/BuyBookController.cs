using MyBookStore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBookStore.Controllers
{
    [Authorize]
    public class BuyBookController : Controller
    {
        BookManagerEntities db = new BookManagerEntities();
        // GET: BuyBook
        public ActionResult Index()
        {
            var ui = (int)Session["uid"];
            var q = db.Books.Where(x => x.BookStatu.Status == "Buyable" && x.UserId == ui).ToList();
            return View(q);
        }

        public ActionResult Create()
        {
            var ui = (int)Session["uid"];
            List<Category> CategoryList = db.Categories.Where(x => x.UserId == ui).ToList();
            ViewBag.CategoryList = new SelectList(CategoryList, "CategoryId", "CategoryName");
            List<Author> AuthorList = db.Authors.Where(x => x.UserId == ui).ToList();
            ViewBag.AuthorList = new SelectList(AuthorList, "AuthorId", "AuthorName");
            List<BookStatu> SatList = db.BookStatus.ToList();
            ViewBag.SatList = new SelectList(SatList, "BookStatusId", "Status");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Book b, HttpPostedFileBase Image)
        {
            var ui = (int)Session["uid"];
            var username = Session["username"].ToString();
            List<Category> CategoryList = db.Categories.Where(x => x.UserId == ui).ToList();
            ViewBag.CategoryList = new SelectList(CategoryList, "CategoryId", "CategoryName");
            List<Author> AuthorList = db.Authors.Where(x => x.UserId == ui).ToList();
            ViewBag.AuthorList = new SelectList(AuthorList, "AuthorId", "AuthorName");
            List<BookStatu> SatList = db.BookStatus.ToList();
            ViewBag.SatList = new SelectList(SatList, "BookStatusId", "Status");
            if (Image != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(Image.FileName);
                string extension = Path.GetExtension(Image.FileName);

                fileName = fileName + username + extension;
                b.Photo = "~/Uploads/" + fileName;

                var folder = Server.MapPath("~/Uploads/");
                Image.SaveAs(Path.Combine(folder, fileName));

                

            }
            else if (Image == null)
            {
                b.Photo = "~/Uploads/" + "NoBook.png";

            }
            b.UserId = (int)Session["uid"];
            b.BookStatusId = 3;
            if (b.CategoryId == null || b.AuthorId == null || b.BookName == null || b.BookStatusId == null)
            {
                TempData["msg"] = "Book isn't Added!" +
                   "You must fill all the '*' sections..";
                return RedirectToAction("Create", "BuyBook");
            }
           
            db.Books.Add(b);
            db.SaveChanges();



            return RedirectToAction("Index", "BuyBook");
        }


        public ActionResult Delete(int? id)
        {
            var query = db.Books.SingleOrDefault(m => m.BookId == id);
            db.Books.Remove(query);
            db.SaveChanges();
            return RedirectToAction("Index", "BuyBook");
        }
    }
}