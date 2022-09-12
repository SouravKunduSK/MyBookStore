﻿using MyBookStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBookStore.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        BookManagerEntities db = new BookManagerEntities();
        // GET: Category
        public ActionResult Index()
        {
            var ui = (int)Session["uid"];
            var q = db.Categories.Where(x => x.UserId == ui).ToList();
            return View(q);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category c)
        {
            try
            {
                var uid = (int)Session["uid"];
                var ed = db.Categories.Where(x => x.CategoryName == c.CategoryName && x.UserId == uid).SingleOrDefault();
                if (ed != null)
                {
                    TempData["msg"] = "Category Name has already been added! Try another...";
                    return View();
                }
                else
                {

                    Category cat = new Category();
                    cat.CategoryName = c.CategoryName;
                    cat.UserId = (int)Session["uid"];
                    db.Categories.Add(cat);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Category");

                }
            }

            catch (Exception ex)
            {
                TempData["msg"] = ex;
                return View();
            }


        }

        #region Edit
        public ActionResult Edit(int? id)
        {


            var query = db.Categories.Where(m => m.CategoryId == id).ToList().FirstOrDefault();
            return View(query);

        }

        [HttpPost]
        public ActionResult Edit(Category c)
        {
            try
            {

                db.Entry(c).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "Category");
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex;
            }
            return RedirectToAction("Index", "Category");
        }

        public ActionResult Delete(int? id)
        {
            var query = db.Categories.SingleOrDefault(m => m.CategoryId == id);
            db.Categories.Remove(query);
            db.SaveChanges();
            return RedirectToAction("Index", "Category");
        }

        //Category Ends

        #endregion
    }
}