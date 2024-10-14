using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Entities
{
    [Table(nameof(LopHP))]
    public class LopHP
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? ClassName { get; set; }
        public int SubjectId { get; set; }
        public required string TeacherId { get; set; }
        public int SoTinChi { get; set; }
        public int PricePerTinChi { get; set; }
        public int TotalLessons { get; set; }
        public int TotalStudents { get; set; } = 50;
    }
}
