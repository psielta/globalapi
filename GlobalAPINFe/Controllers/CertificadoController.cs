using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Strings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList.Extensions;
using GlobalLib.Dto;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificadoController : GenericPagedController<Certificado, int, CertificadoDto>
    {
        public CertificadoController(IQueryRepository<Certificado, int, CertificadoDto> repo, ILogger<GenericPagedController<Certificado, int, CertificadoDto>> logger) : base(repo, logger)
        {
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<Certificado>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<Certificado>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Certificado), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Certificado>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Certificado), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Certificado>> Create([FromBody] CertificadoDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Certificado), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Certificado>> Update(int id, [FromBody] CertificadoDto dto)
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
        [ProducesResponseType(typeof(IEnumerable<Certificado>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<Certificado>>> CreateBulk([FromBody] IEnumerable<CertificadoDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }


        // Método personalizado ajustado
        [HttpGet("GetCertificadoPorEmpresa", Name = nameof(GetCertificadoPorEmpresa))]
        [ProducesResponseType(typeof(PagedResponse<Certificado>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<Certificado>>> GetCertificadoPorEmpresa(
            int idEmpresa,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = await ((CertificadoPagedRepository)repo).GetCertificadoPorEmpresa(idEmpresa);

                if (query == null)
                {
                    return NotFound("Entities not found.");
                }

                var filteredQuery = query.AsEnumerable();

                filteredQuery = filteredQuery.OrderByDescending(p => p.Id);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<Certificado>(pagedList);

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
    }
}
