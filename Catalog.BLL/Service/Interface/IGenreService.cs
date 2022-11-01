using Catalog.BLL.DTO.Request;
using Catalog.BLL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.BLL.Service.Interface
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreResponse>> GetAsync();

        Task<GenreResponse> GetByIdAsync(long id);

        Task InsertAsync(GenreRequest request);

        Task UpdateAsync(GenreRequest request);

        Task DeleteAsync(long id);
    }
}
