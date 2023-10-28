using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LegalTracker.Domain.Entities;
using LegalTracker.Application.Services;

namespace LegalTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActionController : ControllerBase
    {
        private readonly ActionService _actionService;
        private readonly UserService _userService;
        private readonly CaseService _caseService;
        public ActionController (ActionService actionService, UserService userService, CaseService caseService)
        {
            _actionService = actionService;
            _userService = userService;
            _caseService = caseService;
        }

        [Authorize]
        [HttpGet("getActions")]
        public async Task<ActionResult> GetActions()
        {


            User user = await _userService.GetUserFromCookie(HttpContext.User);
            if (user == null)
                return new BadRequestObjectResult(new { error = "User error" });

            try
            {
                var actions = await _actionService.GetActions(user);
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

            User user = await _userService.GetUserFromCookie(HttpContext.User);
            if (user == null)
                return new BadRequestObjectResult(new { error = "User error" });

            try
            {
                var newAction = await _actionService.CreateAction(action, user);

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

            User user = await _userService.GetUserFromCookie(HttpContext.User);
            if (user == null)
                return new BadRequestObjectResult(new { error = "User error" });

            try
            {
                var legalCase = await _caseService.GetCaseById(user, LegalCaseId);
                await _actionService.ExecuteAction(action, user, legalCase);

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
