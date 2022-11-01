using Aggregator.DTO;

namespace Agregator.Services.Interfaces
{
    public interface IRecordMngmtService
    {
        Task deleteExemplar(long id);
        Task deleteLiterature(long id);
        Task createLiterature(LiteratureAggDTO literature);
        Task updateLiterature(long id, LiteratureAggDTO literature);
    }
}
