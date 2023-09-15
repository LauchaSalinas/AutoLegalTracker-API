namespace AutoLegalTracker_API.Models
{
    public class LegalResponseTemplate
    {
        public LegalResponseTemplate()
        {

        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
