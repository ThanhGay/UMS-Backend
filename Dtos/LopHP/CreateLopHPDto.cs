using System.ComponentModel.DataAnnotations;

namespace Server.Dtos.LopHP
{
    public class CreateLopHPDto
    {
        public string? ClassName { get; set; }
        public required string TeacherId { get; set; }
        public int SubjectId { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Số tiền của một tín chỉ phải lớn hơn 0")]
        public int PricePerTinChi { get; set; }

    }
}
