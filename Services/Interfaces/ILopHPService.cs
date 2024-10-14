using Server.Dtos.Common;
using Server.Dtos.LopHP;
using Server.Entities;

namespace Server.Services.Interfaces
{
    public interface ILopHPService
    {
        public void CreateLopHP(CreateLopHPDto input);
        public PageResultDto<LopHpDto> GetAll(FilterDto input);
        public void CreateScheduleOfLopHP(CreateScheduleOfLopHp input);

        public List<LopHP_Room> GetSchedule(int lopHpId);
    }
}
