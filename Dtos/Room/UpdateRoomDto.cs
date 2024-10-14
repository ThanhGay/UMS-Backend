namespace Server.Dtos.Room
{
    public class UpdateRoomDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Building { get; set; }
    }
}
