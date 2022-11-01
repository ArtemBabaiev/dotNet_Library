using Dapper;
using RecordManagment.DAL.Model;
using RecordManagment.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagment.DAL.Repository
{
    public class ReaderRepository : GenericRepository<Reader>, IReaderRepository
    {
        public ReaderRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction) : base(sqlConnection, dbTransaction, "readers")
        {
        }

        public Task<IEnumerable<Reader>> GetInactiveAllInactiveUsersInPeriod(DateTime lowerDate, DateTime upperDate)
        {
            var sql = "exec [GetInactiveAllInactiveUsersInPeriod] @LowerDate, @UpperDate";
            var values = new { LowerDate = lowerDate, UpperDate = upperDate };
            var result = sqlConnection.QueryAsync<Reader>(
                sql: sql,
                param: values,
                transaction: dbTransaction);
            return result;
        }
    
    }
}
