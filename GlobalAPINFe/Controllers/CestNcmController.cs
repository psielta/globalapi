using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    public class CestNcmController : GenericDtoController<CestNcm, int, CestNcmDto>
    {
        public CestNcmController(IRepositoryDto<CestNcm, int, CestNcmDto> repo, ILogger<GenericDtoController<CestNcm, int, CestNcmDto>> logger) : base(repo, logger)
        {
        }
    }
}
