using GlobalErpData.Repository;
using GlobalLib.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlobalErpData.GenericControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class GenericDtoController<TEntity, TKey, TDto> : Controller where TEntity : class, IIdentifiable<TKey>
    {
        protected readonly IRepositoryDto<TEntity, TKey, TDto> repo;
        protected readonly ILogger<GenericDtoController<TEntity, TKey, TDto>> logger;

        public GenericDtoController(IRepositoryDto<TEntity, TKey, TDto> repo, ILogger<GenericDtoController<TEntity, TKey, TDto>> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEntities()
        {
            try
            {
                IEnumerable<TEntity>? entities = await repo.RetrieveAllAsync();
                if (entities == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                return Ok(entities); // 200 OK
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving entities.");
                return StatusCode(500, "An error occurred while retrieving entities. Please try again later.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEntity(TKey id)
        {
            try
            {
                TEntity? entity = await repo.RetrieveAsync(id);
                if (entity == null)
                {
                    return NotFound($"Entity with ID {id} not found."); // 404 Resource not found
                }
                return Ok(entity); // 200 OK
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving the entity with ID {Id}.", id);
                return StatusCode(500, $"An error occurred while retrieving the entity with ID {id}. Please try again later.");
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] TDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Invalid data provided."); // 400 Bad request
                }
                TEntity? added = await repo.CreateAsync(dto);
                if (added == null)
                {
                    return BadRequest("Failed to create the entity.");
                }
                else
                {
                    return CreatedAtAction( // 201 Created
                      nameof(GetEntity),
                      new { id = added.GetId() },
                      added);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating an entity.");
                return StatusCode(500, "An error occurred while creating the entity. Please try again later.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(TKey id, [FromBody] TDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Invalid data provided."); // 400 Bad request
                }
                TEntity? existing = await repo.RetrieveAsync(id);
                if (existing == null)
                {
                    return NotFound($"Entity with ID {id} not found."); // 404 Resource not found
                }
                await repo.UpdateAsync(id, dto);

                // Retrieve the updated entity
                TEntity updated = await repo.RetrieveAsync(id);
                return Ok(updated); // 200 OK with updated object
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating the entity with ID {Id}.", id);
                return StatusCode(500, $"An error occurred while updating the entity with ID {id}. Please try again later.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(TKey id)
        {
            try
            {
                TEntity? existing = await repo.RetrieveAsync(id);
                if (existing == null)
                {
                    return NotFound($"Entity with ID {id} not found."); // 404 Resource not found
                }
                bool? deleted = await repo.DeleteAsync(id);
                if (deleted.HasValue && deleted.Value) // short circuit AND
                {
                    return new NoContentResult(); // 204 No content
                }
                else
                {
                    return BadRequest( // 400 Bad request
                      $"Entity with ID {id} was found but failed to delete.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting the entity with ID {Id}.", id);
                return StatusCode(500, $"An error occurred while deleting the entity with ID {id}. Please try again later.");
            }
        }
    }
}
