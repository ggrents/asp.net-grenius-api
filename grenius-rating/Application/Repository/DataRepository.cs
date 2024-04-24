using Dapper;
using Microsoft.Data.SqlClient;

namespace grenius_rating.Application.Repository
{
    public class DataRepository : IDataRepository
    {
        string _connectionString;
        public DataRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        public async Task AddRatingCount(int entityId, int tableType)
        {
            string tableName = (tableType == 1) ? "dbo.songs_rating" : "dbo.artists_rating";
            string idColumnName = (tableType == 1) ? "song_Id" : "artist_Id";

            string sql = $@"
            MERGE INTO {tableName} AS Target
            USING (VALUES (@EntityId)) AS Source({idColumnName})
            ON Target.{idColumnName} = Source.{idColumnName}
            WHEN MATCHED THEN
            UPDATE SET count = count + 1
            WHEN NOT MATCHED THEN
            INSERT ({idColumnName}, count) VALUES (Source.{idColumnName}, 1);";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(sql, new { EntityId = entityId });
            }
        }
    }
}
