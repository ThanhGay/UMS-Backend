using Server.DbContexts;
using Server.Entities;
using Server.Exceptions;
using Server.Services.Interfaces;

namespace Server.Services.Implements
{
    public class BaseService : IBaseService
    {
        private readonly ApplicationDbContext _dbContext;

        public BaseService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string GetRoomName(int roomId)
        {
            var existRoom = _dbContext.Rooms.FirstOrDefault(r => r.Id == roomId);
            if (existRoom != null)
            {
                var name = existRoom.Name + "." + existRoom.Building;
                return name;

            } else
            {
                throw new UserFriendlyException($"(Không tồn tại phòng có Id: \"{roomId}\"");
            }
        }
    }
}
