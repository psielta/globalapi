using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImptotalnfeController : GenericDtoController<Imptotalnfe, string, ImptotalnfeDto>
    {
        public ImptotalnfeController(IRepositoryDto<Imptotalnfe, string, ImptotalnfeDto> repo, ILogger<GenericDtoController<Imptotalnfe, string, ImptotalnfeDto>> logger) : base(repo, logger)
        {
        }

    }
}
