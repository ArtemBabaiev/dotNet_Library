using Aggregator.DTO;
using Catalog.BLL.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Agregator.Services.Interfaces
{
    public interface ICatalogService
    {
        Task deleteExemplar(long id);
        Task<LiteratureResponse> getLiteratureById(long id);
        Task deleteLiterature(long id);
        Task createLiterature(LiteratureAggDTO literature);
        Task updateLiterature(long id, LiteratureAggDTO literature);
    }
}
