using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

using LegalTracker.Domain.Entities;
using LegalTracker.Application.Services;

namespace LegalTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaseController : ControllerBase
    {
        #region Constructor

        private IConfiguration _configuration;
        private UserService _userService;
        private CaseService _caseService;
        private LegalNotificationService _notificationService;
        private readonly ALTContext _altContext;

        public CaseController(IConfiguration configuration, 
            UserService userService, 
            CaseService caseService, 
            LegalNotificationService notificationService,
            ALTContext aLTContext)
        {
            _configuration = configuration;
            _userService = userService;
            _caseService = caseService;
            _notificationService = notificationService;
            _altContext = aLTContext;
        }

        #endregion Constructor

        #region Public Methods

        [Authorize]
        [HttpGet("getCases")]
        public async Task<ActionResult> GetCases()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = await _userService.GetUserFromCookie(HttpContext.User);
            if (user == null)
                return new BadRequestObjectResult(new { error = "User error" });

            try
            {
                var cases = await _caseService.GetCases(user);

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


        [HttpGet("getCasesFilteredAndPaged")]
        public async Task<IActionResult> GetCasesFilteredAndPaged(
            string? title = null, 
            string? notificationDateFrom = null,
            string? notificationDateTo = null,
            int page = 1)
        {
            int pageSize = 50;

            page = Math.Max(1, page);
            pageSize = Math.Max(1, pageSize);

            // Calculate the number of records to skip
            int skipCount = (page - 1) * pageSize;

            if(notificationDateFrom == null)
                notificationDateFrom = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy");
            if(notificationDateTo == null)
                notificationDateTo = DateTime.Now.ToString("dd/MM/yyyy");

            var notificationDateFromParsed = DateTime.Parse(notificationDateFrom);
            var notificationDateToParsed = DateTime.Parse(notificationDateTo);

            if(title == null)
                title = string.Empty;

            // TODO add user
            // Query the database, applying pagination
            var pagedData = await _altContext.LegalNotifications
                .Where( ln => ln.Title.Contains(title) && 
                        ln.NotificationDate >= notificationDateFromParsed && 
                        ln.NotificationDate <= notificationDateToParsed)
                .OrderBy(e => e.NotificationDate) // Replace with your sorting criteria
                .Skip(skipCount)
                .Take(pageSize)
                .Select(ln => new { ln.Title, ln.Body, ln.NotificationDate, ln.LegalCaseId })
                .AsNoTracking()
                .ToListAsync();

            return new OkObjectResult(pagedData);
        }

        [HttpGet("getCasesFilteredAndPagedPageCount")]
        public async Task<IActionResult> GetCasesFilteredAndPagedPageCount(
            string? title = null,
            string? notificationDateFrom = null,
            string? notificationDateTo = null,
            int page = 1)
        {
            int pageSize = 50;

            page = Math.Max(1, page);
            pageSize = Math.Max(1, pageSize);

            // Calculate the number of records to skip
            int skipCount = (page - 1) * pageSize;

            if (notificationDateFrom == null)
                notificationDateFrom = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy");
            if (notificationDateTo == null)
                notificationDateTo = DateTime.Now.ToString("dd/MM/yyyy");

            var notificationDateFromParsed = DateTime.Parse(notificationDateFrom);
            var notificationDateToParsed = DateTime.Parse(notificationDateTo);

            if (title == null)
                title = string.Empty;

            // TODO add user
            // Query the database, applying pagination
            var pagedData = await _altContext.LegalNotifications
                .CountAsync(ln => ln.Title.Contains(title) &&
                            ln.NotificationDate >= notificationDateFromParsed &&
                            ln.NotificationDate <= notificationDateToParsed);

            var totalPages = Math.Ceiling((decimal)pagedData / pageSize);

            return new OkObjectResult(totalPages);
        }


        [Authorize]
        [HttpGet("GetDashboardStatistics")]
        public async Task<ActionResult> GetDashboardStatistics()
        {
            User user = await _userService.GetUserFromCookie(HttpContext.User);
            if (user == null)
                return new BadRequestObjectResult(new { error = "User error" });

            try
            {
                //Automated Cases
                var automatedCases = await _caseService.GetAutomatedCases(user);
                //Pending Cases in the next week
                var casesWithPendingEventsNextWeek = await _caseService.GetCasesWithPendingEventsNextWeek(user);
                // New Cases in the Month
                var newCasesInThisMonth = await _caseService.GetNewCasesInThisMonth(user);
                // Unseen Notifications
                var casesNotificationUnseen = await _notificationService.GetAllNotifications(user); 

                //Object Messagge Text
                var automated = new string($"Tienes {automatedCases.Count} casos automatizados");
                var pendingEvents = new string($"Tienes {casesWithPendingEventsNextWeek.Count} casos con eventos pendientes para la próxima semana");
                var newCases = new string($"Tienes {newCasesInThisMonth.Count} casos nuevos este mes");
                var notifications = new string($"Tienes {casesNotificationUnseen.Count} notificaciones sin ver");

                return new OkObjectResult(new { automated, pendingEvents, newCases, notifications }); ; 

                // TODO JGonzalez: seguir desde aca y revisar lo hecho previamente, revisar que los modelos se agreguen al contexto y revisar la inyeccion de dependencias
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
