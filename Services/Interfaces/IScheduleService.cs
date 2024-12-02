using Server.Dtos.Schedule;

namespace Server.Services.Interfaces
{
    public interface IScheduleService
    {
        public List<GetScheduleDto> GetScheduleOfClassHp(int lopHpId);
        public void CreateScheduleOfClassHP(CreateScheduleOfLopHp input);
        public List<ScheduleDto> ScheduleOfTeacher(string teacherId);
        public List<ScheduleDto> ScheduleOfStudent(string studentId);
        public void PostponeLesson(int id);
    }
}
