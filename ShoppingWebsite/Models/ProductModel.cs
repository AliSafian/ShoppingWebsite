using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoppingWebsite.Models
{
    public class ProductModel
    {
        [Required]
        public int ProductID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        
        public string PictureName { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public int IsActive { get; set; }
        public int CategoryID { get; set; }
    }
}