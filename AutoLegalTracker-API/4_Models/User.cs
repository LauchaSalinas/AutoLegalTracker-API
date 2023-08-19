using System;
namespace AutoLegalTracker_API.Models
{
	public class User
	{
        public int Id { get; set; }
		public string Sub { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string? WebCredentialUser { get; set; }
		public string? WebCredentialPassword { get; set; }


    }
}

