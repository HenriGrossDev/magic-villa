using AutoMapper;
using MagicVilla_VillaApi.Models;
using MagicVilla_VillaApi.Models.Dto;
using MagicVilla_VillaApi.Repository.IRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaApi.Controllers;

[Route("api/villaNumber")]
[ApiController]
public class VillaNumberController : ControllerBase
{
    protected APIResponse _response;
    private readonly IVillaNumberRepository _dbVillaNumber;
    private readonly IVillaRepository _dbVillas;
    private readonly IMapper _mapper;

    public VillaNumberController(IVillaNumberRepository dbVillaNumber, IMapper mapper, IVillaRepository dvVilla)
    {
        this._mapper = mapper;
        this._dbVillaNumber = dbVillaNumber;
        this._response = new();
        this._dbVillas = dvVilla;
    }

    [HttpGet]
    public async Task<ActionResult<APIResponse>> GetVillaNumbers()
    {
        try
        {
            IEnumerable<VillaNumber> villaNumberList = await _dbVillaNumber.GetAllAsync(includeProperties:"Villa");
            _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumberList);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages =
                new List<string>() { ex.ToString() };
        }

        return _response;
    }

    [HttpGet("id:int", Name = "GetVillaNumber")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id);

            if (villaNumber == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages =
                new List<string>() { ex.ToString() };
        }
        return _response;
    }

    [HttpPost]
    public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO createDTO)
    {
        try
        {
            
            
            if (await _dbVillaNumber.GetAsync(u => u.VillaNo == createDTO.VillaNo) != null)
            {
                ModelState.AddModelError("CustomError", "VillaNumber Already Exists");
                return BadRequest(ModelState);
            }

            if (await _dbVillas.GetAsync(u => u.Id == createDTO.VillaId) == null)
            {
                ModelState.AddModelError("CustomError", "Villa ID is Invalid!");
                return BadRequest(ModelState);
            }

                if (createDTO == null)
            {
                return BadRequest(createDTO);
            }

            VillaNumber villaNumber = _mapper.Map<VillaNumber>(createDTO);

            await _dbVillaNumber.CreateAsync(villaNumber);
            _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
            _response.StatusCode = HttpStatusCode.Created;
           


            return CreatedAtRoute("GetVilla", new { id = villaNumber.VillaNo }, _response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages =
                new List<string>() { ex.ToString() };
        }
        return _response;
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
    public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
    {
        try
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id);
            if (villaNumber == null)
            {
                return NotFound();
            }

            await _dbVillaNumber.RemoveAsync(villaNumber);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages =
                new List<string>() { ex.ToString() };
        }
        return _response;

    }

    [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
    public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO updateDTO)
    {
        try
        {
            if (updateDTO == null || id != updateDTO.VillaNo)
            {
                return BadRequest();
            }

            if (await _dbVillas.GetAsync(u => u.Id == updateDTO.VillaId) == null)
            {
                ModelState.AddModelError("CustomError", "Villa ID is Invalid!");
                return BadRequest(ModelState);
            }

            var villaNumber = _mapper.Map<VillaNumber>(updateDTO);

            await _dbVillaNumber.UpdateAsync(villaNumber);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages =
                new List<string>() { ex.ToString() };
        }
        return _response;

    }

}
