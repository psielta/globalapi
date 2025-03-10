using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalLib.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using GlobalLib.Dto;
using GlobalErpData.Repository.Repositories;
using X.PagedList.Extensions;

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

        [HttpGet("GetEmpresaByUnity", Name = nameof(GetEmpresaByUnity))]
        [ProducesResponseType(typeof(PagedResponse<Empresa>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<Empresa>>> GetEmpresaByUnity(
            int unity,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
            )
        {
            try
            {
                var query = ((EmpresaRepositoryDto)repo).GetQueryableEmpresasByUnity(unity);

                var filteredQuery = query.OrderBy(p => p.CdEmpresa);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);

                var response = new PagedResponse<Empresa>(pagedList);

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities (Empresas).");
                return StatusCode(500, "An error occurred while retrieving entities (Empresas). Please try again later.");
            }
        }

        [HttpGet("GetAllEmpresaByUnity", Name = nameof(GetAllEmpresaByUnity))]
        [ProducesResponseType(typeof(List<Empresa>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<List<Empresa>>> GetAllEmpresaByUnity(
            int unity)
        {
            try
            {
                var list = await ((EmpresaRepositoryDto)repo).GetEmpresasByUnity(unity);

                return Ok(list);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities (Empresas).");
                return StatusCode(500, "An error occurred while retrieving entities (Empresas). Please try again later.");
            }
        }
    }
}
