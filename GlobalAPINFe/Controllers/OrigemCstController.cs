using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    public class OrigemCstController : GenericDtoController<OrigemCst, string, OrigemCstDto>
    {
        public OrigemCstController(IRepositoryDto<OrigemCst, string, OrigemCstDto> repo, ILogger<GenericDtoController<OrigemCst, string, OrigemCstDto>> logger) : base(repo, logger)
        {
        }
    }
}
