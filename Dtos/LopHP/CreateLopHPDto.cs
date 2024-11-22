using System.ComponentModel.DataAnnotations;

namespace Server.Dtos.LopHP
{
    public class CreateLopHPDto
    {
        public string? ClassName { get; set; }
        public required string[] TeacherIds { get; set; }
        public required string MaMonHoc { get; set; }
        public required string TenMonHoc { get; set; }
        [Range(1, 3, ErrorMessage = "Số tín chỉ trong khoảng 1 - 3")]
        public int SoTinChi { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Số tiền của một tín chỉ phải lớn hơn 0")]
        public int PricePerTinChi { get; set; }

    }
}
