using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FacebookTools.FacebookObjects
{
    [Table("Photos")]
    public class Photo
    {
        public string PhotoId { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Tag> Tags { set; get; }

        public Photo()
        {
            Tags = new List<Tag>();
        }
    }
}