namespace Server.Dtos.LopHP
{
    public class AllLopHpByStudentIdDto
    {
        public int LopHpId { get; set; }
        public string? ClassName { get; set; }
        public int SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public required string TeacherId { get; set; }
    }
}
