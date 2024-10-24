﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Dtos.Schedule;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpPost("create-schedule-lopHp")]
        public IActionResult CreateScheduleOfLopHP(CreateScheduleOfLopHp input)
        {
            try
            {
                _scheduleService.CreateScheduleOfClassHP(input);
                return Ok($"Tạo lịch cho lớp có Id {input.LopHpId} thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get/lopHp/{lopHpId}")]
        public IActionResult GetScheduleOfLopHP(int lopHpId)
        {
            try
            {
                return Ok(_scheduleService.GetScheduleOfClassHp(lopHpId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get/teacher/{teacherId}")]
        public IActionResult GetScheduleOfTeacher(string teacherId)
        {
            try
            {
                return Ok(_scheduleService.ScheduleOfTeacher(teacherId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
