namespace Server.Dtos.ChuongTrinhKhung
{
    public class CreateCTKDto
    {
        public required string ChuyenNganhId { get; set; }
        public required List<CreateDetailCTKDto> Details { get; set; }
    }
}
