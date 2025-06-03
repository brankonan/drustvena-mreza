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
    }
}
