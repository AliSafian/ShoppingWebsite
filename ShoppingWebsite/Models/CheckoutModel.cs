using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingWebsite.Models
{
    public class CheckoutModel
    {
        public int UserID { get; set; }
      
       
        public string Name { get; set; }
       
        public string Mobile { get; set; }

        public Double Payable { get; set; }
        public string Address { get; set; }

        public string PaymentType { get; set; } 
        public List<CartItem> ProductsDetail { get; set; }

       

       
    }
}