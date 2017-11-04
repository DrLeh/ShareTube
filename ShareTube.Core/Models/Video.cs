using Newtonsoft.Json;
using ShareTube.Core.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShareTube.Core.Models
{
    public class Video : Entity
    {
        public string ID { get; set; }
        public Guid RoomID { get; set; }

        //[ScriptIgnore]
        public virtual Room Room { get; set; }

        public int Order { get; set; }

        public string Author { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public TimeSpan Length { get; set; }
        public bool IsCurrent { get; set; }

        public string LengthString
        {
            get
            {
                var min = Length.Minutes + (60 * Length.Hours);
                return min + ":" + Length.Seconds;
            }
        }

        //[NotMapped]
        //[JsonIgnore]
        //public string YouTubeUrl
        //{
        //    get => UrlHelper.GetYouTubeUrlFromID(ID);
        //    set => ID = UrlHelper.GetYouTubeID(value);
        //}

        public static Video CopyFrom(Video other)
        {
            return new Video
            {
                ID = other.ID,
                RoomID = other.RoomID,
                Order = other.Order,
                Author = other.Author,
                Title = other.Title,
                ThumbnailUrl = other.ThumbnailUrl,
                Length = other.Length,
                IsCurrent = other.IsCurrent
            };
        }
    }
}