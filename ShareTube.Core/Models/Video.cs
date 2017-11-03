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

        [NotMapped]
        public string YouTubeUrl
        {
            get => UrlHelper.GetYouTubeUrlFromID(ID);
            set => ID = UrlHelper.GetYouTubeID(value);
        }
    }
}