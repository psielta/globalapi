using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    public class ImpcabnfeController : GenericDtoController<Impcabnfe, string, ImpcabnfeDto>
    {
        public ImpcabnfeController(IRepositoryDto<Impcabnfe, string, ImpcabnfeDto> repo, ILogger<GenericDtoController<Impcabnfe, string, ImpcabnfeDto>> logger) : base(repo, logger)
        {
        }
    }
}
