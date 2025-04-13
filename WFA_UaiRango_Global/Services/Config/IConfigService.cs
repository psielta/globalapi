using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalErpData.Dto.Uairango;

namespace WFA_UaiRango_Global.Services.Config
{
    public interface IConfigService
    {
        Task<EstabelecimentoConfigDto> ObterEstabelecimentoAsync(string token, int idEstabelecimento);
        
        Task<List<PrazoDto>> ObterPrazosAsync(string token);

        Task<bool> AtualizarEstabelecimentoAsync(string token, int idEstabelecimento, string campo, string valor);
    }
}
