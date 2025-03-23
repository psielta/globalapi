using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CfopCsosnV2Controller : GenericPagedController<CfopCsosnV2, int, CfopCsosnV2Dto>
    {
        public CfopCsosnV2Controller(IQueryRepository<CfopCsosnV2, int, CfopCsosnV2Dto> repo, ILogger<GenericPagedController<CfopCsosnV2, int, CfopCsosnV2Dto>> logger) : base(repo, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<CfopCsosnV2>), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<PagedResponse<CfopCsosnV2>>> GetEntities([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return await base.GetEntities(pageNumber, pageSize);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CfopCsosnV2), 200)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<CfopCsosnV2>> GetEntity(int id)
        {
            return await base.GetEntity(id);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CfopCsosnV2), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<CfopCsosnV2>> Create([FromBody] CfopCsosnV2Dto dto)
        {
            return await base.Create(dto);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(typeof(IEnumerable<CfopCsosnV2>), 201)]
        [ProducesResponseType(400)]
        public override async Task<ActionResult<IEnumerable<CfopCsosnV2>>> CreateBulk([FromBody] IEnumerable<CfopCsosnV2Dto> dtos)
        {
            return await base.CreateBulk(dtos);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(CfopCsosnV2), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public override async Task<ActionResult<CfopCsosnV2>> Update(int id, [FromBody] CfopCsosnV2Dto dto)
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

        // Métodos personalizados ajustados

        [HttpGet("GetCfopCsosnV2PorUnity", Name = nameof(GetCfopCsosnV2PorUnity))]
        [ProducesResponseType(typeof(PagedResponse<CfopCsosnV2>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PagedResponse<CfopCsosnV2>>> GetCfopCsosnV2PorUnity(
            int unity,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var entitysFilterByEmpresa = await ((CfopCsosnV2Repository)repo).GetCfopCsosnV2AsyncPorUnity(unity);

                var filteredQuery = entitysFilterByEmpresa.OrderByDescending(p => p.Id);

                var pagedList = filteredQuery.ToPagedList(pageNumber, pageSize);
                var response = new PagedResponse<CfopCsosnV2>(pagedList);

                if (response.Items == null || response.Items.Count == 0)
                {
                    return NotFound("Entities not found."); // 404 Resource not found
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities.");
                return StatusCode(500, "An error occurred while retrieving entities. Please try again later.");
            }
        }

            [HttpGet("GetAllCfopCsosnV2PorEmpresa/{idEmpresa}", Name = nameof(GetAllCfopCsosnV2PorEmpresa))]
        [ProducesResponseType(typeof(IEnumerable<CfopCsosnV2>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<CfopCsosnV2>>> GetAllCfopCsosnV2PorEmpresa(int idEmpresa)
        {
            try
            {
                var entitysFilterByEmpresa = await ((CfopCsosnV2Repository)repo).GetCfopCsosnV2AsyncPorUnity(idEmpresa);

                if (entitysFilterByEmpresa == null)
                {
                    return NotFound("Entities not found.");
                }
                if (entitysFilterByEmpresa.Count() == 0)
                {
                    return NotFound("Entities not found.");
                }
                return Ok(entitysFilterByEmpresa);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving paged entities.");
                return StatusCode(500, "An error occurred while retrieving entities. Please try again later.");
            }
        }
    }
}
