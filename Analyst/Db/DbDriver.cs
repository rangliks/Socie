﻿using DbHandler.Objects;
using FacebookTools.FacebookObjects;
using FacebookTools.SocieObjects;
using OxfordTools.OxfordObjects;
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

        public void SavePhotos(List<Photo> albumPhotos)
        {
            var currentPhotos = from photo
                                in db.Photo
                                select new { PhotoId = photo.PhotoId, Name = photo.Name, CreationDate = photo.CreationDate };
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

        public void SavePerson(Person person)
        {
            var personInDB =    from me
                                in db.Person
                                where me.PersonId == person.PersonId
                                select new 
                                {
                                    PersonID = me.PersonId,
                                    Name = me.Name,
                                    Relation = me.Relation,
                                    SocieId = me.SocieId, 
                                    Token = me.Token
                                };
            
            // if selfNot exist in db save it
            // else update record
            if(!personInDB.Any())
            {
                db.Person.Add(person);
            }
            else
            {

            }

            db.SaveChanges();
        }

        public void SaveEmotions(List<EmotionScores> v)
        {
            var emotions = from emo
                           in db.EmotionScores
                           select emo;
            
            var photoIds = emotions.Select(x => x.PhotoId).ToList();
            foreach (var item in v)
            {
                if(!photoIds.Contains(item.PhotoId))
                {
                    db.EmotionScores.Add(item);
                }
            }

            db.SaveChanges();
        }

        public List<EmotionScores> GetEmotions()
        {
            var emotions = from emo
                           in db.EmotionScores
                           select emo;

            return emotions.ToList();
        }

        public List<Person> GetSocieUsers()
        {
            var users = from person
                        in db.Person
                        where !string.IsNullOrEmpty(person.Token)
                        select person;

            return users.ToList();
        }

        /// <summary>
        /// get the inPerson objects of the owners of input photoIds
        /// </summary>
        /// <param name="photoIds"></param>
        /// <returns></returns>
        public List<PersonPhotoWithAlbum> GetPersonsOfPhotos(List<string> photoIds)
        {
            List<PersonPhotoWithAlbum> output = new List<PersonPhotoWithAlbum>();
            var users = from user
                        in db.Person
                        join photoAlbum in db.PhotoAlbum on user.PersonId equals photoAlbum.PersonId
                        join photo in db.Photo on photoAlbum.AlbumId equals photo.AlbumId
                        where photoIds.Contains(photo.PhotoId)
                        select new { User = user, Album = photoAlbum, Photo = photo };

            foreach (var item in users)
            {
                output.Add(new PersonPhotoWithAlbum(item.User, item.Album, item.Photo));
            }

            return output;
        }

        public void SaveNotification(Notification notify)
        {
            notify.NotificationId = Guid.NewGuid().ToString();
            notify.CreationDate = DateTime.Now;
            db.Notifications.Add(notify);
            db.SaveChanges();
        }
    }
}
