using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Entities
{
    [Table(nameof(ChuongTrinhKhung))]
    public class ChuongTrinhKhung
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string ChuyenNganhId { get; set; }
        public int TongTinChi { get; set; } = 0;
        public DateTime CreateAt { get; set; }
    }
}
