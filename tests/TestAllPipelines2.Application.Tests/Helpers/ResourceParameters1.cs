using System.Collections.Generic;
using TestAllPipelines2.Application.Sorting.Models;

namespace TestAllPipelines2.Application.Tests.Helpers
{
    public class ResourceParameters1
        : BaseSortable<MappingSourceModel1>
    {
        public override IEnumerable<SortCriteria> SortBy { get; set; } = new List<SortCriteria>();
    }
}
