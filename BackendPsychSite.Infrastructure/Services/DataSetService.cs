using BackendPsychSite.Core.Models;
using BackendPsychSite.UseCases.Interfaces;
using BackendPsychSite.UseCases.Utils;
namespace BackendPsychSite.Infrastructure.Services
{
    public class DataSetService : IDataSetService
    {
        private IDataSetRepository _repo;
        public DataSetService(IDataSetRepository repository)
        {
            _repo = repository;
        }
        public DataSetService()
        {

        }
        public async Task<Guid> CreateAsync(Stream dataSetFile, DataSetArgs dataSetArgs)
        {
            var prefix = dataSetArgs.ProjectName.ToLower().Trim() + "/" + dataSetArgs.Name;
            DataSet dataSet = new DataSet(Guid.NewGuid(), dataSetArgs.Path, dataSetArgs.Name, prefix, dataSetFile);
            DataSetBucket dataSetBucket = new DataSetBucket { BucketName = dataSetArgs.UserEmail, ProjectPart = dataSetArgs.ProjectName, UserPart = dataSetArgs.UserEmail };
            await _repo.CreateAsync(dataSet, dataSetBucket);
            return dataSet.Id;
        }

        public Task<Guid> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<DataSet>> GetAsyncByProject()
        {
            throw new NotImplementedException();
        }

        public Task<List<DataSet>> GetAsyncByUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> Update(Guid id, string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
