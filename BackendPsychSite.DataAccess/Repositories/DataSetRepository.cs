using BackendPsychSite.Core.Models;
using BackendPsychSite.DataAccess.Entities;
using BackendPsychSite.UseCases.Interfaces;
using BackendPsychSite.UseCases.Utils;
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
            DataSetBucketEntity dataSetBucketEntity = new DataSetBucketEntity(dataSetBucket.BucketName, dataSetBucket.UserPart, dataSetBucket.ProjectPart);
            DataSetEntity dataSetEntity = new DataSetEntity { Id = dataSet.Id, Name = dataSet.Prefix, Stream = dataSet.Stream, FilePath = dataSet.Path, Bucket = dataSetBucketEntity };
            BucketExistsArgs bucketExistsArgs = new BucketExistsArgs().WithBucket(dataSetBucketEntity.UserPart);
            if (!await _client.BucketExistsAsync(bucketExistsArgs))
            {
                MakeBucketArgs makeBucketArgs = new MakeBucketArgs().WithBucket(dataSetBucketEntity.UserPart);
                await _client.MakeBucketAsync(makeBucketArgs);
            }
            PutObjectArgs putObjectArgs = new PutObjectArgs().WithBucket(dataSetBucketEntity.UserPart).WithObject(dataSetEntity.Name).WithStreamData(dataSet.Stream).WithObjectSize(dataSet.Stream.Length).WithContentType("application/xml");
            await _client.PutObjectAsync(putObjectArgs);
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
