using RecordManagment.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagment.DAL.Repository.Interface
{
    public interface IExemplarRepository : IGenericRepository<Exemplar>
    {
        Task<IEnumerable<Exemplar>> GetExemplarsInUse();
        Task<IEnumerable<Exemplar>> GetExemplarsInUseByReader(long readerId);
        Task<IEnumerable<Exemplar>> GetExemplarsTakenInPeriod(DateTime lowerDate, DateTime upperDate);
    }
}
