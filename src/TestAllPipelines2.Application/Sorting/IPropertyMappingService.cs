using System.Collections.Generic;
using TestAllPipelines2.Application.Sorting.Models;

namespace TestAllPipelines2.Application.Sorting
{
    public interface IPropertyMappingService
    {
        IEnumerable<SortCriteria> Resolve(BaseSortable sortableSource, BaseSortable sortableTarget);
    }
}
