using System.Collections.Generic;

namespace Application.DTOs
{
    public class DiffDto
    {
        public string DiffResultType { get; set; }
        public ICollection<StatsDto> Diffs { get; set; } = new List<StatsDto>();
    }
}