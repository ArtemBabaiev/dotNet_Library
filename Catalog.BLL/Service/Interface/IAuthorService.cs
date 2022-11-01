using Catalog.BLL.DTO.Request;
using Catalog.BLL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.BLL.Service.Interface
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorResponse>> GetAsync();

        Task<AuthorResponse> GetByIdAsync(long id);

        Task InsertAsync(AuthorRequest request);

        Task UpdateAsync(AuthorRequest request);

        Task DeleteAsync(long id);
    }
}
