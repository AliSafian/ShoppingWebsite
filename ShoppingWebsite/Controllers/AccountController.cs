using DAL;
using ShoppingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingWebsite.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        public ActionResult Login()
        {
            return View(new LoginModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                var db =new ShoppingWebsiteEntities();

                var User = db.Userss.Where(m => m.Email == login.Email.ToLower()).ToList();

               if(User.Count!=0)
                {
                    if(login.Email.ToLower().Equals(User[0].Email) && login.Password.Equals(User[0].Password))
                    {
                        Session["user"] = User[0];
                        
                        //string uri = Request.RawUrl;
                        if(User[0].UserType == 1)
                        {
                            return Redirect("~/Product/Index");
                        }
                        return Redirect("~/Home/Index");
                    }
                    else
                    {
                        Session["user"] = null;
                        TempData["Msg"] = "Invalid Password";
                    }
                    
                }
                else
                {
                    Session["user"] = null;
                    TempData["Msg"] = "User Does Not Exist";
                }


            }
            return View(login);
        }
        public ActionResult Register()
        {
            return View(new UserModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserModel user)
        {
            if (ModelState.IsValid)
            {
                var uniqueName = "";
                if (Request.Files["image"] != null)
                {
                    var file = Request.Files["image"];
                    if (file.FileName != "")
                    {
                        var ext = System.IO.Path.GetExtension(file.FileName);
                        uniqueName = Guid.NewGuid().ToString() + ext;
                        var rootPath = Server.MapPath("~/Images/Users");
                        var fileSavePath = System.IO.Path.Combine(rootPath, uniqueName);

                        file.SaveAs(fileSavePath);
                        user.PictureName = uniqueName;

                    }
                }

                Userss dto = new Userss();

                dto.FirstName = user.FirstName;
                dto.LastName = user.LastName;
                dto.Email = user.Email.ToLower();
                dto.Mobile = user.Mobile;
                dto.Password = user.Password;
                dto.PictureName = user.PictureName;
                dto.UserType = 2;
                dto.IsActive = 1;
                dto.CreatedOn = DateTime.Now;
                dto.Address1 = user.Address1;
                dto.Address2 = user.Address2;
                dto.Gender = user.Gender;

                var db =new ShoppingWebsiteEntities();

                db.Userss.Add(dto);
                db.SaveChanges();
                Session["user"] = dto;
                return Redirect("~/Home/Index");
                
            }

           
            return View(user);
        }
        public ActionResult Logout()
        {
            Session["user"] = null;
            Session["items"] = new List<CartItem>();
            Session["total"] = 0.0;
            return Redirect("~/Home/Index");

        }
	}
}