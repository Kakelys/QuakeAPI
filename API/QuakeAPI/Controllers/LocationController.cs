using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuakeAPI.Data.Models;
using QuakeAPI.DTO;
using QuakeAPI.Services.Interfaces;

namespace QuakeAPI.Controllers
{
    [ApiController]
    [Route("api/v1/locations")]
    public class LocationController : ControllerBase
    {
        private ILocationService _locationService;
        
        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet, Authorize(Roles = $"{Role.Admin},{Role.User}")]
        public async Task<IActionResult> GetLocations()
        {
            return Ok(await _locationService.GetAll());
        }

        [HttpPost, Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> CreateLocation([FromForm] LocationNew location)
        {
            return Ok(await _locationService.Create(location));
        }

        [HttpDelete("{id}"), Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            await _locationService.Delete(id);
            return Ok();
        }
    }
}