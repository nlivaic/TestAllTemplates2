﻿using System.Collections.Generic;
using TestAllPipelines2.Application.Sorting.Models;

namespace TestAllPipelines2.Application.Tests.Helpers
{
    public class ResourceParameters2
        : BaseSortable<MappingSourceModel2>
    {
        public override IEnumerable<SortCriteria> SortBy { get; set; } = new List<SortCriteria>();
    }
}
