namespace Server.Dtos.LopHP
{
    public class AddTeacherIntoLopHpDto
    {
        public int LopHpId { get; set; }
        public required string TeacherId { get; set; }
    }
}
