using FacebookTools.FacebookObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyst.Db
{
    public class DbDriver
    {
        private readonly string connectionString;
        private SocieContext db;

        public DbDriver()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SocieContext"].ConnectionString;
            db = new SocieContext(connectionString);
        }

        /// <summary>
        /// Get All Persons Data from db (Person table)
        /// </summary>
        /// <returns>List of all persons in db</returns>
        public List<Person> getPersons()
        {
            var users = from person
                        in db.Person
                        select new
                        {
                            SocieId = person.SocieId,
                            PersonId = person.PersonId,
                            Token = person.Token,
                            Name = person.Name
                        };

            List<Person> persons = new List<Person>();
            foreach (var user in users)
            {
                Person person = new Person();
                person.Name = user.Name;
                person.Token = user.Token;
                person.PersonId = user.PersonId;
                person.SocieId = user.PersonId;

                persons.Add(person);
            }

            return persons;
        }

        public void SaveAlbums(List<PhotoAlbum> albums)
        {
            var currentAlbums = from photoAlbum
                                in db.PhotoAlbum
                                select new { AlbumId = photoAlbum.AlbumId, Name = photoAlbum.Name, PersonId = photoAlbum.PersonId };
            var currentAlbumsDictionary = currentAlbums.ToDictionary(x => x.AlbumId);
            foreach (var album in albums)
            {
                if(currentAlbumsDictionary.ContainsKey(album.AlbumId))
                {
                    /*/ UPDATE EXISTING /*/
                }
                else
                {
                    db.PhotoAlbum.Add(album);
                }
            }

            db.SaveChanges();
        }

        internal void SavePhotos(List<Photo> albumPhotos)
        {
            var currentPhotos = from photo
                                in db.Photo
                                select new { PhotoId = photo.PhotoId, Name = photo.Name, Tags = photo.Tags, CreationDate = photo.CreationDate };
            var currentPhotosDictionary = currentPhotos.ToDictionary(x => x.PhotoId);
            foreach (var photo in albumPhotos)
            {
                if (currentPhotosDictionary.ContainsKey(photo.PhotoId))
                {
                    /*/ UPDATE EXISTING /*/
                }
                else
                {
                    db.Photo.Add(photo);
                }
            }

            db.SaveChanges();
        }
    }
}
