using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using LegalTracker.Domain.Entities;
using LegalTracker.Application.Services;

namespace LegalTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        #region Constructor

        private IConfiguration _configuration;
        private UserBusiness _userBusiness;
        private CalendarBusiness _calendarBusiness;

        public CalendarController(IConfiguration configuration, UserBusiness userBusiness, CalendarBusiness calendarBusiness)
        {
            _configuration = configuration;
            _userBusiness = userBusiness;
            _calendarBusiness = calendarBusiness;
        }

        #endregion Constructor

        [Authorize]
        [HttpGet("")]
        //get calendars from user
        public async Task<ActionResult> GetCalendars()
        {
            User user = await _userBusiness.GetUserFromCookie(HttpContext.User);
            if (user == null)
                return new BadRequestObjectResult(new { error = "User error" });

            try
            {
                var result = await _calendarBusiness.GetCalendars(user);

                return new OkObjectResult(result);
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
