using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using AutoMapper;
using System.Collections.Generic;
using ServiceLibraryProject;
using WebAPI.Dtos.BuildingImage;
using System;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingImageController(BuildingImageService buildingImageService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<BuildingImageGetterDto>> GetAll()
        {
            var images = buildingImageService.GetAll();
            var dtos = mapper.Map<IEnumerable<BuildingImageGetterDto>>(images);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public ActionResult<BuildingImageGetterDto> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("El identificador no puede estar vacío.");

            var image = buildingImageService.GetById(id);
            if (image == null)
                return NotFound($"Imagen {id} no encontrada.");

            var dto = mapper.Map<BuildingImageGetterDto>(image);
            return Ok(dto);
        }

        [HttpGet("building/{buildingId}")]
        public ActionResult<IEnumerable<BuildingImageGetterDto>> GetByBuildingId(string buildingId)
        {
            if (string.IsNullOrEmpty(buildingId))
                return BadRequest("El identificador de edificio no puede estar vacío.");

            var images = buildingImageService.GetByBuildingId(buildingId);
            var dtos = mapper.Map<IEnumerable<BuildingImageGetterDto>>(images);
            return Ok(dtos);
        }

        [HttpPost]
        public ActionResult<BuildingImageGetterDto> Create(BuildingImageCreatorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var image = mapper.Map<BuildingImage>(dto);
            image.BuildingImageId = Guid.NewGuid().ToString();

            try
            {
                buildingImageService.Add(image);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }

            var result = mapper.Map<BuildingImageGetterDto>(image);
            return CreatedAtAction(nameof(GetAll), new { id = image.BuildingImageId }, result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, BuildingImageUpdaterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var image = mapper.Map<BuildingImage>(dto);
            image.BuildingImageId = id;

            try
            {
                buildingImageService.Update(image);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                buildingImageService.Delete(id);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            return NoContent();
        }
    }
}
