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
        public string AlbumId { set; get; }
        public string Name { set; get; }
        public string Source { get; set; }

        public Photo()
        {
            CreationDate = DateTime.Now;
        }
    }
}