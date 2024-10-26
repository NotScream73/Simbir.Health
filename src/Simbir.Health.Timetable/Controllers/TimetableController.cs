using Microsoft.AspNetCore.Mvc;
using Simbir.Health.Timetable.Attributes;
using Simbir.Health.Timetable.Models.DTO;
using Simbir.Health.Timetable.Services;
using Simbir.Health.Timetable.Services.DTO;
using System.ComponentModel.DataAnnotations;

namespace Simbir.Health.Timetable.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimetableController : ControllerBase
    {
        private readonly ITimeTableService _timeTableService;
        public TimetableController(ITimeTableService timeTableService)
        {
            _timeTableService = timeTableService;
        }

        [HttpPost]
        [ApiAuthorize("Admin,Manager")]
        public async Task<IActionResult> CreateTimetable([FromBody] TimeTableCreateRequest request)
        {
            var accessTokenHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var accessToken = accessTokenHeader.Replace("Bearer ", "");
            await _timeTableService.CreateTimeTableAsync(request, accessToken);
            return Ok();
        }

        [HttpPut("{id:int}")]
        [ApiAuthorize("Admin,Manager")]
        public async Task<IActionResult> UpdateTimetable([Required] int id, [FromBody] TimeTableCreateRequest request)
        {
            var accessTokenHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var accessToken = accessTokenHeader.Replace("Bearer ", "");
            await _timeTableService.UpdateTimeTableAsync(id, request, accessToken);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        [ApiAuthorize("Admin,Manager")]
        public async Task<IActionResult> DeleteTimetable([Required] int id)
        {
            await _timeTableService.DeleteTimeTableAsync(id);
            return Ok();
        }

        [HttpDelete("Doctor/{id:int}")]
        [ApiAuthorize("Admin,Manager")]
        public async Task<IActionResult> DeleteDoctorTimetable([Required] int id)
        {
            await _timeTableService.DeleteDoctorTimeTableAsync(id);
            return Ok();
        }

        [HttpDelete("Hospital/{id:int}")]
        [ApiAuthorize("Admin,Manager")]
        public async Task<IActionResult> DeleteHospitalTimetable([Required] int id)
        {
            await _timeTableService.DeleteHospitalTimeTableAsync(id);
            return Ok();
        }

        [HttpGet("Hospital/{id:int}")]
        [ApiAuthorize]
        public async Task<IActionResult> GetHospitalTimetable([Required] int id, [Required] string from, [Required] string to)
        {
            var accessTokenHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var accessToken = accessTokenHeader.Replace("Bearer ", "");

            var filter = new TimeTableFilterDTO
            {
                From = from,
                To = to
            };

            var list = await _timeTableService.GetHospitalTimeTableListAsync(id, filter, accessToken);
            return Ok(list);
        }

        [HttpGet("Doctor/{id:int}")]
        [ApiAuthorize]
        public async Task<IActionResult> GetDoctorTimetable([Required] int id, [Required] string from, [Required] string to)
        {
            var accessTokenHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var accessToken = accessTokenHeader.Replace("Bearer ", "");

            var filter = new TimeTableFilterDTO
            {
                From = from,
                To = to
            };

            var list = await _timeTableService.GetDoctorTimeTableListAsync(id, filter, accessToken);
            return Ok(list);
        }

        [HttpGet("Hospital/{id:int}/Room/{room}")]
        [ApiAuthorize]
        public async Task<IActionResult> GetRoomTimetable([Required] int id, [Required] string room, [Required] string from, [Required] string to)
        {
            var accessTokenHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var accessToken = accessTokenHeader.Replace("Bearer ", "");

            var filter = new RoomTimeTableFilterDTO
            {
                HospitalId = id,
                RoomNumber = room,
                From = from,
                To = to
            };

            var list = await _timeTableService.GetRoomTimeTableListAsync(filter, accessToken);
            return Ok(list);
        }

        [HttpGet("{id:int}/Appointments")]
        [ApiAuthorize]
        public async Task<IActionResult> GetAvailableAppointments([Required] int id)
        {
            var list = await _timeTableService.GetAvailableAppointmentsByTimeTableId(id);
            return Ok(list);
        }

        [HttpPost("{id:int}/Appointments")]
        [ApiAuthorize]
        public async Task<IActionResult> CreateAppointment([Required] int id, [Required] AppointmentCreateRequest request)
        {
            var accessTokenHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var accessToken = accessTokenHeader.Replace("Bearer ", "");

            await _timeTableService.CreateAppointmentAsync(id, request, accessToken);

            return Ok();
        }
    }
}
