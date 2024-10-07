using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalLib.Database;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissaoController : GenericDtoController<Permissao, int, PermissaoDto>
    {
        public PermissaoController(IRepositoryDto<Permissao, int, PermissaoDto> repo, ILogger<GenericDtoController<Permissao, int, PermissaoDto>> logger) : base(repo, logger)
        {
        }
    }
}
