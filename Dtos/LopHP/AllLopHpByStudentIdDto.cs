namespace Server.Dtos.LopHP
{
    public class AllLopHpByStudentIdDto
    {
        public int LopHpId { get; set; }
        public string? ClassName { get; set; }
        public required string MaMonHoc { get; set; }
        public required string TenMonHoc { get; set; }
        public required List<string> TeacherIds { get; set; }
    }
}
