using Server.Dtos.Common;
using Server.Dtos.Room;
using Server.Entities;

namespace Server.Services.Interfaces
{
    public interface IBuildingService
    {
        public void CreateRoom(CreateRoomDto input);
        public void UpdateRoom(UpdateRoomDto input);
        public void DeleteRoom(int id);
        public Room GetRoomById(int id);
        public PageResultDto<Room> GetAll();
    }
}
