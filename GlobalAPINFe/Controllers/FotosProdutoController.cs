using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalLib.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
using System;
using GlobalErpData.Data;
using Microsoft.EntityFrameworkCore;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FotosProdutoController : GlobalLib.GenericControllers.GenericPagedControllerMultiKey<FotosProduto, int, int, FotosProdutoDto>
    {
        private readonly GlobalErpFiscalBaseContext _context;
        private readonly IWebHostEnvironment _environment;

        public FotosProdutoController(IQueryRepositoryMultiKey<FotosProduto, int, int, FotosProdutoDto> repo, ILogger<GlobalLib.GenericControllers.GenericPagedControllerMultiKey<FotosProduto, int, int, FotosProdutoDto>> logger, IWebHostEnvironment environment, GlobalErpFiscalBaseContext _context) : base(repo, logger)
        {
            _environment = environment;
            this._context = _context;
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<FotosProduto>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<FotosProduto>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(FotosProduto), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<FotosProduto>> GetEntity(int idEmpresa, int idCadastro)
        {
            return await base.GetEntity(idEmpresa, idCadastro);
        }

        [HttpPost]
        [ProducesResponseType(typeof(FotosProduto), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<FotosProduto>> Create([FromBody] FotosProdutoDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(FotosProduto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<FotosProduto>> Update(int idEmpresa, int idCadastro, [FromBody] FotosProdutoDto dto)
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

        // Métodos personalizados ajustados

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UploadFoto([FromForm] UploadFotoDto dto)
        {
            try
            {
                if (dto.Foto == null || dto.Foto.Length == 0)
                    return BadRequest("Foto não enviada.");

                string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(dto.Foto.FileName);

                string folderPath = System.IO.Path.Combine(_environment.WebRootPath, "imagens", dto.IdEmpresa.ToString(), dto.CdProduto.ToString());
                Directory.CreateDirectory(folderPath);

                string filePath = System.IO.Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Foto.CopyToAsync(stream);
                }

                string relativePath = System.IO.Path.Combine("imagens", dto.IdEmpresa.ToString(), dto.CdProduto.ToString(), fileName);

                var existingFoto = 
                    await this._context.FotosProdutos.Where(f => f.Unity == dto.IdEmpresa && f.Id == dto.Id).FirstOrDefaultAsync();
                //await repo.RetrieveAsync(dto.IdEmpresa, dto.Id);

                if (existingFoto != null)
                {
                    string existingFilePath = System.IO.Path.Combine(_environment.WebRootPath, existingFoto.CaminhoFoto);
                    if (System.IO.File.Exists(existingFilePath))
                    {
                        System.IO.File.Delete(existingFilePath);
                    }

                    existingFoto.CaminhoFoto = relativePath;
                    existingFoto.DescricaoFoto = dto.DescricaoFoto;
                    existingFoto.Excluiu = false;

                    //await repo.UpdateAsync(dto.IdEmpresa, dto.Id, fotosProdutoDto);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    FotosProduto novaFoto = new FotosProduto
                    {
                        Id = dto.Id,
                        Unity = dto.IdEmpresa,
                        CdProduto = dto.CdProduto,
                        CaminhoFoto = relativePath,
                        DescricaoFoto = dto.DescricaoFoto,
                        Excluiu = false
                    };
                    //await repo.CreateAsync(novaFoto);
                    _context.FotosProdutos.Add(novaFoto);
                    await _context.SaveChangesAsync();
                }

                return Ok("Foto enviada com sucesso.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao fazer upload da foto.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpDelete("delete/{idEmpresa}/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteFoto(int idEmpresa, int id)
        {
            try
            {
                if (idEmpresa <= 0 || id <= 0)
                    return BadRequest("Dados inválidos.");

                var existingFoto = await repo.RetrieveAsync(idEmpresa, id);

                if (existingFoto == null)
                {
                    return NotFound("Foto não encontrada.");
                }

                string fullPath = System.IO.Path.Combine(_environment.WebRootPath, existingFoto.CaminhoFoto);
                if (System.IO.File.Exists(fullPath))
                {
                    try
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Erro ao excluir o arquivo físico da foto.");
                    }
                }

                bool? deleted = await repo.DeleteAsync(idEmpresa, id);

                if (deleted.HasValue && deleted.Value)
                {
                    return Ok("Foto excluída com sucesso.");
                }
                else
                {
                    return StatusCode(500, "Erro ao excluir a foto do banco de dados.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao excluir a foto.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }
    }
}
