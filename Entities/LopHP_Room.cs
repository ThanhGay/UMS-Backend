using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Entities
{
    [Table(nameof(LopHP_Room))]
    public class LopHP_Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int LopHpId { get; set; }
        public required string TeacherId { get; set; }
        public int RoomId { get; set; }
        public int CaHoc { get; set; }
        public DateOnly StartAt { get; set; }
        public int Status { get; set; } = 0; // 0 - not taken place, 1 - taken place, 2 - postponed
    }
}
