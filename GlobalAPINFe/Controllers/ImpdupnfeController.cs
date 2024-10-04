using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    public class ImpdupnfeController : GenericDtoController<Impdupnfe, string, ImpdupnfeDto>
    {
        public ImpdupnfeController(IRepositoryDto<Impdupnfe, string, ImpdupnfeDto> repo, ILogger<GenericDtoController<Impdupnfe, string, ImpdupnfeDto>> logger) : base(repo, logger)
        {
        }

    }
}
