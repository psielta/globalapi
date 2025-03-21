using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Dto;
using GlobalErpData.Repository.PagedRepositoriesMultiKey;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransportadoraController : GenericPagedControllerMultiKey<Transportadora, int, int, TransportadoraDto>
    {
        public TransportadoraController(IQueryRepositoryMultiKey<Transportadora, int, int, TransportadoraDto> repo, ILogger<GenericPagedControllerMultiKey<Transportadora, int, int, TransportadoraDto>> logger) : base(repo, logger)
        {
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<Transportadora>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<Transportadora>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(Transportadora), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Transportadora>> GetEntity(int idEmpresa, int idCadastro)
        {
            return await base.GetEntity(idEmpresa, idCadastro);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Transportadora), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Transportadora>> Create([FromBody] TransportadoraDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(Transportadora), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Transportadora>> Update(int idEmpresa, int idCadastro, [FromBody] TransportadoraDto dto)
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

        // Métodos personalizados ajustados

        [HttpGet("GetTransportadoraPorEmpresa", Name = nameof(GetTransportadoraPorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<Transportadora>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<Transportadora>>> GetTransportadoraPorEmpresa(
            int unity,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? nmTransp = null,
            [FromQuery] int? cdTransp = null,
            [FromQuery] string? cnpj = null)
        {
            try
            {
                var query = ((TransportadoraPagedRepository)repo).GetTransportadoraPorEmpresa(unity).Result.AsQueryable();

                if (query == null)
                {
                    return NotFound("Entities not found.");
                }

                var filteredQuery = query.AsEnumerable();

                if (!string.IsNullOrEmpty(nmTransp))
                {
                    var normalizednmTransp = UtlStrings.RemoveDiacritics(nmTransp.ToLower().Trim());
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.NmTransportadora == null) ? "" : p.NmTransportadora.ToLower()).Contains(normalizednmTransp));
                }

                if (cdTransp.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.CdTransportadora == cdTransp.Value);
                }

                if (!string.IsNullOrEmpty(cnpj))
                {
                    var normalizeCnpj = UtlStrings.RemoveSpecialCharacters(UtlStrings.RemoveDiacritics(cnpj.Trim().ToLower()));
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveSpecialCharacters(UtlStrings.RemoveDiacritics((p.CdCnpj == null) ? "" : p.CdCnpj.Trim().ToLower())) == normalizeCnpj);
                }

                filteredQuery = filteredQuery.OrderByDescending(p => p.CdTransportadora);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<Transportadora>(pagedList);

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

        [HttpGet("GetTransportadoraByName/{unity}/{nome}")]
        [ProducesResponseType(typeof(IEnumerable<Transportadora>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Transportadora>>> GetTransportadoraByName(int unity, string nome)
        {
            var Transportadoras = await (repo as TransportadoraPagedRepository).GetTransportadoraPorEmpresa(unity);

            var TransportadorasList = Transportadoras.ToList();
            if (TransportadorasList == null)
            {
                return NotFound("Transportadora não encontrada.");
            }

            var stringNormalizada = UtlStrings.RemoveDiacritics(nome.ToLower());

            var filter = TransportadorasList.Where(c => UtlStrings.RemoveDiacritics(c.NmTransportadora.ToLower()).StartsWith(stringNormalizada))
                                .OrderBy(c => c.NmTransportadora)
                                .ToList();

            if (filter.Count == 0)
            {
                return NotFound("Transportadoras não encontrada.");
            }
            return Ok(filter);
        }
    }
}
