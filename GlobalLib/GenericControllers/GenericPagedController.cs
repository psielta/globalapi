using GlobalLib.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;
using System;
using System.Linq;
using X.PagedList.EF;
using GlobalLib.Repository;
using GlobalLib.Dto;

namespace GlobalLib.GenericControllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public abstract class GenericPagedController<TEntity, TKey, TDto> : Controller where TEntity : class, IIdentifiable<TKey>
    {
        protected readonly IQueryRepository<TEntity, TKey, TDto> repo;
        protected readonly ILogger<GenericPagedController<TEntity, TKey, TDto>> logger;

        public GenericPagedController(IQueryRepository<TEntity, TKey, TDto> repo, ILogger<GenericPagedController<TEntity, TKey, TDto>> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        /// <summary>
        /// Retrieves a paginated list of entities.
        /// </summary>
        /// <param name="pageNumber">The page number (default is 1).</param>
        /// <param name="pageSize">The page size (default is 10).</param>
        /// <returns>A paginated list of entities.</returns>
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
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                return Ok(response); // 200 OK
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities.");
                return StatusCode(500, "An error occurred while retrieving entities. Please try again later.");
            }
        }

        /// <summary>
        /// Retrieves a single entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The requested entity.</returns>
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

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="dto">The data transfer object representing the new entity.</param>
        /// <returns>The created entity.</returns>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
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
                return StatusCode(500, $"An error occurred while creating the entity: {ex.GetType().Name} - {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="id">The ID of the entity to update.</param>
        /// <param name="dto">The data transfer object containing updated data.</param>
        /// <returns>The updated entity.</returns>
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
                    return BadRequest("Invalid data provided."); // 400 Bad request
                }
                /*TEntity? existing = await repo.RetrieveAsync(id);
                if (existing == null)
                {
                    return NotFound($"Entity with ID {id} not found."); // 404 Resource not found
                }*/
                await repo.UpdateAsync(id, dto);

                // Retrieve the updated entity
                TEntity updated = await repo.RetrieveAsync(id);
                return Ok(updated); // 200 OK with updated object
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating the entity with ID {Id}.", id);
                return StatusCode(500, $"An error occurred while updating the entity with ID {id}: {ex.GetType().Name} - {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Deletes an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>No content.</returns>
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
                    return BadRequest("Dados inválidos fornecidos."); // 400 Bad Request
                }

                var entities = await repo.CreateBulkAsync(dtos);

                if (entities == null)
                {
                    return BadRequest("Falha ao criar as entidades.");
                }

                return StatusCode(201, entities); // 201 Created
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao criar entidades em lote.");
                return StatusCode(500, "Ocorreu um erro ao criar as entidades.");
            }
        }

    }
}
