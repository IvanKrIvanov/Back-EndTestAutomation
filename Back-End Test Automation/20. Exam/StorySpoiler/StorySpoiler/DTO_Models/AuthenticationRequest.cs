﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StorySpoiler.DTO_Models
{
	internal class AuthenticationRequest
	{
		[JsonPropertyName("userName")]
		public string UserName { get; set; }

		[JsonPropertyName("password")]
		public string Password { get; set; }
	}
}
