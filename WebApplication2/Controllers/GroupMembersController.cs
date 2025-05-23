using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Repositories;

namespace WebApplication2.Controllers
{
    [Route("api/grupe/{idGrupe}/korisnici")]
    [ApiController]
    public class GroupMembersController : ControllerBase
    {
        public GrupaRepository grupaRepository = new GrupaRepository();
        public KorisnikRepozitorijum korisnikRepository = new KorisnikRepozitorijum();

        //GET /gropus/{groupId}/users

        [HttpGet]
        public ActionResult<List<Korisnik>> GetUserFromGroup(int idGrupe)
        {
            if (!GrupaRepository.Data.ContainsKey(idGrupe))
            {
                return NotFound();
            }

            Grupa grupa = GrupaRepository.Data[idGrupe];
            return Ok(grupa.Korisnici);
        }


        //PUT /groups/{groupId}/ users /{ userId}

        [HttpPut("{idKorisnika}")]
        public ActionResult AddUser([FromRoute]int idGrupe, [FromRoute] int idKorisnika)
        {
            if (!GrupaRepository.Data.ContainsKey(idGrupe) || !KorisnikRepozitorijum.Data.ContainsKey(idKorisnika))
            {
                return NotFound();
            }

            Grupa grupa = GrupaRepository.Data[idGrupe];
            Korisnik korisnik = KorisnikRepozitorijum.Data[idKorisnika];

            if (!grupa.Korisnici.Any(k => k.Id == idKorisnika))
            {
                grupa.Korisnici.Add(korisnik);
                grupaRepository.Sacuvaj();
            }
            return Ok();
        }

        //DELETE /groups/{groupId}/ users /{ userId}
        [HttpDelete("{idKorisnika}")]
        public ActionResult RemoveUser([FromRoute] int idGrupe, [FromRoute] int idKorisnika)
        {
            if (!GrupaRepository.Data.ContainsKey(idGrupe))
            {
                return NotFound();
            }

            Grupa grupa = GrupaRepository.Data[idGrupe];
            Korisnik korisnik = grupa.Korisnici.FirstOrDefault(k => k.Id == idKorisnika);

            if(korisnik != null)
            {
                grupa.Korisnici.Remove(korisnik);
                grupaRepository.Sacuvaj();
            }
            return NoContent();
        }
    }
}