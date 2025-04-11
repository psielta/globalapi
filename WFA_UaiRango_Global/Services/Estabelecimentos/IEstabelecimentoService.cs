using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalErpData.Uairango.Dto;

namespace WFA_UaiRango_Global.Services.Estabelecimentos
{
    public interface IEstabelecimentoService
    {
        Task<VincularEstabelecimentoResponse> VincularEstabelecimentoAsync(string bearerToken, string tokenEstabelecimento);
        Task<RemoverEstabelecimentoResponse> RemoverEstabelecimentoAsync(string bearerToken, int idEstabelecimento);
        Task<List<Estabelecimento>> ListarEstabelecimentosAsync(string bearerToken);
        Task<CheckVinculoResponse> ChecarVinculoPorTokenAsync(string bearerToken, string tokenEstabelecimento);
        Task<CheckVinculoResponse> ChecarVinculoPorIdAsync(string bearerToken, int idEstabelecimento);
    }
}
