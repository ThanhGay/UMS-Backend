﻿using Microsoft.AspNetCore.Http;
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
        private readonly IScheduleService _scheduleService;

        public LopHPController(ILopHPService lhpService, IScheduleService scheduleService)
        {
            _lhpService = lhpService;
            _scheduleService = scheduleService;
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

        [HttpGet("get/{lopHpId}")]
        public IActionResult GetDetailopHpId(int lopHpId)
        {
            {
                try
                {
                    return Ok(_lhpService.GetDetailLopHp(lopHpId));
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
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

        

        [HttpPost("add-student")]
        public IActionResult AddStudent(AddStudentIntoLopHpDto input)
        {
            try
            {
                _lhpService.AddStudents(input);
                return Ok($"Thêm sinh viên vào lớp có Id {input.LopHpId} thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        

        [HttpGet("get-students/{lopHpId}")]
        public IActionResult GetStudentsOfLopHP(int lopHpId)
        {
            try
            {
                return Ok(_lhpService.GetStudents(lopHpId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-lhp-by-student/{studentId}")]
        public IActionResult GetLopHpByStudentId(string studentId)
        {
            try
            {
                return Ok(_lhpService.GetAllLopHpByStudentId(studentId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
