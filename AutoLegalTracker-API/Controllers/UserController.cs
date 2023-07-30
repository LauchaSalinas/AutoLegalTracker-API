using AutoLegalTracker_API.Models;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Apis.Util;
using Google.Apis.Services;
using System.Net;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Oauth2.v2;
using Google.Apis.Oauth2.v2.Data;

public class OneTimeToken
{
    public string oneTimeToken { get; set; }
}

namespace ALTDeployTest.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UserController : ControllerBase
    {
        private IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        [Route("login")]
        public dynamic LogIn([FromBody] object optData)
        {
            if (optData == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            // Deserialize JSON data from post request
            OneTimeToken data = JsonConvert.DeserializeObject<OneTimeToken>(optData.ToString());
            string oneTimeTokenString = data.oneTimeToken;

            // TODO MISING VALIDATION OF TOKEN


            
            // Return the JWT token
            return StatusCode(StatusCodes.Status200OK, returnToken);
        }
    }
}
