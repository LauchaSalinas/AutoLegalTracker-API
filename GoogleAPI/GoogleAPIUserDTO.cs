
namespace GoogleAPI;

public class GoogleAPIUserDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string IdToken { get; set; }
    public DateTime TokenCreatedAt { get; set; }
    public long TokenExpiration { get; set; }
}
