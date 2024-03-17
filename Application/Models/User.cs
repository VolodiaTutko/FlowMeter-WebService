﻿using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
	public class User
	{
		[Key]
		public string ConsumerEmail { get; set; }
		[Required]
		public string Password { get; set; }	
		
	}
}