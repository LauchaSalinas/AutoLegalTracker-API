using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using AutoLegalTracker_API.Business;

namespace ALTDeployTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        #region Constructor

        private IConfiguration _configuration;
        private UserBusiness _userBusiness;
        private JwtBusiness _jwtBusiness;

        public UserController(IConfiguration configuration, UserBusiness userBusiness, JwtBusiness jwtBusiness)
        {
            _configuration = configuration;
            _userBusiness = userBusiness;
            _jwtBusiness = jwtBusiness;
        }

        #endregion Constructor

        #region Public Methods

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> LogInAsync([FromBody] LoginRequestModel loginRequest)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed, return a Bad Request response
                return BadRequest(ModelState);
            }
            try
            {
                // From user business we will reach google OAuth2 service to exchange the one time token for a TokenResponse with an access token and a refresh token, we will return a user object with the tokens
                var user = await _userBusiness.GetUser(loginRequest.OneTimeToken);
                // From jwt business we will create a JWT token from the user object
                var returnToken = _jwtBusiness.CreateJwt(user);

                // Create an HttpOnly cookie to store the token
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Optional: Set to true if your site uses HTTPS
                    SameSite = SameSiteMode.None, // Optional: Set the appropriate SameSite policy
                    Domain = "localhost",
                    Path = "/",
                    Expires = DateTime.UtcNow.AddDays(30)
                };

                Response.Cookies.Append("AuthToken", returnToken, cookieOptions);
                // Return the JWT token
                return StatusCode(StatusCodes.Status200OK, returnToken);
            }
            catch (ApplicationException appex)
            {
                return BadRequest(new { error = appex });
            }
            catch (Exception ex)
            {
                //save to log table
                var errorMsgToTable = ex;
                var errorMsg = String.Concat("Ha ocurrido un error a las ", DateTime.Now.ToString());
                return BadRequest(new { error = errorMsg });
            }
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<ActionResult> SignUpAsync([FromBody] LoginRequestModel loginRequest)
        {
            if (!ModelState.IsValid)
            {
                // Model validation failed, return a Bad Request response
                return BadRequest(ModelState);
            }
            try
            {
                // From user business we will reach google OAuth2 service to exchange the one time token for a TokenResponse with an access token and a refresh token, we will return a user object with the tokens
                var user = await _userBusiness.GetUser(loginRequest.OneTimeToken);
                // From jwt business we will create a JWT token from the user object
                var returnToken = _jwtBusiness.CreateJwt(user);

                // Create an HttpOnly cookie to store the token
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Optional: Set to true if your site uses HTTPS
                    SameSite = SameSiteMode.None, // Optional: Set the appropriate SameSite policy
                    Domain = "localhost",
                    Path = "/",
                    Expires = DateTime.UtcNow.AddDays(30)
                };

                Response.Cookies.Append("AuthToken", returnToken, cookieOptions);
                // Return the JWT token
                return StatusCode(StatusCodes.Status200OK, returnToken);
            }
            catch (ApplicationException appex)
            {
                return BadRequest(new { error = appex });
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
        [HttpGet("Info")]
        public ActionResult Info()
        {
            var name = HttpContext.User.FindFirst(ClaimTypes.GivenName)?.Value.ToString();
            var imageUrl = HttpContext.User.FindFirst("ImageUrl")?.Value.ToString();

            return new JsonResult(new { name = name, imageUrl = imageUrl });
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

            var userSub = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString();

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
                return BadRequest(new { error = appex });
            }
            catch (Exception ex)
            {
                //save to log table
                var errorMsgToTable = ex;
                var errorMsg = String.Concat("Ha ocurrido un error a las ", DateTime.Now.ToString());
                return BadRequest(new { error = errorMsg });
            }
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
