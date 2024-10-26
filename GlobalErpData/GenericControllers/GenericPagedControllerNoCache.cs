using GlobalLib.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;
using System;
using System.Linq;
using X.PagedList.EF;
using GlobalErpData.Repository;
using GlobalErpData.Dto;

namespace GlobalErpData.GenericControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class GenericPagedControllerNoCache<TEntity, TKey, TDto> : Controller where TEntity : class, IIdentifiable<TKey>
    {
        protected readonly IQueryRepositoryNoCache<TEntity, TKey, TDto> repo;
        protected readonly ILogger<GenericPagedControllerNoCache<TEntity, TKey, TDto>> logger;

        public GenericPagedControllerNoCache(IQueryRepositoryNoCache<TEntity, TKey, TDto> repo, ILogger<GenericPagedControllerNoCache<TEntity, TKey, TDto>> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public virtual async Task<ActionResult<PagedResponse<TEntity>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = await repo.RetrieveAllAsync();
                var pagedList = await query.AsQueryable().ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<TEntity>(pagedList);

                if (response.Items == null || response.Items.Count == 0)
                {
                    return NotFound("Entities not found.");
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities.");
                return StatusCode(500, "An error occurred while retrieving entities. Please try again later.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public virtual async Task<ActionResult<TEntity>> GetEntity(TKey id)
        {
            try
            {
                TEntity? entity = await repo.RetrieveAsync(id);
                if (entity == null)
                {
                    return NotFound($"Entity with ID {id} not found.");
                }
                return Ok(entity);
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
        public virtual async Task<ActionResult<TEntity>> Create([FromBody] TDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Invalid data provided.");
                }
                TEntity? added = await repo.CreateAsync(dto);
                if (added == null)
                {
                    return BadRequest("Failed to create the entity.");
                }
                else
                {
                    return CreatedAtAction(
                      nameof(GetEntity),
                      new { id = added.GetId() },
                      added);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating an entity.");
                return StatusCode(500, $"An error occurred while creating the entity: {ex.GetType().Name} - {ex.Message} - {ex.InnerException}");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public virtual async Task<ActionResult<TEntity>> Update(TKey id, [FromBody] TDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Invalid data provided.");
                }
                TEntity? existing = await repo.RetrieveAsync(id);
                if (existing == null)
                {
                    return NotFound($"Entity with ID {id} not found.");
                }
                TEntity? updated = await repo.UpdateAsync(id, dto);
                if (updated == null)
                {
                    return BadRequest("Failed to update the entity.");
                }
                return Ok(updated);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating the entity with ID {Id}.", id);
                return StatusCode(500, $"An error occurred while updating the entity with ID {id}: {ex.GetType().Name} - {ex.Message} - {ex.InnerException}");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public virtual async Task<IActionResult> Delete(TKey id)
        {
            try
            {
                TEntity? existing = await repo.RetrieveAsync(id);
                if (existing == null)
                {
                    return NotFound($"Entity with ID {id} not found.");
                }
                bool? deleted = await repo.DeleteAsync(id);
                if (deleted.HasValue && deleted.Value)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest($"Entity with ID {id} was found but failed to delete.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting the entity with ID {Id}.", id);
                return StatusCode(500, $"Error occurred while deleting the entity with ID {id}: {ex.GetType().Name} - {ex.Message} - {ex.InnerException}");
            }
        }

        [HttpPost("bulk")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public virtual async Task<ActionResult<IEnumerable<TEntity>>> CreateBulk([FromBody] IEnumerable<TDto> dtos)
        {
            try
            {
                if (dtos == null || !dtos.Any())
                {
                    return BadRequest("Invalid data provided.");
                }

                var entities = await repo.CreateBulkAsync(dtos);

                if (entities == null)
                {
                    return BadRequest("Failed to create the entities.");
                }

                return StatusCode(201, entities);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating entities in bulk.");
                return StatusCode(500, "An error occurred while creating the entities.");
            }
        }
    }
}
