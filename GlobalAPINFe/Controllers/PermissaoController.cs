using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using GlobalLib.Dto;
using System.Threading.Tasks;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissaoController : GenericDtoController<Permissao, int, PermissaoDto>
    {
        public PermissaoController(IRepositoryDto<Permissao, int, PermissaoDto> repo, ILogger<GenericDtoController<Permissao, int, PermissaoDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Permissao>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<Permissao>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Permissao), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Permissao>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Permissao), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Permissao>> Create([FromBody] PermissaoDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Permissao), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Permissao>> Update(int id, [FromBody] PermissaoDto dto)
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
