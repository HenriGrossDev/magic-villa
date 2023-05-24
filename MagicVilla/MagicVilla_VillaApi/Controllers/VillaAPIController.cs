using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaApi.Controllers
{
    [Route("api/villaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.villaList);
        }

        [HttpGet("id:int", Name="GetVilla")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {

            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);

            if (id == 0)
            {
                return BadRequest();
            }
            

            if  (villa == null)
            {
                return NotFound();
            }

            return Ok(villa);
        }


        [HttpPost]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
        {

            if(VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower()==villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa Already Exists");
                return BadRequest(ModelState);
            }

            if(villaDTO == null)
            {
                return BadRequest(villaDTO);
            }
            if(villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            villaDTO.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villaDTO);

            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id },villaDTO);
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        [HttpDelete("{id:int}", Name ="DeleteVilla")]
        public IActionResult DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u =>u.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            
            VillaStore.villaList.Remove(villa);
            return NoContent();

        }
    }
}
