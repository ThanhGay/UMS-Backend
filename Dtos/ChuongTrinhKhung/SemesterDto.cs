using Server.Dtos.Subject;
using Server.Entities;

namespace Server.Dtos.ChuongTrinhKhung
{
    public class SemesterDto
    {
        public required string KiHoc { get; set; }
        public required List<DetailCTK_Subject_Dto> Subjects { get; set; }
        
    }
}
