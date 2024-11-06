using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    public class NcmController : GenericDtoController<Ncm, int, NcmDto>
    {
        public NcmController(IRepositoryDto<Ncm, int, NcmDto> repo, ILogger<GenericDtoController<Ncm, int, NcmDto>> logger) : base(repo, logger)
        {
        }
    }
}
