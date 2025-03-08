using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositoriesMultiKey;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GlobalLib.Dto;
using GlobalErpData.Data;
using Microsoft.EntityFrameworkCore;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeaturedController : GlobalLib.GenericControllers.GenericPagedControllerMultiKey<Featured, int, int, FeaturedDto>
    {
        private readonly IWebHostEnvironment _environment;
        private readonly GlobalErpFiscalBaseContext _context;

        public FeaturedController(IQueryRepositoryMultiKey<Featured, int, int, FeaturedDto> repo, ILogger<GlobalLib.GenericControllers.GenericPagedControllerMultiKey<Featured, int, int, FeaturedDto>> logger, IWebHostEnvironment environment, GlobalErpFiscalBaseContext _context) : base(repo, logger)
        {
            _environment = environment;
            this._context = _context;
        }

        // Sobrescrevendo os métodos herdados e adicionando os atributos [ProducesResponseType]

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<Featured>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<Featured>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(Featured), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Featured>> GetEntity(int idEmpresa, int idCadastro)
        {
            return await base.GetEntity(idEmpresa, idCadastro);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Featured), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<Featured>> Create([FromBody] FeaturedDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{idEmpresa}/{idCadastro}")]
        [ProducesResponseType(typeof(Featured), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<Featured>> Update(int idEmpresa, int idCadastro, [FromBody] FeaturedDto dto)
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

        // Método personalizado ajustado
        [HttpGet("/api/FeaturedsPorEmpresa/{id}", Name = nameof(GetFeaturedsPorEmpresa))]
        [ProducesResponseType(typeof(IEnumerable<Featured>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Featured>>> GetFeaturedsPorEmpresa(int id)
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
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
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

                var existingFoto = await _context.Featureds.Where(f => f.IdEmpresa == dto.IdEmpresa && f.Id == dto.Id).FirstOrDefaultAsync();

                //await repo.RetrieveAsync(dto.IdEmpresa, dto.Id);

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
                    existingFoto.Name = dto.Name;
                    existingFoto.Excluiu = false;

                    //await repo.UpdateAsync(dto.IdEmpresa, dto.Id, uploadedFeaturedDto);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    Featured uploadedFeaturedDto = new Featured()
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
                    //await repo.CreateAsync(uploadedFeaturedDto);
                    _context.Featureds.Add(uploadedFeaturedDto);
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
                //if (string.IsNullOrEmpty(existingFoto.ImageSrc))
                //{
                //return BadRequest("Foto não encontrada.");
                //}
                if (!string.IsNullOrEmpty(existingFoto.ImageSrc))
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
