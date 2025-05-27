using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Repositories;

namespace WebApplication2.Controllers
{
    [Route("api/korisnici")]
    [ApiController]
    public class KorisnikController : ControllerBase
    {
        private KorisnikRepozitorijum korisnikRepozitorijum = new KorisnikRepozitorijum();

        [HttpGet]
        public ActionResult<List<Korisnik>> GetAll()
        {
            List<Korisnik> korisnici = KorisnikRepozitorijum.Data.Values.ToList();

            return Ok(korisnici);
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
    }
}
