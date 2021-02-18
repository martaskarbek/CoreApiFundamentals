using System;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Internal;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    [ApiController]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;
        
        public CampsController(ICampRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        //false is important cause query string can or can not exist
        //so if it is not present, then we will get camps without talks, but when path
        //looks like /api/camps?includeTalks=true then we will get 
        //camps with talks []
        public async Task<ActionResult<CampModel[]>> Get(bool includeTalks = false)
        {
            //if (false) return this.NotFound("Bad stuff happens");
            try
            {
                var results = await _repository.GetAllCampsAsync(includeTalks);
                CampModel[] models = _mapper.Map<CampModel[]>(results);
                return Ok(models);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Databae failure");
            }
        }

        //[HttpGet("{moniker:int}")]
        //if I want to be shure that it is converting to int if moniker woulb be int in db
        //but i suppose I can use also annotation for that format filter or something
        [HttpGet("{moniker}")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<CampModel>> Get(string moniker)
        {
            try
            {
                var result = await _repository.GetCampAsync(moniker);
                if (result == null) return NotFound();
                return _mapper.Map<CampModel>(result);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Databae failure");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<CampModel[]>> SearchByDate(DateTime theDate, bool includeTalks = false)
        {
            try
            {
                var results = await _repository.GetAllCampsByEventDate(theDate, includeTalks);
                if (!results.Any()) return NotFound();
                return _mapper.Map<CampModel[]>(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Databae failure");
            } 
        }
        
        //public async Task<ActionResult<CampModel>> Post([FromBody]CampModel model)
        //[FromBody] menas from json sended from front, but we do not need to use that when we have 
        //[ApiController]
        public async Task<ActionResult<CampModel>> Post(CampModel model)
        {
            try
            {
                var existing = await _repository.GetCampAsync(model.Moniker);
                if (existing != null)
                {
                    return BadRequest("Moniker in use");
                }
                //in the path is method name, controller name
                //it is redirecting, gives status code 201
                //and put header location
                var location = _linkGenerator.GetPathByAction("Get", "Camps",
                    new {moniker = model.Moniker});
                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Could not use current moniker");
                }
                var camp = _mapper.Map<Camp>(model);
                _repository.Add(camp);
                if (await _repository.SaveChangesAsync())
                {
                    return Created($"/api/camps/{camp.Moniker}", _mapper.Map<CampModel>(camp));
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Databae failure");
            }

            return BadRequest();
        }

        [HttpPut("{moniker")]
        public async Task<ActionResult<CampModel>> Put(string moniker, CampModel model)
        {
            try
            {
                var oldCamp = await _repository.GetCampAsync(model.Moniker);
                if (oldCamp == null) return NotFound($"Could not find camp with moniker of {model.Moniker}");
                _mapper.Map(model, oldCamp);
                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<CampModel>(oldCamp);
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Databae failure");
            }

            return BadRequest();
        }
        
        [HttpDelete("{moniker")]
        //Action result uzywamy jak chcemy cos zwrocic, a Iaction result jesli chcemy zwrocic tylko status code
        public async Task<IActionResult> Delete(string moniker)
        {
            try
            {
                var oldCamp = await _repository.GetCampAsync(moniker);
                if (oldCamp == null) return NotFound();
                _repository.Delete(oldCamp);
                if (await _repository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Databae failure");
            }

            return BadRequest("Failed to delete the camp");
        }
    }
}