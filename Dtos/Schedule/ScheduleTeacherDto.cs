namespace Server.Dtos.Schedule
{
    public class ScheduleTeacherDto
    {
        public int Id { get; set; }
        public string? ClassName { get; set; }
        public string? SubjectName { get; set; }
        public int CaHoc { get; set; }
        public string? RoomName { get; set; }
        public DateTime StartAt { get; set; }
        public int Status { get; set; }
    }
}
