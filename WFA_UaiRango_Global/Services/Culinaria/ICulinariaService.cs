using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalErpData.Dto;
using WFA_UaiRango_Global.Dto;

namespace WFA_UaiRango_Global.Services.Culinaria
{
    public interface ICulinariaService
    {
        Task<List<CulinariaDto>> ObterCulinariasAsync(string token);
    }
}
