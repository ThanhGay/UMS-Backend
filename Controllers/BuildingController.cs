using Microsoft.AspNetCore.Mvc;
using Server.Dtos.Room;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class BuildingController : ControllerBase
    {
        private readonly IBuildingService _buildingService;

        public BuildingController(IBuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        [HttpGet("all")]
        public IActionResult All()
        {
            try
            {
                return Ok(_buildingService.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public IActionResult Create(CreateRoomDto input)
        {
            try
            {
                _buildingService.CreateRoom(input);
                return Ok("Tạo phòng thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get/{id}")]
        public IActionResult GetRoom(int id)
        {
            try
            {
                return Ok(_buildingService.GetRoomById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public IActionResult Update(UpdateRoomDto input)
        {
            try
            {
                _buildingService.UpdateRoom(input);
                return Ok("Sửa thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _buildingService.DeleteRoom(id);
                return Ok("Đã xóa thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
