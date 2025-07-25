﻿using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using WebApplication2.Models;

namespace WebApplication2.Repositories
{
    public class GrupaDbRepository
    {
        private readonly string connectionString;

        public GrupaDbRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        public int CountAll()
        {
            int totalCount = 0;

            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT COUNT(*) FROM Groups";
                using SqliteCommand command = new SqliteCommand(query, connection);
               
                totalCount = Convert.ToInt32(command.ExecuteScalar());

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

            return totalCount;

        }

        public bool Delete(int id)
        {
            int affectedRows = 0;

            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "DELETE FROM Groups WHERE Id=@Id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                affectedRows = command.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    Console.WriteLine($"Uspesno ste obrisali grupu sa ID-jem: {id}");
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

            return affectedRows >0;
        }
        public Grupa Update(Grupa editedGroup)
        {
            try{
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "UPDATE Groups SET Name=@Name, CreationDate=@Date WHERE Id=@Id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", editedGroup.Id);
                command.Parameters.AddWithValue("@Name", editedGroup.Ime);
                command.Parameters.AddWithValue("@Date", editedGroup.DatumOsnivanja);

                int affectedRows = command.ExecuteNonQuery();

                if (affectedRows == 0) { return null; }
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


            return editedGroup;

        }
        public Grupa Create(Grupa newGroup)
        {
            try{
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"INSERT INTO Groups (Name, CreationDate) VALUES (@Name, @Date); SELECT last_insert_rowid();";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Name", newGroup.Ime);
                command.Parameters.AddWithValue("@Date", newGroup.DatumOsnivanja);

                int newId = Convert.ToInt32(command.ExecuteScalar());
                newGroup.Id = newId;
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

            return newGroup;

        }
        public List<Grupa> GetById(int id)
        {
            List<Grupa> groups = new List<Grupa>();

            try
                {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @" SELECT g.Id AS GroupId, g.Name AS GroupName, g.CreationDate,
                                u.Id AS UserId, u.Username, u.Name AS UserName, u.Surname, u.Birthday
                                FROM Groups g
                                LEFT JOIN GroupMemberships gm ON g.Id = gm.GroupId   
                                LEFT JOIN Users u ON gm.UserId = u.Id
                                WHERE g.Id=@Id
                                ORDER BY g.Id;";

                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                using SqliteDataReader reader = command.ExecuteReader();
                Grupa currentGroup = null;

                while (reader.Read())
                {
                    int groupId = Convert.ToInt32(reader["GroupId"]);

                    if (currentGroup == null || currentGroup.Id != groupId)
                    {

                        string name = reader["GroupName"].ToString();
                        DateTime date = Convert.ToDateTime(reader["CreationDate"]);

                        Grupa g = new Grupa(groupId, name, date);
                        currentGroup = g;

                        groups.Add(currentGroup);
                    }

                    if (reader["UserId"] != DBNull.Value)
                    {
                        int userID = Convert.ToInt32(reader["UserId"]);
                        string username = Convert.ToString(reader["Username"]);
                        string name = Convert.ToString(reader["UserName"]);
                        string surname = Convert.ToString(reader["Surname"]);
                        DateTime birthday = Convert.ToDateTime(reader["Birthday"]);

                        Korisnik user = new Korisnik(userID, username, name, surname, birthday);

                        currentGroup.Korisnici.Add(user);
                    }
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
            return groups;

        }

        public List<Grupa> GetPaged(int page, int pageSize)
        {

            List<Grupa> grupe = new List<Grupa>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT Id, Name, CreationDate FROM Groups LIMIT @PageSize OFFSET @Offset";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                command.Parameters.AddWithValue("@Offset", pageSize * (page - 1));

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
