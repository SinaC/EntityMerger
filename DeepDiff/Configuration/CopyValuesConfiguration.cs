using System.Collections.Generic;
using System.Reflection;

namespace DeepDiff.Configuration
{
    internal sealed class CopyValuesConfiguration
    {
        public IReadOnlyCollection<PropertyInfo> CopyValuesProperties { get; init; } = null!;
    }
}