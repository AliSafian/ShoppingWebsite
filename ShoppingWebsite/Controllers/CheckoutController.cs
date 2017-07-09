using DAL;
using ShoppingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingWebsite.Controllers
{
   
    public class CheckoutController : Controller
    {
        
        private ShoppingWebsiteEntities db = new ShoppingWebsiteEntities();
        public ActionResult Index()
        {
            List<CartItem> cList = (List<CartItem>)Session["items"];

            return View(cList);
        }
        public ActionResult Add(CartItemModel model)
        {
            if(model.Quantity <=0)
            {
                TempData["Msg"] = "Invalid Quantity";
                return Redirect("~/Product/Single/" + model.ProductID.ToString());
            }
            var prod = db.Products.Find(model.ProductID);
            if ( prod != null)
            {
                List<CartItem> cList=(List<CartItem>)Session["items"];

                Double totalCost = (Double)Session["total"];
                int index = IsExist(model.ProductID);
                if(index != -1)
                {
                    cList[index].Quantity += model.Quantity;
                    cList[index].Cost += model.Quantity * prod.Price;
                    totalCost += model.Quantity * prod.Price;
                }
                else
                {
                    CartItem item = new CartItem();
                    item.ProductID = prod.ProductID;
                    item.Quantity = model.Quantity;
                    item.Cost = model.Quantity * prod.Price;
                    item.Product = prod;
                    cList.Add(item);
                    totalCost += item.Cost;
                }
                
               
               
                Session["total"] = totalCost;
            }

            return Redirect("~/Product/Single/"+ model.ProductID.ToString());
        }
        public ActionResult Delete(int ProductID)
        {
            List<CartItem> cList = (List<CartItem>)Session["items"];
            Double totalCost = (Double)Session["total"];

            int index = IsExist(ProductID);
            if(index !=-1 )
            {
                CartItem item = cList[index];
                totalCost -= item.Cost;
                
                Session["total"] = totalCost;

                cList.Remove(item);
                Session["items"] = cList;
            }
            return View("Index", cList);
        }
        public ActionResult Empty()
        {
            EmptyCart();
            return Redirect("~/Home/Index");
        }
        private void EmptyCart()
        {
            List<CartItem> cList = (List<CartItem>)Session["items"];
            Session["total"] = 0.0;

            for (int i = 0; i < cList.Count; i++)
            {
                cList.RemoveAt(i);
            }
            Session["items"] = new List<CartItem>();
        }
        public ActionResult CheckoutDetail()
        {
            
            Userss user =(Userss)Session["user"];
            List<CartItem> cList = (List<CartItem>)Session["items"];
            Double totalCost = (Double)Session["total"];
            if(user == null)
            {
                TempData["Msg"] = "You Must Login First";
                return Redirect("~/Account/Login");
            }
            if(cList.Count == 0)
            {

                return Redirect("~/Checkout/Index");
            }
            //creating empty cart
            Cart cart = new Cart();
            cart.TotalCost = totalCost;
            db.Carts.Add(cart);

            db.SaveChanges();
            //adding items in cart
            foreach(CartItem cItem in cList)
            {
                CartItem item = new CartItem();

                item.ProductID = cItem.ProductID;
                item.Quantity = cItem.ProductID;
                item.Cost = cItem.Cost;
                item.CartID = cart.CartID;

                db.CartItems.Add(item);
                db.SaveChanges();
            }
            //making payment

            Payment payment = new Payment();
            payment.Amount = cart.TotalCost;
            payment.CartID = cart.CartID;
            payment.PayDate = DateTime.Now;
            payment.PayType = "On Delivry";
            payment.CustomerID = user.UserID;
            db.Payments.Add(payment);
            db.SaveChanges();

            TempData["Msg"] = "Thank You: Here is your Shopping Detail";
            

            CheckoutModel model = new CheckoutModel();

            model.UserID = user.UserID;
            model.Name = user.FirstName + " " + user.LastName;
            model.Mobile = user.Mobile;

            model.Address = user.Address1;
            model.PaymentType = "On Delivery";

            model.Payable = totalCost;
            model.ProductsDetail = new List<CartItem>();
            foreach (CartItem cItem in cList)
            {
                CartItem item = new CartItem();
                var prod = db.Products.Find(cItem.ProductID);
                item.ProductID = cItem.ProductID;
                item.Quantity = cItem.ProductID;
                item.Cost = cItem.Cost;
                item.Product = prod;

                model.ProductsDetail.Add(item);
            }
            EmptyCart();
            return View(model);
        }

        private int IsExist(int pid)
        {
            List<CartItem> cList = (List<CartItem>)Session["items"];
            for(int i =0; i < cList.Count ; i++)
            {
                if(cList[i].ProductID == pid)
                {
                    return i;
                }
            }
            return -1;
        }
	}
}