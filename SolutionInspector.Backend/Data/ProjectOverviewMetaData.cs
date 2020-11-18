namespace SolutionInspector.Data
{
    public record ProjectOverviewMetaData
    {
        public string AssemblyName { get; init; } = "";
        public string OutputType { get; init; } = "";
        public string TargetFramework { get; init; } = "";
        public string LangVersion { get; init; } = "";
        public string ProducesDocumentation { get; init; } = "";
        public string ContainsUnstablePackages { get; init; } = "";
        public string PackageCount { get; init; } = "";
        public string GenerateDocumentationFile { get; init; } = "";
        public string RuntimeIdentifiers { get; init; } = "";
        public string SignAssembly { get; init; } = "";
    }
}
