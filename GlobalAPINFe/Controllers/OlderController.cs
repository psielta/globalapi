using AutoMapper;
using AutoMapper.QueryableExtensions;
using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.PagedRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.EF;
using X.PagedList.Extensions;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OlderController : GenericPagedController<Older, Guid, OlderDto>
    {
        private readonly IMapper _mapper;
        public OlderController(IQueryRepository<Older, Guid, OlderDto> repo, ILogger<GenericPagedController<Older, Guid, OlderDto>> logger, IMapper mapper) : base(repo, logger)
        {
            this._mapper = mapper;
        }

        [HttpGet("GetOlderPorEmpresa", Name = nameof(GetOlderPorEmpresa))]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetOlderPorEmpresa(
    int idEmpresa,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = await ((OlderRepository)repo).GetOlderPorEmpresa(idEmpresa);

                if (query == null)
                {
                    return NotFound("Entities not found.");
                }

                // Inclua os OlderItems na consulta
                query = query.Include(o => o.OlderItems);

                // Projete a consulta para GetOldersDto usando AutoMapper
                var mappedQuery = query
                    .OrderByDescending(p => p.Id)
                    .ProjectTo<GetOldersDto>(_mapper.ConfigurationProvider);

                var pagedList = await mappedQuery.ToPagedListAsync(pageNumber, pageSize);

                if (pagedList == null)
                {
                    return NotFound("Entities not found.");
                }

                var response = new PagedResponse<GetOldersDto>(pagedList);

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities.");
                return StatusCode(500, "An error occurred while retrieving entities. Please try again later.");
            }
        }

    }
}
