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

        private GroupMembersDbRepository groupMembersRepo;

        public GroupMembersController(IConfiguration configuration)
        {
            groupMembersRepo = new GroupMembersDbRepository(configuration);
        }
        public KorisnikRepozitorijum korisnikRepository = new KorisnikRepozitorijum();
        public GrupaRepository grupaRepository = new GrupaRepository();

        //GET /gropus/{groupId}/users

        [HttpGet]
        public ActionResult<List<Korisnik>> GetUserFromGroup([FromRoute] int idGrupe)
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
            if (idGrupe == 0 || idKorisnika == 0) {

                return BadRequest();
            }
            try
            {

                groupMembersRepo.Create(idGrupe, idKorisnika);

                return Ok();
            }
            catch (Exception ex) { return Problem("An error occurred while fetching the books."); }

        }

        //DELETE /groups/{groupId}/ users /{ userId}
        [HttpDelete("{idKorisnika}")]
        public ActionResult RemoveUser([FromRoute] int idGrupe, [FromRoute] int idKorisnika)
        {
            if (idGrupe == 0 || idKorisnika == 0)
            {

                return BadRequest();
            }
            try
            {

                groupMembersRepo.RemoveUserFromGroup(idGrupe, idKorisnika);

                return Ok();
            }
            catch (Exception ex) { return Problem("An error occurred while fetching the books."); }

        }
    }
}