using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilLojaController : GenericDtoController<PerfilLoja, int, PerfilLojaDto>
    {
        public PerfilLojaController(IRepositoryDto<PerfilLoja, int, PerfilLojaDto> repo, ILogger<GenericDtoController<PerfilLoja, int, PerfilLojaDto>> logger) : base(repo, logger)
        {
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PerfilLoja>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<PerfilLoja>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PerfilLoja), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PerfilLoja>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PerfilLoja), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<PerfilLoja>> Create([FromBody] PerfilLojaDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PerfilLoja), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PerfilLoja>> Update(int id, [FromBody] PerfilLojaDto dto)
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

        // Método personalizado ajustado
        [HttpGet("/api/PerfilLojasPorEmpresa/{id}", Name = nameof(GetPerfilLojasPorEmpresa))]
        [ProducesResponseType(typeof(IEnumerable<PerfilLoja>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<PerfilLoja>>> GetPerfilLojasPorEmpresa(int id)
        {
            IEnumerable<PerfilLoja>? entities = await ((PerfilLojaRepository)repo).GetPerfilLojasByEmpresaAsync(id);
            if (entities == null || !entities.Any())
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(entities); // 200 OK
        }
    }
}
