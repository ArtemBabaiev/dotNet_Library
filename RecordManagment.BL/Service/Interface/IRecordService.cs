using RecordManagment.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagment.BLL.Service.Interface
{
    public interface IRecordService
    {
        Task<RecordDTO> CreateRecord(RecordDTO recordDTO);
        Task<RecordDTO> GetRecordById(long id);
        Task<List<RecordDTO>> GetAllRecords();
        Task DeleteRecord(long id);
        Task<RecordDTO> UpdateRecord(RecordDTO recordDTO);
    }
}
