using Microsoft.AspNetCore.Mvc;
using Simbir.Health.Hospital.Attributes;
using Simbir.Health.Hospital.Models.DTO;
using Simbir.Health.Hospital.Services;
using System.ComponentModel.DataAnnotations;

namespace Simbir.Health.Hospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalsController : ControllerBase
    {
        private readonly IHospitalService _hospitalService;

        public HospitalsController(IHospitalService hospitalService)
        {
            _hospitalService = hospitalService;
        }

        [HttpGet]
        [ApiAuthorize]
        public async Task<IActionResult> GetHospitalsAsync([FromQuery] int from = 0, [FromQuery] int count = 5)
        {
            var hospitals = await _hospitalService.GetHospitalsAsync(from, count);
            return Ok(hospitals);
        }

        [HttpGet("{id:int}")]
        [ApiAuthorize]
        public async Task<IActionResult> GetHospitalByIdAsync([Required] int id)
        {
            var hospital = await _hospitalService.GetHospitalByIdAsync(id);
            return Ok(hospital);
        }

        [HttpGet("{id:int}/Rooms")]
        [ApiAuthorize]
        public async Task<IActionResult> GetHospitalRoomsByIdAsync([Required] int id)
        {
            var rooms = await _hospitalService.GetHospitalRoomsAsync(id);
            return Ok(rooms);
        }

        [HttpPost]
        [ApiAuthorize("Admin")]
        public async Task<IActionResult> CreateHospital([FromBody] HospitalCreateRequest request)
        {
            var hospitalId = await _hospitalService.CreateHospitalAsync(request);
            return Ok(hospitalId);
        }

        [HttpPut("{hospitalId:int}")]
        [ApiAuthorize("Admin")]
        public async Task<IActionResult> UpdateHospital([Required] int hospitalId, [FromBody] HospitalUpdateRequest request)
        {
            await _hospitalService.UpdateHospitalAsync(hospitalId, request);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        [ApiAuthorize("Admin")]
        public async Task<IActionResult> DeleteHospitalByIdAsync([Required] int id)
        {
            await _hospitalService.DeleteHospitalByIdAsync(id);

            return Ok();
        }
    }
}