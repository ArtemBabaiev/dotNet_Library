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
    public class ExemplarRepository : GenericRepository<Exemplar>, IExemplarRepository
    {
        public ExemplarRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction) : base(sqlConnection, dbTransaction, "exemplars")
        {
        }

        public async Task<IEnumerable<Exemplar>> GetExemplarsInUse()
        {
            SqlCommand command = new SqlCommand();
            command.CommandText = $"SELECT * FROM exemplars WHERE IsLend = 1";
            command.Connection = sqlConnection;
            SqlDataReader sqlReader = await command.ExecuteReaderAsync();

            List<Exemplar> result = new List<Exemplar>();
            while (sqlReader.Read())
            {
                result.Add(
                    new Exemplar
                    {
                        Id = sqlReader.GetInt64(0),
                        LiteratureId = sqlReader.GetInt64(1),
                        IsLend = sqlReader.GetBoolean(2),
                        CreatedAt = sqlReader.GetDateTime(3),
                        UpdatedAt = sqlReader.GetDateTime(4)
                    }
                    );
            }
            return result;
        }

        public async Task<IEnumerable<Exemplar>> GetExemplarsInUseByReader(long readerId)
        {
            SqlCommand command = new SqlCommand();
            command.CommandText = $"GetExemplarsInUseByReader";
            command.Connection = sqlConnection;
            command.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter readerIdParam = new SqlParameter
            {
                ParameterName = "@ReaderId",
                Value = readerId
            };
            command.Parameters.Add(readerIdParam);

            SqlDataReader sqlReader = await command.ExecuteReaderAsync();

            List<Exemplar> result = new List<Exemplar>();
            while (sqlReader.Read())
            {
                result.Add(
                    new Exemplar
                    {
                        Id = sqlReader.GetInt64(0),
                        LiteratureId = sqlReader.GetInt64(1),
                        IsLend = sqlReader.GetBoolean(2),
                        CreatedAt = sqlReader.GetDateTime(3),
                        UpdatedAt = sqlReader.GetDateTime(4)
                    }
                    );
            }
            return result;
        }

        public async Task<IEnumerable<Exemplar>> GetExemplarsTakenInPeriod(DateTime lowerDate, DateTime upperDate)
        {
            SqlCommand command = new SqlCommand();
            command.CommandText = $"GetExemplarsTakenInPeriod";
            command.Connection = sqlConnection;
            command.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter lowerDateParam = new SqlParameter
            {
                ParameterName = "@LowerDate",
                Value = lowerDate
            };
            SqlParameter upperDateParam = new SqlParameter
            {
                ParameterName = "@UpperDate",
                Value = upperDate
            };
            command.Parameters.Add(lowerDateParam);
            command.Parameters.Add(upperDateParam);

            SqlDataReader sqlReader = await command.ExecuteReaderAsync();

            List<Exemplar> result = new List<Exemplar>();
            while (sqlReader.Read())
            {
                result.Add(
                    new Exemplar
                    {
                        Id = sqlReader.GetInt64(0),
                        LiteratureId = sqlReader.GetInt64(1),
                        IsLend = sqlReader.GetBoolean(2),
                        CreatedAt = sqlReader.GetDateTime(3),
                        UpdatedAt = sqlReader.GetDateTime(4)
                    }
                    );
            }
            return result;
        }
    }
}
