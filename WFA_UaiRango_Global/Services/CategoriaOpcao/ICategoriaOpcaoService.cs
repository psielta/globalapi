using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalErpData.Dto.Uairango;

namespace WFA_UaiRango_Global.Services.CategoriaOpcao
{
    public interface ICategoriaOpcaoService
    {
        Task<List<CategoriaOpcaoDto>?> ObterOpcoesDaCategoriaAsync(string token, int idEstabelecimento, int idCategoria);

        Task<CategoriaOpcaoDto?> ObterOpcaoAsync(string token, int idEstabelecimento, int idOpcao);

        Task<int?> CriarOpcaoAsync(string token, int idEstabelecimento, int idCategoria, CategoriaOpcaoNovoDto dados);

        Task<bool> AlterarOpcaoAsync(string token, int idEstabelecimento, int idCategoria, int idOpcao, CategoriaOpcaoAlterarDto dados);

        Task<bool> AlterarStatusOpcaoAsync(string token, int idEstabelecimento, int idOpcao, int status);
    }
}
