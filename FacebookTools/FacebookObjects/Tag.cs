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
        public string PhotoId { get; set; }
        public DateTime CreationDate { get; set; }
        public string PersonId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }

        public Tag()
        {

        }

        public Tag(string tag, string photoid)
        {
            PhotoId = photoid;

            JObject taggedJson = JObject.Parse(tag);
            try
            {
                TagId = taggedJson["id"].ToString();
            }
            catch (Exception ex)
            {
                
            }

            CreationDate = DateTime.Parse(taggedJson["created_time"].ToString());

            PersonId = taggedJson["id"] != null ? taggedJson["id"].ToString() : null;

            if (taggedJson["y"] != null && taggedJson["x"] != null)
            {
                Y = float.Parse(taggedJson["y"].ToString());
                X = float.Parse(taggedJson["x"].ToString());
            }
        }
    }
}