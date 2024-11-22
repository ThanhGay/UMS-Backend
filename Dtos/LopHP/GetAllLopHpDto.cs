namespace Server.Dtos.LopHP
{
    public class GetAllLopHpDto
    {
        public int Id { get; set; }
        public string? ClassName { get; set; }
        public required string MaMonHoc { get; set; }
        public required string TenMonHoc { get; set; }
        public int SoTinChi { get; set; }
        public int PricePerTinChi { get; set; }
        public required List<string> TeacherIds { get; set; }
        public required int RealityStudent { get; set; }
        public int TotalStudents { get; set; }
        public int TotalLesson { get; set; }

    }
}
