using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalErpData.Dto.Uairango;

namespace WFA_UaiRango_Global.Services.Categoria
{
    public interface ICategoriaService
    {
        Task<CategoriaDto?> ObterCategoriaAsync(string token, int idEstabelecimento, int idCategoria);
        Task<List<CategoriaDto>?> ObterCategoriasAsync(string token, int idEstabelecimento);

        Task<bool> AlterarCategoriaAsync(string token, int idEstabelecimento, int idCategoria, CategoriaAlterarDto dados);
        Task<bool> AlterarStatusCategoriaAsync(string token, int idEstabelecimento, int idCategoria, int status);

        Task<int?> CriarCategoriaAsync(string token, int idEstabelecimento, CategoriaNovoDto dados);
    }
}
