using Server.DbContexts;
using Server.Dtos.Common;
using Server.Dtos.Room;
using Server.Entities;
using Server.Exceptions;
using Server.Services.Interfaces;

namespace Server.Services.Implements
{
    public class BuildingService : IBuildingService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBaseService _baseService;
        public BuildingService(ApplicationDbContext dbContext, IBaseService baseService)
        {
            _dbContext = dbContext;
            _baseService = baseService;
        }
        public void CreateRoom(CreateRoomDto input)
        {
            var existRoom = _dbContext.Rooms.Any(r => r.Name == input.Name);
            if (existRoom)
            {
                throw new UserFriendlyException($"Đã có phòng {input.Name}");

            }
            else
            {
                var newRoom = new Room
                {
                    Name = input.Name,
                    Building = input.Building,
                };

                _dbContext.Rooms.Add(newRoom);
                _dbContext.SaveChanges();
            }
        }

        public void DeleteRoom(int id)
        {
            var existRoom = _dbContext.Rooms.FirstOrDefault(r => r.Id == id);
            if (existRoom != null)
            {
                _dbContext.Rooms.Remove(existRoom);
                _dbContext.SaveChanges();
            }
            else
            {

                throw new UserFriendlyException($"Không thấy phòng có Id: \"{id}\"");
            }
        }

        public PageResultDto<Room> GetAll()
        {
            var result = new PageResultDto<Room>();

            var roomQuery = _dbContext.Rooms.ToList();

            var totalItem = roomQuery.Count();

            result.TotalItem = totalItem;
            result.Items = roomQuery;

            return result;
        }

        public Room GetRoomById(int id)
        {
            var existRoom = _dbContext.Rooms.FirstOrDefault(r => r.Id == id);
            if (existRoom != null)
            {
                return existRoom;
            }
            else
            {

                throw new UserFriendlyException($"Không thấy phòng có Id: \"{id}\"");
            }
        }

        public void UpdateRoom(UpdateRoomDto input)
        {
            var existRoom = _dbContext.Rooms.FirstOrDefault(r => r.Id == input.Id);
            if (existRoom != null)
            {
                existRoom.Name = input.Name;
                existRoom.Building = input.Building;

                _dbContext.SaveChanges();
            }
            else
            {

                throw new UserFriendlyException($"Không thấy phòng có Id: \"{input.Id}\"");
            }
        }
    }
}
