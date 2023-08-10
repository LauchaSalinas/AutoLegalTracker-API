using AutoLegalTracker_API.Business;
using Microsoft.AspNetCore.Mvc;

public class LoginRequestModel
{
    public string OneTimeToken { get; set; } = string.Empty;
}

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

        #endregion Public Methods
    }
}
