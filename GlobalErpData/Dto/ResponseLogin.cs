using GlobalErpData.Models;

namespace GlobalErpData.Dto
{
    public class Token
    {
        public ResponseLogin token { get; set; }
    }
    public class ResponseLogin
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public DateTime Expiration { get; set; }
        public Usuario Usuario { get; set; }
        public IEnumerable<UsuarioPermissao> PermissoesUsuario { get; set; }
        public IEnumerable<Permissao> Permissoes { get; set; }

        public IEnumerable<Empresa> Empresas { get; set; }

        public Unity Unity { get; set; }


    }
}
