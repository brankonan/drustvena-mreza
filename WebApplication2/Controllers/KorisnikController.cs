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
        private KorisnikRepozitorijum korisnikRepozitorijum = new KorisnikRepozitorijum();
        //private readonly string connectionString = "Data Source=database/mydatabase.db";
        private readonly UserDbRepository userRepo;

        public KorisnikController()
        {
            userRepo = new UserDbRepository();
        }

        //Izmenjena metoda GET svih korisnika
        [HttpGet]
        public ActionResult<List<Korisnik>> GetAll()
        {
            //List<Korisnik> korisnici = KorisnikRepozitorijum.Data.Values.ToList();

            //return Ok(korisnici);


            try
            {
                //var korisnici = GetAllFromDatabase();
                return Ok(userRepo.GetAll());
            }
            catch (Exception ex)
            {
                return BadRequest("Greska pri dohvatanju korisnika" + ex.Message);
            }
        }


        [HttpGet("{id}")]
        public ActionResult<Korisnik> GetById(int id)
        {
            if (!KorisnikRepozitorijum.Data.ContainsKey(id))
            {
                return NotFound();
            }
            return Ok(KorisnikRepozitorijum.Data[id]);
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


            noviKorisnik.Id = SracunajNoviId(KorisnikRepozitorijum.Data.Keys.ToList());
            KorisnikRepozitorijum.Data[noviKorisnik.Id] = noviKorisnik;
            korisnikRepozitorijum.Sacuvaj();

            return Ok(noviKorisnik);
        }

        [HttpPut("{id}")]
        public ActionResult<Korisnik> Update(int id, [FromBody] Korisnik korisnik) {

            if (string.IsNullOrWhiteSpace(korisnik.Ime) || string.IsNullOrWhiteSpace(korisnik.KorisnickoIme) || string.IsNullOrWhiteSpace(korisnik.Prezime) || string.IsNullOrWhiteSpace(korisnik.Datum.ToShortDateString()))
                { return BadRequest(); }
            if (!KorisnikRepozitorijum.Data.ContainsKey(id)) 
                { return NotFound(); }
            Korisnik noviKorisnik = KorisnikRepozitorijum.Data[id];
            noviKorisnik.KorisnickoIme = korisnik.KorisnickoIme;
            noviKorisnik.Ime = korisnik.Ime;
            noviKorisnik.Prezime = korisnik.Prezime;
            noviKorisnik.Datum = korisnik.Datum;

            return Ok(korisnik);
        }


        [HttpDelete("{id}")]

        public ActionResult Delete(int id)
        {
            if (!KorisnikRepozitorijum.Data.ContainsKey(id))
            { return NotFound(); }

            KorisnikRepozitorijum.Data.Remove(id);
            korisnikRepozitorijum.Sacuvaj();
            return NoContent();
        }

        private int SracunajNoviId(List<int> idList)
        {
            if (idList.Count == 0)
            {
                return 1;
            }
            else
            {
                return idList.Max() + 1;
            }
        }

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
