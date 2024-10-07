using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Database;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;
using GlobalLib.Strings;
using GlobalErpData.GenericControllers;
using GlobalErpData.Repository;
using GlobalErpData.Repository.Repositories;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CidadeController : GenericDtoController<Cidade, string, CidadeDto>
    {
        public CidadeController(IRepositoryDto<Cidade, string, CidadeDto> repo, ILogger<GenericDtoController<Cidade, string, CidadeDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet("GetCidadeByName/{nome}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCidadeByName(string nome)
        {
            var cidades = await (repo as CidadeRepositoryDto).RetrieveAllAsync();
            if (cidades == null)
            {
                return NotFound("Cidade não encontrada.");
            }

            var stringNormalizada = UtlStrings.RemoveDiacritics(nome.ToLower());

            var filter = cidades.Where(c => UtlStrings.RemoveDiacritics(c.NmCidade.ToLower()).StartsWith(stringNormalizada))
                                .OrderBy(c => c.NmCidade)
                                .ToList();

            if (filter.Count == 0)
            {
                return NotFound("Cidade não encontrada.");
            }
            return Ok(filter);
        }

    }
}
