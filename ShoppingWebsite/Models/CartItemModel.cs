using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingWebsite.Models
{
    public class CartItemModel
    {
        public int ItemID { get; set; }
        public int ProductID { get; set; }
        public double Cost { get; set; }
        public int Quantity { get; set; }
        public int CartID { get; set; }
    }
}