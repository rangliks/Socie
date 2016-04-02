using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookTools.FacebookObjects;

namespace Analyst.Db
{
    public class SocieContext : DbContext
    {
        public SocieContext()
        {

        }

        public SocieContext(string connString)
            : base(connString)
        {

        }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Photo> Photo { get; set; }
    }
}
