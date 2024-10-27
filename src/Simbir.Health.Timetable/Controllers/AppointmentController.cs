using Microsoft.AspNetCore.Mvc;
using Simbir.Health.Timetable.Attributes;
using Simbir.Health.Timetable.Services;
using System.ComponentModel.DataAnnotations;

namespace Simbir.Health.Timetable.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : Controller
    {
        private readonly ITimeTableService _timeTableService;
        public AppointmentController(ITimeTableService timeTableService)
        {
            _timeTableService = timeTableService;
        }

        [HttpDelete("{id:int}")]
        [ApiAuthorize("Admin,Manager,User")]
        public async Task<IActionResult> DeleteAppointment([Required] int id)
        {
            var accessTokenHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var accessToken = accessTokenHeader.Replace("Bearer ", "");

            await _timeTableService.DeleteAppointmentAsync(id, accessToken);

            return Ok();
        }
    }
}
