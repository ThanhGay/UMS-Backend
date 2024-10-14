using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Entities
{
    [Table(nameof(MonHoc_ChuongTrinhKhung))]
    public class MonHoc_ChuongTrinhKhung
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int ChuongTrinhKhungId { get; set; }
        public required string KiHoc { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
