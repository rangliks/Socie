using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookTools.FacebookObjects
{
    [Table("PhotoAlbum")]
    public class PhotoAlbum
    {
        [Key]
        public string AlbumId { get; set; }
        public string PersonId { get; set; }
        public string Name { get; set; }
    }
}
