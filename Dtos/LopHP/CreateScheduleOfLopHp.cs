namespace Server.Dtos.LopHP
{
    public class CreateScheduleOfLopHp
    {
        public int LopHpId { get; set; }
        public int RoomId { get; set; }
        public int CaHoc { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }
}
