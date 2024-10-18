using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalLib.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : GenericDtoController<Empresa, int, EmpresaDto>
    {
        public EmpresaController(IRepositoryDto<Empresa, int, EmpresaDto> repo, ILogger<GenericDtoController<Empresa, int, EmpresaDto>> logger) : base(repo, logger)
        {
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Empresa>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<Empresa>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Empresa), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Empresa>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Empresa), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Empresa>> Create([FromBody] EmpresaDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Empresa), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Empresa>> Update(int id, [FromBody] EmpresaDto dto)
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
    }
}
