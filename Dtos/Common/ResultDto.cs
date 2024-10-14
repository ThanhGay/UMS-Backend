namespace Server.Dtos.Common
{
    public class ResultDto<T>
    {
        public bool Status { get; set; }
        public IEnumerable<T>? Data { get; set; }
        public string? Message { get; set; }
    }
}
