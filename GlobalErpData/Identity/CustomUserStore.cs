using GlobalErpData.Data;
using GlobalErpData.Models;
using Microsoft.AspNetCore.Identity;
using GlobalErpData.Models;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Identity
{
    public class CustomUserStore : IUserStore<Usuario>, IUserPasswordStore<Usuario>, IUserEmailStore<Usuario>
    {
        private readonly GlobalErpFiscalBaseContext _context;

        public CustomUserStore(GlobalErpFiscalBaseContext context)
        {
            _context = context;
        }

        public Task<string> GetUserIdAsync(Usuario user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task SetUserNameAsync(Usuario user, string userName, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.NmUsuario = userName;
            return Task.CompletedTask;
        }

        public Task<string> GetUserNameAsync(Usuario user, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.NmUsuario);
        }


        public Task SetNormalizedUserNameAsync(Usuario user, string normalizedName, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.NmUsuarioNormalized = normalizedName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync(Usuario user, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.NmUsuarioNormalized);
        }


        public void Dispose()
        {
        }

        public Task SetPasswordHashAsync(Usuario user, string passwordHash, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.CdSenha = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(Usuario user, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.CdSenha);
        }

        public Task<bool> HasPasswordAsync(Usuario user, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(!string.IsNullOrEmpty(user.CdSenha));
        }


        public Task SetEmailAsync(Usuario user, string email, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.Email = email;
            return Task.CompletedTask;
        }

        public Task<string> GetEmailAsync(Usuario user, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.Email);
        }


        public Task<bool> GetEmailConfirmedAsync(Usuario user, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(Usuario user, bool confirmed, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }


        public async Task<Usuario> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(normalizedEmail))
                throw new ArgumentNullException(nameof(normalizedEmail));

            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.EmailNormalized == normalizedEmail, cancellationToken);
        }

        public Task<string> GetNormalizedEmailAsync(Usuario user, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.EmailNormalized);
        }

        public Task SetNormalizedEmailAsync(Usuario user, string normalizedEmail, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.EmailNormalized = normalizedEmail;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> CreateAsync(Usuario user, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _context.Add(user);
            await _context.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(Usuario user, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _context.Attach(user);
            _context.Update(user);
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Concurrency failure" });
            }
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(Usuario user, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _context.Remove(user);
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Concurrency failure" });
            }
            return IdentityResult.Success;
        }

        public async Task<Usuario?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            int id = int.Parse(userId);
            return await _context.Usuarios.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<Usuario?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(normalizedUserName))
                throw new ArgumentNullException(nameof(normalizedUserName));

            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NmUsuarioNormalized == normalizedUserName, cancellationToken);
        }

        public Task<string> GetSecurityStampAsync(Usuario user, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(Usuario user, string stamp, CancellationToken cancellationToken)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.SecurityStamp = stamp;
            return Task.CompletedTask;
        }
    }
}
