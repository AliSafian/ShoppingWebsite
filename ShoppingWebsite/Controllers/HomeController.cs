using DAL;
using ShoppingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingWebsite.Controllers
{
    public class HomeController : Controller
    {
        private ShoppingWebsiteEntities db = new ShoppingWebsiteEntities();
        public ActionResult Index()
        {   
            if(Session["items"] == null)
            {
                List<CartItem> cList = new List<CartItem>();
                Double toTalCost = 0;

                Session["items"] = cList;
                Session["total"] = toTalCost;
            }
           
             return View();
        }
        
    }

}