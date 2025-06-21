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
        //public GrupaRepository grupaRepository = new GrupaRepository();

        private GrupaDbRepository grupaRepo { get; }

        public GrupaController(GrupaDbRepository grupaRepo)
        {
            this.grupaRepo = grupaRepo;
        }


        [HttpGet]
        public ActionResult<List<Grupa>> GetAll()
        {
            try
            {
                List<Grupa> grupe = grupaRepo.GetAll();

                if (grupe == null || grupe.Count == 0)
                    return NoContent();

                return Ok(grupe);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška prilikom dobavljanja podataka: {ex.Message}");
            }
        }

        [HttpGet("{id}")]

        public ActionResult<Grupa> GetById(int id)
        {
            try
            {
                Grupa grupa = grupaRepo.GetById(id);

                if (grupa == null)
                    return NotFound($"Grupa sa ID-jem {id} nije pronađena.");

                return Ok(grupa);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška na serveru: {ex.Message}");
            }
        }


        [HttpPost]
        public ActionResult<Grupa> Create([FromBody] Grupa novaGrupa)
        {
            if (string.IsNullOrWhiteSpace(novaGrupa.Ime) || string.IsNullOrWhiteSpace(novaGrupa.DatumOsnivanja.ToShortDateString()))
            {
                return BadRequest();
            }

            Grupa newGroup = grupaRepo.Create(novaGrupa);

            return Ok(newGroup);
        }


        [HttpPut("{id}")]
        public ActionResult<Grupa> Update(int id, [FromBody] Grupa grupaAzuriranje)
        {
            if (grupaAzuriranje == null || string.IsNullOrWhiteSpace(grupaAzuriranje.Ime))
            {
                return BadRequest("Ime grupe je obavezno.");
            }

            try
            {
                grupaAzuriranje.Id = id;

                Grupa updated = grupaRepo.Update(grupaAzuriranje);

                if (updated == null)
                {
                    return NotFound($"Grupa sa ID-jem {id} nije pronađena.");
                }

                return Ok(updated);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Greška pri izmeni grupe: {ex.Message}");
            }
        }


        [HttpDelete("{id}")]

        public ActionResult Delete(int id)
        {
            
            try
            {
                bool deleted = grupaRepo.Delete(id);
                if (!deleted)
                    return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest("Greska pri brisanju grupe: " + ex.Message);
            }

        }


        //private int DodeliNoviId(List<int> identifikatori)
        //{
        //    int maxId = 0;

        //    foreach(int id in identifikatori)
        //    {
        //        if(id > maxId)
        //        {
        //            maxId = id;
        //        }
        //    }

        //    return maxId + 1;
        //}
    }
}
