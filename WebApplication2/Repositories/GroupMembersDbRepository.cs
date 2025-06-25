using Microsoft.Data.Sqlite;

namespace WebApplication2.Repositories
{
    public class GroupMembersDbRepository
    {
        private readonly string connectionString;

        public GroupMembersDbRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        //Potrebno je da omogućite da se dodavanje korisnika u grupu zabeleži u bazi podataka.

        public void Create(int groupId, int userId)
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();

            string query = @"INSERT INTO GroupMemberships (UserId, GroupId) VALUES (@UserId, @GroupId); ";
            using SqliteCommand command= new SqliteCommand(query, connection);

            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@GroupId", groupId);

            command.ExecuteNonQuery();

        }
    }
}
