namespace AutoLegalTracker_API.Models
{
    public class Jwt
    {
        public string? Key { get; set; }
        public string? Value { get; set; } = null;
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }
        public Jwt() { }
    }
}
