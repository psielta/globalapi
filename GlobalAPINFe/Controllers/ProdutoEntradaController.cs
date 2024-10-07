using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoEntradaController : GenericPagedController<ProdutoEntradum, int, ProdutoEntradaDto>
    {
        public ProdutoEntradaController(IQueryRepository<ProdutoEntradum, int, ProdutoEntradaDto> repo, ILogger<GenericPagedController<ProdutoEntradum, int, ProdutoEntradaDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet("GetProdutoEntradaPorEmpresa", Name = nameof(GetProdutoEntradaPorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProdutoEntradaPorEmpresa(
            int idEmpresa,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] int? numeroEntrada = null)
        {
            try
            {
                var query = ((ProdutoEntradaRepository)repo).GetProdutoEntradaAsyncPorEmpresa(idEmpresa).Result.AsQueryable();

                if (query == null)
                {
                    return NotFound("Entidades não encontradas.");
                }

                var filteredQuery = query.AsEnumerable();

                // Filtro por número da entrada
                if (numeroEntrada.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.NrEntrada == numeroEntrada.Value);
                }
                /*
                // Filtro por período (datas)
                if (dataInicio.HasValue && dataFim.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.DataEntrada >= dataInicio.Value && p.DataEntrada <= dataFim.Value);
                }
                else if (dataInicio.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.DataEntrada >= dataInicio.Value);
                }
                else if (dataFim.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.DataEntrada <= dataFim.Value);
                }
                
                // Filtro por nome do fornecedor
                if (!string.IsNullOrEmpty(nomeFornecedor))
                {
                    var normalizedNomeFornecedor = UtlStrings.RemoveDiacritics(nomeFornecedor.ToLower());
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.NomeFornecedor == null) ? "" : p.NomeFornecedor.ToLower()).Contains(normalizedNomeFornecedor));
                }*/

                filteredQuery = filteredQuery.OrderBy(p => p.Nr);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<ProdutoEntradum>(pagedList);

                if (response.Items == null || response.Items.Count == 0)
                {
                    return NotFound("Entidades não encontradas."); // 404 Resource not found
                }

                return Ok(response); // 200 OK
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ocorreu um erro ao recuperar as entidades paginadas.");
                return StatusCode(500, "Ocorreu um erro ao recuperar as entidades. Por favor, tente novamente mais tarde.");
            }
        }
    }
}
