using Microsoft.Data.Sqlite;
using WebApplication2.Models;

namespace WebApplication2.Repositories
{
    public class GrupaDbRepository
    {
        private readonly string connectionString="Data Source= database/mydatabase.db";

        public Grupa GetById(int id)
        {
            try
                {using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT * FROM Groups WHERE Id=@Id";
                SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int identifikator = Convert.ToInt32(reader["Id"]);
                    string ime = reader["Name"].ToString();
                    DateTime datum = Convert.ToDateTime(reader["CreationDate"]);

                    return new Grupa(identifikator, ime, datum);

                }
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
            return null;

        }

        public List<Grupa> GetAll()
        {

            List<Grupa> grupe = new List<Grupa>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT Id, Name, CreationDate FROM Groups";
                using SqliteCommand command = new SqliteCommand(query, connection);

                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string ime = reader["Name"].ToString();
                    DateTime datum = Convert.ToDateTime(reader["CreationDate"]);

                    Grupa g = new Grupa(id, ime, datum);
                    grupe.Add(g);
                }

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

            return grupe;
        }
    }
}
