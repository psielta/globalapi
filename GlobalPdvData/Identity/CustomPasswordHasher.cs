using GlobalPdvData.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace GlobalErpData.Identity
{
    

    public class CustomPasswordHasher : PasswordHasher<Usuario>
    {
        public override PasswordVerificationResult VerifyHashedPassword(Usuario user, string hashedPassword, string providedPassword)
        {
            // Primeiro, tenta verificar usando o método padrão
            try
            {
                var result = base.VerifyHashedPassword(user, hashedPassword, providedPassword);
                if (result != PasswordVerificationResult.Failed)
                {
                    return result;
                }
            }
            catch (FormatException)
            {
                // Ignora a exceção de formato inválido
            }

            // Se a verificação padrão falhar, tenta verificar usando o método antigo (texto simples)
            if (hashedPassword == providedPassword)
            {
                // Atualiza a senha do usuário para o novo formato hash
                UpdateUserPasswordHash(user, providedPassword);
                return PasswordVerificationResult.Success;
            }

            return PasswordVerificationResult.Failed;
        }

        private void UpdateUserPasswordHash(Usuario user, string providedPassword)
        {
            // Gera o novo hash da senha
            var newHashedPassword = base.HashPassword(user, providedPassword);

            // Atualiza a senha do usuário
            user.CdSenha = newHashedPassword;

            // Marca que a senha precisa ser atualizada no banco de dados
            user.NeedPasswordHashUpdate = true;
        }
    }

}
