﻿using System;

namespace TestAllPipelines2.Application.Sorting
{
    // Marker interface, to facilitate generic collections.
    public interface IPropertyMapping
    {
        Type Source { get; }
        Type Target { get; }

        PropertyMappingValue GetMapping(string sourcePropertyName);
    }
}
