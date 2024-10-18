namespace Server.Dtos.LopHP
{
    public class AddStudentIntoLopHpDto
    {
        public int LopHpId { get; set; }
        public required List<string> StudentIds { get; set; }
    }
}
