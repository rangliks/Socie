using Analyst.Db;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Analyst
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["SocieContext"].ConnectionString;
            SocieContext db = new SocieContext(connectionString);
            var users = from person
                        in db.Person
                        select new
                        {
                            SocieId = person.SocieId,
                            PersonId = person.PersonId,
                            Token = person.Token
                        };

            
            foreach(var user in users)
            {
                var v = user.PersonId;
            }
        }
    }
}
