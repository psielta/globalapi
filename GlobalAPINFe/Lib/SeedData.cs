﻿using GlobalErpData.Models;
using GlobalErpData.Data;

namespace GlobalAPINFe.Lib
{
    public class SeedData
    {
        public static void Initialize(GlobalErpFiscalBaseContext context, Serilog.ILogger logger)
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
                new Permissao { Chave = "cad-controle-numeracao-nfe", Modulo = "ES", Descricao = "Controle de numeração" },
                new Permissao { Chave = "cad-protocolo-estado-ncm", Modulo = "ES", Descricao = "Cadastro de Protocolo Estado NCM" },
                new Permissao { Chave = "cad-ncm-protocolo-estado", Modulo = "ES", Descricao = "Cadastro de Amarração NCM x Protocolo" },
                new Permissao { Chave = "cad-icm", Modulo = "ES", Descricao = "Cadastro de ICMS" },
                new Permissao { Chave = "cad-cfop-csosn", Modulo = "ES", Descricao = "Cadastro de CFOP por CSOSN" },
                new Permissao { Chave = "imp-nfe", Modulo = "ES", Descricao = "Importação XML" },
                new Permissao { Chave = "get-xml", Modulo = "ES", Descricao = "Baixar XML(s)" },
                new Permissao { Chave = "configuracao-empresa", Modulo = "ADM", Descricao = "Configurações" },
                new Permissao { Chave = "cad-certificado", Modulo = "ADM", Descricao = "Cadastro de Certificado" },
                new Permissao { Chave = "cad-cfop-importacao", Modulo = "ES", Descricao = "Cfop de Importação" },
                new Permissao { Chave = "cad-conta-caixa", Modulo = "FI", Descricao = "Cadastro de Contas do Caixa" },
                new Permissao { Chave = "cad-contas-a-pagar", Modulo = "FI", Descricao = "Cadastro de Contas a Pagar" },
                new Permissao { Chave = "cad-contas-a-receber", Modulo = "FI", Descricao = "Cadastro de Contas a Receber" },
                new Permissao { Chave = "cad-forma-pagt", Modulo = "FI", Descricao = "Cadastro de Forma de Pagamento" },
                new Permissao { Chave = "cad-historico-caixa", Modulo = "FI", Descricao = "Cadastro de Histórico" },
                new Permissao { Chave = "cad-plano-caixa", Modulo = "FI", Descricao = "Cadastro de Plano de Caixa" },
                new Permissao { Chave = "livro-caixa", Modulo = "FI", Descricao = "Livro de Caixa" },
                new Permissao { Chave = "sintegra", Modulo = "ES", Descricao = "Sintegra" },
                new Permissao { Chave = "relatorio-painel-cliente", Modulo = "ADM", Descricao = "Relatorio de Clientes" },
                new Permissao { Chave = "relatorio-estoque-saida-empresa", Modulo = "ES", Descricao = "Relatório de Saida por Empresa"},
                new Permissao { Chave = "relatorio-estoque-entrada-empresa", Modulo = "ES", Descricao = "Relatório de Entrada por Empresa"},
                new Permissao { Chave = "cad-servico", Modulo = "OS", Descricao = "Cadastro de servico"},
                new Permissao { Chave = "cad-departamento", Modulo = "OS", Descricao = "Cadastro de departamento"},
                new Permissao { Chave = "cad-planosimultaneo", Modulo = "ES", Descricao = "Cadastro de planos simultaneos"},
                new Permissao { Chave = "relatorio-financeiro-livro-caixa", Modulo = "FI", Descricao = "Relatorio de Caixa"},
                
            };
            try
            {
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
            }
            catch (Exception e)
            {
                logger.Error(e, "Error occurred while seeding permissions.");
            }
            try
            {
                var existingUnities = context.Unities.ToList();
                var existingIcm = context.Icms.ToList();
                foreach (Unity unity in existingUnities)
                {
                    var existingIcm_ = existingIcm.Where(p => p.Unity == unity.Id);
                    if (existingIcm_ == null || existingIcm_.Count() == 0)
                    {
                        Icm icm = new Icm
                        {
                            Am = 17.00m,
                            Ac = 17.00m,
                            Al = 17.00m,
                            Ap = 17.00m,
                            Ba = 17.00m,
                            Ce = 17.00m,
                            Df = 17.00m,
                            Es = 17.00m,
                            Go = 17.00m,
                            Ma = 17.00m,
                            Mg = 18.00m,
                            Mt = 17.00m,
                            Ms = 17.00m,
                            Pa = 18.00m,
                            Pb = 17.00m,
                            Pi = 17.00m,
                            Pr = 17.00m,
                            Rj = 19.00m,
                            Rn = 17.00m,
                            Ro = 17.00m,
                            Rr = 17.00m,
                            Rs = 17.00m,
                            Sc = 17.00m,
                            Se = 17.00m,
                            Sp = 18.00m,
                            To = 17.00m,
                            Ex = 17.00m,
                            Unity = unity.Id
                        };

                        context.Icms.Add(icm);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Error occurred while seeding ICMS.");
            }

            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {
                logger.Error(e, "Error occurred while saving changes.");
            }
        }
    }
}
