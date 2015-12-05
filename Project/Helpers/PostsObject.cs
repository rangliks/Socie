using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Helpers
{
    public class Datum
    {
        public string message { get; set; }
        public string created_time { get; set; }
        public string id { get; set; }
        public string story { get; set; }
    }

    public class Paging
    {
        public string previous { get; set; }
        public string next { get; set; }
    }

    public class PostsObject
    {
        public List<Datum> data { get; set; }
        public Paging paging { get; set; }
    }
}