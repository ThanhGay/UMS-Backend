namespace Server.Dtos.ChuongTrinhKhung
{
    public class DetailCTK_Subject_Dto
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public required string MaHocPhan { get; set; }
        public required string SubjectName { get; set; }
        public int SoTinChi { get; set; }
        //public required string KiHoc { get; set; }

    }
}
