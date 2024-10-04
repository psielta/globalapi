using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.Repositories;
using GlobalLib.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace GlobalAPIDominio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IRepositoryDto<Usuario, string, UsuarioDto> repositoryUsuario;
        private readonly IRepositoryDto<Empresa, int, EmpresaDto> repositoryEmpresa;
        private readonly IRepositoryDto<Permissao, int, PermissaoDto> repositoryPermissao;
        private readonly IRepositoryDto<UsuarioPermissao, int, UsuarioPermissaoDto> repositoryUsuarioPermissao;

        public AuthController(IConfiguration configuration,
            IRepositoryDto<Usuario, string, UsuarioDto> repositoryUsuario,
            IRepositoryDto<Empresa, int, EmpresaDto> repositoryEmpresa,
            IRepositoryDto<Permissao, int, PermissaoDto> repositoryPermissao,
            IRepositoryDto<UsuarioPermissao, int, UsuarioPermissaoDto> repositoryUsuarioPermissao)
        {
            _configuration = configuration;
            this.repositoryUsuario = repositoryUsuario;
            this.repositoryEmpresa = repositoryEmpresa;
            this.repositoryPermissao = repositoryPermissao;
            this.repositoryUsuarioPermissao = repositoryUsuarioPermissao;
        }

        [HttpPost("login")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Invalid client request");
            }

            IEnumerable<Usuario> usuarios = await repositoryUsuario.RetrieveAllAsync();

            if (usuarios == null || usuarios.Count() == 0)
            {
                return NotFound();
            }

            Usuario usuario = usuarios.FirstOrDefault(u => u.NmUsuario == user.Username && u.CdSenha == user.Password, null);
            if (usuario == null)
            {
                return NotFound();
            }
            if (!usuario.Ativo.Equals("S"))
            {
                return Unauthorized();
            }

            Empresa empresa = await repositoryEmpresa.RetrieveAsync(usuario.CdEmpresa);
            if (empresa == null)
            {
                return NotFound();
            }

            /*if (!empresa.Ativa)
            {
                return Unauthorized();
            }*/
            var permissoes = await repositoryPermissao.RetrieveAllAsync();
            var permissoesUsuario = await ((UsuarioPermissaoRepositoryDto)repositoryUsuarioPermissao).RetrieveAllAsyncPerUser(usuario.NmUsuario);


            var token = GenerateJwtToken(usuario, empresa, permissoes, permissoesUsuario);
            return Ok(new { token });
        }

        private ResponseLogin GenerateJwtToken(Usuario usuario, Empresa empresa,
            IEnumerable<Permissao> permissoes,
            IEnumerable<UsuarioPermissao> permissoesUsuario
            )
        {
            var y = _configuration["Jwt:Issuer"];
            var x = _configuration["Jwt:Audience"];
            var z = _configuration["Jwt:Key"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expiracao = DateTime.Now.AddHours(24); // Increase expiration time to 24 hours
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                null,
                expires: expiracao,
                signingCredentials: credentials);

            string jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return new ResponseLogin
            {
                Token = jwt,
                Empresa = empresa,
                Usuario = usuario,
                Expiration = expiracao,
                Username = usuario.NmUsuario,
                Permissoes = permissoes,
                PermissoesUsuario = permissoesUsuario

            };
        }
    }

}
