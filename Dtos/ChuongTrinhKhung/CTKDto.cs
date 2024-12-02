namespace Server.Dtos.ChuongTrinhKhung
{
    public class CTKDto
    {
        public int Id { get; set; }
        public required string ChuyenNganhId { get; set; }
        //public int TongTinChi { get; set; }
        public required List<SemesterDto> detailCTKByKiHocDtos { get; set; }
    }
}
