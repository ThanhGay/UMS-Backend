using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Entities
{
    [Table(nameof(LopHP_Student))]
    public class LopHP_Student
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int LopHpId { get; set; }
        public required string StudentId { get; set; }
        public double? DiemCC { get; set; }
        public double? DiemGK { get; set; }
        public double? DiemCK { get; set; }
        public double? DiemKT { get; set; }
        public bool IsSaved { get; set; } = false;
    }
}
