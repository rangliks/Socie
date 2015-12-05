using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages.Html;

namespace Project.Models.Store
{
    public class DepartmentsViewModel
    {
        // Display Attribute will appear in the Html.LabelFor
        [Display(Name = "Departments")]
        public int SelectedDepartmentId { get; set; }
        public SelectList Departments { get; set; }
    }
}