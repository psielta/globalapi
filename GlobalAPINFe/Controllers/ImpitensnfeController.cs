using GlobalErpData.Dto;
using GlobalErpData.GenericControllers;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using Microsoft.AspNetCore.Mvc;

namespace GlobalAPINFe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImpitensnfeController : GenericPagedControllerMultiKey<Impitensnfe, string, string, ImpitensnfeDto>
    {
        public ImpitensnfeController(IQueryRepositoryMultiKey<Impitensnfe, string, string, ImpitensnfeDto> repo, ILogger<GenericPagedControllerMultiKey<Impitensnfe, string, string, ImpitensnfeDto>> logger) : base(repo, logger)
        {
        }
    }
}
