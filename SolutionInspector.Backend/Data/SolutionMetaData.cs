using System.Collections.Generic;
using System.Linq;

namespace SolutionInspector.Data
{
    public record SolutionMetaData
    {
        public IEnumerable<ProjectOverviewMetaData> Projects { get; init; } = Enumerable.Empty<ProjectOverviewMetaData>();
        public IEnumerable<string> TargetFrameworks { get; init; } = Enumerable.Empty<string>();
        public IEnumerable<string> SignableAssemblies { get; init; } = Enumerable.Empty<string>();
        public Dictionary<string, string> UnstablePackages { get; init; } = new Dictionary<string, string>();
    }

    public record ProjectMetaData<T>
    {
        public IEnumerable<T> Projects { get; init; } = Enumerable.Empty<T>();
    }
}
