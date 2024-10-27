using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Simbir.Health.Document.Attributes;
using Simbir.Health.Document.Controllers.DTO;
using Simbir.Health.Document.Services;
using Simbir.Health.Document.Services.DTO;
using System.ComponentModel.DataAnnotations;

namespace Simbir.Health.Document.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;
        private readonly IMapper _mapper;

        public HistoryController(IHistoryService historyService, IMapper mapper)
        {
            _historyService = historyService;
            _mapper = mapper;
        }

        [HttpGet("Account/{id:int}")]
        [ApiAuthorize("Doctor,User")]
        public async Task<IActionResult> GetAccountHistory([Required] int id)
        {
            var accessTokenHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var accessToken = accessTokenHeader.Replace("Bearer ", "");

            var list = await _historyService.GetPatientHistoryAsync(id, accessToken);

            return StatusCode(StatusCodes.Status200OK, list);
        }

        [HttpGet("{id:int}")]
        [ApiAuthorize("Doctor,User")]
        public async Task<IActionResult> GetHistory([Required] int id)
        {
            var accessTokenHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var accessToken = accessTokenHeader.Replace("Bearer ", "");

            var item = await _historyService.GetHistoryAsync(id, accessToken);

            return StatusCode(StatusCodes.Status200OK, item);
        }

        [HttpPost]
        [ApiAuthorize("Admin,Manager,Doctor")]
        public async Task<IActionResult> CreateHistory([Required] CreateHistoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            var accessTokenHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var accessToken = accessTokenHeader.Replace("Bearer ", "");

            await _historyService.CreateHistoryAsync(_mapper.Map<CreateHistoryDTO>(request), accessToken);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id:int}")]
        [ApiAuthorize("Admin,Manager,Doctor")]
        public async Task<IActionResult> UpdateHistory([Required] int id, [Required] UpdateHistoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            var accessTokenHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var accessToken = accessTokenHeader.Replace("Bearer ", "");

            await _historyService.UpdateHistoryAsync(id, _mapper.Map<UpdateHistoryDTO>(request), accessToken);
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}
