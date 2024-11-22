using System.ComponentModel.DataAnnotations;

namespace Server.Dtos.Subject
{
    public class UpdateSubjectDto
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mã học phần không được để trống.")]
        public required string MaMonHoc { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên môn học không được để trống.")]
        public required string Name { get; set; }
        [Range(1, 3, ErrorMessage = "Số tín chỉ ít nhất 1 tín, tối đa 3 tín.")]
        public int SoTinChi { get; set; }
        public required string BoMonId { get; set; }
    }
}
