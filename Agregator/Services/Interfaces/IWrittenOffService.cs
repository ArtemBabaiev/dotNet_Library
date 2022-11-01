using Catalog.BLL.DTO.Request;
using Catalog.BLL.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WrittenOffManagement.Application.DTO.Request;
using WrittenOffManagement.Application.DTO.Response;

namespace Agregator.Services.Interfaces
{
    public interface IWrittenOffService
    {

        Task<IEnumerable<WrittenOffResponse>> GetWrittenOffs();

        Task createWrittenOff(ExemplarRequest request, LiteratureResponse literature);
    }
}
