using RecordManagment.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagment.BLL.Service.Interface
{
    public interface IExemplarService
    {
        Task<ExemplarDTO> CreateExemplar(ExemplarDTO exemplarDTO);
        Task<ExemplarDTO> GetExemplarById(long id);
        Task<List<ExemplarDTO>> GetAllExemplars();
        Task DeleteExemplar(long id);
        Task<ExemplarDTO> UpdateExemplar(ExemplarDTO exemplarDTO);
    }
}
