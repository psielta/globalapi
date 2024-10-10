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
using Npgsql;
using System.Globalization;
using System.Text.RegularExpressions;
using X.PagedList;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                await using var _context = dbContextFactory.CreateDbContext();

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
                    .Where(p => p.IdEmpresa == idEmpresa && p.FotosProdutos.Any());

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
                            details = new List<ProductDetailv2>() { new ProductDetailv2() { name="Código", items = [p.CdProduto.ToString()] } } 
                }).ToList();

                // Carregar os ProductDetails e ItemDetails de forma separada
                var produtoIds = pagedList.Select(p => p.CdProduto).ToList();
                var productDetails = await _context.ProductDetails
                    .Where(pd => produtoIds.Contains(pd.IdProduto) && pd.IdEmpresa == idEmpresa)
                    .ToListAsync();
                var idOfProductDetails = productDetails.Select(pd => pd.Id).ToList();

                var itemDetails = await _context.ItemDetails
                    .Where(id => idOfProductDetails.Contains(id.IdProductDetails) && id.IdEmpresa == idEmpresa)
                    .ToListAsync();

                // Atribuir os detalhes aos produtos correspondentes
                foreach (var dto in dtoList)
                {
                    var productDetail = productDetails.Where(pd => pd.IdProduto == dto.id && pd.IdEmpresa == idEmpresa).ToList();
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
    }
}