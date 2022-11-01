using Catalog.BLL.DTO.Request;
using Catalog.BLL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.BLL.Service.Interface
{
    public interface ITypeService
    {
        Task<IEnumerable<TypeResponse>> GetAsync();

        Task<TypeResponse> GetByIdAsync(long id);

        Task InsertAsync(TypeRequest request);

        Task UpdateAsync(TypeRequest request);

        Task DeleteAsync(long id);
    }
}
