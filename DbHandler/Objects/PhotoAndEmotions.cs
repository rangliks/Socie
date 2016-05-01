using FacebookTools.FacebookObjects;
using OxfordTools.OxfordObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DbHandler.Objects
{
    public class PhotoAndEmotions
    {
        public Photo photo { get; set; }
        public EmotionScores emotions { get; set; }
    }
}