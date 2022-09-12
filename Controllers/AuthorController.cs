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
    public class AuthorController : Controller
    {
        BookManagerEntities db = new BookManagerEntities();
        // GET: Author
        #region author
        public ActionResult Index()
        {
            var ui = (int)Session["uid"];
            var q = db.Authors.Where(x => x.UserId == ui).ToList();
            return View(q);
        }
        #endregion
        #region Author Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Author a)
        {
            try
            {
                var uid = (int)Session["uid"];
                var ed = db.Authors.Where(x => x.AuthorName == a.AuthorName && x.UserId == uid).SingleOrDefault();
                if (ed != null)
                {
                    TempData["msg"] = "Author Name has already been added! Try another...";
                    return View();
                }
                else
                {
                    Author aut = new Author();
                    aut.AuthorName = a.AuthorName;
                    aut.UserId = (int)Session["uid"];
                    db.Authors.Add(aut);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Author");

                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex;
                return View();
            }


        }
        #endregion

        #region Edit
        public ActionResult Edit(int? id)
        {


            var query = db.Authors.Where(m => m.AuthorId == id).ToList().FirstOrDefault();
            return View(query);

        }

        [HttpPost]
        public ActionResult Edit(Author a)
        {
            try
            {

                db.Entry(a).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "Author");
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex;
            }
            return RedirectToAction("Index", "Author");
        }

        public ActionResult Delete(int? id)
        {
            var query = db.Authors.SingleOrDefault(m => m.AuthorId == id);
            db.Authors.Remove(query);
            db.SaveChanges();
            return RedirectToAction("Index", "Author");
        }



        #endregion
    }
}