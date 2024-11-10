using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalErpData.Repository;
using GlobalErpData.Repository.Repositories;
using GlobalErpData.Services;
using GlobalLib.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GlobalLib.Repository;
using GlobalLib.GenericControllers;


namespace GlobalAPIDominio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IRepositoryDto<Empresa, int, EmpresaDto> repositoryEmpresa;
        private readonly IRepositoryDto<Permissao, int, PermissaoDto> repositoryPermissao;
        private readonly IRepositoryDto<UsuarioPermissao, int, UsuarioPermissaoDto> repositoryUsuarioPermissao;
        private readonly EmailService _emailService;

        public AuthController(
            IConfiguration configuration,
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager,
            IRepositoryDto<Empresa, int, EmpresaDto> repositoryEmpresa,
            IRepositoryDto<Permissao, int, PermissaoDto> repositoryPermissao,
            IRepositoryDto<UsuarioPermissao, int, UsuarioPermissaoDto> repositoryUsuarioPermissao,
            EmailService emailService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            this.repositoryEmpresa = repositoryEmpresa;
            this.repositoryPermissao = repositoryPermissao;
            this.repositoryUsuarioPermissao = repositoryUsuarioPermissao;
            _emailService = emailService;
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

            // Buscar o usuário pelo e-mail ou nome de usuário
            var usuario = await _userManager.FindByEmailAsync(user.Username) ?? await _userManager.FindByNameAsync(user.Username);
            if (usuario == null)
            {
                return NotFound();
            }

            // Verificar se o usuário está ativo
            if (usuario.Ativo != "S")
            {
                return Unauthorized();
            }

            // Verificar a senha
            var result = await _signInManager.CheckPasswordSignInAsync(usuario, user.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized();
            }

            // Atualizar a senha se necessário
            if (usuario.NeedPasswordHashUpdate)
            {
                var updateResult = await _userManager.UpdateAsync(usuario);
                if (!updateResult.Succeeded)
                {
                    return StatusCode(500, "Erro ao atualizar a senha do usuário.");
                }
                usuario.NeedPasswordHashUpdate = false;
            }

            // Buscar empresa
            var empresa = await repositoryEmpresa.RetrieveAsync(usuario.CdEmpresa);
            if (empresa == null)
            {
                return NotFound();
            }

            // Buscar permissões
            var permissoes = await repositoryPermissao.RetrieveAllAsync();
            var permissoesUsuario = await ((UsuarioPermissaoRepositoryDto)repositoryUsuarioPermissao).RetrieveAllAsyncPerUser(usuario.NmUsuario);

            var token = GenerateJwtToken(usuario, empresa, permissoes, permissoesUsuario);
            return Ok(new { token });
        }


        private ResponseLogin GenerateJwtToken(
            Usuario usuario,
            Empresa empresa,
            IEnumerable<Permissao> permissoes,
            IEnumerable<UsuarioPermissao> permissoesUsuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.NmUsuario),
                new Claim(ClaimTypes.Email, usuario.Email ?? string.Empty)
            };

            // Adicionar outras claims se necessário

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiracao = DateTime.Now.AddHours(24);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiracao,
                signingCredentials: creds);

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

        [HttpPost("forgot-password")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email))
            {
                return BadRequest("Requisição inválida");
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return NotFound();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = $"{_configuration["FrontendUrl"]}/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email)}";

            var placeholders = new Dictionary<string, string>
            {
                { "Nome", user.NmPessoa },
                { "Subject", "Redefinição de Senha" },
                { "Body", "Por favor, clique no link abaixo para redefinir sua senha." },
                { "Link", callbackUrl }
            };

            var templatePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Templates/Email/EnvioBaseV1.html");

            await _emailService.SendTemplatedEmailAsync(
                user.Email,
                "Redefinição de Senha",
                templatePath,
                placeholders
            );

            return Ok("E-mail de redefinição de senha enviado.");
        }

        [HttpPost("reset-password")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Token) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Requisição inválida");
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("Erro ao redefinir a senha.");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return Ok("Senha redefinida com sucesso.");
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }
        }
    }
}
