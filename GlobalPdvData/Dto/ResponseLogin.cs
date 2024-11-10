using GlobalPdvData.Models;

namespace GlobalPdvData.Dto
{
    public class ResponseLogin
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public DateTime Expiration { get; set; }
        public Empresa Empresa { get; set; }
        public Usuario Usuario { get; set; }
        public IEnumerable<UsuarioPermissao> PermissoesUsuario { get; set; }
        public IEnumerable<Permissao> Permissoes { get; set; }


    }
}
