using System.Collections.Generic;

namespace SolutionInspector.Data
{
    public record Project(
        IReadOnlyDictionary<string, ProjInfo[]> Items ,
        IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> PackageReferences,
        IReadOnlyDictionary<string, string> Properties,
        bool Succeeded,
        string ProjectFilePath);
}
