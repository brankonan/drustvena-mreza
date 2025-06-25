using System.Numerics;
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

        private GrupaDbRepository grupaRepo;

        public GrupaController(IConfiguration configuration)
        {
            grupaRepo = new GrupaDbRepository(configuration);
        }


        [HttpGet]
        public ActionResult GetPaged([FromQuery] int page = 1 , [FromQuery] int pageSize= 10)
        {
            if (page < 1 || pageSize < 1) 
            {
                return BadRequest("Page and PageSize must be greater than zero.");
            }
            try
            {
                List<Grupa> grupe = grupaRepo.GetPaged(page, pageSize);
                int totalCount = grupaRepo.CountAll();

                Object result = new
                {
                    Data = grupe,
                    TotalCount = totalCount
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while fetching the books.");
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
