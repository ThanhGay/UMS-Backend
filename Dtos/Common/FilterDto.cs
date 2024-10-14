using Microsoft.AspNetCore.Mvc;

namespace Server.Dtos.Common
{
    public class FilterDto
    {
        [FromQuery(Name = "pageIndex")]
        public int PageIndex { get; set; } = 1;

        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = 100;

        private string? _keyword;
        [FromQuery(Name = "keyword")]
        public string? Keyword
        {
            get => _keyword;
            set => _keyword = value?.Trim();
        }

        public int SkipCount()
        {
            return (PageIndex - 1) * PageSize;
        }
    }
}
