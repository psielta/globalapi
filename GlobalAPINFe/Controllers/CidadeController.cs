using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalErpData.GenericControllers;
using GlobalErpData.Repository;
using GlobalErpData.Repository.Repositories;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CidadeController : GenericDtoController<Cidade, string, CidadeDto>
    {
        public CidadeController(IRepositoryDto<Cidade, string, CidadeDto> repo, ILogger<GenericDtoController<Cidade, string, CidadeDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Cidade>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<IEnumerable<Cidade>>> GetEntities()
        {
            return await base.GetEntities();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Cidade), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Cidade>> GetEntity(string id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Cidade), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Cidade>> Create([FromBody] CidadeDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Cidade), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Cidade>> Update(string id, [FromBody] CidadeDto dto)
        {
            return await base.Update(id, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(string id)
        {
            return await base.Delete(id);
        }

        [HttpGet("GetCidadeByName/{nome}")]
        [ProducesResponseType(typeof(IEnumerable<Cidade>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Cidade>>> GetCidadeByName(string nome)
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

        [HttpGet("GetCidadeByIBGE/{ibge}")]
        [ProducesResponseType(typeof(IEnumerable<Cidade>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Cidade>>> GetCidadeByIBGE(string ibge)
        {
            var cidades = await (repo as CidadeRepositoryDto).RetrieveAllAsync();
            if (cidades == null)
            {
                return NotFound("Cidade não encontrada.");
            }

            var filter = cidades.Where(c => c.CdCidade.Equals(ibge))
                                .OrderBy(c => c.CdCidade)
                                .ToList();

            if (filter.Count == 0)
            {
                return NotFound("Cidade não encontrada.");
            }
            return Ok(filter);
        }
    }
}
