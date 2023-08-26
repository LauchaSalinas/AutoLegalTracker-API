using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using AutoLegalTracker_API.Business;
using AutoLegalTracker_API.Models;

namespace AutoLegalTracker_API._1_Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseController : ControllerBase
    {
        #region Constructor

        private IConfiguration _configuration;
        private UserBusiness _userBusiness;
        private JwtBusiness _jwtBusiness;

        public CaseController(IConfiguration configuration, UserBusiness userBusiness, JwtBusiness jwtBusiness)
        {
            _configuration = configuration;
            _userBusiness = userBusiness;
            _jwtBusiness = jwtBusiness;
        }

        #endregion Constructor

        #region Public Methods

        [Authorize]
        [HttpGet("getCases")]
        public async Task<ActionResult> GetCases()
        {
            // QUE HACE UN CONTROLLER? VALIDAR LOS DATOS ENVIADOS
            if (!ModelState.IsValid)
            {
                // Model validation failed, return a Bad Request response
                return BadRequest(ModelState);
            }

            User user = await _userBusiness.GetUserFromCookie(HttpContext.User);
            if (user == null)
                return new BadRequestObjectResult(new { error = "User error" });

            try
            {
                // QUE HACE EL BUSINESS?
                // 1. TIENE NOMBRE DE PROCESO DE NEGOCIO
                // 2. CONTACTA SERVICIOS Y DATA ACCESS PARA DEVOLVER UN RESULTADO
                // TODO: Get cases from database from an user
                // cases = await _caseBusiness.GetCases(user);

                // TODO: delete when cases are ready
                IEnumerable<LegalCase> cases = new List<LegalCase>();

                return new OkObjectResult(new { cases = cases });
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

            #endregion Public Methods
        }
    }
}
