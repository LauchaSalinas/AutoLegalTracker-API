using System;
namespace AutoLegalTracker_API.Models
{
	public class User
	{
		public User ()
		{
			LegalCases = new HashSet<LegalCase>();
		}
        public int Id { get; set; }
		public int UserTypeId { get; set; }
		public virtual UserType userType { get; set; } // Navigation property
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

		public virtual ICollection<LegalCase> LegalCases { get; set; }

		public int LastScrappedPage { get; set; } = 1;
		public DateTime? LastScrappedAt { get; set; }

    }

    public class UserType
    {
		public UserType()
		{
			Users = new HashSet<User>();
		}
		public int Id { get; set; }
        public string Name { get; set; }
		public string? Description { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public virtual ICollection<User> Users { get; set; } // navigation property
    }
}

