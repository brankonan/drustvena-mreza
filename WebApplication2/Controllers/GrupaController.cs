using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Repositories;

namespace WebApplication2.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GrupaController : ControllerBase
    {
        public GrupaRepository grupaRepository = new GrupaRepository();



        [HttpGet]
        public ActionResult<List<Grupa>> GetAll()
        {
            List<Grupa> grupe = GrupaRepository.Data.Values.ToList();
            return Ok(grupe);
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
