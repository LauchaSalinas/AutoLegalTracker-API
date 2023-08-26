using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using System.Security.Claims;

using AutoLegalTracker_API.Business;
using AutoLegalTracker_API.Models;

namespace AutoLegalTracker_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicalAppointmentController : ControllerBase
    {
        #region Constructor
        private MedicalAppointmentBusiness _medicalAppointmentBusiness;
        private IConfiguration _configuration;
        private UserBusiness _userBusiness;
        public MedicalAppointmentController(IConfiguration configuration, MedicalAppointmentBusiness medicalAppointmentBusiness, UserBusiness userBusiness)
        {
            _configuration = configuration;
            _medicalAppointmentBusiness = medicalAppointmentBusiness;
            _userBusiness = userBusiness;
        }

        #endregion Constructor

        // get the events from calendar id, start and end date
        [Authorize]
        [HttpGet("GetEvents/{calendarId}/{startDate}/{endDate}")]
        public async Task<ActionResult> GetEvents(string calendarId, DateTime startDate, DateTime endDate)
        {
            User user = await _userBusiness.GetUserFromCookie(HttpContext.User);
            if (user == null)
                return new BadRequestObjectResult(new { error = "User error" });

            try
            {
                var result = await _medicalAppointmentBusiness.GetEvents(user, calendarId, startDate, endDate);

                // parse the result to a json object
                // TODO create a class to parse the result and map from google CalendarEvent to MedicalAppointment
                var jsonResult = JsonConvert.SerializeObject(result);

                return new OkObjectResult(jsonResult);
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

        //get the freebusy from calendar id, start and end date
        [Authorize]
        [HttpGet("GetFreeBusy/{calendarId}/{startDate}/{endDate}")]
        public async Task<ActionResult> GetFreeBusy(string calendarId, string startDate, string endDate)
        {
            User user = await _userBusiness.GetUserFromCookie(HttpContext.User);
            if (user == null)
                return new BadRequestObjectResult(new { error = "User error" });

            //parse dates from an input of yyyy/MM/dd
            var startDateParsed = DateTime.ParseExact(startDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var endDateParsed = DateTime.ParseExact(endDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            try
            {
                var result = await _medicalAppointmentBusiness.GetFreeBusy(user, calendarId, startDateParsed, endDateParsed);

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
