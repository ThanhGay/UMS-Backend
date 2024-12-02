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

        [HttpPost("add-teacher")]
        public IActionResult AddTeacher(AddTeacherIntoLopHpDto input)
        {
            try
            {
                _lhpService.AddTeacherToLopHp(input);
                return Ok($"Chỉ định giảng viên cho lớp có Id: {input.LopHpId} thành công");
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

        [HttpGet("get-teachers/{lopHpId}")]
        public IActionResult GetTeachersOfLopHP(int lopHpId)
        {
            try
            {
                return Ok(_lhpService.GetTeachers(lopHpId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-lhp-by-student/{studentId}")]
        public IActionResult GetLopHpByStudentId(string studentId, FilterDto input)
        {
            try
            {
                return Ok(_lhpService.GetAllLopHpByStudentId(studentId, input));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-lhp-by-teacher/{teacherId}")]
        public IActionResult GetLopHpByTeacherId(string teacherId, FilterDto input)
        {
            try
            {
                return Ok(_lhpService.GetAllLopHpByTeacherId(teacherId, input));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{lopHpId}")]
        public IActionResult DeleteClassHp(int lopHpId)
        {
            try
            {
                _lhpService.DeleteLopHP(lopHpId);
                return Ok($"Đã xóa lớp học phần {lopHpId}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
