using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositoriesMultiKey;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using X.PagedList;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace GlobalAPINFe.Controllers
{
    public class ProdutoEstoqueController : GenericPagedControllerMultiKey<ProdutoEstoque, int, int, ProdutoEstoqueDto>
    {
        private readonly IDbContextFactory<GlobalErpFiscalBaseContext> dbContextFactory;

        public ProdutoEstoqueController(
            IQueryRepositoryMultiKey<ProdutoEstoque, int, int, ProdutoEstoqueDto> repo,
            ILogger<GenericPagedControllerMultiKey<ProdutoEstoque, int, int, ProdutoEstoqueDto>> logger,
            IDbContextFactory<GlobalErpFiscalBaseContext> context) : base(repo, logger)
        {
            dbContextFactory = context;
        }

        [HttpGet("GetProdutoEstoquePorEmpresa", Name = nameof(GetProdutoEstoquePorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProdutoEstoquePorEmpresa(
            int idEmpresa,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? nmProduto = null,
            [FromQuery] int? cdProduto = null,
            [FromQuery] string? cdClassFiscal = null,
            [FromQuery] string? cest = null,
            [FromQuery] string? cdInterno = null,
            [FromQuery] string? cdBarra = null)
        {
            try
            {
                var query = ((ProdutoEstoquePagedRepositoryMultiKey)repo).GetProdutoEstoqueAsyncPorEmpresa(idEmpresa).Result.AsQueryable();

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

                filteredQuery = filteredQuery.OrderBy(p => p.CdProduto);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
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
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProdutosComDetalhes(
            int idEmpresa,
            [FromQuery] int? cdGrupo = null,
            [FromQuery] int? cdRef = null,
            [FromQuery] int? cdProduto = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                await using var _context = dbContextFactory.CreateDbContext();

                var query = _context.ProdutoEstoques
                    .Include(p => p.FotosProdutos)
                    .Include(p => p.CdGrupoNavigation)
                    .Include(p => p.CdRefNavigation)
                    .Where(p => p.IdEmpresa == idEmpresa && p.FotosProdutos.Any());

                if (cdGrupo.HasValue)
                {
                    query = query.Where(p => p.CdGrupo == cdGrupo.Value);
                }

                if (cdRef.HasValue)
                {
                    query = query.Where(p => p.CdRef == cdRef.Value);
                }

                if (cdProduto.HasValue)
                {
                    query = query.Where(p => p.CdProduto == cdProduto.Value);
                }

                query = query.OrderBy(p => p.CdProduto);

                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);

                if (pagedList == null || pagedList.Count == 0)
                {
                    return NotFound("Nenhum produto encontrado.");
                }

                var dtoList = pagedList.Select(p => new ProductDetails
                {
                    id = p.CdProduto,
                    name = p.NmProduto,
                    color = string.Empty,
                    href = "#",
                    imageSrc = GetImageUrl(p.FotosProdutos.FirstOrDefault()?.CaminhoFoto) ?? string.Empty,
                    imageAlt = p.FotosProdutos.FirstOrDefault()?.DescricaoFoto ?? "Imagem do produto",
                    price = p.VlAVista?.ToString("C2") ?? "R$0,00",
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
                    details = new List<ProductDetail>
                    {
                        new ProductDetail
                        {
                            name = "Detalhes",
                            items = new List<string> { p.CdInterno ?? "Código interno não disponível" }
                        }
                    }
                }).ToList();

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
    }
}