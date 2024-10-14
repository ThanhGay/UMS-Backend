using Microsoft.AspNetCore.Mvc;
using Server.Dtos.Common;
using Server.Dtos.Subject;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet("all")]
        public IActionResult GetAll(FilterDto input)
        {
            try
            {
                return Ok(_subjectService.GetAll(input));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(_subjectService.GetSubjectById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public IActionResult Create(CreateSubjectDto input)
        {
            try
            {
                _subjectService.CreateSubject(input);
                return Ok("Tạo thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public IActionResult Update(UpdateSubjectDto input)
        {
            try
            {
                _subjectService.UpdateSubject(input);
                return Ok("Cập nhật thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                _subjectService.DeleteSubject(id);
                return Ok("Xóa thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
