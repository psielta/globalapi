﻿using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using X.PagedList.EF;
using GlobalLib.Dto;
using Microsoft.EntityFrameworkCore;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContaCaixaController : GenericPagedController<ContaDoCaixa, int, ContaCaixaDto>
    {
        public ContaCaixaController(IQueryRepository<ContaDoCaixa, int, ContaCaixaDto> repo, ILogger<GenericPagedController<ContaDoCaixa, int, ContaCaixaDto>> logger) : base(repo, logger)
        {
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<ContaDoCaixa>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<ContaDoCaixa>>> CreateBulk([FromBody] IEnumerable<ContaCaixaDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<ContaDoCaixa>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<ContaDoCaixa>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContaDoCaixa), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ContaDoCaixa>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ContaDoCaixa), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<ContaDoCaixa>> Create([FromBody] ContaCaixaDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ContaDoCaixa), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<ContaDoCaixa>> Update(int id, [FromBody] ContaCaixaDto dto)
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

        // Método personalizado ajustado
        [HttpGet("GetContaDoCaixaPorUnity", Name = nameof(GetContaDoCaixaPorUnity))]
        [ProducesResponseType(typeof(PagedResponse<ContaDoCaixa>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<ContaDoCaixa>>> GetContaDoCaixaPorUnity(int unity, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = await ((ContaCaixaRepository)repo).GetContaDoCaixaAsyncPorUnity(unity);
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                query = query.OrderByDescending(x => x.Id);
                var pagedList = await query.ToPagedListAsync(pageNumber, pageSize);
                var response = new PagedResponse<ContaDoCaixa>(pagedList);

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

        [HttpGet("GetContaDoCaixaPorEmpresa_ALL", Name = nameof(GetContaDoCaixaPorEmpresa_ALL))]
        [ProducesResponseType(typeof(IEnumerable<ContaDoCaixa>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ContaDoCaixa>>> GetContaDoCaixaPorEmpresa_ALL(int idEmpresa)
        {
            try
            {
                var query = ((ContaCaixaRepository)repo).GetContaDoCaixaAsyncPorEmpresa(idEmpresa).Result.AsQueryable();
                if (query == null)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }

                var list = await query.AsNoTracking().ToListAsync();

                if (list == null || list.Count == 0)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }
                return Ok(list); // 200 OK
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities.");
                return StatusCode(500, "An error occurred while retrieving entities. Please try again later.");
            }
        }
    }
}
