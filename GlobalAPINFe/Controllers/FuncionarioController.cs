using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalErpData.Repository.PagedRepositoriesMultiKey;
using GlobalErpData.Repository.Repositories;
using GlobalLib.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionarioController : GlobalLib.GenericControllers.GenericPagedControllerMultiKey<Funcionario, int, int, FuncionarioDto>
    {
        public FuncionarioController(IQueryRepositoryMultiKey<Funcionario, int, int, FuncionarioDto> repo, ILogger<GlobalLib.GenericControllers.GenericPagedControllerMultiKey<Funcionario, int, int, FuncionarioDto>> logger) : base(repo, logger)
        {
        }
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<Funcionario>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<Funcionario>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{cdEmpresa}/{cdFuncionario}")]
        [ProducesResponseType(typeof(Funcionario), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Funcionario>> GetEntity(int cdEmpresa
            
            , int cdFuncionario)
        {
            return await base.GetEntity(cdEmpresa, cdFuncionario);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Funcionario), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Funcionario>> Create([FromBody] FuncionarioDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Invalid data provided."); // 400 Bad request
                }
                Funcionario? added = await repo.CreateAsync(dto);
                if (added == null)
                {
                    return BadRequest("Failed to create the entity.");
                }
                else
                {
                    return CreatedAtAction( // 201 Created
                      nameof(GetEntity),
                      new { cdEmpresa = added.GetId().Item1, cdFuncionario = added.GetId().Item2 },
                      added);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating an entity.");
                return StatusCode(500, $"An error occurred while creating the entity: {ex.GetType().Name} - {ex.Message} - {ex.InnerException}");
            }
        }

        [HttpPut("{cdEmpresa}/{cdFuncionario}")]
        [ProducesResponseType(typeof(Funcionario), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Funcionario>> Update(int cdEmpresa, int cdFuncionario, [FromBody] FuncionarioDto dto)
        {
            return await base.Update(cdEmpresa, cdFuncionario, dto);
        }

        [HttpDelete("{cdEmpresa}/{cdFuncionario}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(int cdEmpresa, int cdFuncionario)
        {
            return await base.Delete(cdEmpresa, cdFuncionario);
        }

        [HttpGet("GetFuncionarioByUnity", Name = nameof(GetFuncionarioByUnity))]
        [ProducesResponseType(typeof(PagedResponse<Funcionario>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<Funcionario>>> GetFuncionarioByUnity(
            int unity,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
            )
        {
            try
            {
                var query = ((FuncionarioRepository)repo).GetQueryableFuncionariosByUnity(unity);

                var filteredQuery = query.OrderBy(p => p.CdFuncionario);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);

                var response = new PagedResponse<Funcionario>(pagedList);

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities (Funcionarios).");
                return StatusCode(500, "An error occurred while retrieving entities (Funcionarios). Please try again later.");
            }
        }

        [HttpGet("GetAllFuncionarioByUnity", Name = nameof(GetAllFuncionarioByUnity))]
        [ProducesResponseType(typeof(List<Funcionario>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<Funcionario>>> GetAllFuncionarioByUnity(
            int unity)
        {
            try
            {
                List<Funcionario> list = await ((FuncionarioRepository)repo).GetFuncionariosByUnity(unity);

                return Ok(list);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities (Funcionarios).");
                return StatusCode(500, "An error occurred while retrieving entities (Funcionarios). Please try again later.");
            }
        }
    }
}
