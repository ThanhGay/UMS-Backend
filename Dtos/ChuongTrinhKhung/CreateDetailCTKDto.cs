namespace Server.Dtos.ChuongTrinhKhung
{
    public class CreateDetailCTKDto
    {
        public required string KiHoc { get; set; }
        public required List<int> MonHocIds { get; set; }
    }
}
