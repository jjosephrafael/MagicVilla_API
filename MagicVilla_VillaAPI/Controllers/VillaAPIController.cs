using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    // Rename VillaApi to Villa APIController. Then add : ControllerBase where Controller was derived from
    // [ApiController] attribute to define that it's an API Controller

    // Set a route scheme is [Route("api/ControllerName")] or [Route("api/[controller]")] to automatically map it. 
    // [ApiController] helps in adding validations, if you don't want to add [ApiController] but want validations add the !ModelState.IsValid before a 
    // Create and Update
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {

        //// USING THE DEFAULT LOGGER
        //private readonly ILogger<VillaAPIController> _logger;

        //// CTOR to create a constructor for Ilogger and use dependency injection. Ctrl. on logger to create a private readonly field.
        //// this will provide the implementation of ILogger<VillaAPIController> to logger
        //public VillaAPIController(ILogger<VillaAPIController> logger)
        //{
        //    _logger = logger;
        //}

        //// USING A CUSTOM LOGGER

        private readonly ILogging _logger;
        public VillaAPIController(ILogging logger) 
        {
            _logger = logger;
        }

        // Define the Verb on the endpoint
        // If you do not want to include all property of the model use DTO
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            // to log information using default
            //_logger.LogInformation("Getting all villas");

            // to log information using custom logging
            _logger.Log("Getting all villas","");
            return Ok(VillaStore.villaList);
        }

        // If it's expecting a paramenter explicitly declare it.
        // You can also define its datatype by using [HttpGet("{id:int}")]
        // Define the response type that will be produced [ProducesResponseType]
        // Define the type by using ActionResult<VillaDTO> or [ProducesResponseType(200, Type = typeof(VillaDTO))]
        // Define the route name
        //[HttpGet("{id:int}"]
        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                // to log error
                // to log information using default
                //_logger.LogError("Get Villa Error with Id " + id);

                // to log information using custom logging
                _logger.Log("Get Villa Error with Id " + id, "error");

                // Bad Request
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                // Return Not Found
                return NotFound();
            }

            // Valid Response
            return Ok(villa);
        }


        // Add the StatusCode 201

        [HttpPost]
        //[ProducesResponseType(StatusCodes.Status200OK)]        
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // When working with HTTP post typically the object that you receive is FromBody so add the attribute before the object
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO)
        {
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if (VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                // Key should be unique, it can also be empty
                ModelState.AddModelError("CustomError", "Villa already Exists!");
                return BadRequest(ModelState);
            }

            if (villaDTO == null)
            {
                return BadRequest(villaDTO);
            }

            if (villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            villaDTO.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villaDTO);

            // Created a route will define the route it will be created and assign value to the parameter being passed
            //return Ok(villaDTO);
            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
        }

        // using IActionResult you do not need to define the return type, in ActionResult you need to.
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            VillaStore.villaList.Remove(villa);
            return NoContent();
        }



        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);

            villa.Name = villaDTO.Name;
            villa.Sqft = villaDTO.Sqft;
            villa.Occupancy = villaDTO.Occupancy;

            return NoContent();
        }


        // Add the following Nuget package for the HTTP Patch
        // Microsoft.AspNetCore.JsonPatch
        // Microsoft.AspNetCore.Mvc.NewtonsoftJson
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villa, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();
        }
    }
}
