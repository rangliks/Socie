using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbHandler.Objects
{
    public class UserPhotosEmotions
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public List<PhotoAndEmotions> photosEmotions { get; set; }
    }
}
