namespace Server.Dtos.ChuongTrinhKhung
{
    public class CreateDetailCTKDto
    {
        public required string KiHoc { get; set; }
        public required List<string> MaMonHocs { get; set; }
    }
}
