namespace SolutionInspector.Data
{
    public record ProjectPackagingMetaData
    {
        public string IsPackable { get; init; } = "";
        public string Product { get; init; } = "";
        public string Description { get; init; } = "";
        public string Company { get; init; } = "";
        public string Authors { get; init; } = "";
        public string Copyright { get; init; } = "";
        public string PackageProjectUrl { get; init; } = "";
        public string PackageLicenseUrl { get; init; } = "";
        public string PackageTags { get; init; } = "";
        public string PackageId { get; init; } = "";
        public string PackageVersion { get; init; } = "";
        public string PackageDescription { get; init; } = "";
        public string NuspecFile { get; init; } = "";
    }
}
