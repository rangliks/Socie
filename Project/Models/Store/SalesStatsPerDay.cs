using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models.Store
{
    public class SalesStatsPerDay
    {
        [Key]
        public int ID { get; set; }
        public float Amount { get; set; }
        public DateTime Date { get; set; }
    }
}