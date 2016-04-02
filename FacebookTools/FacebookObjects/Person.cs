using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FacebookTools.FacebookObjects
{
    [Table("Person")]
    public class Person
    {
        public string PersonId { get; set; }
        public string SocieId { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public string Relation { get; set; }

        public Person()
        {
            PersonId = string.Empty;
            Name = string.Empty;
            Relation = string.Empty;
        }

        public Person(string tag)
        {
            JObject personJson = JObject.Parse(tag);
            try
            {
                PersonId = personJson["id"].ToString();
                Name = personJson["name"].ToString();

                Relation = personJson["relation"].ToString();
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}