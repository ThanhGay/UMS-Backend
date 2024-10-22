using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Dtos.ChuongTrinhKhung;
using Server.Dtos.Common;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChuongTrinhKhungController : ControllerBase
    {
        private readonly IChuongTrinhKhungService _chuongTrinhKhungService;

        public ChuongTrinhKhungController(IChuongTrinhKhungService chuongTrinhKhungService)
        {
            _chuongTrinhKhungService = chuongTrinhKhungService;
        }

        [HttpGet("all")]
        public IActionResult GetAll(FilterDto input)
        {
            try
            {
                return Ok(_chuongTrinhKhungService.GetAll(input));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-by-id/{id}")]
        public IActionResult GetCTK(int id)
        {
            try
            {
                return Ok(_chuongTrinhKhungService.DetailCTK(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get/{chuyenNganhId}")]
        public IActionResult GetCTKByNganhId(string chuyenNganhId)
        {
            try
            {
                return Ok(_chuongTrinhKhungService.DetailCTKByChuyenNganhId(chuyenNganhId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public IActionResult Create(CreateCTKDto input)
        {
            try
            {
                _chuongTrinhKhungService.CreateCTK(input);
                return Ok("Thêm thành công");
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
                _chuongTrinhKhungService.DeleteCtk(id);
                return Ok("Xóa thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
