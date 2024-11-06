using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    public class CsosnController : GenericDtoController<Csosn, string, CsosnDto>
    {
        public CsosnController(IRepositoryDto<Csosn, string, CsosnDto> repo, ILogger<GenericDtoController<Csosn, string, CsosnDto>> logger) : base(repo, logger)
        {
        }
    }
}
