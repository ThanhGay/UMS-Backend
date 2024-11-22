using Server.Dtos.Schedule;

namespace Server.Services.Interfaces
{
    public interface IScheduleService
    {
        public List<GetScheduleDto> GetScheduleOfClassHp(int lopHpId);
        public void CreateScheduleOfClassHP(CreateScheduleOfLopHp input);
        public List<ScheduleTeacherDto> ScheduleOfTeacher(string teacherId);
        public void PostponeLesson(int id);
    }
}
