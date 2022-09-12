using MyBookStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBookStore.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        BookManagerEntities db = new BookManagerEntities();
        // GET: Book
        
        public ActionResult Index()
        {
            var ui = (int)Session["uid"];
            var q = db.Books.Where(x => x.BookStatu.Status != "Buyable" && x.Reading.ReadingStatus != null
                                    && x.UserId == ui).ToList();
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
            Book book = new Book();
            List<Category> CategoryList = db.Categories.Where(x => x.UserId == ui).ToList();
            ViewBag.CategoryList = new SelectList(CategoryList, "CategoryID", "CategoryName");
            List<Author> AuthorList = db.Authors.Where(x => x.UserId == ui).ToList();
            ViewBag.AuthorList = new SelectList(AuthorList, "AuthorId", "AuthorName");
            List<BookStatu> SatList = db.BookStatus.ToList();
            ViewBag.SatList = new SelectList(SatList, "BookStatusId", "Status");
            b.ReadingId = 3;
           
            b.UserId = (int)Session["uid"];
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
            if (b.CategoryId == null || b.AuthorId == null || b.BookName == null || b.BookStatusId == null || b.BuyingDate == null)
            {
                TempData["msg"] = "Book isn't Added!" +
                   "You must fill all the '*' sections..";
                return RedirectToAction("Create", "Book");
            }
            db.Books.Add(b);
            db.SaveChanges();



            return RedirectToAction("Index", "Book");

        }




        //Get Edit

        
        [HttpGet]
        public ActionResult Edit(int? id)
        {

            var ui = (int)Session["uid"];
            Session["bookid"] = id;
            List<Category> CategoryList = db.Categories.Where(x => x.UserId == ui).ToList();
            ViewBag.CategoryList = new SelectList(CategoryList, "CategoryId", "CategoryName");
            List<Author> AuthorList = db.Authors.Where(x => x.UserId == ui).ToList();
            ViewBag.AuthorList = new SelectList(AuthorList, "AuthorId", "AuthorName");
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
                Session["image"] = query.Photo;
                Session["buydate"] = query.BuyingDate;
                return View(query);
            }


        }

        //Post Edit
        [HttpPost]
        public ActionResult Edit(Book b, HttpPostedFileBase Image)
        {
            
            var username = Session["username"].ToString();
            
            
            var ui = (int)Session["uid"];
            List<Category> CategoryList = db.Categories.Where(x => x.UserId == ui).ToList();
            ViewBag.CategoryList = new SelectList(CategoryList, "CategoryId", "CategoryName");
            List<Author> AuthorList = db.Authors.Where(x => x.UserId == ui).ToList();
            ViewBag.AuthorList = new SelectList(AuthorList, "AuthorId", "AuthorName");
            List<BookStatu> SatList = db.BookStatus.ToList();
            ViewBag.SatList = new SelectList(SatList, "BookStatusId", "Status");
            List<Reading> ReadList = db.Readings.ToList();
            ViewBag.ReadList = new SelectList(ReadList, "ReadingId", "ReadingStatus");
            try
            {

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
                    b.Photo = Session["image"].ToString();
                    //b.Photo = bookdetail.Photo;

                }

                if(b.BuyingDate==null)
                {
                    b.BuyingDate =(DateTime) Session["buydate"];
                }
               
                if (b.ReadingId == 4)
                {
                    b.StartDate =DateTime.Today;
                }

                db.Entry(b).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index", "Book");


            }
            catch(Exception ex)
            {
                TempData["msg"] = "Detail isn't updated!"+ex;

                return RedirectToAction("Edit", "Book");
            }
        }
       
        public ActionResult Delete(int? id)
        {
            var query = db.Books.SingleOrDefault(m => m.BookId == id);
            db.Books.Remove(query);
            db.SaveChanges();
            return RedirectToAction("Index", "Book");
        }
        
        public ActionResult Completed()
        {
            var ui = (int)Session["uid"];
            var q = db.Books.Where(x => x.Reading.ReadingStatus == "Completed" && x.UserId == ui).ToList();
            return View(q);
        }

    }
}