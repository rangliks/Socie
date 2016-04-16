using FacebookTools.FacebookObjects;
using Microsoft.ProjectOxford.Emotion.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OxfordTools.OxfordObjects
{
    [Table("EmotionScores")]
    public class EmotionScores
    {
        private Emotion emo;

        public EmotionScores()
        {

        }

        public EmotionScores(Emotion emo, string photoId)
        {
            Photo photo = new Photo();
            photo.PhotoId = photoId;
            PhotoId = photo.PhotoId;

            // TODO: Complete member initialization
            this.emo = emo;
            anger = emo.Scores.Anger;
            contempt = emo.Scores.Contempt;
            disgust = emo.Scores.Disgust;
            fear = emo.Scores.Fear;
            happiness = emo.Scores.Happiness;
            neutral = emo.Scores.Neutral;
            sadness = emo.Scores.Sadness;
            surprise = emo.Scores.Surprise;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string PhotoId { get; set; }
        public double anger { get; set; }
        public double contempt { get; set; }
        public double disgust { get; set; }
        public double fear { get; set; }
        public double happiness { get; set; }
        public double neutral { get; set; }
        public double sadness { get; set; }
        public double surprise { get; set; }
    }
}
