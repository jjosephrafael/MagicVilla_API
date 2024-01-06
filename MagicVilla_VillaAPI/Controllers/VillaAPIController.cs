﻿using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
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
        // Define the Verb on the endpoint
        // If you do not want to include all property of the model use DTO
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.villaList);
        }

        // If it's expecting a paramenter explicitly declare it.
        // You can also define its datatype by using [HttpGet("{id:int}")]
        // Define the response type that will be produced [ProducesResponseType]
        // Define the type by using ActionResult<VillaDTO> or [ProducesResponseType(200, Type = typeof(VillaDTO))]
        // Define the route name
        //[HttpGet("{id:int}"]
        [HttpGet("{id:int}",Name="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200)]
        //[ProducesResponseType(404)]
        //[ProducesResponseType(400)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
            {
                // Bad Request
                return BadRequest(); 
            }
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if(villa == null)
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
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO)
        {
            if(!ModelState.IsValid)
            {
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

            // Created a route will define the route it will be created and assign value to the parameter being passed
            //return Ok(villaDTO);
            return CreatedAtRoute("GetVilla", new {  id = villaDTO.Id }, villaDTO);
        }
    }
}