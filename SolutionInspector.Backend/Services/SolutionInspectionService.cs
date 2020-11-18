using SolutionInspector.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolutionInspector.Services
{
    public class SolutionInspectionService
    {
        readonly SolutionLoader _solutionLoader;
        readonly MetaDataTransformer _metaDataTransformer;

        public SolutionInspectionService(SolutionLoader solutionLoader, MetaDataTransformer metaDataTransformer)
        {
            _solutionLoader = solutionLoader;
            _metaDataTransformer = metaDataTransformer;
        }

        public Task<SolutionMetaData> GetSolutionMetaDataAsync() =>
            _solutionLoader.LoadAsync().ContinueWith(x => _metaDataTransformer.ConstructMetaData(x.Result));

        public Task<ProjectMetaData<T>> GetProjectMetaDataAsync<T>(Func<Project, T> projectMetaDataExtractor)
        {
            return _solutionLoader.LoadAsync().ContinueWith(x => _metaDataTransformer.ConstructProjectMetaData(x.Result, projectMetaDataExtractor));
        }

        public Task<Solution> GetSolutionAsync() =>
            _solutionLoader.LoadAsync();

         public Dictionary<string,string> Search(Solution sln, string propertyName)
         {
            return sln.Projects.ToDictionary( //when deserializing from disk the Properties dic doesnt get IgnoreCase comparer, so caseINsensitive search doesnt work
                x => x.ProjectFilePath,
                x => x.GetValue(propertyName)
                );
         }
    }
}
