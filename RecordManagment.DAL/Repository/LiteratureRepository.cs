using Dapper;
using RecordManagment.DAL.Model;
using RecordManagment.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagment.DAL.Repository
{
    public class LiteratureRepository : GenericRepository<Literature>, ILiteratureRepository
    {
        public LiteratureRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction) : base(sqlConnection, dbTransaction, "literature")
        {
        }

        public async Task<IEnumerable<long>> GetTenMostPopularLiteratureIds()
        {
            string sql = @"exec [Get10MostPopularLiteratureIds]";
            
            var results = await sqlConnection.QueryAsync<long>(
                sql,
                transaction: dbTransaction
                );
            return results;
        }
    }
}
