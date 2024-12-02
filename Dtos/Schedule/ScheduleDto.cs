namespace Server.Dtos.Schedule
{
    public class ScheduleDto
    {
        public int ScheduleId { get; set; }
        public int LopHpId { get; set; }
        public required string ClassName { get; set; }
        public string? MaMonHoc { get; set; }
        public required string SubjectName { get; set; }
        public required string RoomName { get; set; }
        public int CaHoc { get; set; }
        public DateOnly StartAt { get; set; }
        public int Status { get; set; }
    }
}
