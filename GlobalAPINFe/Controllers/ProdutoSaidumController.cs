﻿using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;
using GlobalLib.Dto;
using GlobalErpData.Services;

namespace GlobalAPINFe.Controllers
{
    public class ProdutoSaidumController : GenericPagedController<ProdutoSaidum, int, ProdutoSaidumDto>
    {
        private readonly GlobalErpFiscalBaseContext _context;
        private readonly ProdutoSaidumService produtoSaidumService;
        public ProdutoSaidumController(
                IQueryRepository<ProdutoSaidum, int, ProdutoSaidumDto> repo,
                ILogger<GenericPagedController<ProdutoSaidum, int, ProdutoSaidumDto>> logger,
                GlobalErpFiscalBaseContext _context,
                ProdutoSaidumService produtoSaidumService
                ) : base(repo, logger)
        {
            this._context = _context;
            this.produtoSaidumService = produtoSaidumService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<ProdutoSaidum>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<ProdutoSaidum>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProdutoSaidum), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ProdutoSaidum>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProdutoSaidum), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<ProdutoSaidum>> Create([FromBody] ProdutoSaidumDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProdutoSaidum), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ProdutoSaidum>> Update(int id, [FromBody] ProdutoSaidumDto dto)
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

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<ProdutoSaidum>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<ProdutoSaidum>>> CreateBulk([FromBody] IEnumerable<ProdutoSaidumDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("UpdateSemRecalculo/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public virtual async Task<ActionResult<ProdutoSaidum>> UpdateSemRecalculo(int id, [FromBody] ProdutoSaidumDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Invalid data provided."); // 400 Bad request
                }
                ProdutoSaidum? existing = await repo.RetrieveAsync(id);
                if (existing == null)
                {
                    return NotFound($"Entity with ID {id} not found."); // 404 Resource not found
                }
                await ((ProdutoSaidumRepository)repo).UpdateAsyncSemRecalcular(id, dto);

