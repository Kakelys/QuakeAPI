using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuakeAPI.DTO;
using QuakeAPI.Extensions;
using QuakeAPI.Services.Interfaces;

namespace QuakeAPI.Controllers
{
    [ApiController]
    [Route("api/v1/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(
            INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> GetPage([FromQuery] string name, [FromQuery] Page page)
        {
            var result = await _notificationService.GetPage(User.Id(), name, page);
            return Ok(result);
        }

        [HttpPut("{notificationId}"), Authorize]
        public async Task<IActionResult> Read(Guid notificationId)
        {
            await _notificationService.Read(User.Id(), notificationId);
            return Ok();
        }
    }
}