﻿using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using GlobalErpData.Repository.PagedRepositoriesMultiKey;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using GlobalLib.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoEstoqueController : GlobalLib.GenericControllers.GenericPagedControllerMultiKey<ProdutoEstoque, int, int, ProdutoEstoqueDto>
    {
        private readonly GlobalErpFiscalBaseContext _context;
        private readonly IQueryRepository<ProdutoEntradum, int, ProdutoEntradaDto> repProdutoEntrada;
        private readonly IQueryRepositoryMultiKey<Entrada, int, int, EntradaDto> repEntrada;
        private readonly IMapper _mapper;


        public ProdutoEstoqueController(
            IQueryRepositoryMultiKey<ProdutoEstoque, int, int, ProdutoEstoqueDto> repo,
            ILogger<GlobalLib.GenericControllers.GenericPagedControllerMultiKey<ProdutoEstoque, int, int, ProdutoEstoqueDto>> logger,
            GlobalErpFiscalBaseContext _context,
            IQueryRepository<ProdutoEntradum, int, ProdutoEntradaDto> repProdutoEntrada,
            IQueryRepositoryMultiKey<Entrada, int, int, EntradaDto> repEntrada,
            IMapper mapper) : base(repo, logger)
        {
            this._context = _context;
            this.repProdutoEntrada = repProdutoEntrada;
            this.repEntrada = repEntrada;
            _mapper = mapper;
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<ProdutoEstoque>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<ProdutoEstoque>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(ProdutoEstoque), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ProdutoEstoque>> GetEntity(int idEmpresa, int idCadastro)
        {
            return await base.GetEntity(idEmpresa, idCadastro);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProdutoEstoque), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<ProdutoEstoque>> Create([FromBody] ProdutoEstoqueDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(ProdutoEstoque), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ProdutoEstoque>> Update(int idEmpresa, int idCadastro, [FromBody] ProdutoEstoqueDto dto)
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

        // Método personalizado ajustado

        [HttpGet("GetProdutoEstoquePorUnity", Name = nameof(GetProdutoEstoquePorUnity))]
        [ProducesResponseType(typeof(PagedResponse<ProdutoEstoque>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<ProdutoEstoque>>> GetProdutoEstoquePorUnity(
            int unity,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? nmProduto = null,
            [FromQuery] int? cdProduto = null,
            [FromQuery] string? cdClassFiscal = null,
            [FromQuery] string? cest = null,
            [FromQuery] string? cdInterno = null,
            [FromQuery] string? cdBarra = null,
            [FromQuery] int? cdPlanoEstoque = -1
            )
        {
            try
            {
                var query = ((ProdutoEstoquePagedRepositoryMultiKey)repo)
                    .GetProdutoEstoqueAsyncPorUnity(unity)
                    .Result
                    .AsQueryable();

                if (cdPlanoEstoque != -1)
                {
                    query = query.Include(p => p.SaldoEstoques.Where(s => s.CdPlano == cdPlanoEstoque));
                }
                else
                {
                    query = query.Include(p => p.SaldoEstoques);
                }

                if (query == null)
                {
                    return NotFound("Entities not found.");
                }

                var filteredQuery = query.AsEnumerable();

                if (!string.IsNullOrEmpty(nmProduto))
                {
                    var normalizedNmProduto = UtlStrings.RemoveDiacritics(nmProduto.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.NmProduto == null) ? "" : p.NmProduto.ToLower()).Contains(normalizedNmProduto));
                }

                if (cdProduto.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.CdProduto == cdProduto.Value);
                }

                if (!string.IsNullOrEmpty(cdClassFiscal))
                {
                    var normalizedCdClassFiscal = UtlStrings.RemoveDiacritics(cdClassFiscal.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.CdClassFiscal == null) ? "" : p.CdClassFiscal.ToLower()) == normalizedCdClassFiscal);
                }

                if (!string.IsNullOrEmpty(cest))
                {
                    var normalizedCest = UtlStrings.RemoveDiacritics(cest.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.Cest == null) ? "" : p.Cest.ToLower()) == normalizedCest);
                }

                if (!string.IsNullOrEmpty(cdInterno))
                {
                    var normalizedCdInterno = UtlStrings.RemoveDiacritics(cdInterno.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.CdInterno == null) ? "" : p.CdInterno.ToLower()) == normalizedCdInterno);
                }

                if (!string.IsNullOrEmpty(cdBarra))
                {
                    var normalizedCdBarra = UtlStrings.RemoveDiacritics(cdBarra.ToLower()).Trim();
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.CdBarra == null) ? "" : p.CdBarra.ToLower()) == normalizedCdBarra);
                }

                filteredQuery = filteredQuery.OrderByDescending(p => p.CdProduto);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);

                foreach (var produto in pagedList)
                {
                    double? quantidade = 0;

                    if (cdPlanoEstoque != -1)
                    {
                        var saldo = produto.SaldoEstoques.FirstOrDefault();
                        if (saldo != null)
                        {
                            quantidade = (double?)(saldo.QuantF);
                        }
                    }
                    else
                    {
                        // Sum quantities across all SaldoEstoques
                        foreach (var saldo in produto.SaldoEstoques)
                        {
                            quantidade += (double?)(saldo.QuantF);
                        }
                    }
                    produto.Quantidade = quantidade;
                }

                var response = new PagedResponse<ProdutoEstoque>(pagedList);

                if (response.Items == null || response.Items.Count == 0)
                {
                    return NotFound("Entities not found.");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities.");
                return StatusCode(500, "An error occurred while retrieving entities. Please try again later.");
            }
        }


        [HttpGet("GetProdutosComDetalhes", Name = nameof(GetProdutosComDetalhes))]
        [ProducesResponseType(typeof(GlobalErpData.Dto.PagedList.PagedResponse<ProductDetails>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<GlobalErpData.Dto.PagedList.PagedResponse<ProductDetails>>> GetProdutosComDetalhes(
            int idEmpresa,
            [FromQuery] int? categoryId = null,
            [FromQuery] int? sectionId = null,
            [FromQuery] int? sectionItemId = null,
            [FromQuery] int? featuredId = null,
            [FromQuery] int? cdProduto = null,
            [FromQuery] string? nmProduto = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                string sqlQuery = @"
                    SELECT pe.* FROM produto_estoque pe
                    WHERE pe.id_empresa = @idEmpresa
                    {0}";

                var parametros = new List<NpgsqlParameter>
                {
                    new NpgsqlParameter("idEmpresa", idEmpresa)
                };

                string filtroNomeProduto = "";
                if (!string.IsNullOrEmpty(nmProduto))
                {
                    var termos = nmProduto.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                          .Select(t => $"%{t}%")
                                          .ToList();

                    for (int i = 0; i < termos.Count; i++)
                    {
                        string paramName = $"@termo{i}";
                        filtroNomeProduto += $" AND unaccent(LOWER(pe.nm_produto)) LIKE CONCAT('%',unaccent(LOWER({paramName})),'%')";
                        parametros.Add(new NpgsqlParameter(paramName, termos[i]));
                    }
                }

                sqlQuery = string.Format(sqlQuery, filtroNomeProduto);

                var query = _context.ProdutoEstoques.FromSqlRaw(sqlQuery, parametros.ToArray())
                    .Include(p => p.FotosProdutos)
                    .Where(p => p.Unity == idEmpresa && p.FotosProdutos.Any());

                // Aplicar filtros adicionais via LINQ
                if (categoryId.HasValue)
                {
                    query = query.Where(p => p.Category == categoryId.Value);
                }

                if (cdProduto.HasValue)
                {
                    query = query.Where(p => p.CdProduto == cdProduto.Value);
                }

                if (sectionId.HasValue)
                {
                    query = query.Where(p => p.SectionId == sectionId.Value);
                }

                if (sectionItemId.HasValue)
                {
                    query = query.Where(p => p.SectionItemId == sectionItemId.Value);
                }

                if (featuredId.HasValue)
                {
                    query = query.Where(p => p.FeaturedId == featuredId.Value);
                }

                // Ordenar e paginar
                query = query.OrderBy(p => p.CdProduto);

                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);

                if (pagedList == null || pagedList.Count == 0)
                {
                    return NotFound("Nenhum produto encontrado.");
                }

                // Montar os DTOs para os produtos
                var dtoList = pagedList.Select(p => new ProductDetails
                {
                    id = p.CdProduto,
                    idGlobal = p.CdProdutoErp,
                    name = UtlStrings.FormatProductName(p.NmProduto),
                    color = string.Empty,
                    href = "#",
                    imageSrc = GetImageUrl(p.FotosProdutos.FirstOrDefault()?.CaminhoFoto) ?? string.Empty,
                    imageAlt = p.FotosProdutos.FirstOrDefault()?.DescricaoFoto ?? "Imagem do produto",
                    price = p.VlAVista?.ToString("C2", new CultureInfo("pt-BR")) ?? "R$0,00",
                    priceNumber = p.VlAVista ?? 0,
                    rating = 5,
                    images = p.FotosProdutos.Select(f => new ProductImage
                    {
                        id = f.Id,
                        name = f.DescricaoFoto ?? "Imagem do produto",
                        src = GetImageUrl(f.CaminhoFoto) ?? string.Empty,
                        alt = f.DescricaoFoto ?? "Imagem do produto"
                    }).ToList(),
                    colors = new List<ProductColor>
                    {
                        new ProductColor
                        {
                            name = string.Empty,
                            bgColor = string.Empty,
                            selectedColor = string.Empty
                        }
                    },
                    description = p.DescricaoProduto ?? "Descrição não disponível",
                    details = new List<ProductDetailv2>() { new ProductDetailv2() { name = "Código", items = [p.CdProduto.ToString()] } }
                }).ToList();

                // Carregar os ProductDetails e ItemDetails de forma separada
                var produtoIds = pagedList.Select(p => p.CdProduto).ToList();
                var productDetails = await _context.ProductDetails
                    .Where(pd => produtoIds.Contains(pd.IdProduto) && pd.Unity == idEmpresa)
                    .ToListAsync();
                var idOfProductDetails = productDetails.Select(pd => pd.Id).ToList();

                var itemDetails = await _context.ItemDetails
                    .Where(id => idOfProductDetails.Contains(id.IdProductDetails) && id.IdEmpresa == idEmpresa)
                    .ToListAsync();

                // Atribuir os detalhes aos produtos correspondentes
                foreach (var dto in dtoList)
                {
                    var productDetail = productDetails.Where(pd => pd.IdProduto == dto.id && pd.Unity == idEmpresa).ToList();
                    foreach (var pd in productDetail)
                    {
                        var details = new ProductDetailv2
                        {
                            name = pd.Name,
                            items = itemDetails.Where(id => id.IdProductDetails == pd.Id)
                                               .Select(id => id.Value)
                                               .ToList()
                        };

                        dto.details.Add(details);
                    }
                }

                var response = new GlobalErpData.Dto.PagedList.PagedResponse<ProductDetails>
                {
                    Items = dtoList,
                    PageNumber = pagedList.PageNumber,
                    PageSize = pagedList.PageSize,
                    TotalItemCount = pagedList.TotalItemCount,
                    PageCount = pagedList.PageCount,
                    HasNextPage = pagedList.HasNextPage,
                    HasPreviousPage = pagedList.HasPreviousPage
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao recuperar os produtos: {ex.Message}");
            }
        }

        private string GetImageUrl(string? imagePath)
        {
            if (imagePath is null)
            {
                return string.Empty;
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host.Value}";
            var imageUrl = imagePath.Replace("\\", "/");
            return $"{baseUrl}/{imageUrl}";
        }

        private static string NormalizeSearchTerm(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Remove special characters and convert to lowercase
            var normalized = Regex.Replace(input.ToLowerInvariant(), @"[^a-z0-9\s]", "");

            // Remove extra spaces
            normalized = Regex.Replace(normalized, @"\s+", " ").Trim();

            return normalized;
        }

        private static string FormatProductName(string nmProduto)
        {
            if (string.IsNullOrEmpty(nmProduto))
                return nmProduto;

            var formattedName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nmProduto.ToLower());
            return formattedName;
        }

        [HttpPut("AtualizarCusto/{idEmpresa}/{nrEntrada}")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Success>> AtualizarCusto(int idEmpresa, int nrEntrada, [FromBody] AttCustoDtoList dto)
        {
            if (dto == null || dto.Itens == null || dto.Itens.Count == 0)
            {
                return BadRequest(new BadRequest("No items to update."));
            }
            if (idEmpresa == 0 || nrEntrada == 0)
            {
                return BadRequest(new BadRequest("Invalid parameters."));
            }

            try
            {
                var entrada = await _context.Entradas
                        .Where((item) => item.Nr == nrEntrada && item.CdEmpresa == idEmpresa)
                        .FirstOrDefaultAsync();
                if (entrada == null)
                {
                    return NotFound(new NotFound("Product entry not found."));
                }

                foreach (var item in dto.Itens)
                {
                    var produto = await
                        _context.ProdutoEstoques
                        .Where((produto) => produto.CdProduto == item.cdProduto && produto.Unity == entrada.Unity)
                        .FirstOrDefaultAsync();

                    if (produto == null)
                    {
                        return NotFound(new NotFound($"Product {item.cdProduto} not found."));
                    }

                    produto.VlCusto = item.custo;

                    _context.Update(produto);
                    int affected = await _context.SaveChangesAsync();
                    if (affected == 1)
                    {
                        logger.LogInformation("Entity updated with ID: {idEmpresa}-{idCadastro}", idEmpresa, produto.CdProduto);
                    }
                    else
                    {
                        logger.LogWarning("Failed to update entity with ID: {idEmpresa}-{idCadastro}", idEmpresa, produto.CdProduto);
                        return BadRequest(new BadRequest($"Error updating product {item.cdProduto}; idEmpresa {item.idEmpresa}."));

                    }

                    var oldProdutoEntrada = await
                        _context.ProdutoEntrada
                        .Where((produtoEntrada) => produtoEntrada.Nr == item.Item.Nr && produtoEntrada.CdEmpresa == idEmpresa)
                        .FirstOrDefaultAsync();
                    if (oldProdutoEntrada == null)
                    {
                        return NotFound(new NotFound($"Product entry {item.Item.Nr} not found."));
                    }

                    oldProdutoEntrada.CustoAtualizado = item.Item.CustoAtualizado;

                    _context.Update(oldProdutoEntrada);

                    int affectedProdutoEntrada = await
                        _context.SaveChangesAsync();
                    if (affectedProdutoEntrada == 1)
                    {
                        logger.LogInformation("Entity updated with ID: {idEmpresa}-{idCadastro}", idEmpresa, oldProdutoEntrada.Nr);
                    }
                    else
                    {
                        return BadRequest(new BadRequest($"Error updating product entry {item.Item.Nr}."));
                    }
                }

                return Ok(new Success("Costs updated successfully."));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating costs.");
                return StatusCode(500, new InternalServerError("An error occurred while updating costs. Please try again later."));
            }
        }

        [HttpPut("AtualizarPrecoPorLucroBruto/{idEmpresa}/{nrEntrada}")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Success>> AtualizarPrecoPorLucroBruto(
                int idEmpresa, int nrEntrada, [FromBody] AttPrecoDtoList dto)
        {
            if (dto == null || dto.Itens == null || dto.Itens.Count == 0)
            {
                return BadRequest(new BadRequest("No items to update."));
            }
            if (idEmpresa == 0 || nrEntrada == 0)
            {
                return BadRequest(new BadRequest("Invalid parameters."));
            }

            try
            {
                var entrada = await _context.Entradas
                        .Where((item) => item.Nr == nrEntrada && item.CdEmpresa == idEmpresa)
                        .FirstOrDefaultAsync();
                if (entrada == null)
                {
                    return NotFound(new NotFound("Product entry not found."));
                }

                foreach (var item in dto.Itens)
                {
                    var produto = await
                        _context.ProdutoEstoques
                        .Where((produto) => produto.CdProduto == item.cdProduto && produto.Unity == entrada.Unity)
                        .FirstOrDefaultAsync();

                    if (produto == null)
                    {
                        return NotFound(new NotFound($"Product {item.cdProduto} not found."));
                    }

                    produto.LucroPor = item.lucroPor;
                    produto.PercentualComissao = 0;
                    produto.PercentualCustoFixo = 0;
                    produto.PercentualImpostos = 0;
                    decimal preco = Math.Round(((item.lucroPor) / 100 + 1) * (produto.VlCusto ?? 0), 2);
                    decimal percentualLucroLiquido = 0;
                    if (preco > 0)
                    {
                        percentualLucroLiquido = 100 * (1 - (produto.VlCusto ?? 0) / (preco));
                        produto.PercentualLucroLiquidoFiscal = percentualLucroLiquido;
                        produto.IndiceMarkupFiscal = (1 - percentualLucroLiquido / 100);
                        produto.VlAVista = preco;
                    }

                    _context.Update(produto);
                    int affected = await _context.SaveChangesAsync();
                    if (affected == 1)
                    {
                        logger.LogInformation("Entity updated with ID: {idEmpresa}-{idCadastro}", idEmpresa, produto.CdProduto);
                    }
                    else
                    {
                        logger.LogWarning("Failed to update entity with ID: {idEmpresa}-{idCadastro}", idEmpresa, produto.CdProduto);
                        return BadRequest(new BadRequest($"Error updating product {item.cdProduto}; idEmpresa {item.idEmpresa}."));

                    }

                    var oldProdutoEntrada = await
                        _context.ProdutoEntrada
                        .Where((produtoEntrada) => produtoEntrada.Nr == item.Item.Nr && produtoEntrada.CdEmpresa == idEmpresa)
                        .FirstOrDefaultAsync();
                    if (oldProdutoEntrada == null)
                    {
                        return NotFound(new NotFound($"Product entry {item.Item.Nr} not found."));
                    }

                    oldProdutoEntrada.PrecoAtualizado = preco;

                    _context.Update(oldProdutoEntrada);

                    int affectedProdutoEntrada = await
                        _context.SaveChangesAsync();
                    if (affectedProdutoEntrada == 1)
                    {
                        logger.LogInformation("Entity updated with ID: {idEmpresa}-{idCadastro}", idEmpresa, oldProdutoEntrada.Nr);
                    }
                    else
                    {
                        return BadRequest(new BadRequest($"Error updating product entry {item.Item.Nr}."));
                    }
                }

                return Ok(new Success("Costs updated successfully."));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating costs.");
                return StatusCode(500, new InternalServerError("An error occurred while updating costs. Please try again later."));
            }
        }

        [HttpPut("AtualizarPrecoPorLucroLiquido/{idEmpresa}/{nrEntrada}")]
        [ProducesResponseType(typeof(Success), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Success>> AtualizarPrecoPorLucroLiquido(
                int idEmpresa, int nrEntrada, [FromBody] AttPrecoMarkupDtoList dto)
        {
            if (dto == null || dto.Itens == null || dto.Itens.Count == 0)
            {
                return BadRequest(new BadRequest("No items to update."));
            }
            if (idEmpresa == 0 || nrEntrada == 0)
            {
                return BadRequest(new BadRequest("Invalid parameters."));
            }

            try
            {
                var entrada = await _context.Entradas
                        .Where((item) => item.Nr == nrEntrada && item.CdEmpresa == idEmpresa)
                        .FirstOrDefaultAsync();
                if (entrada == null)
                {
                    return NotFound(new NotFound("Product entry not found."));
                }

                foreach (var item in dto.Itens)
                {
                    var produto = await
                        _context.ProdutoEstoques
                        .Where((produto) => produto.CdProduto == item.cdProduto && produto.Unity == entrada.Unity)
                        .FirstOrDefaultAsync();

                    if (produto == null)
                    {
                        return NotFound(new NotFound($"Product {item.cdProduto} not found."));
                    }

                    produto.PercentualLucroLiquidoFiscal = item.percentualLiquido;
                    produto.PercentualComissao = item.percentualComissao;
                    produto.PercentualImpostos = item.percentualImpostos;
                    produto.PercentualCustoFixo = item.percentualCustoFixo;

                    decimal indiceMarkup = 1 - (item.percentualLiquido + item.percentualImpostos + item.percentualComissao + item.percentualCustoFixo) / 100;
                    decimal preco = 0;
                    if (indiceMarkup > 0)
                    {
                        preco = Math.Round((produto.VlCusto ?? 0) / indiceMarkup, 2);

                        produto.IndiceMarkupFiscal = indiceMarkup;
                        produto.VlAVista = preco;
                    }
                    _context.Update(produto);
                    int affected = await _context.SaveChangesAsync();
                    if (affected == 1)
                    {
                        logger.LogInformation("Entity updated with ID: {idEmpresa}-{idCadastro}", idEmpresa, produto.CdProduto);
                    }
                    else
                    {
                        logger.LogWarning("Failed to update entity with ID: {idEmpresa}-{idCadastro}", idEmpresa, produto.CdProduto);
                        return BadRequest(new BadRequest($"Error updating product {item.cdProduto}; idEmpresa {item.idEmpresa}."));

                    }

                    var oldProdutoEntrada = await
                        _context.ProdutoEntrada
                        .Where((produtoEntrada) => produtoEntrada.Nr == item.Item.Nr && produtoEntrada.CdEmpresa == idEmpresa)
                        .FirstOrDefaultAsync();
                    if (oldProdutoEntrada == null)
                    {
                        return NotFound(new NotFound($"Product entry {item.Item.Nr} not found."));
                    }

                    oldProdutoEntrada.PrecoAtualizado = preco;

                    _context.Update(oldProdutoEntrada);

                    int affectedProdutoEntrada = await
                        _context.SaveChangesAsync();
                    if (affectedProdutoEntrada == 1)
                    {
                        logger.LogInformation("Entity updated with ID: {idEmpresa}-{idCadastro}", idEmpresa, oldProdutoEntrada.Nr);
                    }
                    else
                    {
                        return BadRequest(new BadRequest($"Error updating product entry {item.Item.Nr}."));
                    }
                }

                return Ok(new Success("Costs updated successfully."));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating costs.");
                return StatusCode(500, new InternalServerError("An error occurred while updating costs. Please try again later."));
            }
        }
    }
}
