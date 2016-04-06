using System.Configuration;
using System.IO;
using System.Net;
using Facebook;
using Newtonsoft.Json.Linq;
using Project.Helpers;
using Project.Models;
using Project.Models.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.Weather;
using System.Xml;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
namespace Project.Controllers
{
    public class FbUser {
        
        public String Id { set; get; }
        public String Name { set; get; }
    }

    [RequireHttps]
    public class HomeController : Controller
    {
        ApplicationDbContext db;
        public HomeController()
        {
            db = new ApplicationDbContext();
            ViewBag.SelectedSearchCriteria = string.Empty;
            ViewBag.SelectedFromPrice = ViewBag.SelectedToPrice = "";
        }

        public ActionResult GetAside()
        {
            return PartialView("Weather");
        }

        public ActionResult GetBody(List<ProductSupllier> productsInput = null)
        {
            if (productsInput != null)
            {
                ViewBag.Products = productsInput;
            }
            else
            {
                ViewBag.Products = generalProductsQuery();
            }


            ViewBag.Departments = getDepartments(db);
            ViewBag.Suppliers = getSuppliers(db);

            return PartialView("ProductsDiv");
        }

        public ViewResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Search()
        {
            int departmentId = getDepartmentsInput();
            int supplierId = getSuppliersInput();
            string searchCriteria = getSearchCriteriaInput();

            //Admin better search 
            float fromPrice = 0f;
            float toPrice = float.MaxValue;
            if(User.IsInRole("Admin"))
            {
                fromPrice = getPriceFrom();
                toPrice = getPriceTo();
            }
            
            List<ProductSupllier> products = searchDB(searchCriteria, departmentId, supplierId, fromPrice, toPrice);
            return GetBody(products);
        }

        private void getStatistics()
        {
            //var stats = from st in db.StatisticsPerDay
            //            select new
            //            {
            //                Amount = st.Amount,
            //                Date = st.Date
            //            };
            //var c = stats.ToList();
            //List<SaleStat> dict = new List<SaleStat>();
            //foreach (var v in c)
            //{
            //    SaleStat s = new SaleStat()
            //    {
            //        date = v.Date,
            //        total = v.Amount
            //    };
            //    dict.Add(s);
            //}

            //ViewBag.StatsJson = dict;
            //ViewBag.Stats = c;
            return;
        }

        private void getWeather()
        {
            try
            {
                Weather.GlobalWeatherSoapClient client = new GlobalWeatherSoapClient("GlobalWeatherSoap12");
                var xmlCitiesIsrael = client.GetCitiesByCountry("israel");
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlCitiesIsrael);
                List<string> Cities = new List<string>();
                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    Cities.Add(node.InnerText.Replace("Israel", "")); //or loop through its children as well
                }

