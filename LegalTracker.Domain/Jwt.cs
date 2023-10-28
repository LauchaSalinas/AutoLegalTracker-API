namespace LegalTracker.Models
{
    public class Jwt
    {
        public string? Key { get; set; }
        public string? Value { get; set; } = null;
        public string Issuer => JwtConstants.Issuer;
        public string Audience => JwtConstants.Audience;
        public string Subject { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpirationTime { get; set; }
        public Jwt() { }
    }

    public static class JwtConstants
    {
        public static string Issuer { get; private set; }
        public static string Audience { get; private set; }

        //public static void Initialize(IConfiguration configuration)
        //{
        //    Issuer = configuration["JWT_ISSUER"];
        //    Audience = configuration["JWT_AUDIENCE"];
        //}
    }
}
