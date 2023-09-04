using System;
namespace AutoLegalTracker_API.Models
{
	public class User
	{
        public int Id { get; set; }
		public string Sub { get; set; }
		public string Name { get; set; }
		public string FamilyName { get; set; }
		public string Email { get; set; }
		public string GoogleProfilePicture { get; set; }
		public string? WebCredentialUser { get; set; }
		public string? WebCredentialPassword { get; set; }
		public string GoogleOAuth2RefreshToken { get; set; }
		public string GoogleOAuth2AccessToken { get; set; }
		public long GoogleOAuth2TokenExpiration { get; set; }
		public DateTime GoogleOAuth2TokenCreatedAt { get; set; }
		public string? GoogleOAuth2IdToken { get; set; }

		public virtual List<LegalCase> LegalCases { get; set; } = new List<LegalCase>();
    }
}

