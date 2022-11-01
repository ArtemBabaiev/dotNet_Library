using RecordManagment.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagment.BLL.Service.Interface
{
    public interface ILiteratureService
    {
        Task<LiteratureDTO> CreateLiterature(LiteratureDTO literatureDTO);
        Task<LiteratureDTO> GetLiteratureById(long id);
        Task<List<LiteratureDTO>> GetAllLiterature();
        Task DeleteLiterature(long id);
        Task<LiteratureDTO> UpdateLiterature(LiteratureDTO literatureDTO);
    }
}
