using RecordManagment.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagment.BLL.Service.Interface
{
    public interface IReaderService
    {
        Task<ReaderDTO> CreateReader(ReaderDTO readerDTO);
        Task<ReaderDTO> GetReaderById(long id);
        Task<List<ReaderDTO>> GetAllReaders();
        Task DeleteReader(long id);
        Task<ReaderDTO> UpdateReader(ReaderDTO readerDTO);
    }
}
