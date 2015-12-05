using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models.Store
{
    public class Departments
    {
        [Key]
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string ManagerName { get; set; }
    }
}