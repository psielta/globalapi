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
    public class EmpresaController : GenericDtoController<Empresa, int, EmpresaDto>
    {
        public EmpresaController(IRepositoryDto<Empresa, int, EmpresaDto> repo, ILogger<GenericDtoController<Empresa, int, EmpresaDto>> logger) : base(repo, logger)
        {
        }
    }
}
