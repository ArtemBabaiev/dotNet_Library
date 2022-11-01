using RecordManagment.DAL.Repository.Interface;
using RecordManagment.DAL.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagment.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public IExemplarRepository ExemplarRepository { get; }
        public ILiteratureRepository LiteratureRepository { get; }
        public IEmployeeRepository EmployeeRepository { get; }
        public IReaderRepository ReaderRepository { get; }
        public IRecordRepository RecordRepository { get; }

        readonly IDbTransaction _dbTransaction;

        public UnitOfWork(IExemplarRepository exemplarRepository, ILiteratureRepository literatureRepository, IEmployeeRepository employeeRepository, IReaderRepository readerRepository, IRecordRepository recordRepository, IDbTransaction dbTransaction)
        {
            ExemplarRepository = exemplarRepository;
            LiteratureRepository = literatureRepository;
            EmployeeRepository = employeeRepository;
            ReaderRepository = readerRepository;
            RecordRepository = recordRepository;
            _dbTransaction = dbTransaction;
        }

        public void Commit()
        {
            try
            {
                _dbTransaction.Commit();
                // By adding this we can have muliple transactions as part of a single request
                //_dbTransaction.Connection.BeginTransaction();
            }
            catch (Exception ex)
            {
                _dbTransaction.Rollback();
            }
        }

        public void Dispose()
        {
            _dbTransaction.Connection?.Close();
            _dbTransaction.Connection?.Dispose();
            _dbTransaction.Dispose();
        }
    }
}
