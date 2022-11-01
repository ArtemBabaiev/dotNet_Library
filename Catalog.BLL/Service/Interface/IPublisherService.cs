using Catalog.BLL.DTO.Request;
using Catalog.BLL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.BLL.Service.Interface
{
    public interface IPublisherService
    {
        Task<IEnumerable<PublisherResponse>> GetAsync();

        Task<PublisherResponse> GetByIdAsync(long id);

        Task InsertAsync(PublisherRequest request);

        Task UpdateAsync(PublisherRequest request);

        Task DeleteAsync(long id);
    }
}
