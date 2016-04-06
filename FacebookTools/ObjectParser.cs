using FacebookTools.FacebookObjects;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookTools
{
    public static class ObjectParser
    {
        public static List<PhotoAlbum> ParseAlbums(object obj, string personId)
        {
            List<PhotoAlbum> albums = new List<PhotoAlbum>();

            // object to string
            string jsonString = obj.ToString();
            
            // parse using jobject
            JObject parsedObj = JsonToJObject(jsonString);

            // iterate the data section in result
            var albumData = parsedObj["data"];
            foreach (var item in albumData.Children())
            {
                PhotoAlbum photoAlbum = new PhotoAlbum();
                var itemProperties = item.Children<JProperty>();
                foreach (JProperty a in itemProperties)
                {
                    var albumid = a.Value.ToString();
                    photoAlbum.AlbumId = albumid;
                }

                photoAlbum.PersonId = personId;
                albums.Add(photoAlbum);
            }

            return albums;
        }

        public static JObject JsonToJObject(string json)
        {
            return JObject.Parse(json);
        }

        public static List<Photo> ParseAlbum(object albumPhotos)
        {
            List<Photo> photos = new List<Photo>();
            // object to string
            string jsonString = albumPhotos.ToString();

            // parse using jobject
            JObject parsedObj = JsonToJObject(jsonString);

            // iterate the data section in result
            var albumData = parsedObj["data"];
            foreach (var item in albumData.Children())
            {
                Photo photo = new Photo();
                var itemProperties = item.Children<JProperty>();
                foreach (JProperty a in itemProperties)
                {
                    if (a.Name.Equals("created_time")) photo.CreationDate = DateTime.Parse(a.Value.ToString());
                    if (a.Name.Equals("id")) photo.PhotoId = a.Value.ToString();
                    if (a.Name.Equals("name")) photo.Name = a.Value.ToString();

                }

                photos.Add(photo);
            }

            return photos;
        }
    }
}
