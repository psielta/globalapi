using AutoMapper;
using AutoMapper.QueryableExtensions;
using GlobalErpData.Data;
using GlobalLib.Dto;
using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrcamentoController : GenericPagedController<OrcamentoCab, Guid, OrcamentoCabDto>
    {
        public OrcamentoController(IQueryRepository<OrcamentoCab, Guid, OrcamentoCabDto> repo, ILogger<GenericPagedController<OrcamentoCab, Guid, OrcamentoCabDto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<OrcamentoCab>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<OrcamentoCab>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrcamentoCab), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<OrcamentoCab>> GetEntity(Guid id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrcamentoCab), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<OrcamentoCab>> Create([FromBody] OrcamentoCabDto dto)
        {
            return await base.Create(dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<OrcamentoCab>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<OrcamentoCab>>> CreateBulk([FromBody] IEnumerable<OrcamentoCabDto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(OrcamentoCab), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<OrcamentoCab>> Update(Guid id, [FromBody] OrcamentoCabDto dto)
        {
            return await base.Update(id, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<IActionResult> Delete(Guid id)
        {
            return await base.Delete(id);
        }
    }
}
