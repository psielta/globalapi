using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    public class CstController : GenericDtoController<Cst, string, CstDto>
    {
        public CstController(IRepositoryDto<Cst, string, CstDto> repo, ILogger<GenericDtoController<Cst, string, CstDto>> logger) : base(repo, logger)
        {
        }
    }
}
