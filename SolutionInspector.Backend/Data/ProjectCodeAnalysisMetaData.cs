namespace SolutionInspector.Data
{
    public record ProjectCodeAnalysisMetaData
    {
        public string AssemblyName { get; init; } = "";
        public string UsesEditorConfig { get; init; } = "";
        public string Optimize { get; init; } = "";
        public string WarningLevel { get; init; } = "";
        public string NoWarn { get; init; } = "";
        public string WarningsAsErrors { get; init; } = "";
    }
}
