using AutoLegalTracker_API.Business;
using AutoLegalTracker_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoLegalTracker_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActionController : ControllerBase
    {
        private readonly ActionBusiness _actionBusiness;
        private readonly UserBusiness _userBusiness;
        private readonly CaseBusiness _caseBusiness;
        public ActionController (ActionBusiness actionBusiness, UserBusiness userBusiness, CaseBusiness caseBusiness)
        {
            _actionBusiness = actionBusiness;
            _userBusiness = userBusiness;
            _caseBusiness = caseBusiness;
        }

        [Authorize]
        [HttpGet("getActions")]
        public async Task<ActionResult> GetActions()
        {


            User user = await _userBusiness.GetUserFromCookie(HttpContext.User);
            if (user == null)
                return new BadRequestObjectResult(new { error = "User error" });

            try
            {
                // QUE HACE EL BUSINESS?
                // 1. TIENE NOMBRE DE PROCESO DE NEGOCIO
                // 2. CONTACTA SERVICIOS Y DATA ACCESS PARA DEVOLVER UN RESULTADO
                var actions = await _actionBusiness.GetActions(user);

                return new OkObjectResult(new { actions });
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
        [HttpPost("createAction")]
        public async Task<ActionResult> CreateAction([FromBody] LegalCaseAction action)
        {
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
                var newAction = await _actionBusiness.CreateAction(action, user);

                return new OkObjectResult(new { newAction });
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
        [HttpPost("executeAction")]
        public async Task<ActionResult> ExecuteAction([FromBody] LegalCaseAction action, int LegalCaseId)
        {
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
                var legalCase = await _caseBusiness.GetCaseById(user, LegalCaseId);
                await _actionBusiness.ExecuteAction(action, user, legalCase);

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

        [HttpGet]
        public async Task<IActionResult> test()
        {
            try
            {
                await _actionBusiness.RunActionsToNewNotifications();

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
    }

    
}
