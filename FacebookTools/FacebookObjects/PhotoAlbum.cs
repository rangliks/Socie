using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookTools.FacebookObjects
{
    [Table("PhotoAlbums")]
    public class PhotoAlbum
    {
        [Key]
        public string AlbumId { get; set; }
        public Person PersonId { get; set; }
    }
}
