using Catalog.BLL.DTO.Request;
using Catalog.BLL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.BLL.Service.Interface
{
    public interface IWritingService
    {
        Task<IEnumerable<WritingResponse>> GetAsync();

        Task<WritingResponse> GetByIdAsync(long id);

        Task InsertAsync(WritingRequest request);

        Task UpdateAsync(WritingRequest request);

        Task DeleteAsync(long id);

        public Task<IEnumerable<WritingResponse>> GetAllWritingsWithAuthor(long authorId);

    }
}
