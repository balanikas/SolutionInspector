using SolutionInspector.Data;
using Buildalyzer;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SolutionInspector.Services
{

    public class SolutionLoader
    {
        readonly IWebHostEnvironment _hostEnvironment;

        public SolutionLoader(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public Task<Solution> LoadAsync()
        {
            if (State.Solution is not null) return Task.FromResult(State.Solution); 

            var cached = ReadFromDisk();
            if (cached is not null)
            {
                State.Solution = cached;
                return Task.FromResult(cached);
            }

            return Task.Run(() =>
            {
                ConcurrentBag<IAnalyzerResult> analyzerResults = new();
                AnalyzerManager manager = new(State.SolutionPath);

                Parallel.ForEach(manager.Projects, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, x =>
                {
                    var projectAnalyzer = manager.GetProject(x.Key);
                    var analyzerResult = projectAnalyzer.Build();

                    if (analyzerResult.Count > 1) throw new Exception("multi-targeting is not supported");
                    if(analyzerResult.Count == 1) analyzerResults.Add(analyzerResult.First());
                });

                var projs = analyzerResults.Select(x => new Project(
                    x.Items.ToDictionary(a => a.Key, a => a.Value.Select(b => new ProjInfo(b.Metadata)).ToArray()),
                    x.PackageReferences,
                    new Dictionary<string, string>(x.Properties, StringComparer.OrdinalIgnoreCase),
                    x.Succeeded,
                    x.ProjectFilePath
                )).ToArray();

                Solution sln = new(projs);

                WriteToDisk(sln);

                return sln;
            });
        }

        private Solution? ReadFromDisk()
        {
            var fileName = State.SolutionPath.Replace("\\", "-") + ".json";
            var filePath = Path.Combine(_hostEnvironment.ContentRootPath, fileName);
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<Solution>(json);
            }

            return null;
        }

        private void WriteToDisk(Solution sln)
        {
            var json = JsonSerializer.Serialize(sln, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            var fileName = State.SolutionPath.Replace("\\", "-") + ".json";
            File.WriteAllText(Path.Combine(_hostEnvironment.ContentRootPath, fileName), json);
        }
    }
}
