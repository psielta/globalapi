using GlobalLib.Database;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlobalLib.GenericControllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public abstract class GenericController<TEntity, TKey> : Controller where TEntity : class, IIdentifiable<TKey>
    {
        protected readonly IRepository<TEntity, TKey> repo;

        public GenericController(IRepository<TEntity, TKey> repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TEntity>>> GetEntities()
        {
            IEnumerable<TEntity>? entities = await repo.RetrieveAllAsync();
            if (entities == null)
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(entities); // 200 OK
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TEntity>> GetEntity(TKey id)
        {
            TEntity? entity = await repo.RetrieveAsync(id);
            if (entity == null)
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(entity); // 200 OK
        }

        [HttpPost]
        public virtual async Task<ActionResult<TEntity>> Create([FromBody] TEntity entity)
        {
            if (entity == null)
            {
                return BadRequest(); // 400 Bad request
            }
            TEntity? added = await repo.CreateAsync(entity);
            if (added == null)
            {
                return BadRequest("Repository failed to create.");
            }
            else
            {
                return CreatedAtAction( // 201 Created
                  nameof(GetEntity),
                  new { id = added.GetId() },
                  added);
            }
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(TKey id, [FromBody] TEntity entity)
        {
            if (entity == null || !entity.GetId()!.Equals(id))
            {
                return BadRequest(); // 400 Bad request
            }
            TEntity? existing = await repo.RetrieveAsync(id);
            if (existing == null)
            {
                return NotFound(); // 404 Resource not found
            }
            await repo.UpdateAsync(id, entity);
            return NoContent(); // 204 No content
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(TKey id)
        {
            TEntity? existing = await repo.RetrieveAsync(id);
            if (existing == null)
            {
                return NotFound(); // 404 Resource not found
            }
            bool? deleted = await repo.DeleteAsync(id);
            if (deleted.HasValue && deleted.Value) // short circuit AND
            {
                return NoContent(); // 204 No content
            }
            else
            {
                return BadRequest( // 400 Bad request
                  $"{id} was found but failed to delete.");
            }
        }
    }
}
