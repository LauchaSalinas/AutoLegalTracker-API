using AutoLegalTracker_API.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoLegalTracker_API;

[Route("api/[controller]")]
[ApiController]
public class ScrapController : ControllerBase
{
    private readonly ScrapBusiness _scrapBusiness;
    private readonly UserBusiness _userBusiness;

    public ScrapController(ScrapBusiness scrapBusiness, UserBusiness userBusiness)
    {
        _scrapBusiness = scrapBusiness;
        _userBusiness = userBusiness;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult> StartScrapping()
    {
        try
        {
            // await _scrapBusiness.CheckNewCases();
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
