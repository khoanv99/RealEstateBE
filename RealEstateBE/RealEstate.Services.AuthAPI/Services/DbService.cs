using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using RealEstate.Services.AuthAPI.Services.IServices;
namespace RealEstate.Services.AuthAPI.Services
{
    public class DbService : IDbService
    {
        private readonly IDbConnection _db;

        public DbService(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("DefaultConnectStrings"));
        }

        public async Task<T> GetAsync<T>(string command, object parms)
        {
            T result;

            result = (await _db.QueryAsync<T>(command, parms).ConfigureAwait(false)).FirstOrDefault();

            return result;

        }

        public async Task<List<T>> GetAll<T>(string command, object parms)
        {

            List<T> result = new List<T>();

            result = (await _db.QueryAsync<T>(command, parms)).ToList();

            return result;
        }

        public async Task<int> EditData(string command, object parms)
        {
            int result;
            try
            {
                result = await _db.ExecuteAsync(command, parms);
            }
            catch (Exception ex)
            {
                result = -1;
            }
            return result;

        }

        public async Task<List<T>> GetAll<T>(string command)
        {
            List<T> result = new List<T>();

            result = (await _db.QueryAsync<T>(command)).ToList();

            return result;
        }
    }
}
