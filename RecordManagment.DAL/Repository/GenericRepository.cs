using Dapper;
using RecordManagment.DAL.Repository.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RecordManagment.DAL.Repository
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected SqlConnection sqlConnection;
        protected IDbTransaction dbTransaction;
        private readonly string tableName;

        protected GenericRepository(SqlConnection sqlConnection, IDbTransaction dbTransaction, string tableName)
        {
            this.sqlConnection = sqlConnection;
            this.dbTransaction = dbTransaction;
            this.tableName = tableName;
        }

        public async Task<long> AddAsync(T t)
        {
            var insertQuery = GenerateInsertQuery();
            Console.WriteLine(insertQuery);
            var newId = await sqlConnection.ExecuteScalarAsync<long>(
                insertQuery,
                param: t,
                transaction: dbTransaction
                );
            return newId;
        }

        public async Task<int> AddRangeAsync(IEnumerable<T> list)
        {
            var inserted = 0;
            var query = GenerateInsertQuery();
            inserted += await sqlConnection.ExecuteAsync(query,
                param: list);
            return inserted;
        }

        public async Task DeleteAsync(long id)
        {
            await sqlConnection.ExecuteAsync(
                $"DELETE FROM {tableName} WHERE Id=@Id",
                param: new { Id = id },
                transaction: dbTransaction
                );
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await sqlConnection.QueryAsync<T>(
                $"SELECT * FROM {tableName}",
                transaction: dbTransaction
                );
        }

        public async Task<T> GetAsync(long id)
        {
            var result = await sqlConnection.QuerySingleOrDefaultAsync<T>(
                $"SELECT * FROM {tableName} WHERE Id=@Id",
                param: new { Id = id },
                transaction: dbTransaction
                );
            return result;
        }

        public async Task ReplaceAsync(T t)
        {
            var updateQuery = GenerateUpdateQuery();
            await sqlConnection.ExecuteAsync(
                updateQuery,
                param: t,
                transaction: dbTransaction
                );
        }


        #region work with properties
        private IEnumerable<PropertyInfo> GetProperties => typeof(T).GetProperties();

        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                    select prop.Name).ToList();
        }

        private string GenerateUpdateQuery()
        {
            var updateQuery = new StringBuilder($"UPDATE {tableName} SET ");
            var properties = GenerateListOfProperties(GetProperties);
            properties.ForEach(property =>
            {
                if (!property.Equals("Id"))
                {
                    updateQuery.Append($"{property}=@{property},");
                }
            });
            updateQuery.Remove(updateQuery.Length - 1, 1);
            updateQuery.Append(" WHERE Id=@Id");
            return updateQuery.ToString();
        }

        private string GenerateInsertQuery()
        {
            var insertQuery = new StringBuilder($"INSERT INTO {tableName} ");
            insertQuery.Append("(");
            var properties = GenerateListOfProperties(GetProperties);
            properties.Remove("Id");
            properties.ForEach(prop => { insertQuery.Append($"[{prop}],"); });
            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(") VALUES (");

            properties.ForEach(prop => { insertQuery.Append($"@{prop},"); });
            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(")");
            insertQuery.Append("; SELECT SCOPE_IDENTITY()");
            return insertQuery.ToString();
        }
        #endregion
    }
}
