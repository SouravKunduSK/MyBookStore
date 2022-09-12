using MyBookStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace MyBookStore.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        BookManagerEntities db = new BookManagerEntities();
        [AllowAnonymous]
        #region register
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var un = db.Users.Count();
                if (un < 2)
                {
                    var isExist = IsEmailExist(user.Email);
                    if (!isExist)
                    {
                        User u = new User();
                        u.FirstName = user.FirstName;
                        u.LastName = user.LastName;
                        u.Username = user.FirstName + " " + user.LastName;
                        u.Email = user.Email;
                        u.Password = user.Password;
                        u.HashPassword = Crypto.Hash(user.Password);
                        u.RegDate = DateTime.Now.Date;
                        u.Photo = "~/Uploads/" + "NoImage.png";
                        u.RoleId = 2;

                        db.Users.Add(u);
                        db.SaveChanges();

                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        ViewBag.Message = "Email has already been Register!!";
                    }
                }

                else
                {
                    ViewBag.Message = "Currently Registration is not possible!! Try Again Later..." +
                        "Thank You......";
                }


            }
            else
            {
                ViewBag.Message = "Not Register!! Try Again...";
            }
            return View();
        }
        #endregion register
        #region login
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Logout", "Account");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserViewModel user)
        {

            var cust = db.Users.SingleOrDefault(x => x.Email == user.Email);
            if (cust != null && cust.RoleId == 2)
            {
                if (string.Compare(Crypto.Hash(user.Password), cust.HashPassword) == 0)
                {
                    Session["uid"] = cust.UserId;
                    Session["username"] = cust.Username;
                    FormsAuthentication.SetAuthCookie(cust.Email, false);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Email or Password Error!";
                }
            }

            else
            {
                ViewBag.Message = "Email or Password Error!";
            }
            return View();
        }

        #endregion
        #region profile
        [Authorize]
        public ActionResult Index(int? id)
        {
            var ui = id;
            var u = db.Users.Find(ui);
            return View(u);
        }

        #endregion
        [Authorize]
        #region Picture Edit
        public ActionResult Edit(int? id)
        {
            var query = db.Users.Where(m => m.UserId == id).ToList().SingleOrDefault();

            if (query == null)
            {
                return HttpNotFound();
            }
            else
            {
                Session["image"] = query.Photo;
                Session["regdate"] = query.RegDate;
                return View(query);
            }


        }

        
        [HttpPost]
        public ActionResult Edit(User u, HttpPostedFileBase Image)
        {
            try
            {

                if (Image != null)
                {
                    var username = Session["username"].ToString();
                    string fileName = Path.GetFileNameWithoutExtension(Image.FileName);
                    string extension = Path.GetExtension(Image.FileName);

                    fileName = fileName + username + extension;
                    u.Photo = "~/Uploads/" + fileName;

                    var folder = Server.MapPath("~/Uploads/");
                    Image.SaveAs(Path.Combine(folder, fileName));

                    //Path.Combine(Server.MapPath("~/Uploads/"), fileName);
                    //Image.SaveAs(fileName);

                }
                else if (Image == null)
                {
                    u.Photo = "~/Uploads/" + "NoImage.png";

                }
                else
                {
                    u.Photo = Session["image"].ToString();
                }

                db.Entry(u).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", new RouteValueDictionary(new { Controller = "Account", Action = "Index", id = (int)Session["uid"] }));


            }
            catch
            {
                TempData["msg"] = "Profile picture isn't updated!";
                return RedirectToAction("Edit", "Account");
            }
        }
        #endregion

        //#region ChangePassword
        //public ActionResult EditPassword(int?id)
        //{
        //    var query = db.Users.Where(m => m.UserId == id).ToList().SingleOrDefault();

        //    if (query == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    else
        //    {
        //        Session["pass"] = query.Password;
        //        Session["hashpass"] = query.HashPassword;
        //        return View(query);
        //    }
        //}


        //public ActionResult EditPassword(User u)
        //{
        //    try
        //    {
        //        var uid = (int)Session["uid"];
        //        var usr = db.Users.Where(m => m.UserId == uid).SingleOrDefault();

        //        if (u != null)
        //        {
                   
        //           if(u.Password==usr.Password && u.HashPassword==usr.HashPassword)
        //            {

        //                db.Entry(u).State = EntityState.Modified;
        //                db.SaveChanges();

        //                return RedirectToAction("Index", new RouteValueDictionary(new { Controller = "Account", Action = "Index", id = uid }));
        //            }

        //        }
               

        //        db.Entry(u).State = EntityState.Modified;
        //        db.SaveChanges();

        //        return RedirectToAction("Index", new RouteValueDictionary(new { Controller = "Account", Action = "Index", id = (int)Session["uid"] }));


        //    }
        //    catch
        //    {
        //        TempData["msg"] = "Profile picture isn't updated!";
        //        return RedirectToAction("Edit", "Account");
        //    }
        //}
        //#endregion

        public ActionResult Logout()
        {
            Session["uid"] = null;
            Session["username"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }


        [NonAction]
        public bool IsEmailExist(string email)
        {
            var v = db.Users.Where(x => x.Email == email).FirstOrDefault();
            return v != null;
        }

     
    }
}