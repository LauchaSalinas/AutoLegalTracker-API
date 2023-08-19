using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using AutoLegalTracker_API.Business;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ALTDeployTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        #region Constructor

        private IConfiguration _configuration;
        private UserBusiness _userBusiness;

        public UserController(IConfiguration configuration, UserBusiness userBusiness)
        {
            _configuration = configuration;
            _userBusiness = userBusiness;
        }

        #endregion Constructor

        #region Public Methods

        [HttpPost]
        [Route("login")]
        public ActionResult<string> LogIn([FromBody] LoginRequestModel loginRequest)
        {
            if (loginRequest != null)
            {
                // TODO: MISSING VALIDATION OF TOKEN

                var returnToken = _userBusiness.ExchangeToken(loginRequest.OneTimeToken);

                // Return the JWT token
                return StatusCode(StatusCodes.Status200OK, returnToken);
            }

            return StatusCode(StatusCodes.Status400BadRequest);
        }

        [HttpPost]
        [Route("signup")]
        public ActionResult SignUp()
        {
            // Authenticate user (replace with your authentication logic)
            //if (IsValidCredentials(username, password))
            //{
            // Create an authentication token (replace with your token generation logic)

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "123456"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, "email@email.com"),
                new Claim(JwtRegisteredClaimNames.GivenName, "Lautaro"),
                new Claim(JwtRegisteredClaimNames.FamilyName, "Salinas"),
                // TODO create custom claims

                new Claim("Environment", _configuration["ASPNETCORE_ENVIRONMENT"] ?? string.Empty)
            };

            // Create the key and the signing credentials for the JWT token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT_KEY"] ?? String.Empty));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var jwtToken = new JwtSecurityToken(
                issuer: _configuration["JWT_ISSUER"] ?? String.Empty,
                audience: _configuration["JWT_AUDIENCE"] ?? String.Empty,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: signIn
            );
            var returnToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);


            // Create an HttpOnly cookie to store the token
            var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Optional: Set to true if your site uses HTTPS
                    //SameSite = SameSiteMode.Lax // Optional: Set the appropriate SameSite policy
                };

                Response.Cookies.Append("AuthToken", returnToken, cookieOptions);

                // Return success response
                return Ok(new { message = "Authentication successful." });
            //}

            // Invalid credentials
            return Unauthorized(new { message = "Invalid username or password." });
        }

        [HttpPost]
        [Route("signout")]
        public ActionResult SignOut()
        {
            return new JsonResult(new { key1 = "test" });
        }

        [Authorize]
        [HttpPost("setscrappingcredentials")]
        public async Task<ActionResult> SetScrappingCredentialsAsync([FromBody] ScrappingCredentialsModel credentials)
        {
            // QUE HACE UN CONTROLLER? VALIDAR LOS DATOS ENVIADOS
            if (!ModelState.IsValid)
            {
                // Model validation failed, return a Bad Request response
                return BadRequest(ModelState);
            }

            var userSub = HttpContext.User.FindFirst("Sub")?.ToString();

            if (String.IsNullOrEmpty(userSub))
                return new BadRequestObjectResult(new { error = "User error" });

            try
            {
                // QUE HACE EL BUSINESS?
                        // 1. TIENE NOMBRE DE PROCESO DE NEGOCIO
                        // 2. CONTACTA SERVICIOS Y DATA ACCESS PARA DEVOLVER UN RESULTADO
                await _userBusiness.SetScrappingCredentials(userSub, credentials.Username, credentials.Password);

                return new OkResult();
            }
            catch (ApplicationException appex)
            {
                return BadRequest(new {error = appex });
            }
            catch (Exception ex)
            {
                //save to log table
                var errorMsgToTable = ex;
                var errorMsg = String.Concat("Ha ocurrido un error a las ", DateTime.Now.ToString());
                return BadRequest(new { error = errorMsg });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public ActionResult Logout()
        {
            // Clear the user's session on the server
            // Perform any necessary cleanup (e.g., token revocation)

            // Clear the HttpOnly cookie on the client side
            Response.Cookies.Delete("AuthToken");

            return Ok();
        }

        [Authorize]
        [HttpPost("getinfo")]
        public ActionResult GetUserInfo()
        {
            var userSub = HttpContext.User.Claims.First();

            // Clear the user's session on the server
            // Perform any necessary cleanup (e.g., token revocation)
            
            return new JsonResult(new {user = userSub.ToString()});
        }

        #endregion Public Methods
    }

    #region Controller models

    public class LoginRequestModel
    {
        [Required]
        [StringLength(200, MinimumLength = 30, ErrorMessage = "Token is required")]
        public string OneTimeToken { get; set; } = string.Empty;
    }

    public class ScrappingCredentialsModel
    {
        [Required]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Username must be between 3 and 50 characters.")]
        [DataType(DataType.EmailAddress)]
        public string? Username { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }

    #endregion Controller models
}
