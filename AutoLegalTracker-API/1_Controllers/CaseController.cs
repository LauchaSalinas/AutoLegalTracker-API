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
        private CaseBusiness _caseBusiness;

        public CaseController(IConfiguration configuration, UserBusiness userBusiness, CaseBusiness caseBusiness)
        {
            _configuration = configuration;
            _userBusiness = userBusiness;
            _caseBusiness = caseBusiness;
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
                var cases = await _caseBusiness.GetCases(user);

                return new OkObjectResult(new { cases });
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
        [HttpGet("GetDashboardStatistics")]
        public async Task<ActionResult> GetDashboardStatistics()
        {
            User user = await _userBusiness.GetUserFromCookie(HttpContext.User);
            if (user == null)
                return new BadRequestObjectResult(new { error = "User error" });

            try
            {
                var automatedCases = await _caseBusiness.GetAutomatedCases(user);
                var casesWithPendingEventsNextWeek = await _caseBusiness.GetCasesWithPendingEventsNextWeek(user);
                // Obtener casos nuevos del mes
                // Obtener notificaciones sin responder
                var automated = new string($"Tienes {automatedCases.Count} casos automatizados");
                var pendingEvents = new string($"Tienes {casesWithPendingEventsNextWeek.Count} casos con eventos pendientes para la próxima semana");

                // TODO JGonzalez: seguir desde aca y revisar lo hecho previamente, revisar que los modelos se agreguen al contexto y revisar la inyeccion de dependencias

                return new OkObjectResult( new { automated, pendingEvents } );
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
}
