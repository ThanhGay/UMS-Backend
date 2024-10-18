namespace Server.Dtos.LopHP
{
    public class GetScheduleDto
    {
        public int Id { get; set; }
        public int LopHpId { get; set; }
        public int RoomId { get; set; }
        public required string RoomName { get; set; }
        public int CaHoc { get; set; }
        public DateTime StartAt { get; set; }
        public int Status { get; set; }
    }
}
