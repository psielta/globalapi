using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    public class IbptController : GenericDtoController<Ibpt, int, IbptDto>
    {
        public IbptController(IRepositoryDto<Ibpt, int, IbptDto> repo, ILogger<GenericDtoController<Ibpt, int, IbptDto>> logger) : base(repo, logger)
        {
        }
    }
}
