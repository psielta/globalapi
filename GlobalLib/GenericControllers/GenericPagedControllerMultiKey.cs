using GlobalLib.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.Linq;
using GlobalLib.Repository;
using GlobalLib.Dto;
using X.PagedList.EF;

namespace GlobalLib.GenericControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class GenericPagedControllerMultiKey<TEntity, TKey1, TKey2, TDto> : Controller
        where TEntity : class, IIdentifiableMultiKey<TKey1, TKey2>
    {
        protected readonly IQueryRepositoryMultiKey<TEntity, TKey1, TKey2, TDto> repo;
        protected readonly ILogger<GenericPagedControllerMultiKey<TEntity, TKey1, TKey2, TDto>> logger;

        public GenericPagedControllerMultiKey(IQueryRepositoryMultiKey<TEntity, TKey1, TKey2, TDto> repo, ILogger<GenericPagedControllerMultiKey<TEntity, TKey1, TKey2, TDto>> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }

        [HttpGet]
        public virtual async Task<ActionResult<PagedResponse<TEntity>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = await repo.RetrieveAllAsync();
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
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

        [HttpGet("{idEmpresa}/{idCadastro}")]
        public virtual async Task<ActionResult<TEntity>> GetEntity(TKey1 idEmpresa, TKey2 idCadastro)
        {
            try
            {
                TEntity? entity = await repo.RetrieveAsyncAsNoTracking(idEmpresa, idCadastro);
                if (entity == null)
                {
                    return NotFound($"Entity with ID {idEmpresa}-{idCadastro} not found."); // 404 Resource not found
                }
                return Ok(entity); // 200 OK
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving the entity with ID {idEmpresa}-{idCadastro}.", idEmpresa, idCadastro);
                return StatusCode(500, $"An error occurred while retrieving the entity with ID {idEmpresa}-{idCadastro}. Please try again later.");
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
                    (TKey1, TKey2) key = added.GetId();
                    TEntity? entity = await repo.RetrieveAsyncAsNoTracking(key.Item1, key.Item2);
                    if (entity == null)
                    {
                        return BadRequest("Failed to create the entity.");
                    }
                    return CreatedAtAction( // 201 Created
                      nameof(GetEntity),
                      new { idEmpresa = added.GetId().Item1, idCadastro = added.GetId().Item2 },
                      entity);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating an entity.");
                return StatusCode(500, $"An error occurred while creating the entity: {ex.GetType().Name} - {ex.Message} - {ex.InnerException}");
            }
        }

        [HttpPut("{idEmpresa}/{idCadastro}")]
        public virtual async Task<ActionResult<TEntity>> Update(TKey1 idEmpresa, TKey2 idCadastro, [FromBody] TDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Invalid data provided."); // 400 Bad request
                }
                TEntity? existing = await repo.RetrieveAsyncAsNoTracking(idEmpresa, idCadastro);
                if (existing == null)
                {
                    return NotFound($"Entity with ID {idEmpresa}-{idCadastro} not found."); // 404 Resource not found
                }

                // Retrieve the updated entity
                TEntity updated = await repo.UpdateAsync(idEmpresa, idCadastro, dto);
                return Ok(updated); // 200 OK with updated object
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating the entity with ID {idEmpresa}-{idCadastro}.", idEmpresa, idCadastro);
                return StatusCode(500, $"An error occurred while updating the entity with ID {idEmpresa}-{idCadastro}: {ex.GetType().Name} - {ex.Message} - {ex.InnerException}");
            }
        }

        [HttpDelete("{idEmpresa}/{idCadastro}")]
        public virtual async Task<IActionResult> Delete(TKey1 idEmpresa, TKey2 idCadastro)
        {
            try
            {
                TEntity? existing = await repo.RetrieveAsyncAsNoTracking(idEmpresa, idCadastro);
                if (existing == null)
                {
                    return NotFound($"Entity with ID {idEmpresa}-{idCadastro} not found."); // 404 Resource not found
                }
                bool? deleted = await repo.DeleteAsync(idEmpresa, idCadastro);
                if (deleted.HasValue && deleted.Value) // short circuit AND
                {
                    return NoContent(); // 204 No content
                }
                else
                {
                    return BadRequest( // 400 Bad request
                      $"Entity with ID {idEmpresa}-{idCadastro} was found but failed to delete.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting the entity with ID {idEmpresa}-{idCadastro}.", idEmpresa, idCadastro);
                return StatusCode(500, $"Error occurred while deleting the entity with ID {idEmpresa}-{idCadastro}: {ex.GetType().Name} - {ex.Message} - {ex.InnerException}");
            }
        }
    }
}
