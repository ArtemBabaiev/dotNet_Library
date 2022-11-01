using Catalog.BLL.DTO.Request;
using Catalog.BLL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.BLL.Service.Interface
{
    public interface IExemplarService
    {
        Task<IEnumerable<ExemplarResponse>> GetAsync();

        Task<ExemplarResponse> GetByIdAsync(long id);

        Task InsertAsync(ExemplarRequest request);

        Task UpdateAsync(ExemplarRequest request);

        Task DeleteAsync(long id);

        Task DeleteUsingRabbitMQ(long id);
    }
}
