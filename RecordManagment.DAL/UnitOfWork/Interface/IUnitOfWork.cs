using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecordManagment.DAL.Repository.Interface;

namespace RecordManagment.DAL.UnitOfWork.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IExemplarRepository ExemplarRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        ILiteratureRepository LiteratureRepository { get; }
        IReaderRepository ReaderRepository { get; }
        IRecordRepository RecordRepository { get; }
        void Commit();
        void Dispose();
    }
}
