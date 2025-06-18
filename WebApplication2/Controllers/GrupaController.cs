using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using WebApplication2.Models;
using WebApplication2.Repositories;

namespace WebApplication2.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GrupaController : ControllerBase
    {
        public GrupaRepository grupaRepository = new GrupaRepository();

        private string connectionString = "Data Source=database/mydatabase.db";


        private List<Grupa> GetAllFromDataBase()
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

        [HttpGet]
        public ActionResult<List<Grupa>> GetAll()
        {
            try
            {
                List<Grupa> grupe = GetAllFromDataBase();
                return Ok(grupe);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška prilikom dobavljanja podataka: {ex.Message}");
            }
        }



        [HttpPost]
        public ActionResult<Grupa> Create([FromBody] Grupa novaGrupa)
        {
            if(string.IsNullOrWhiteSpace(novaGrupa.Ime) || string.IsNullOrWhiteSpace(novaGrupa.DatumOsnivanja.ToShortDateString()))
            {
                return BadRequest();
            }

            novaGrupa.Id = DodeliNoviId(GrupaRepository.Data.Keys.ToList());
            GrupaRepository.Data[novaGrupa.Id] = novaGrupa;
            grupaRepository.Sacuvaj();

            return Ok(novaGrupa);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (!GrupaRepository.Data.ContainsKey(id))
            {
                return NotFound();
            }

            GrupaRepository.Data.Remove(id);
            grupaRepository.Sacuvaj();

            return NoContent();
        }


        private int DodeliNoviId(List<int> identifikatori)
        {
            int maxId = 0;

            foreach(int id in identifikatori)
            {
                if(id > maxId)
                {
                    maxId = id;
                }
            }

            return maxId + 1;
        }
    }
}
