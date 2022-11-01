using Catalog.BLL.DTO.Request;
using Catalog.BLL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.BLL.Service.Interface
{
    public interface ILiteratureService
    {
        Task<IEnumerable<LiteratureResponse>> GetAsync();

        Task<LiteratureResponse> GetByIdAsync(long id);

        Task InsertAsync(LiteratureRequest request);

        Task UpdateAsync(LiteratureRequest request);

        Task DeleteAsync(long id);

        public Task<IEnumerable<LiteratureResponse>> GetAllWithAuthor(long authorId);

    }
}
