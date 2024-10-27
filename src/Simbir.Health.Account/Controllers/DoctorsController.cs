using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simbir.Health.Account.Services;
using Simbir.Health.Account.Services.DTO;
using System.ComponentModel.DataAnnotations;

namespace Simbir.Health.Account.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IUserService _userService;
        public DoctorsController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] string? nameFilter, [FromQuery][Required] int from, [FromQuery][Required] int count)
        {
            var filter = new DoctorListFilterDTO
            {
                NameFilter = nameFilter,
                From = from,
                Count = count
            };

            var response = await _userService.GetAllDoctorsAsync(filter);

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Get([Required] int id)
        {
            var response = await _userService.GetDoctorByIdAsync(id);

            return Ok(response);
        }
    }
}