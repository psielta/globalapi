using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalErpData.Repository.Repositories;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentoController : GenericDtoController<Departamento, long, DepartamentoDto>
    {
        private readonly GlobalErpFiscalBaseContext context;
        public DepartamentoController(IRepositoryDto<Departamento, long, DepartamentoDto> repo, ILogger<GenericDtoController<Departamento, long, DepartamentoDto>> logger, GlobalErpFiscalBaseContext _context) : base(repo, logger)
        {
            this.context = _context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Departamento>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<Departamento>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("GetByUnity/{unity}")]
        [ProducesResponseType(typeof(IEnumerable<Departamento>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Departamento>>> GetByUnity([FromRoute] int unity)
        {
            try
            {
                var entitys = await this.context.Departamentos.Where(x => x.Unity == unity).ToListAsync();
                if (entitys == null || entitys.Count == 0)
                {
                    return NotFound("Entities not found.");
                }
                return Ok(entitys);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while retrieving entities.");
                return StatusCode(500, "An error occurred while retrieving entities. Please try again later.");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Departamento), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Departamento>> GetEntity(long id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Departamento), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Departamento>> Create([FromBody] DepartamentoDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Departamento), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Departamento>> Update(long id, [FromBody] DepartamentoDto dto)
        {
            return await base.Update(id, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(long id)
        {
            return await base.Delete(id);
        }
    }
}
