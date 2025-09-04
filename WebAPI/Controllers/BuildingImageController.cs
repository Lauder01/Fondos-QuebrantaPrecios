using Microsoft.AspNetCore.Mvc;
using ClassLibraryProject.Entities;
using ServiceLibraryProject;
using AutoMapper;
using System.Collections.Generic;
using WebAPI.Dtos.BuildingImage;
using System;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingImageController(BuildingImageService buildingImageService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BuildingImageGetterDto>>> GetAll()
        {
            var images = await buildingImageService.GetAllAsync();
            var dtos = mapper.Map<IEnumerable<BuildingImageGetterDto>>(images);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BuildingImageGetterDto>> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("El identificador no puede estar vacío.");

            var image = await buildingImageService.GetByIdAsync(id);
            if (image == null)
                return NotFound($"Imagen {id} no encontrada.");

            var dto = mapper.Map<BuildingImageGetterDto>(image);
            return Ok(dto);
        }

        [HttpGet("building/{buildingId}")]
        public async Task<ActionResult<IEnumerable<BuildingImageGetterDto>>> GetByBuildingId(string buildingId)
        {
            if (string.IsNullOrEmpty(buildingId))
                return BadRequest("El identificador de edificio no puede estar vacío.");

            var images = await buildingImageService.GetByBuildingIdAsync(buildingId);
            var dtos = mapper.Map<IEnumerable<BuildingImageGetterDto>>(images);
            return Ok(dtos);
        }

        [HttpPost]
        public async Task<ActionResult<BuildingImageGetterDto>> Create(BuildingImageCreatorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var image = mapper.Map<BuildingImage>(dto);
            image.BuildingImageId = Guid.NewGuid().ToString();

            try
            {
                await buildingImageService.AddAsync(image);
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
        public async Task<IActionResult> Update(string id, BuildingImageUpdaterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var image = mapper.Map<BuildingImage>(dto);
            image.BuildingImageId = id;

            try
            {
                await buildingImageService.UpdateAsync(image);
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
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await buildingImageService.DeleteAsync(id);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            return NoContent();
        }
    }
}
