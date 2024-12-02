namespace Server.Dtos.Schedule
{
    public class CreateScheduleOfLopHp
    {
        public int LopHpId { get; set; }
        public required string TeacherId { get; set; }
        public int RoomId { get; set; }
        public int CaHoc { get; set; }
        public DateOnly StartAt { get; set; }
        public DateOnly EndAt { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }
}
