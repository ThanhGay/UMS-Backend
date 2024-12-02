namespace Server.Dtos.LopHP
{
    public class AllLopHpByTeacherIdDto
    {
        public int LopHpId { get; set; }
        public string? ClassName { get; set; }
        public required string MaMonHoc { get; set; }
        public int SoTinChi { get; set; }
        public required string TenMonHoc { get; set; }
    }
}
