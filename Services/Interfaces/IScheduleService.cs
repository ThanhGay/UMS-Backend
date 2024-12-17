using Server.Dtos.Common;
using Server.Dtos.Schedule;

namespace Server.Services.Interfaces
{
    public interface IScheduleService
    {
        public void SyncSchedule();
        public List<GetScheduleDto> GetScheduleOfClassHp(int lopHpId);
        public PageResultDto<ScheduleDto> GetAll(FilterDto pageFilter, FilterScheduleDto filter);
        public void CreateScheduleOfClassHP(CreateScheduleOfLopHp input);
        public List<ScheduleDto> ScheduleOfTeacher(string teacherId);
        public List<ScheduleDto> ScheduleOfStudent(string studentId);
        public void PostponeLesson(int id);
    }
}
