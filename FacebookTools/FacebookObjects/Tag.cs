using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FacebookTools.FacebookObjects
{
    [Table("Tag")]
    public class Tag
    {
        public string TagId { get; set; }
        public DateTime CreationDate { get; set; }
        public Person PersonTagged { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public Tag(string tag)
        {
            JObject taggedJson = JObject.Parse(tag);
            try
            {
                TagId = taggedJson["id"].ToString();
            }
            catch (Exception ex)
            {
                
            }

            CreationDate = DateTime.Parse(taggedJson["created_time"].ToString());

            PersonTagged = new Person();
            PersonTagged.Name = taggedJson["name"].ToString();
            PersonTagged.PersonId = taggedJson["id"].ToString();

            Y = float.Parse(taggedJson["y"].ToString());
            X = float.Parse(taggedJson["x"].ToString());
        }
    }
}