using Server.Dtos.Schedule;

namespace Server.Dtos.LopHP
{
    public class GetDetailLopHpDto
    {
        public int Id { get; set; }
        public string? ClassName { get; set; }
        public string? MaHocPhan { get; set; }
        public string? SubjectName { get; set; }
        public int SoTinChi { get; set; }
        public int PricePerTinChi { get; set; }
        public string? TeacherId { get; set; }
        public int RealityStudent { get; set; }
        public int TotalStudents { get; set; }
        public int TotalLesson { get; set; }
        public List<string>? Students { get; set; }
        public List<GetScheduleDto>? Schedules { get; set; }
    }
}
