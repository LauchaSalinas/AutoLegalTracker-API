using LegalTracker.Domain.Entities;

namespace LegalTracker.Domain.DTOs
{
    public class UserToScrapNewLegalCasesDTO
    {
        public UserToScrapNewLegalCasesDTO()
        {
            LastScrappedLegalCase = new LegalCase();
            ScrappedLegalCases = new List<LegalCase>();
            WebCredentialUser = string.Empty;
            WebCredentialPassword = string.Empty;
        }
        public int Id { get; set; }
        public string WebCredentialUser { get; set; }
        public string WebCredentialPassword { get; set; }

        public LegalCase LastScrappedLegalCase { get; set; }

        public int LastScrappedPage { get; set; } = 1;

        public DateTime? LastScrappedAt { get; set; }

        public List<LegalCase> ScrappedLegalCases { get; set; }

    }
}

