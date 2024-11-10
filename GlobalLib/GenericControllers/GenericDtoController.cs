using GlobalLib.Repository;
using GlobalLib.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlobalLib.GenericControllers
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
        public virtual async Task<ActionResult<IEnumerable<TEntity>>> GetEntities()
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
        public virtual async Task<ActionResult<TEntity>> GetEntity(TKey id)
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
        public virtual async Task<ActionResult<TEntity>> Create([FromBody] TDto dto)
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
        public virtual async Task<ActionResult<TEntity>> Update(TKey id, [FromBody] TDto dto)
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
        public virtual async Task<IActionResult> Delete(TKey id)
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
                    return NoContent(); // 204 No content
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
