using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositoriesMultiKey;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeaturedController : GenericPagedControllerMultiKey<Featured, int,int, FeaturedDto>
    {
        private readonly IWebHostEnvironment _environment;
        
        public FeaturedController(IQueryRepositoryMultiKey<Featured, int, int, FeaturedDto> repo, ILogger<GenericPagedControllerMultiKey<Featured, int, int, FeaturedDto>> logger, IWebHostEnvironment environment) : base(repo, logger)
        {
            _environment = environment;
        }

        [HttpGet("/api/FeaturedsPorEmpresa/{id}", Name = nameof(GetFeaturedsPorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetFeaturedsPorEmpresa(int id)
        {
            IEnumerable<Featured>? entities = await ((FeaturedRepository)repo).GetFeaturedByEmpresaAsync(id);
            if (entities == null)
            {
                return NotFound(); // 404 Resource not found
            }
            return Ok(entities); // 200 OK
        }
        
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadFeaturedFoto([FromForm] UploadFeaturedDto dto)
        {
            try
            {
                if (dto.Foto == null || dto.Foto.Length == 0)
                    return BadRequest("Foto não enviada.");

                string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(dto.Foto.FileName);

                string folderPath = System.IO.Path.Combine(_environment.WebRootPath, "featured", dto.IdEmpresa.ToString(), dto.CategoryId.ToString());
                Directory.CreateDirectory(folderPath);

                string filePath = System.IO.Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Foto.CopyToAsync(stream);
                }

                string relativePath = System.IO.Path.Combine("featured", dto.IdEmpresa.ToString(), dto.CategoryId.ToString(), fileName);

                var existingFoto = await repo.RetrieveAsync(dto.IdEmpresa, dto.Id);

                if (existingFoto != null)
                {
                    if (string.IsNullOrEmpty(existingFoto.ImageSrc))
                    { 
                        return BadRequest("Foto não encontrada.");
                    }
                    string existingFilePath = System.IO.Path.Combine(_environment.WebRootPath, existingFoto.ImageSrc);
                    if (System.IO.File.Exists(existingFilePath))
                    {
                        System.IO.File.Delete(existingFilePath);
                    }

                    existingFoto.ImageSrc = relativePath;
                    existingFoto.ImageAlt = dto.ImageAlt;
                    existingFoto.Excluiu = false;

                    FeaturedDto uploadedFeaturedDto = new FeaturedDto()
                    {
                        Id = existingFoto.Id,
                        IdEmpresa = existingFoto.IdEmpresa,
                        CategoryId = existingFoto.CategoryId,
                        ImageSrc = existingFoto.ImageSrc,
                        ImageAlt = existingFoto.ImageAlt,
                        Excluiu = existingFoto.Excluiu,
                        Name = existingFoto.Name,
                        Href = string.Empty
                    };

                    await repo.UpdateAsync(dto.IdEmpresa, dto.Id, uploadedFeaturedDto);
                }
                else
                {
                    FeaturedDto uploadedFeaturedDto = new FeaturedDto()
                    {
                        Id = dto.Id,
                        IdEmpresa = dto.IdEmpresa,
                        ImageSrc = relativePath,
                        ImageAlt = dto.ImageAlt,
                        Excluiu = false,
                        Name = dto.Name,
                        Href = string.Empty,
                        CategoryId = dto.CategoryId
                    };
                    await repo.CreateAsync(uploadedFeaturedDto);
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
                if (idEmpresa <= 0 || id <= 0)
                    return BadRequest("Dados inválidos.");

                var existingFoto = await repo.RetrieveAsync(idEmpresa, id);

                if (existingFoto == null)
                {
                    return NotFound("Foto não encontrada.");
                }
                //if (string.IsNullOrEmpty(existingFoto.ImageSrc))
                //{
                //return BadRequest("Foto não encontrada.");
                //}
                if (! string.IsNullOrEmpty(existingFoto.ImageSrc))
                {
                    string fullPath = System.IO.Path.Combine(_environment.WebRootPath, existingFoto.ImageSrc);
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
