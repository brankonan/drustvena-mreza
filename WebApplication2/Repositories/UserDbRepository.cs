using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using WebApplication2.Models;


namespace WebApplication2.Repositories
{
    public class UserDbRepository
    {
        private readonly string connectionString = "Data Source=database/mydatabase.db";


        public List<Korisnik> GetAll()
        {
            var korisnici = new List<Korisnik>();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Username, Name, Surname, Birthday FROM Users";
                using (var command = new SqliteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        korisnici.Add(new Korisnik(
                            Convert.ToInt32(reader["id"]),
                            reader["Username"].ToString(),
                            reader["Name"].ToString(),
                            reader["Surname"].ToString(),
                            DateTime.Parse(reader["Birthday"].ToString())
                            ));
                    }
                }
            }
            return korisnici;
        }
        public Korisnik GetById(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Username, Name, Surname, BIrthday FROM Users WHERE Id=@id";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Korisnik(
                                Convert.ToInt32(reader["Id"]),
                                reader["Username"].ToString(),
                                reader["Name"].ToString(),
                                reader["Surname"].ToString(),
                                DateTime.Parse(reader["Birthday"].ToString())
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
                string query = "INSERT INTO Users (Username,Name, Surname, Birthday) VALUES (@username, @name, @surname, @birthday); SELECT last_insert_rowid();";
                using (var command = new SqliteCommand(query,connection))
                {
                    command.Parameters.AddWithValue("@username", korisnik.KorisnickoIme);
                    command.Parameters.AddWithValue("@name", korisnik.Ime);
                    command.Parameters.AddWithValue("@surname", korisnik.Prezime);
                    command.Parameters.AddWithValue("@birthday", korisnik.Datum.ToString("yyyy-MM-dd"));
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
                string query = "UPDATE Users SET Username = @username, Name = @name, Surname = @surname, Birthday = @birthday WHERE Id = @id";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", korisnik.KorisnickoIme);
                    command.Parameters.AddWithValue("@name", korisnik.Ime);
                    command.Parameters.AddWithValue("@surname", korisnik.Prezime);
                    command.Parameters.AddWithValue("@birthday", korisnik.Datum.ToString("yyyy-MM-dd"));
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
                string query = "DELETE FROM Users WHERE id = @id";
                using (var command = new SqliteCommand(query,connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
