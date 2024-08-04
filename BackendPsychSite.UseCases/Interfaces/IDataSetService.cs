using BackendPsychSite.Core.Models;
using BackendPsychSite.UseCases.Utils;
namespace BackendPsychSite.UseCases.Interfaces
{
    public interface IDataSetService
    {
        Task<Guid> CreateAsync(Stream dataSetFile, DataSetArgs dataSetArgs);
        Task<Guid> DeleteAsync(Guid id);
        Task<List<DataSet>> GetAsyncByProject();
        Task<List<DataSet>> GetAsyncByUser(Guid userId);
        Task<Guid> Update(Guid id, string filePath);
    }
}
