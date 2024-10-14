using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Dtos.Common;
using Server.Dtos.LopHP;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LopHPController : ControllerBase
    {
        private readonly ILopHPService _lhpService;

        public LopHPController(ILopHPService lhpService)
        {
            _lhpService = lhpService;
        }

        [HttpPost("create")]
        public IActionResult Create(CreateLopHPDto input)
        {
            try
            {
                _lhpService.CreateLopHP(input);
                return Ok("Tạo lớp thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("all")]
        public IActionResult GetAll(FilterDto input)
        {
            try
            {
                return Ok(_lhpService.GetAll(input));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create-schedule")]
        public IActionResult CreateScheduleOfLopHP(CreateScheduleOfLopHp input)
        {
            try
            {
                _lhpService.CreateScheduleOfLopHP(input);
                return Ok($"Tạo lịch cho lớp có Id {input.LopHpId} thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("schedule/{lopHpId}")]
        public IActionResult GetScheduleOfLopHP(int lopHpId)
        {
            try
            {
                return Ok(_lhpService.GetSchedule(lopHpId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        
        }
        
    }
}
