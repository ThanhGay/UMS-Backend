using Microsoft.AspNetCore.Mvc;

namespace Server.Dtos.Schedule
{
    public class FilterScheduleDto
    {
        [FromQuery(Name = "nameBuilding")]
        public string? Building { get; set; }

        [FromQuery(Name = "caHoc")]
        public int CaHoc { get; set; } = 1;

        [FromQuery(Name = "teacherId")]
        public string? TeacherId { get; set; }
    }
}
