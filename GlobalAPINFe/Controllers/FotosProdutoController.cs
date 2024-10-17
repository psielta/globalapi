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
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

                var existingFoto = await repo.RetrieveAsync(dto.IdEmpresa, dto.Id);

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

                    FotosProdutoDto fotosProdutoDto = new FotosProdutoDto()
                    {
                        Id = existingFoto.Id,
                        IdEmpresa = existingFoto.IdEmpresa,
                        CdProduto = existingFoto.CdProduto,
                        CaminhoFoto = existingFoto.CaminhoFoto,
                        DescricaoFoto = existingFoto.DescricaoFoto,
                        Excluiu = existingFoto.Excluiu
                    };

                    await repo.UpdateAsync(dto.IdEmpresa, dto.Id, fotosProdutoDto);
                }
                else
                {
                    FotosProdutoDto novaFoto = new FotosProdutoDto
                    {
                        Id = dto.Id,
                        IdEmpresa = dto.IdEmpresa,
                        CdProduto = dto.CdProduto,
                        CaminhoFoto = relativePath,
                        DescricaoFoto = dto.DescricaoFoto,
                        Excluiu = false
                    };
                    await repo.CreateAsync(novaFoto);
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
        public async Task<IActionResult> DeleteFoto(int idEmpresa, int id)
        {
            try
            {
                if (dto == null || dto.Id <= 0 || dto.IdEmpresa <= 0)
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
