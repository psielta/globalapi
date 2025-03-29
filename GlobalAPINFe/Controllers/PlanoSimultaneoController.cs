using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanoSimultaneoController : GenericDtoController<PlanoSimultaneo, int, PlanoSimultaneoDto>
    {
        private readonly GlobalErpFiscalBaseContext context;
        public PlanoSimultaneoController(IRepositoryDto<PlanoSimultaneo, int, PlanoSimultaneoDto> repo, ILogger<GenericDtoController<PlanoSimultaneo, int, PlanoSimultaneoDto>> logger, GlobalErpFiscalBaseContext _context) : base(repo, logger)
        {
            this.context = _context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PlanoSimultaneo>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<PlanoSimultaneo>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PlanoSimultaneo), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PlanoSimultaneo>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PlanoSimultaneo), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<PlanoSimultaneo>> Create([FromBody] PlanoSimultaneoDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PlanoSimultaneo), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PlanoSimultaneo>> Update(int id, [FromBody] PlanoSimultaneoDto dto)
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

        [HttpGet("GetPlanoSimultaneoByUnity/{unity}")]
        [ProducesResponseType(typeof(IEnumerable<PlanoSimultaneo>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<PlanoSimultaneo>>> GetPlanoSimultaneoByUnity([FromRoute] int unity)
        {
            try
            {
                var entitys = await this.context.PlanoSimultaneos
                    .Include(p => p.CdPlanoPrincNavigation).ThenInclude(e => e.CdEmpresaNavigation)
                .Include(p => p.CdPlanoReplicaNavigation).ThenInclude(e => e.CdEmpresaNavigation)
                .Where(x => x.Unity == unity).ToListAsync();
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
    }
}
