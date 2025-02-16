using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Dto;
using GlobalLib.Enum;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using X.PagedList;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CteController : GenericPagedController<Cte, int, CteDto>
    {
        private readonly GlobalErpFiscalBaseContext _context;
        private readonly ILogger<CteController> _log;
        public CteController(IQueryRepository<Cte, int, CteDto> repo, ILogger<GenericPagedController<Cte, int, CteDto>> logger, GlobalErpFiscalBaseContext _context, ILogger<CteController> _log) : base(repo, logger)
        {
            this._context = _context;
            this._log = _log;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<Cte>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<Cte>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Cte), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Cte>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Cte), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Cte>> Create([FromBody] CteDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<Cte>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<Cte>>> CreateBulk([FromBody] IEnumerable<CteDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Cte), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Cte>> Update(int id, [FromBody] CteDto dto)
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
        /// <summary>
        /// Endpoint para pesquisa customizada de CTEs com campos calculados.
        /// Parâmetros opcionais: pDataInicial, pDataFinal, pIdCte, pId, pId_Cliente.
        /// O parâmetro pId_Empresa é obrigatório e modelosNFe possui valor padrão.
        /// </summary>
        /// <param name="idEmpresa">Identificador da empresa (obrigatório)</param>
        /// <param name="pDataInicial">Data inicial da pesquisa (opcional)</param>
        /// <param name="pDataFinal">Data final da pesquisa (opcional)</param>
        /// <param name="pIdCte">Número do CTE (opcional)</param>
        /// <param name="pId">Identificador do CTE (opcional)</param>
        /// <param name="pId_Cliente">Identificador do cliente (opcional)</param>
        /// <param name="modelosNFe">Modelo de NFe (valor padrão: GGUCtePassageiros)</param>
        /// <returns>Lista de CTEs encontrados</returns>
        [HttpGet("GetCtePorEmpresa", Name = nameof(GetCtePorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<Cte>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<Cte>>> GetCtePorEmpresa(
            int idEmpresa,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? pDataInicial = "",
            [FromQuery] string? pDataFinal = "",
            [FromQuery] string? pIdCte = "",
            [FromQuery] string? pId = "",
            [FromQuery] string? pId_Cliente = "",
            [FromQuery] GGUModelosNFe modelosNFe = GGUModelosNFe.GGUCtePassageiros)
        {
            try
            {
                var cteRepo = repo as CteRepository;
                if (cteRepo == null)
                {
                    return StatusCode(500, "Erro interno: repositório inválido");
                }

                var query = cteRepo.Get_Pesquisa_Nome(idEmpresa.ToString(), pDataInicial, pDataFinal, pIdCte, pId, pId_Cliente, modelosNFe);

                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                if (pagedList == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                var response = new PagedResponse<Cte>(pagedList);

                if (response.Items == null || response.Items.Count == 0)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao executar pesquisa de CTE.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }
    }
}
