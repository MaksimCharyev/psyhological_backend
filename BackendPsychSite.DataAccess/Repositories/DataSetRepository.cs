using BackendPsychSite.Core.Models;
using BackendPsychSite.UseCases.Interfaces;
using Minio;
using Minio.DataModel.Args;

namespace BackendPsychSite.DataAccess.Repositories
{
    public class DataSetRepository : IDataSetRepository
    {
        private IMinioClient _client;
        public DataSetRepository(IMinioClient minioClient)
        {
            _client = minioClient;

        }
        public DataSetRepository()
        {

        }
        public async Task<Guid> CreateAsync(DataSet dataSet, DataSetBucket dataSetBucket)
        {
            BucketExistsArgs bucketExistsArgs = new BucketExistsArgs().WithBucket(dataSetBucket.UserPart);
            if (!await _client.BucketExistsAsync(bucketExistsArgs))
            {
                MakeBucketArgs makeBucketArgs = new MakeBucketArgs().WithBucket(dataSetBucket.UserPart);
                await _client.MakeBucketAsync(makeBucketArgs);
            }
            PutObjectArgs putObjectArgs = new PutObjectArgs().WithBucket(dataSetBucket.UserPart).WithObject(dataSet.Prefix).WithStreamData(dataSet.Stream).WithObjectSize(dataSet.Stream.Length).WithContentType("text/csv");
            await _client.PutObjectAsync(putObjectArgs);
            return dataSet.Id;
        }

        public Task<Guid> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DataSet>> GetAsyncByProject(DataSet dataSet, DataSetBucket dataSetBucket)
        {
            BucketExistsArgs bucketExistsArgs = new BucketExistsArgs().WithBucket(dataSetBucket.UserPart); //TODO: dataSetBucket.UserPart
            if (!await _client.BucketExistsAsync(bucketExistsArgs))
            {
                throw new NotImplementedException();
            }
            await _client.GetObjectAsync(new GetObjectArgs().WithBucket(dataSetBucket.BucketName).WithObject(dataSet.Name));
            return null;
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
