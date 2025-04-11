using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalErpData.Dto;
using GlobalErpData.Uairango.Dto;

namespace WFA_UaiRango_Global.Services.Culinaria
{
    public interface ICulinariaService
    {
        Task<List<CulinariaDto>> ObterCulinariasAsync(string token);
    }
}
