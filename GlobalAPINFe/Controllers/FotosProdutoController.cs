using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using System;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FotosProdutoController : GenericPagedControllerMultiKey<FotosProduto, int, int, FotosProdutoDto>
    {
        private readonly IWebHostEnvironment _environment;

        public FotosProdutoController(IQueryRepositoryMultiKey<FotosProduto, int, int, FotosProdutoDto> repo, ILogger<GenericPagedControllerMultiKey<FotosProduto, int, int, FotosProdutoDto>> logger, IWebHostEnvironment environment) : base(repo, logger)
        {
            _environment = environment;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFoto(
           [FromForm] int id,
           [FromForm] int idEmpresa,
           [FromForm] int cdProduto,
           [FromForm] string descricaoFoto,
           [FromForm] IFormFile foto)
        {
            try
            {
                if (foto == null || foto.Length == 0)
                    return BadRequest("Foto não enviada.");

                string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(foto.FileName);

                string folderPath = System.IO.Path.Combine(_environment.WebRootPath, "imagens", idEmpresa.ToString(), cdProduto.ToString());
                Directory.CreateDirectory(folderPath);

                string filePath = System.IO.Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await foto.CopyToAsync(stream);
                }

                string relativePath = System.IO.Path.Combine("imagens", idEmpresa.ToString(), cdProduto.ToString(), fileName);

                var existingFoto = await repo.RetrieveAsync(idEmpresa, id);

                if (existingFoto != null)
                {
                    if (System.IO.File.Exists(System.IO.Path.Combine(_environment.WebRootPath, existingFoto.CaminhoFoto)))
                    {
                        System.IO.File.Delete(System.IO.Path.Combine(_environment.WebRootPath, existingFoto.CaminhoFoto));
                    }

                    existingFoto.CaminhoFoto = relativePath;
                    existingFoto.DescricaoFoto = descricaoFoto;
                    existingFoto.Excluiu = false;

                    FotosProdutoDto fotoProdutoDto = new FotosProdutoDto
                    {
                        Id = existingFoto.Id,
                        IdEmpresa = existingFoto.IdEmpresa,
                        CdProduto = existingFoto.CdProduto,
                        CaminhoFoto = existingFoto.CaminhoFoto,
                        Excluiu = existingFoto.Excluiu,
                        DescricaoFoto = existingFoto.DescricaoFoto
                    };
                    await repo.UpdateAsync(idEmpresa, id, fotoProdutoDto);
                }
                else
                {
                    // Criar uma nova foto
                    FotosProdutoDto fotoProdutoDto = new FotosProdutoDto
                    {
                        Id = id,
                        IdEmpresa = idEmpresa,
                        CdProduto = cdProduto,
                        CaminhoFoto = relativePath,
                        DescricaoFoto = descricaoFoto,
                        Excluiu = false
                    };
                    await repo.CreateAsync(fotoProdutoDto);
                }

                return Ok("Foto enviada com sucesso.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao fazer upload da foto.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteFoto([FromBody] DeleteFotoDto dto)
        {
            try
            {
                if (dto == null || dto.Id <= 0 || dto.IdEmpresa <= 0)
                    return BadRequest("Dados inválidos.");

                var existingFoto = await repo.RetrieveAsync(dto.IdEmpresa, dto.Id);

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

                bool? deleted = await repo.DeleteAsync(dto.IdEmpresa, dto.Id);

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
