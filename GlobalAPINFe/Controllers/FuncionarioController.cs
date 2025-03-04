using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionarioController : GenericPagedControllerMultiKey<Funcionario, int, int, FuncionarioDto>
    {
        public FuncionarioController(IQueryRepositoryMultiKey<Funcionario, int, int, FuncionarioDto> repo, ILogger<GenericPagedControllerMultiKey<Funcionario, int, int, FuncionarioDto>> logger) : base(repo, logger)
        {
        }
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<Funcionario>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<Funcionario>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(Funcionario), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Funcionario>> GetEntity(int idEmpresa, int idCadastro)
        {
            return await base.GetEntity(idEmpresa, idCadastro);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Funcionario), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Funcionario>> Create([FromBody] FuncionarioDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(Funcionario), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Funcionario>> Update(int idEmpresa, int idCadastro, [FromBody] FuncionarioDto dto)
        {
            return await base.Update(idEmpresa, idCadastro, dto);
        }

        [HttpDelete("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(int idEmpresa, int idCadastro)
        {
            return await base.Delete(idEmpresa, idCadastro);
        }
    }
}
