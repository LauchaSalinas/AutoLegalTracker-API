using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegalTracker.Domain.Entities
{
    public class UserToScrapDTO
    {
        public UserToScrapDTO()
        {
            LastScrappedLegalCase = new LegalCase();
            WebCredentialUser = string.Empty;
            WebCredentialPassword = string.Empty;
        }
        public int Id { get; set; }
        public string WebCredentialUser { get; set; }
        public string WebCredentialPassword { get; set; }

        public LegalCase LastScrappedLegalCase { get; set; }

        public int LastScrappedPage { get; set; } = 1;

        public DateTime? LastScrappedAt { get; set; }

    }
}

