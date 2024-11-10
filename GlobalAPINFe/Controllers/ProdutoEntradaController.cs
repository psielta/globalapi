using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalLib.Dto;
using GlobalErpData.Repository.PagedRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoEntradaController : GenericPagedController<ProdutoEntradum, int, ProdutoEntradaDto>
    {
        private readonly GlobalErpFiscalBaseContext _context;

        public ProdutoEntradaController(IQueryRepository<ProdutoEntradum, int, ProdutoEntradaDto> repo, ILogger<GenericPagedController<ProdutoEntradum, int, ProdutoEntradaDto>> logger, GlobalErpFiscalBaseContext _context) : base(repo, logger)
        {
            this._context = _context;
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<ProdutoEntradum>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<ProdutoEntradum>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProdutoEntradum), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ProdutoEntradum>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProdutoEntradum), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<ProdutoEntradum>> Create([FromBody] ProdutoEntradaDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<ProdutoEntradum>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<ProdutoEntradum>>> CreateBulk([FromBody] IEnumerable<ProdutoEntradaDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProdutoEntradum), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ProdutoEntradum>> Update(int id, [FromBody] ProdutoEntradaDto dto)
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

        // Métodos personalizados ajustados

        [HttpGet("GetProdutoEntradaPorEntrada", Name = nameof(GetProdutoEntradaPorEntrada))]
        [ProducesResponseType(typeof(PagedResponse<ProdutoEntradum>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<ProdutoEntradum>>> GetProdutoEntradaPorEntrada(
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
        
        [HttpGet("GetProdutoEntradaPorEntradaSemPaginacao", Name = nameof(GetProdutoEntradaPorEntradaSemPaginacao))]
        [ProducesResponseType(typeof(IEnumerable<ProdutoEntradum>), 200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<ProdutoEntradum>> GetProdutoEntradaPorEntradaSemPaginacao(
            int idEmpresa,
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

                filteredQuery = filteredQuery.OrderBy(p => p.Nr);


                if (filteredQuery == null || filteredQuery.Count() == 0)
                {
                    return NotFound("Entidades não encontradas."); // 404 Resource not found
                }

                return Ok(filteredQuery); // 200 OK
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ocorreu um erro ao recuperar as entidades paginadas.");
                return StatusCode(500, "Ocorreu um erro ao recuperar as entidades. Por favor, tente novamente mais tarde.");
            }
        }
        
        [HttpPost("InserirProdutoEntrada", Name = nameof(InserirProdutoEntrada))]
        [ProducesResponseType(typeof(ProdutoEntradum), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> InserirProdutoEntrada([FromBody] InsercaoEntradaDto dto)
        {
            try
            {
                
                try
                {
                    ProdutoEstoque? produto = await _context.ProdutoEstoques.FirstOrDefaultAsync(obj =>
                    obj.IdEmpresa == dto.CdEmpresa && obj.CdProduto == dto.CdProduto);
                    if (produto == null)
                    {
                        throw new Exception("Produto não encontrado.");
                    }
                    ProdutoEntradaDto produtoEntradaDto = new ProdutoEntradaDto();
                    produtoEntradaDto.NrEntrada = dto.NrEntrada;
                    produtoEntradaDto.CdEmpresa = dto.CdEmpresa;
                    produtoEntradaDto.CdProduto = dto.CdProduto;
                    produtoEntradaDto.Quant = dto.Quant;
                    produtoEntradaDto.CdPlano = dto.CdPlano;
                    produtoEntradaDto.VlUnitario = produto.VlCusto ?? 0;

                    var response = await repo.CreateAsync(produtoEntradaDto);
                    if (response == null)
                    {
                        return BadRequest("Falha ao criar a entidade.");
                    }
                    return CreatedAtAction(nameof(InserirProdutoEntrada), new { id = response.Nr }, response);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Produto não encontrado.");
                    return BadRequest("Produto não encontrado.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ocorreu um erro ao inserir a entidade.");
                return StatusCode(500, "Ocorreu um erro ao inserir a entidade. Por favor, tente novamente mais tarde.");
            }
        }

        [HttpPost("InserirProdutoEntradaEan", Name = nameof(InserirProdutoEntradaEan))]
        [ProducesResponseType(typeof(ProdutoEntradum), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> InserirProdutoEntradaEan([FromBody] InsercaoEntradaEanDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.Ean))
                {
                    return BadRequest("EAN não pode ser nulo ou vazio.");
                }
               
                try
                {
                    ProdutoEstoque produto = await _context.ProdutoEstoques.FirstOrDefaultAsync(obj =>
                    obj.IdEmpresa == dto.CdEmpresa && obj.CdBarra.Equals(dto.Ean));
                    if (produto == null)
                    {
                        throw new Exception("Produto não encontrado.");
                    }
                    ProdutoEntradaDto produtoEntradaDto = new ProdutoEntradaDto();
                    produtoEntradaDto.NrEntrada = dto.NrEntrada;
                    produtoEntradaDto.CdEmpresa = dto.CdEmpresa;
                    produtoEntradaDto.CdProduto = produto.CdProduto;
                    produtoEntradaDto.Quant = dto.Quant;
                    produtoEntradaDto.CdPlano = dto.CdPlano;
                    produtoEntradaDto.VlUnitario = produto.VlCusto ?? 0;

                    var response = await repo.CreateAsync(produtoEntradaDto);
                    if (response == null)
                    {
                        return BadRequest("Falha ao criar a entidade.");
                    }
                    return CreatedAtAction(nameof(InserirProdutoEntradaEan), new { id = response.Nr }, response);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Produto não encontrado.");
                    return BadRequest("Produto não encontrado.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ocorreu um erro ao inserir a entidade.");
                return StatusCode(500, "Ocorreu um erro ao inserir a entidade. Por favor, tente novamente mais tarde.");
            }
        }
    }
}
