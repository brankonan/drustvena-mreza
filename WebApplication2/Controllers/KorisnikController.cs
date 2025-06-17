using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Repositories;
using Microsoft.Data.Sqlite;

namespace WebApplication2.Controllers
{
    [Route("api/korisnici")]
    [ApiController]
    public class KorisnikController : ControllerBase
    {
        //private readonly string connectionString = "Data Source=database/mydatabase.db";
        private readonly UserDbRepository userRepo;

        public KorisnikController(IConfiguration configuration)
        {
            userRepo = new UserDbRepository(configuration);
        }

        //Izmenjena metoda GET svih korisnika

        //GET: api/users/paged
        [HttpGet("paged")]
        public ActionResult GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            //List<Korisnik> korisnici = KorisnikRepozitorijum.Data.Values.ToList();

            //return Ok(korisnici);
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page and PageSize must be greater than zero");
            }
            try
            {
                //var korisnici = GetAllFromDatabase();
                var users = userRepo.GetPaged(page, pageSize);
                var totalCount = userRepo.CountAll();

                var result = new
                {
                    Data = users,
                    TotalCount = totalCount
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem("Greska pri dohvatanju korisnika" + ex.Message);
            }
        }


        [HttpGet("{id}")]
        public ActionResult<Korisnik> GetById(int id)
        {
            try
            {
                var korisnik = userRepo.GetById(id);
                if (korisnik == null)
                {
                    return NotFound();
                }
                return Ok(korisnik);
            }
            catch (Exception ex)
            {
                return BadRequest("Greska pri dohvatanju korisnika: " + ex.Message);
            }
        }

        [HttpPost]

            public ActionResult<Korisnik> Create([FromBody] Korisnik noviKorisnik)
        {
            if (string.IsNullOrWhiteSpace(noviKorisnik.KorisnickoIme) ||
                string.IsNullOrWhiteSpace(noviKorisnik.Ime) ||
                string.IsNullOrWhiteSpace(noviKorisnik.Prezime) ||
                string.IsNullOrWhiteSpace(noviKorisnik.Datum.ToShortDateString()))
            {
                return BadRequest();
            }
            //noviKorisnik.Id = SracunajNoviId(KorisnikRepozitorijum.Data.Keys.ToList());
            //KorisnikRepozitorijum.Data[noviKorisnik.Id] = noviKorisnik;
            //korisnikRepozitorijum.Sacuvaj();

            //return Ok(noviKorisnik);

            try
            {
                var kreiraniKorisnik = userRepo.Create(noviKorisnik);
                return CreatedAtAction("GetById", new { id = noviKorisnik.Id }, kreiraniKorisnik);
            }
            catch (Exception ex)
            {
                return BadRequest("Greska pri unosu korisnika: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Korisnik> Update(int id, [FromBody] Korisnik korisnik) {

            if (string.IsNullOrWhiteSpace(korisnik.Ime) || string.IsNullOrWhiteSpace(korisnik.KorisnickoIme) || string.IsNullOrWhiteSpace(korisnik.Prezime) || string.IsNullOrWhiteSpace(korisnik.Datum.ToShortDateString()))
                { return BadRequest("Neka od polja nisu popunjena"); }
            //if (!KorisnikRepozitorijum.Data.ContainsKey(id)) 
            //    { return NotFound(); }
            //Korisnik noviKorisnik = KorisnikRepozitorijum.Data[id];
            //noviKorisnik.KorisnickoIme = korisnik.KorisnickoIme;
            //noviKorisnik.Ime = korisnik.Ime;
            //noviKorisnik.Prezime = korisnik.Prezime;
            //noviKorisnik.Datum = korisnik.Datum;
            try
            {
                korisnik.Id = id;
                bool updated = userRepo.Update(korisnik);
                if(!updated)
                 return NotFound();
                return Ok(korisnik);
            }
            catch (Exception ex)
            {
                return BadRequest("Greska pri izmeni korisnika: " + ex.Message);
            }   
        }


        [HttpDelete("{id}")]

        public ActionResult Delete(int id)
        {
            //if (!KorisnikRepozitorijum.Data.ContainsKey(id))
            //{ return NotFound(); }

            //KorisnikRepozitorijum.Data.Remove(id);
            //korisnikRepozitorijum.Sacuvaj();
            //return NoContent();

            try
            {
                bool deleted = userRepo.Delete(id);
                if (!deleted)
                    return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest("Greska pri brisanju korisnika: " + ex.Message);
            }

        }

        //private int SracunajNoviId(List<int> idList)
        //{
        //    if (idList.Count == 0)
        //    {
        //        return 1;
        //    }
        //    else
        //    {
        //        return idList.Max() + 1;
        //    }
        //}

        //private List<Korisnik> GetAllFromDatabase()
        //{
        //    List<Korisnik> korisnici = new List<Korisnik>();

        //    using (var connection = new SqliteConnection(connectionString))
        //    {
        //        connection.Open();
        //        string query = "SELECT Id, Username, Name, Surname, Birthday FROM Users";
        //        using var command = new SqliteCommand(query, connection);
        //        using var reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            korisnici.Add(new Korisnik(
        //                Convert.ToInt32(reader["Id"]),
        //                reader["Username"].ToString(),
        //                reader["Name"].ToString(),
        //                reader["Surname"].ToString(),
        //                DateTime.Parse(reader["Birthday"].ToString())
        //            ));
        //        }
        //    }
        //    return korisnici;
        //}
    }
}
