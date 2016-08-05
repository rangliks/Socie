using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookTools.FacebookObjects;
using OxfordTools.OxfordObjects;
using FacebookTools.SocieObjects;

namespace Analyst.Db
{
    /// <summary>
    /// The context from which the db entities are taken when connecting to db
    /// </summary>
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
        public virtual DbSet<PhotoAlbum> PhotoAlbum { get; set; }
        public virtual DbSet<EmotionScores> EmotionScores { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
    }
}
