using GlobalErpData.Models;
using GlobalErpData.Data;

namespace GlobalAPINFe.Lib
{
    public class SeedData
    {
        public static void Initialize(GlobalErpFiscalBaseContext context)
        {
            var requiredPermissions = new List<Permissao>
            {
                new Permissao { Chave = "dashboard", Modulo = "ADM", Descricao = "Dashboard" },
                new Permissao { Chave = "cad-usuarios", Modulo = "ADM", Descricao = "Cadastro de Usuários" },
                new Permissao { Chave = "cad-empresa", Modulo = "ADM", Descricao = "Cadastro de Empresa" },
                new Permissao { Chave = "cad-cliente", Modulo = "ADM", Descricao = "Cadastro de Cliente" },
                new Permissao { Chave = "cad-fornecedor", Modulo = "ADM", Descricao = "Cadastro de Fornecedor" },
                new Permissao { Chave = "cad-transportadora", Modulo = "ADM", Descricao = "Cadastro de Transportadora" },
                new Permissao { Chave = "cad-produto-estoque", Modulo = "ES", Descricao = "Cadastro de Produtos" },
                new Permissao { Chave = "cad-grupo-estoque", Modulo = "ES", Descricao = "Cadastro de Grupo" },
                new Permissao { Chave = "cad-plano-estoque", Modulo = "ES", Descricao = "Cadastro de Plano" },
                new Permissao { Chave = "cad-referencia-estoque", Modulo = "ES", Descricao = "Cadastro de Referência" },
                new Permissao { Chave = "cad-saldo-estoque", Modulo = "ES", Descricao = "Cadastro de Saldo de Estoque" },
                new Permissao { Chave = "cad-unidade-medida", Modulo = "ES", Descricao = "Cadastro de Unidade de Medida" },
                new Permissao { Chave = "cad-entrada-nfe", Modulo = "ES", Descricao = "Entrada" },
                new Permissao { Chave = "cad-saida-nfe", Modulo = "ES", Descricao = "Saida" },
                new Permissao { Chave = "cad-protocolo-estado-ncm", Modulo = "ES", Descricao = "Cadastro de Protocolo Estado NCM" },
                new Permissao { Chave = "cad-ncm-protocolo-estado", Modulo = "ES", Descricao = "Cadastro de Amarração NCM x Protocolo" },
                new Permissao { Chave = "cad-cfop-csosn", Modulo = "ES", Descricao = "Cadastro de CFOP por CSOSN" },
                new Permissao { Chave = "imp-nfe", Modulo = "ES", Descricao = "Importação XML" },
                new Permissao { Chave = "configuracao-empresa", Modulo = "ADM", Descricao = "Configurações" },
                new Permissao { Chave = "cad-certificado", Modulo = "ADM", Descricao = "Cadastro de Certificado" },
                new Permissao { Chave = "cad-cfop-importacao", Modulo = "ES", Descricao = "Cfop de Importação" },
                new Permissao { Chave = "cad-conta-caixa", Modulo = "FI", Descricao = "Cadastro de Contas do Caixa" },
                new Permissao { Chave = "cad-contas-a-pagar", Modulo = "FI", Descricao = "Cadastro de Contas a Pagar" },
                new Permissao { Chave = "cad-contas-a-receber", Modulo = "FI", Descricao = "Cadastro de Contas a Receber" },
                new Permissao { Chave = "cad-forma-pagt", Modulo = "FI", Descricao = "Cadastro de Forma de Pagamento" },
                new Permissao { Chave = "cad-historico-caixa", Modulo = "FI", Descricao = "Cadastro de Histórico" },
                new Permissao { Chave = "cad-plano-caixa", Modulo = "FI", Descricao = "Cadastro de Plano de Caixa" },
                
            };

            var existingPermissions = context.Permissaos.ToList();

            var permissionDict = existingPermissions.ToDictionary(p => p.Chave, p => p);

            foreach (var requiredPermission in requiredPermissions)
            {
                if (permissionDict.TryGetValue(requiredPermission.Chave, out var existingPermission))
                {
                    existingPermission.Modulo = requiredPermission.Modulo;
                    existingPermission.Descricao = requiredPermission.Descricao;
                }
                else
                {
                    context.Permissaos.Add(requiredPermission);
                }
            }

            context.SaveChanges();
        }
    }
}
