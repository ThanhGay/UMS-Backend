namespace Server.Dtos.Subject
{
    public class SubjectDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string MaMonHoc { get; set; }
        public int SoTinChi { get; set; }
        public required string BoMonId { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