                // Retrieve the updated entity
                ProdutoSaidum updated = await repo.RetrieveAsync(id);
                return Ok(updated); // 200 OK with updated object
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating the entity with ID {Id}.", id);
                return StatusCode(500, $"An error occurred while updating the entity with ID {id}: {ex.GetType().Name} - {ex.Message} - {ex.InnerException}");
            }
        }

        [HttpGet("GetProdutoSaidumPorSaida", Name = nameof(GetProdutoSaidumPorSaida))]
        [ProducesResponseType(typeof(PagedResponse<ProdutoSaidum>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<ProdutoSaidum>>> GetProdutoSaidumPorSaida(
            int numeroSaida,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = ((ProdutoSaidumRepository)repo).GetProdutoSaidumAsyncPorSaida(numeroSaida).Result.AsQueryable();

                if (query == null)
                {
                    return NotFound("Entidades não encontradas.");
                }

                var filteredQuery = query.AsEnumerable();

                filteredQuery = filteredQuery.OrderBy(p => p.Nr);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<ProdutoSaidum>(pagedList);

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

        [HttpGet("GetProdutoSaidumPorSaidaSemPaginacao/{nrSaida}", Name = nameof(GetProdutoSaidumPorSaidaSemPaginacao))]
        [ProducesResponseType(typeof(IEnumerable<ProdutoSaidum>), 200)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<ProdutoSaidum>> GetProdutoSaidumPorSaidaSemPaginacao(
            int nrSaida)
        {
            try
            {
                var query = ((ProdutoSaidumRepository)repo).GetProdutoSaidumAsyncPorSaida(nrSaida).Result.AsQueryable();

                if (query == null)
                {
                    return NotFound("Entidades não encontradas.");
                }

                var filteredQuery = query.AsEnumerable();

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

        [HttpPost("InserirProdutoSaidum", Name = nameof(InserirProdutoSaidum))]
        [ProducesResponseType(typeof(ProdutoSaidum), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProdutoSaidum>> InserirProdutoSaidum([FromBody] InsercaoProdutoSaidumDto dto)
        {
            try
            {

                try
                {
                    ProdutoEstoque? produto = await _context.ProdutoEstoques.FirstOrDefaultAsync(obj =>
                    obj.Unity == dto.Unity && obj.CdProduto == dto.CdProduto);

                    if (produto == null)
                    {
                        throw new Exception("Produto não encontrado.");
                    }
                    if (dto.CdEmpresa == null)
                    {
                        throw new Exception("Empresa não encontrada.");
                    }
                    if (produto.VlAVista == null)
                    {
                        throw new Exception("Valor de venda não encontrado.");
                    }
                    ProdutoSaidumDto produtoSaidumDto = new ProdutoSaidumDto();
                    //ProdutoSaidumDto.NrSaida = dto.NrSaida;
                    //ProdutoSaidumDto.CdEmpresa = dto.CdEmpresa ?? 0;
                    //ProdutoSaidumDto.CdProduto = produto.CdProduto;
                    //ProdutoSaidumDto.CdBarra = produto.CdBarra;
                    //ProdutoSaidumDto.NmProduto = produto.NmProduto;
                    //ProdutoSaidumDto.Lote = "-1";
                    //ProdutoSaidumDto.Desconto = 0;
                    //ProdutoSaidumDto.DtValidade = DateUtils.DateTimeToDateOnly(DateTime.Now);
                    //ProdutoSaidumDto.Quant = dto.Quant;
                    //ProdutoSaidumDto.CdPlano = dto.CdPlano;
                    //ProdutoSaidumDto.VlVenda = Math.Round(produto.VlAVista ?? 0, 4);
                    //ProdutoSaidumDto.VlTotal = Math.Round(ProdutoSaidumDto.Quant * ProdutoSaidumDto.VlVenda, 4);

                    Saida? saida = await _context.Saidas
                        .AsNoTracking()
                        .Include(s => s.ClienteNavigation).ThenInclude(c => c.CdCidadeNavigation)
                        .FirstOrDefaultAsync(obj => obj.NrLanc == dto.NrSaida && obj.Empresa == dto.CdEmpresa);

                    await produtoSaidumService.InserirDadosProduto(dto, produtoSaidumDto, produto, saida.ClienteNavigation, saida);
                    
                    var response = await repo.CreateAsync(produtoSaidumDto);

                    if (response == null)
                    {
                        return BadRequest("Falha ao criar a entidade.");
                    }
                    return CreatedAtAction(nameof(InserirProdutoSaidum), new { id = response.Nr }, response);
                }
                catch (Exception ex)
                {
                    if (ex.Message == "Produto não encontrado.")
                    {
                        logger.LogError(ex, "Produto não encontrado.");
                        return BadRequest("Produto não encontrado.");
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ocorreu um erro ao inserir a entidade.");
                return StatusCode(500, "Ocorreu um erro ao inserir a entidade. Por favor, tente novamente mais tarde.");
            }
        }

        [HttpPost("InserirProdutoSaidumEan", Name = nameof(InserirProdutoSaidumEan))]
        [ProducesResponseType(typeof(ProdutoSaidum), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProdutoSaidum>> InserirProdutoSaidumEan([FromBody] InsercaoProdutoSaidumEanDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.Ean))
                {
                    return BadRequest("EAN não pode ser nulo ou vazio.");
                }

                try
                {
                    ProdutoEstoque? produto = await _context.ProdutoEstoques.FirstOrDefaultAsync(obj =>
                    obj.Unity == dto.Unity && obj.CdBarra.Equals(dto.Ean));
                    if (produto == null)
                    {
                        throw new Exception("Produto não encontrado.");
                    }
                    if (dto.CdEmpresa == null)
                    {
                        throw new Exception("Empresa não encontrada.");
                    }
                    if (produto.VlAVista == null)
                    {
                        throw new Exception("Valor de venda não encontrado.");
                    }
                    ProdutoSaidumDto ProdutoSaidumDto = new ProdutoSaidumDto();
                    //ProdutoSaidumDto.NrSaida = dto.NrSaida;
                    //ProdutoSaidumDto.CdEmpresa = dto.CdEmpresa ;
                    //ProdutoSaidumDto.CdBarra = produto.CdBarra;
                    //ProdutoSaidumDto.CdProduto = produto.CdProduto;
                    //ProdutoSaidumDto.NmProduto = produto.NmProduto;
                    //ProdutoSaidumDto.Lote = "-1";
                    //ProdutoSaidumDto.Desconto = 0;
                    //ProdutoSaidumDto.DtValidade = DateUtils.DateTimeToDateOnly(DateTime.Now);
                    //ProdutoSaidumDto.Quant = dto.Quant;
                    //ProdutoSaidumDto.CdPlano = dto.CdPlano;
                    //ProdutoSaidumDto.VlVenda = Math.Round(produto.VlAVista ?? 0, 4);
                    //ProdutoSaidumDto.VlTotal = Math.Round(ProdutoSaidumDto.Quant * ProdutoSaidumDto.VlVenda, 4);
                    Saida? saida = await _context.Saidas
                        .AsNoTracking()
                        .Include(s => s.ClienteNavigation).ThenInclude(c => c.CdCidadeNavigation)
                        .FirstOrDefaultAsync(obj => obj.NrLanc == dto.NrSaida && obj.Empresa == dto.CdEmpresa);

                    await produtoSaidumService.InserirDadosProduto(dto, ProdutoSaidumDto, produto, saida.ClienteNavigation, saida);

                    var response = await repo.CreateAsync(ProdutoSaidumDto);
                    if (response == null)
                    {
                        return BadRequest("Falha ao criar a entidade.");
                    }
                    return CreatedAtAction(nameof(InserirProdutoSaidumEan), new { id = response.Nr }, response);
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
