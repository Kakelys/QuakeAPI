using Microsoft.AspNetCore.Mvc;
using QuakeAPI.Data.Repository.Interfaces;
using QuakeAPI.DTO.Analytic;
using QuakeAPI.Services.Interfaces;

namespace QuakeAPI.Controllers
{
    [ApiController]
    [Route("api/v1/analytics")]
    public class AnalyticController : ControllerBase
    {
        private IAnalyticService _analyticService;

        public AnalyticController(IAnalyticService analyticService)
        {
            _analyticService = analyticService;
        }

        [HttpGet("most-popular-location")]
        public IActionResult GetMostPopularLocation([FromQuery] MostPopularLocation dto)
        {
            return Ok(_analyticService.MostPopularLocationByMonth(dto.Month, dto.Year));
        }
    }
}