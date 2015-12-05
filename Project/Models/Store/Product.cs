using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models.Store
{
    public enum ProductTypes
    {
        FruitsAndVegtables,
        Cheese
    }

    public class Product
    {
        public int                      ProductId { get; set; }
        public string                   Title { get; set; }
        public string                   Description { get; set; }
        public string                   Image { get; set; }
        public ProductTypes             Type { get; set; }
        public float                    Price { get; set; }
        public float                    Amount { get; set; }
        //Foreign key for supplier
        
        public int SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual List<Suppliers> Suppliers { get; set; }

        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual List<Departments> Departments { get; set; }
    }
}