using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Facebook
{
    public class PhotoCsvRecord
    {
        public string PersonId { get; set; }

        public string AlbumId { get; set; }

        public string Name { get; set; }

        public string PhotoId { get; set; }

        public PhotoCsvRecord(string csvLine)
        {
            var splitted = csvLine.Split(',');

            PersonId = splitted[0];
            AlbumId = splitted[1];
            Name = splitted[2];
            PhotoId = splitted[3];
        }
    }
}
