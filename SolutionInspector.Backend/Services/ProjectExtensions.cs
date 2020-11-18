using SolutionInspector.Data;

namespace SolutionInspector.Services
{
    public static class ProjectExtensions
    {
        public static string GetValue(this Project proj, string key) =>
            proj.Properties.TryGetValue(key, out var value) ? value : "";
    }
}
