using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositoriesMultiKey;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList.Extensions;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedorController : GenericPagedControllerMultiKey<Fornecedor, int, int, FornecedorDto>
    {
        public FornecedorController(IQueryRepositoryMultiKey<Fornecedor, int, int, FornecedorDto> repo, ILogger<GenericPagedControllerMultiKey<Fornecedor, int, int, FornecedorDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<Fornecedor>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<Fornecedor>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(Fornecedor), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Fornecedor>> GetEntity(int idEmpresa, int idCadastro)
        {
            return await base.GetEntity(idEmpresa, idCadastro);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Fornecedor), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Fornecedor>> Create([FromBody] FornecedorDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(Fornecedor), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Fornecedor>> Update(int idEmpresa, int idCadastro, [FromBody] FornecedorDto dto)
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

        [HttpGet("GetFornecedorPorEmpresa", Name = nameof(GetFornecedorPorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<Fornecedor>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<Fornecedor>>> GetFornecedorPorEmpresa(
            int idEmpresa,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? nmForn = null,
            [FromQuery] int? cdForn = null,
            [FromQuery] string? cpf = null,
            [FromQuery] string? cnpj = null,
            [FromQuery] string? ie = null)
        {
            try
            {
                var query = ((FornecedorPagedRepositoryMultiKey)repo).GetFornecedorPorEmpresa(idEmpresa).Result.AsQueryable();

                if (query == null)
                {
                    return NotFound("Entities not found.");
                }

                var filteredQuery = query.AsEnumerable();

                if (!string.IsNullOrEmpty(nmForn))
                {
                    var normalizedNmForn = UtlStrings.RemoveDiacritics(nmForn.ToLower());
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveDiacritics((p.NmForn == null) ? "" : p.NmForn.ToLower()).Contains(normalizedNmForn));
                }

                if (cdForn.HasValue)
                {
                    filteredQuery = filteredQuery.Where(p => p.CdForn == cdForn.Value);
                }

                if (!string.IsNullOrEmpty(cpf))
                {
                    var normalizeCpf = UtlStrings.RemoveSpecialCharacters(UtlStrings.RemoveDiacritics(cpf.ToLower().Trim()));
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveSpecialCharacters(UtlStrings.RemoveDiacritics((p.Cpf == null) ? "" : p.Cpf.ToLower().Trim())) == normalizeCpf);
                }

                if (!string.IsNullOrEmpty(ie))
                {
                    var normalizedIe = UtlStrings.RemoveSpecialCharacters(UtlStrings.RemoveDiacritics(ie.ToLower().Trim()));
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveSpecialCharacters(UtlStrings.RemoveDiacritics((p.NrInscrEstadual == null) ? "" : p.NrInscrEstadual.ToLower().Trim())) == normalizedIe);
                }

                if (!string.IsNullOrEmpty(cnpj))
                {
                    var normalizeCnpj = UtlStrings.RemoveSpecialCharacters(UtlStrings.RemoveDiacritics(cnpj.Trim().ToLower()));
                    filteredQuery = filteredQuery.Where(p => UtlStrings.RemoveSpecialCharacters(UtlStrings.RemoveDiacritics((p.Cnpj == null) ? "" : p.Cnpj.Trim().ToLower())) == normalizeCnpj);
                }

                filteredQuery = filteredQuery.OrderByDescending(p => p.CdForn);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<Fornecedor>(pagedList);

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

        [HttpGet("GetFornecedorByName/{idEmpresa}/{nome}")]
        [ProducesResponseType(typeof(IEnumerable<Fornecedor>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Fornecedor>>> GetFornecedorByName(int idEmpresa, string nome)
        {
            var Fornecedors = await (repo as FornecedorPagedRepositoryMultiKey).GetFornecedorPorEmpresa(idEmpresa);
            IEnumerable<Fornecedor> _Fr = Fornecedors.ToList();
            if (_Fr == null)
            {
                return NotFound("Fornecedor não encontrada.");
            }

            var stringNormalizada = UtlStrings.RemoveDiacritics(nome.ToLower());

            var filter = _Fr.Where(c => UtlStrings.RemoveDiacritics(c.NmForn.ToLower()).StartsWith(stringNormalizada))
                                .OrderBy(c => c.NmForn)
                                .ToList();

            if (filter.Count == 0)
            {
                return NotFound("Fornecedors não encontrada.");
            }
            return Ok(filter);
        }
    }
}
