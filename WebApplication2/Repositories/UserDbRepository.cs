using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using WebApplication2.Models;

namespace WebApplication2.Repositories
{
    public class UserDbRepository
    {
        private readonly string connectionString;

        public UserDbRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("SQLiteConnection");
        }


        public List<Korisnik> GetPaged(int page, int pageSize)
        {
            var korisnici = new List<Korisnik>();
            try
            {
                using var connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT Id, KorisnickoIme, Ime, Prezime, Datum FROM Korisnici LIMIT @PageSize OFFSET @Offset";
                using var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                command.Parameters.AddWithValue("@Offset", pageSize * (page - 1));
                using var reader = command.ExecuteReader();
                {
                    while (reader.Read())
                    {
                        korisnici.Add(new Korisnik(
                            Convert.ToInt32(reader["Id"]),
                            reader["KorisnickoIme"].ToString(),
                            reader["Ime"].ToString(),
                            reader["Prezime"].ToString(),
                            DateTime.Parse(reader["Datum"].ToString())
                            ));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greska u GetPaged {ex.Message}");
                throw;
            }

            return korisnici;
        }

        public int CountAll()
        {
            try
            {
                using var connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT COUNT(*) FROM Korisnici";
                using var command = new SqliteCommand(query, connection);
                int totalCount = Convert.ToInt32(command.ExecuteScalar());

                return totalCount;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greska i CountAll: {ex.Message}");
                throw;
            }
        }

        public Korisnik GetById(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id, KorisnickoIme, Ime, Prezime, Datum FROM Korisnici WHERE Id=@id";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Korisnik(
                                Convert.ToInt32(reader["Id"]),
                                reader["KorisnickoIme"].ToString(),
                                reader["Ime"].ToString(),
                                reader["Prezime"].ToString(),
                                DateTime.Parse(reader["Datum"].ToString())
                                );
                        }
                    }
                }
            }
            return null;
        }

        public Korisnik Create(Korisnik korisnik)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Korisnici (KorisnickoIme, Ime, Prezime, Datum) VALUES (@korisnickoIme, @ime, @prezime, @datum); SELECT last_insert_rowid();";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@korisnickoIme", korisnik.KorisnickoIme);
                    command.Parameters.AddWithValue("@ime", korisnik.Ime);
                    command.Parameters.AddWithValue("@prezime", korisnik.Prezime);
                    command.Parameters.AddWithValue("@datum", korisnik.Datum.ToString("yyyy-MM-dd"));
                    long id = (long)command.ExecuteScalar();
                    korisnik.Id = (int)id;
                    return korisnik;
                }
            }
        }

        public bool Update(Korisnik korisnik)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Korisnici SET KorisnickoIme = @korisnickoIme, Ime = @ime, Prezime = @prezime, Datum = @datum WHERE Id = @id";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@korisnickoIme", korisnik.KorisnickoIme);
                    command.Parameters.AddWithValue("@ime", korisnik.Ime);
                    command.Parameters.AddWithValue("@prezime", korisnik.Prezime);
                    command.Parameters.AddWithValue("@datum", korisnik.Datum.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@id", korisnik.Id);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public bool Delete(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Korisnici WHERE id = @id";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
