﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StorySpoiler.DTO_Models
{
	public class StoryDTO
	{
		[JsonPropertyName("title")]
		public string Title { get; set; }

		[JsonPropertyName("description")]
		public string Description { get; set; }

		[JsonPropertyName("url")]
		public string Url { get; set; }
		public string StoryId { get; internal set; }
	
	}
}
