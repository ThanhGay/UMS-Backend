using Server.Dtos.Common;
using Server.Dtos.LopHP;
using Server.Entities;

namespace Server.Services.Interfaces
{
    public interface ILopHPService
    {
        public void CreateLopHP(CreateLopHPDto input);
        public PageResultDto<GetAllLopHpDto> GetAll(FilterDto input);
        public void CreateScheduleOfLopHP(CreateScheduleOfLopHp input);
        public List<GetScheduleDto> GetSchedule(int lopHpId);
        public List<string> GetStudents(int lopHpId);
        public void AddStudents(AddStudentIntoLopHpDto input);
        public GetDetailLopHpDto GetDetailLopHp(int lopHpId);
    }
}
