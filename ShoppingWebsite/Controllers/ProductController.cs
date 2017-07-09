using DAL;
using ShoppingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingWebsite.Controllers
{
    public class ProductController : Controller
    {
        private ShoppingWebsiteEntities db = new ShoppingWebsiteEntities();
        public ActionResult Index()
        {
            if(Session["user"]==null)
            {
                return Redirect("~/Home/Index");
            }
            Userss User= (Userss)Session["user"];

            if(User.UserType !=1)
            {
                return Redirect("~/Home/Index");
            }

            var products = db.Products.ToList();
            List<ProductModel> productList = new List<ProductModel>();
            foreach (Product item in products)
            {
                if (item.IsActive == 1)
                {
                    productList.Add(new ProductModel
                    {

                        ProductID = item.ProductID,
                        Name = item.Name,
                        Description = item.Description,
                        Price = item.Price,
                        PictureName = item.PictureName

                    });
                }
            }
            return View(productList);
        }
        public ActionResult GetProduct(int id)
        {
            var result = db.Products.Where(x => x.CategoryID == id).ToList();
            List<ProductModel> productList = new List<ProductModel>();
            String CategoryName = "";
            foreach (Product p in result)
            {
                ProductModel dto = new ProductModel();

                dto.ProductID = p.ProductID;
                dto.Name = p.Name;
                dto.Description = p.Description;
                dto.Price = p.Price;
                dto.PictureName = p.PictureName;
                dto.IsActive = p.IsActive;
                dto.CreatedOn = p.CreatedOn;
                dto.ModifiedOn = p.ModifiedOn;
                dto.CategoryID = p.CategoryID;
                CategoryName = p.Category.CategoryName;
                productList.Add(dto);
            }
            ViewBag.CName = CategoryName;
            ViewBag.ActiveCategory = id;
            return View(productList);
        }
        
        public ActionResult New()
        {
            if (Session["user"] == null)
            {
                return Redirect("~/Home/Index");
            }
            Userss User = (Userss)Session["user"];

            if (User.UserType != 1)
            {
                return Redirect("~/Home/Index");
            }

            var list = db.Categories.ToList();
            List<SelectListItem> cList = new List<SelectListItem>();
            foreach(Category c in list )
            {
                cList.Add(new SelectListItem { Value = Convert.ToString(c.CategoryID), Text = c.CategoryName });
            }

            ViewBag.CategoryList = cList;

            return View(new ProductModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(ProductModel product)
        {
            if (Session["user"] == null)
            {
                return Redirect("~/Home/Index");
            }
            Userss User = (Userss)Session["user"];

            if (User.UserType != 1)
            {
                return Redirect("~/Home/Index");
            }

            if(ModelState.IsValid)
            {
                var uniqueName = "";
                if (Request.Files["image"] != null)
                {
                    var file = Request.Files["image"];
                    if(file.FileName != "")
                    {
                        var ext = System.IO.Path.GetExtension(file.FileName);
                        uniqueName = Guid.NewGuid().ToString() + ext;
                        var rootPath = Server.MapPath("~/Images");
                        var fileSavePath = System.IO.Path.Combine(rootPath, uniqueName);

                        file.SaveAs(fileSavePath);
                        product.PictureName = uniqueName;

                    }
                }

                Product prod = null; 
                if(product.ProductID !=0)
                {
                    prod = db.Products.Find(product.ProductID);
                    if (prod != null)
                    {
                        prod.Name = product.Name;
                        prod.Description = product.Description;
                        prod.Price = product.Price;
                        prod.PictureName = product.PictureName;
                        prod.ModifiedOn = DateTime.Now;
                    }
                    
                    TempData["Msg"] = "Product Updated Successfully";
                }
                else
                {
                    prod = new Product();
                    prod.Name = product.Name;
                    prod.Description = product.Description;
                    prod.Price = product.Price;
                    prod.PictureName = product.PictureName;
                    prod.CreatedOn = DateTime.Now;
                    prod.IsActive = 1;
                    prod.CategoryID = product.CategoryID;
                    TempData["Msg"] = "Product Created Successfully";
                    db.Products.Add(prod);
                    
                }
                
                db.SaveChanges();
               
                return Redirect("Index");
            }
            var list = db.Categories.ToList();
            List<SelectListItem> cList = new List<SelectListItem>();
            foreach (Category c in list)
            {
                cList.Add(new SelectListItem { Value = Convert.ToString(c.CategoryID), Text = c.CategoryName });
            }

            ViewBag.CategoryList = cList;
            return View(new ProductModel());
        }
        public ActionResult Delete(int id)
        {
            if (Session["user"] == null)
            {
                return Redirect("~/Home/Index");
            }
            Userss User = (Userss)Session["user"];

            if (User.UserType != 1)
            {
                return Redirect("~/Home/Index");
            }

            var result = db.Products.Find(id);
            if(result != null)
            {
                result.IsActive = 0;
                result.ModifiedOn = DateTime.Now;
                db.SaveChanges();
            }
            TempData["Msg"] = "Product Deleted Successfully";
            return RedirectToAction("Index", "Product");
        }

        public ActionResult Edit(int id)
        {
            if (Session["user"] == null)
            {
                return Redirect("~/Home/Index");
            }
            Userss User = (Userss)Session["user"];

            if (User.UserType != 1)
            {
                return Redirect("~/Home/Index");
            }

            var prod = db.Products.Find(id);
            ProductModel model = new ProductModel();
            if(prod != null)
            {
                model.ProductID = prod.ProductID;
                model.Name = prod.Name;
                model.Description = prod.Description;
                model.PictureName = prod.PictureName;
                model.Price = prod.Price;
                model.CreatedOn = prod.CreatedOn;
                model.ModifiedOn = prod.ModifiedOn;
                model.IsActive = prod.IsActive;
                model.CategoryID = prod.CategoryID;

            }
            return View("New" , model);
        }

        public ActionResult Single(int id)
        {
            var result = db.Products.Find(id);
            ProductModel model = new ProductModel();

            if(result!= null)
            {
                model.ProductID = result.ProductID;
                model.Name = result.Name;
                model.PictureName = result.PictureName;
                model.Price = result.Price;
                model.Description = result.Description;
                model.CategoryID = result.CategoryID;
                model.IsActive = result.IsActive;
                model.CreatedOn = result.CreatedOn;
                model.ModifiedOn = result.ModifiedOn;
            }

            return View(model);
        }

        public ActionResult Detail(int id)
        {
            if (Session["user"] == null)
            {
                return Redirect("~/Home/Index");
            }
            Userss User = (Userss)Session["user"];

            if (User.UserType != 1)
            {
                return Redirect("~/Home/Index");
            }

            var result = db.Products.Find(id);
            ProductModel model = new ProductModel();

            if (result != null)
            {
                model.ProductID = result.ProductID;
                model.Name = result.Name;
                model.PictureName = result.PictureName;
                model.Price = result.Price;
                model.Description = result.Description;
                model.CategoryID = result.CategoryID;
                model.IsActive = result.IsActive;
                model.CreatedOn = result.CreatedOn;
                model.ModifiedOn = result.ModifiedOn;
            }
            return View(model);
        }
	}
}