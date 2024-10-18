using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList.EF;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormaPagtController : GenericPagedController<FormaPagt, int, FormaPagtDto>
    {
        public FormaPagtController(IQueryRepository<FormaPagt, int, FormaPagtDto> repo, ILogger<GenericPagedController<FormaPagt, int, FormaPagtDto>> logger) : base(repo, logger)
        {
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<FormaPagt>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<FormaPagt>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FormaPagt), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<FormaPagt>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(FormaPagt), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<FormaPagt>> Create([FromBody] FormaPagtDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(FormaPagt), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<FormaPagt>> Update(int id, [FromBody] FormaPagtDto dto)
        {
            return await base.Update(id, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }

        // Métodos personalizados ajustados

        [HttpGet("GetFormaPagtPorEmpresa", Name = nameof(GetFormaPagtPorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<FormaPagt>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<FormaPagt>>> GetFormaPagtPorEmpresa(int idEmpresa, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = ((FormaPagtRepository)repo).GetFormaPagtAsyncPorEmpresa(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<FormaPagt>(pagedList);

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

        [HttpGet("GetFormaPagtPorEmpresa_ALL", Name = nameof(GetFormaPagtPorEmpresa_ALL))]
        [ProducesResponseType(typeof(IEnumerable<FormaPagt>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<FormaPagt>>> GetFormaPagtPorEmpresa_ALL(int idEmpresa)
        {
            try
            {
                var query = ((FormaPagtRepository)repo).GetFormaPagtAsyncPorEmpresa(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var list = await query.AsNoTracking().ToListAsync();

                if (list == null || list.Count == 0)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                return Ok(list); // 200 OK
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities.");
                return StatusCode(500, "An error occurred while retrieving entities. Please try again later.");
            }
        }
    }
}
