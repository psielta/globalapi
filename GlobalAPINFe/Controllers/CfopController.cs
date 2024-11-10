using GlobalErpData.Dto;
using GlobalLib.GenericControllers;
using GlobalLib.Repository;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CfopController : GenericDtoController<Cfop, string, CfopDto>
    {
        public CfopController(IRepositoryDto<Cfop, string, CfopDto> repo, ILogger<GenericDtoController<Cfop, string, CfopDto>> logger) : base(repo, logger)
        {
        }
    }
}
