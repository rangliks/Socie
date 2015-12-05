using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models.Store
{
    public class Suppliers
    {
        [Key]
        public int SupllierID { get; set; }
        public string Name { get; set; }
        public string address { get; set; }
        public string PhoneNumber { get; set; }
        public virtual List<Product> Products{ get; set; }
    }
}