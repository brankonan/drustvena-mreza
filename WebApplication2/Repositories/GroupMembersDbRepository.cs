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
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"INSERT INTO GroupMemberships (UserId, GroupId) VALUES (@UserId, @GroupId); ";
                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@GroupId", groupId);

                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }

        }


        public void RemoveUserFromGroup(int groupId, int userId)
        {
            try
            {
                using var connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "DELETE FROM GroupMemberships WHERE UserId = @userId AND GroupId = @groupId;";

                using var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@groupId", groupId);

                command.ExecuteNonQuery();
            } 
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }
        }

    }
}
