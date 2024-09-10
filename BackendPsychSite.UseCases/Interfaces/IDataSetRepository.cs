using BackendPsychSite.Core.Models;
namespace BackendPsychSite.UseCases.Interfaces
{
    public interface IDataSetRepository
    {
        Task<Guid> CreateAsync(DataSet dataSet, DataSetBucket dataSetBucket);
        Task<Guid> DeleteAsync(Guid id);
        Task<List<DataSet>> GetAsyncByProject(DataSet dataSet, DataSetBucket dataSetBucket);
        Task<List<DataSet>> GetAsyncByUser(Guid userId);
        Task<Guid> Update(Guid id, string filePath);//TODO: Зачем, если через createasync получается обновлять датасет с тем же именем? Разве что только для изменения имени файла
    }
}