                Dictionary<string, Dictionary<string, string>> weatherOutput = new Dictionary<string, Dictionary<string, string>>();
                foreach (var city in Cities)
                {
                    var xml = client.GetWeather(city, "israel");
                    if (xml != "Data Not Found")
                    {
                        weatherOutput.Add(city, new Dictionary<string, string>());
                        XmlDocument docu = new XmlDocument();
                        docu.LoadXml(xml);

                        foreach (XmlNode node in docu.DocumentElement.ChildNodes)
                        {
                            if (node.Name == "Temperature")
                            {
                                weatherOutput[city].Add(node.Name, node.InnerText.Split('(')[1].Split(')')[0]);
                            }
                            else
                            {
                                weatherOutput[city].Add(node.Name, node.InnerText.Split('(')[0]);
                            }

                        }
                    }
                }
                ViewBag.Weather = weatherOutput;
            }
            catch (Exception ex)
            {

            }
        }
        private List<ProductSupllier> generalProductsQuery()
        {
            //var v = from product in db.Products
            //        join supplier in db.Suppliers
            //        on product.SupplierId equals supplier.SupllierID
            //        select new ProductSupllier
            //        {
            //            Image = product.Image,
            //            Price = product.Price,
            //            Title = product.Title,
            //            Productid = product.ProductId,
            //            Type = product.Type,
            //            Description = product.Description,
            //            SupplierName = supplier.Name,
            //            SupplierAddress = supplier.address
            //        };

            return new List<ProductSupllier> ();
        }

        private int getDepartmentsInput()
        {
            var inputDepartmentId = Request.Params["department-dropdown"];
            ViewBag.SelectedDepartment = int.Parse(inputDepartmentId);
            return int.Parse(inputDepartmentId);
        }

        private int getSuppliersInput()
        {
            var inputSupplierId = Request.Params["supplier-dropdown"];
            ViewBag.SelectedSupplier = int.Parse(inputSupplierId);
            return int.Parse(inputSupplierId);
        }

        private string getSearchCriteriaInput()
        {
            var inputSearchCriteria = Request.Params["criteria"];
            ViewBag.SelectedSearchCriteria = inputSearchCriteria;
            return inputSearchCriteria ?? "";
        }

        private float getPriceTo()
        {
            var inputSearchCriteria = Request.Params["price-to"].Length > 0 ? Request.Params["price-to"] : float.MaxValue.ToString();
            ViewBag.SelectedToPrice = inputSearchCriteria;
            return float.Parse(inputSearchCriteria);
        }

        private float getPriceFrom()
        {
            var inputSearchCriteria = Request.Params["price-from"].Length > 0 ? Request.Params["price-from"] : "0";
            ViewBag.SelectedFromPrice = inputSearchCriteria;
            return float.Parse(inputSearchCriteria);
        }

        private List<ProductSupllier> searchDB(string criteria, int departmentId, int supplierId, float fromPrice, float toPrice)
        {
            List<ProductSupllier> output = new List<ProductSupllier>();
            //if (departmentId == 0 && supplierId == 0)
            //{
            //    output = generalProductsQuery();
            //}
            //else if(supplierId == 0)
            //{
            //    var prods = from product in db.Products
            //                join supplier in db.Suppliers
            //                on product.SupplierId equals supplier.SupllierID
            //                where product.DepartmentId == departmentId
            //                select new ProductSupllier
            //                {
            //                    Image = product.Image,
            //                    Price = product.Price,
            //                    Title = product.Title,
            //                    Productid = product.ProductId,
            //                    Type = product.Type,
            //                    Description = product.Description,
            //                    SupplierName = supplier.Name,
            //                    SupplierAddress = supplier.address
            //                };
            //    output = prods.ToList();

            //}
            //else if(departmentId == 0)
            //{
            //    var prods = from product in db.Products
            //                join supplier in db.Suppliers
            //                on product.SupplierId equals supplier.SupllierID
            //                where supplier.SupllierID == supplierId
            //                select new ProductSupllier
            //                {
            //                    Image = product.Image,
            //                    Price = product.Price,
            //                    Title = product.Title,
            //                    Productid = product.ProductId,
            //                    Type = product.Type,
            //                    Description = product.Description,
            //                    SupplierName = supplier.Name,
            //                    SupplierAddress = supplier.address
            //                };
            //    output = prods.ToList();
            //}
            //else
            //{
            //    var prods = from product in db.Products
            //                join supplier in db.Suppliers
            //                on product.SupplierId equals supplier.SupllierID
            //                where supplier.SupllierID == supplierId && product.DepartmentId == departmentId
            //                select new ProductSupllier
            //                {
            //                    Image = product.Image,
            //                    Price = product.Price,
            //                    Title = product.Title,
            //                    Productid = product.ProductId,
            //                    Type = product.Type,
            //                    Description = product.Description,
            //                    SupplierName = supplier.Name,
            //                    SupplierAddress = supplier.address
            //                };
            //    output = prods.ToList();
            //}

            //if(!string.IsNullOrEmpty(criteria))
            //{
            //    output = output.Where(x => x.Title.ToLower().Contains(criteria.ToLower())).ToList();
            //}

            //output = output.Where(x => x.Price >= fromPrice && x.Price <= toPrice).ToList();
            return output;
        }

        private List<Suppliers> getSuppliers(ApplicationDbContext db)
        {
            //var suppliers = from sup in db.Suppliers
            //                  select new
            //                  {
            //                      SupllierID = sup.SupllierID,
            //                      Name = sup.Name,
            //                      Address = sup.address
            //                  };

            List<Suppliers> l = new List<Suppliers>();
            //foreach (var d in suppliers)
            //{
            //    Suppliers su = new Suppliers()
            //    {
            //        SupllierID = d.SupllierID,
            //        Name = d.Name,
            //        address = d.Address
            //    };
            //    l.Add(su);
            //}
            return l;
        }

        private List<Departments> getDepartments(ApplicationDbContext db)
        {
            //var departments = from dep in db.Departments
            //                  select new
            //                  {
            //                      DepartmentId = dep.DepartmentId,
            //                      Name = dep.Name,
            //                      ManagerName = dep.Name
            //                  };

            List<Departments> l = new List<Departments>();
            //foreach (var d in departments)
            //{
            //    Departments de = new Departments()
            //    {
            //        DepartmentId = d.DepartmentId,
            //        Name = d.Name,
            //        ManagerName = d.ManagerName
            //    };
            //    l.Add(de);
            //}
            return l;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}