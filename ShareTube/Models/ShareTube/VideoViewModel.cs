using ShareTube.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareTube.Models.ShareTube
{
	public class VideoViewModel
	{
		public string ID { get; set; }
		public string Author { get; set; }
		public string Title { get; set; }
		public string ThumbnailUrl { get; set; }
		public TimeSpan Length { get; set; }
		public bool IsCurrent { get; set; }

		public VideoViewModel(Video v)
		{

		}
	}
}