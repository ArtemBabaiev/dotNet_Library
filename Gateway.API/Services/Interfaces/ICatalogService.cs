using Catalog.BLL.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Gateway.API.Services.Interfaces
{
    public interface ICatalogService
    {
        Task deleteExemplar(long id);
        Task<LiteratureResponse> getById(long id);
    }
}
