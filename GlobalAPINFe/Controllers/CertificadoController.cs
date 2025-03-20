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
using static HotChocolate.ErrorCodes;
using Microsoft.EntityFrameworkCore;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificadoController : GenericPagedController<Certificado, int, CertificadoDto>
    {
        private readonly IConfiguration _config;
        private readonly GlobalErpFiscalBaseContext _context;

        public CertificadoController(IQueryRepository<Certificado, int, CertificadoDto> repo, ILogger<GenericPagedController<Certificado, int, CertificadoDto>> logger,
            IConfiguration config, GlobalErpFiscalBaseContext _context) : base(repo, logger)
        {
            _config = config;
            this._context = _context;
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
        
        [HttpGet("GetCertificadoPorUnity", Name = nameof(GetCertificadoPorUnity))]
        [ProducesResponseType(typeof(PagedResponse<Certificado>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<Certificado>>> GetCertificadoPorUnity(
            int unity,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = ((CertificadoPagedRepository)repo).GetCertificadoPorUnity(unity);

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

        [HttpPost("upload")]
        [ProducesResponseType(typeof(Certificado), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Certificado>> UploadCertificado([FromForm] UploadCertificadoRequest request)
        {
            if (string.IsNullOrEmpty(_config["Certificado:Path"]))
            {
                throw new Exception("Path não configurado");
            }

            if (!Directory.Exists(_config["Certificado:Path"]))
            {
                Directory.CreateDirectory(_config["Certificado:Path"]);
            }

            if (request.ArquivoPfx == null || request.ArquivoPfx.Length == 0)
            {
                return BadRequest("Nenhum arquivo foi enviado.");
            }

            var certificadoExistente = await _context.Certificados
                .FirstOrDefaultAsync(c => c.IdEmpresa == request.Dados.IdEmpresa);

            var caminhoDestino = System.IO.Path.Combine(_config["Certificado:Path"], $"CERT_EMPRESA_{request.Dados.IdEmpresa}.pfx");
            if (System.IO.File.Exists(caminhoDestino))
            {
                System.IO.File.Delete(caminhoDestino);
            }
            // Salvar o arquivo no sistema de arquivos
            using (var stream = new FileStream(caminhoDestino, FileMode.Create))
            {
                await request.ArquivoPfx.CopyToAsync(stream);
            }

            var empresa = await _context.Empresas.FindAsync(request.Dados.IdEmpresa);
            if (empresa == null) {
                return NotFound("Empresa não encontrada.");
            }

            if (certificadoExistente == null)
            {
                certificadoExistente = new Certificado
                {
                    Unity = empresa.Unity,
                    IdEmpresa = request.Dados.IdEmpresa,
                    CaminhoCertificado = caminhoDestino,
                    SerialCertificado = request.Dados.SerialCertificado,
                    Senha = request.Dados.Senha,
                    ValidadeCert = request.Dados.ValidadeCert,
                    Tipo = request.Dados.Tipo,
                    Certificado1 = request.Dados.Certificado1,
                    CertificadoByte = await System.IO.File.ReadAllBytesAsync(caminhoDestino)
                };

                _context.Certificados.Add(certificadoExistente);
            }
            else
            {
                if (System.IO.File.Exists(certificadoExistente.CaminhoCertificado) && 
                    (!certificadoExistente.CaminhoCertificado.Equals(caminhoDestino)))
                {
                    System.IO.File.Delete(certificadoExistente.CaminhoCertificado);
                }

                certificadoExistente.CaminhoCertificado = caminhoDestino;
                certificadoExistente.SerialCertificado = request.Dados.SerialCertificado;
                certificadoExistente.Senha = request.Dados.Senha;
                certificadoExistente.ValidadeCert = request.Dados.ValidadeCert;
                certificadoExistente.Tipo = request.Dados.Tipo;
                certificadoExistente.Certificado1 = request.Dados.Certificado1;
                certificadoExistente.CertificadoByte = await System.IO.File.ReadAllBytesAsync(caminhoDestino);

                _context.Certificados.Update(certificadoExistente);
            }

            await _context.SaveChangesAsync();
            ((CertificadoPagedRepository)repo).UpdateCache(certificadoExistente.Id, certificadoExistente);
            return Ok(certificadoExistente);
        }

    }
}
