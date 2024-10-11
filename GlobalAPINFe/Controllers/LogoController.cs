using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogoController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<LogoController> _logger;

        public LogoController(IWebHostEnvironment environment, ILogger<LogoController> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        [HttpPost("upload-light")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadLightLogo([FromForm] UploadLogoDto dto)
        {
            return await UploadLogo(dto, "light");
        }

        [HttpPost("upload-dark")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadDarkLogo([FromForm] UploadLogoDto dto)
        {
            return await UploadLogo(dto, "dark");
        }

        private async Task<IActionResult> UploadLogo(UploadLogoDto dto, string mode)
        {
            try
            {
                if (dto.Logo == null || dto.Logo.Length == 0)
                    return BadRequest("Logo não enviado.");

                string folderPath = System.IO.Path.Combine(_environment.WebRootPath, "logos", dto.IdEmpresa.ToString());
                Directory.CreateDirectory(folderPath);

                // Delete existing logo of the same mode
                DeleteExistingLogo(folderPath, mode);

                string extension = System.IO.Path.GetExtension(dto.Logo.FileName).ToLowerInvariant();
                string fileName = $"logo_{mode}{extension}";
                string filePath = System.IO.Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Logo.CopyToAsync(stream);
                }

                return Ok($"Logo {mode} enviado com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao fazer upload do logo {mode}.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        private void DeleteExistingLogo(string folderPath, string mode)
        {
            string[] extensions = { ".png", ".jpg", ".jpeg", ".svg", ".gif" };
            var existingFiles = extensions
                .SelectMany(ext => Directory.GetFiles(folderPath, $"logo_{mode}*{ext}"))
                .ToList();

            foreach (string file in existingFiles)
            {
                try
                {
                    System.IO.File.Delete(file);
                    _logger.LogInformation($"Arquivo de logo existente excluído: {file}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Erro ao excluir o arquivo de logo existente: {file}");
                }
            }
        }

    }
    public class UploadLogoDto
    {
        public IFormFile Logo { get; set; }
        public int IdEmpresa { get; set; }
    }
}
