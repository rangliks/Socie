﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Helpers
{
    public class Datumz
    {
        public string message { get; set; }
        public string created_time { get; set; }
        public string id { get; set; }
        public string story { get; set; }
    }

    public class Pagingz
    {
        public string previous { get; set; }
        public string next { get; set; }
    }

    public class PostsObjectz
    {
        public List<Datumz> data { get; set; }
        public Pagingz paging { get; set; }
    }
}