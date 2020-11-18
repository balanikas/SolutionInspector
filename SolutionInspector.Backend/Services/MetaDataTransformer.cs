using SolutionInspector.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolutionInspector.Services
{
    public class MetaDataTransformer
    {
        public SolutionMetaData ConstructMetaData(Solution sln) => new()
        {
            Projects = sln.Projects.Select(GetProjectOverviewMetaData),
            TargetFrameworks = GetTargetFrameworks(sln),
            SignableAssemblies = GetSignableAssemblies(sln),
            UnstablePackages = GetUnstablePackages(sln)
        };

        public ProjectMetaData<T> ConstructProjectMetaData<T>(Solution sln, Func<Project, T> projectMetaDataExtractor) => new()
        {
            Projects = sln.Projects.Select(projectMetaDataExtractor),
        };

        string[] GetSignableAssemblies(Solution sln)
        {
            return sln.Projects.Select(x =>
            {
                if (bool.TryParse(x.GetValue("SignAssembly"), out _))
                {
                    return x.GetValue("AssemblyName");
                }

                return "";
                
            }).Where(x => x is not "" or null).ToArray();
        }

        string[] GetTargetFrameworks(Solution sln)
        {
            return sln.Projects.Select(x =>
            {
                return x.GetValue("TargetFramework");
            }).Where(x => x is not "" or null).Distinct().ToArray();
        }

        Dictionary<string, string> GetUnstablePackages(Solution sln)
        {
            var unstablePackages = new Dictionary<string, string>();
            foreach (var x in sln.Projects.SelectMany(x => x.PackageReferences))
            {
                if (x.Value.TryGetValue("Version", out var version))
                {
                    if (version.Contains("-"))
                    {
                        unstablePackages.TryAdd(x.Key, version);
                    }
                }
            }

            return unstablePackages;
        }

        Dictionary<string, string> GetUnstablePackages(Project proj)
        {
            var unstablePackages = new Dictionary<string, string>();
            foreach (var x in proj.PackageReferences)
            {
                if (x.Value.TryGetValue("Version", out var version))
                {
                    if (version.Contains("-"))
                    {
                        unstablePackages.Add(x.Key, version);
                    }
                }
            }

            return unstablePackages;
        }

        public ProjectOverviewMetaData GetProjectOverviewMetaData(Project proj) => new()
        {
            ContainsUnstablePackages = GetUnstablePackages(proj).Any().ToString(),
            AssemblyName = proj.GetValue("AssemblyName") + (proj.Succeeded ? "" : " (FAILED)"),
            TargetFramework = proj.GetValue("TargetFramework"),
            LangVersion = proj.GetValue("LangVersion"),
            GenerateDocumentationFile = proj.GetValue("GenerateDocumentationFile"),
            PackageCount = proj.PackageReferences.Count.ToString(),
            RuntimeIdentifiers = proj.GetValue("RuntimeIdentifiers"),
            SignAssembly = proj.GetValue("SignAssembly")
        };

        public ProjectCodeAnalysisMetaData GetProjectCodeAnalysisMetaData(Project proj) => new()
        {
            AssemblyName = proj.GetValue("AssemblyName"),
            UsesEditorConfig = proj.GetValue("UsesEditorConfig"),
            Optimize = proj.GetValue("Optimize"),
            WarningLevel = proj.GetValue("WarningLevel"),
            NoWarn = proj.GetValue("NoWarn"),
            WarningsAsErrors = proj.GetValue("WarningsAsErrors")
        };

        public ProjectPackagingMetaData GetProjectPackagingMetaData(Project proj) => new()
        {
            IsPackable = proj.GetValue("IsPackable"),
            Product = proj.GetValue("Product"),
            Description = proj.GetValue("Description"),
            Company = proj.GetValue("Company"),
            Authors = proj.GetValue("Authors"),
            Copyright = proj.GetValue("Copyright"),
            PackageProjectUrl = proj.GetValue("PackageProjectUrl"),
            PackageLicenseUrl = proj.GetValue("PackageLicenseUrl"),
            PackageTags = proj.GetValue("PackageTags"),
            PackageId = proj.GetValue("PackageId"),
            PackageVersion = proj.GetValue("PackageVersion"),
            PackageDescription = proj.GetValue("PackageDescription"),
            NuspecFile = proj.GetValue("NuspecFile"),
        };
    }
}
