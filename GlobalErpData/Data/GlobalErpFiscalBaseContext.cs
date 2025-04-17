using System;
using System.Collections.Generic;
using GlobalErpData.Models;
using GlobalLib.Utils;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Data;

public partial class GlobalErpFiscalBaseContext : DbContext
{
    public GlobalErpFiscalBaseContext()
    {
    }

    public GlobalErpFiscalBaseContext(DbContextOptions<GlobalErpFiscalBaseContext> options)
        : base(options)
    {
    }
    public virtual DbSet<UairangoOpcoesProduto> UairangoOpcoesProdutos { get; set; }
    public virtual DbSet<UairangoPrazo> UairangoPrazos { get; set; }
    public virtual DbSet<UairangoConfiguraco> UairangoConfiguracoes { get; set; }
    public virtual DbSet<UairangoFormasPagamento> UairangoFormasPagamentos { get; set; }
    public virtual DbSet<UairangoCulinaria> UairangoCulinarias { get; set; }
    public virtual DbSet<PlanoSimultaneo> PlanoSimultaneos { get; set; }
    public virtual DbSet<Departamento> Departamentos { get; set; }
    public virtual DbSet<OrcamentoCab> OrcamentoCabs { get; set; }
    public virtual DbSet<OrcamentoIten> OrcamentoItens { get; set; }
    public virtual DbSet<OsTabelaPreco> OsTabelaPrecos { get; set; }
    public virtual DbSet<Servico> Servicos { get; set; }
    public virtual DbSet<OrcamentoServico> OrcamentoServicos { get; set; }
    public virtual DbSet<EntregaNfe> EntregaNves { get; set; }
    public virtual DbSet<RetiradaNfe> RetiradaNves { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<TabelaAnp> TabelaAnps { get; set; }
    public virtual DbSet<DistribuicaoDfe> DistribuicaoDfes { get; set; }
    public virtual DbSet<Funcionario> Funcionarios { get; set; }
    public virtual DbSet<UsuarioFuncionario> UsuarioFuncionarios { get; set; }
    public virtual DbSet<Vendedor> Vendedors { get; set; }

    public virtual DbSet<Certificado> Certificados { get; set; }

    public virtual DbSet<CestNcm> CestNcms { get; set; }

    public virtual DbSet<Cfop> Cfops { get; set; }

    public virtual DbSet<CfopCsosnV2> CfopCsosnV2s { get; set; }

    public virtual DbSet<CfopImportacao> CfopImportacaos { get; set; }

    public virtual DbSet<Cidade> Cidades { get; set; }
    public virtual DbSet<UsuarioEmpresa> UsuarioEmpresas { get; set; }
    public virtual DbSet<Unity> Unities { get; set; }
    public virtual DbSet<NfceAberturaCaixa> NfceAberturaCaixas { get; set; }

    public virtual DbSet<NfceFormaPgt> NfceFormaPgts { get; set; }

    public virtual DbSet<NfceProdutoSaidum> NfceProdutoSaida { get; set; }

    public virtual DbSet<NfceSaida> NfceSaidas { get; set; }
    public virtual DbSet<SangriaCaixa> SangriaCaixas { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<ConfEmail> ConfEmails { get; set; }

    public virtual DbSet<ConfiguracoesEmpresa> ConfiguracoesEmpresas { get; set; }

    public virtual DbSet<ConfiguracoesUsuario> ConfiguracoesUsuarios { get; set; }

    public virtual DbSet<ContaDoCaixa> ContaDoCaixas { get; set; }

    public virtual DbSet<ContasAPagar> ContasAPagars { get; set; }

    public virtual DbSet<ContasAReceber> ContasARecebers { get; set; }

    public virtual DbSet<Csosn> Csosns { get; set; }

    public virtual DbSet<ControleNumeracaoNfe> ControleNumeracaoNves { get; set; }

    public virtual DbSet<Cst> Csts { get; set; }

    public virtual DbSet<Cte> Ctes { get; set; }

    public virtual DbSet<CteCompPrestacao> CteCompPrestacaos { get; set; }

    public virtual DbSet<CteDocAnterior> CteDocAnteriors { get; set; }

    public virtual DbSet<CteDocAnteriorNfe> CteDocAnteriorNves { get; set; }

    public virtual DbSet<CteDuplicatum> CteDuplicata { get; set; }

    public virtual DbSet<CteInutilizarNumero> CteInutilizarNumeros { get; set; }

    public virtual DbSet<CteNf> CteNfs { get; set; }

    public virtual DbSet<CteNfe> CteNves { get; set; }

    public virtual DbSet<CteOb> CteObs { get; set; }

    public virtual DbSet<CteOrdemColetum> CteOrdemColeta { get; set; }

    public virtual DbSet<CteOutrosDoc> CteOutrosDocs { get; set; }

    public virtual DbSet<CtePassagem> CtePassagems { get; set; }

    public virtual DbSet<CteProdPerigoso> CteProdPerigosos { get; set; }

    public virtual DbSet<CteQtdCarga> CteQtdCargas { get; set; }

    public virtual DbSet<CteRodMotoristum> CteRodMotorista { get; set; }

    public virtual DbSet<CteRodVeiculo> CteRodVeiculos { get; set; }

    public virtual DbSet<CteSeguro> CteSeguros { get; set; }

    public virtual DbSet<CteValePedagio> CteValePedagios { get; set; }

    public virtual DbSet<CteVeiculo> CteVeiculos { get; set; }

    public virtual DbSet<Diverso> Diversos { get; set; }

    public virtual DbSet<Empresa> Empresas { get; set; }

    public virtual DbSet<Entrada> Entradas { get; set; }

    public virtual DbSet<EnvioEmailAutomatico> EnvioEmailAutomaticos { get; set; }

    public virtual DbSet<Featured> Featureds { get; set; }

    public virtual DbSet<FormaPagt> FormaPagts { get; set; }

    public virtual DbSet<Fornecedor> Fornecedors { get; set; }

    public virtual DbSet<FotosProduto> FotosProdutos { get; set; }

    public virtual DbSet<Frete> Fretes { get; set; }

    public virtual DbSet<GrupoEstoque> GrupoEstoques { get; set; }

    public virtual DbSet<HistoricoCaixa> HistoricoCaixas { get; set; }

    public virtual DbSet<Ibpt> Ibpts { get; set; }

    public virtual DbSet<Icm> Icms { get; set; }

    public virtual DbSet<Impcabnfe> Impcabnves { get; set; }

    public virtual DbSet<Impdupnfe> Impdupnves { get; set; }

    public virtual DbSet<Impitensnfe> Impitensnves { get; set; }

    public virtual DbSet<Imptotalnfe> Imptotalnves { get; set; }

    public virtual DbSet<Impxml> Impxmls { get; set; }

    public virtual DbSet<ItemDetail> ItemDetails { get; set; }

    public virtual DbSet<Mdfe> Mdves { get; set; }

    public virtual DbSet<MdfeChafe> MdfeChaves { get; set; }

    public virtual DbSet<MdfeChavesDfe> MdfeChavesDves { get; set; }

    public virtual DbSet<MdfeCondutor> MdfeCondutors { get; set; }

    public virtual DbSet<EntradaOutrasDesp> EntradaOutrasDesps { get; set; }
    public virtual DbSet<MdfeInfcarregamento> MdfeInfcarregamentos { get; set; }

    public virtual DbSet<UairangoEmpresaCategorium> UairangoEmpresaCategoria { get; set; }
    public virtual DbSet<UairangoOpcoesCategorium> UairangoOpcoesCategoria { get; set; }
    public virtual DbSet<MdfePercurso> MdfePercursos { get; set; }

    public virtual DbSet<MdfeReboque> MdfeReboques { get; set; }

    public virtual DbSet<MdfeRodoviario> MdfeRodoviarios { get; set; }

    public virtual DbSet<MdfeSeguro> MdfeSeguros { get; set; }

    public virtual DbSet<Ncm> Ncms { get; set; }

    public virtual DbSet<NcmProtocoloEstado> NcmProtocoloEstados { get; set; }

    public virtual DbSet<ObsNf> ObsNfs { get; set; }

    public virtual DbSet<Older> Olders { get; set; }

    public virtual DbSet<OlderItem> OlderItems { get; set; }

    public virtual DbSet<OrigemCst> OrigemCsts { get; set; }

    public virtual DbSet<PerfilLoja> PerfilLojas { get; set; }

    public virtual DbSet<Permissao> Permissaos { get; set; }

    public virtual DbSet<PlanoDeCaixa> PlanoDeCaixas { get; set; }

    public virtual DbSet<PlanoEstoque> PlanoEstoques { get; set; }

    public virtual DbSet<ProductDetail> ProductDetails { get; set; }

    public virtual DbSet<ProdutoEntradum> ProdutoEntrada { get; set; }

    public virtual DbSet<ProdutoEstoque> ProdutoEstoques { get; set; }

    public virtual DbSet<ProdutoSaidum> ProdutoSaida { get; set; }

    public virtual DbSet<ProdutosForn> ProdutosForns { get; set; }

    public virtual DbSet<ProtocoloEstadoNcm> ProtocoloEstadoNcms { get; set; }

    public virtual DbSet<ReferenciaEstoque> ReferenciaEstoques { get; set; }

    public virtual DbSet<Saida> Saidas { get; set; }

    public virtual DbSet<SaidaNotasDevolucao> SaidaNotasDevolucaos { get; set; }

    public virtual DbSet<SaidasVolume> SaidasVolumes { get; set; }

    public virtual DbSet<SaldoEstoque> SaldoEstoques { get; set; }

    public virtual DbSet<LivroCaixa> LivroCaixas { get; set; }
    public virtual DbSet<PagtosParciaisCp> PagtosParciaisCps { get; set; }
    public virtual DbSet<PagtosParciaisCr> PagtosParciaisCrs { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<SectionItem> SectionItems { get; set; }

    public virtual DbSet<Transportadora> Transportadoras { get; set; }

    public virtual DbSet<UnidadeMedida> UnidadeMedidas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioPermissao> UsuarioPermissaos { get; set; }
    public virtual DbSet<TipoNf> TipoNfs { get; set; }

    public virtual DbSet<UairangoRequest> UairangoRequests { get; set; }

    public virtual DbSet<UairangoToken> UairangoTokens { get; set; }

    /************************************************************/
    /* DBSET PERSONALIZADOS => Triggers & Views => NAO APAGAR */
    /************************************************************/
    public DbSet<DashboardEstoqueTotalEntradas> cdsDashboardEstoqueTotalEntradas { get; set; }
    public DbSet<DashboardEstoqueTotalSaidas> cdsDashboardEstoqueTotalSaidas { get; set; }

    public DbSet<DashboardEstoqueTotalSaidasPorMes> cdsDashboardEstoqueTotalSaidasPorMes { get; set; }
    public DbSet<DashboardEstoqueTotalEntradasPorMes> cdsDashboardEstoqueTotalEntradasPorMes { get; set; }
    public DbSet<DashboardEstoqueTotalEntradasPorDia> cdsDashboardEstoqueTotalEntradasPorDia { get; set; }
    public DbSet<DashboardEstoqueTotalSaidasPorDia> cdsDashboardEstoqueTotalSaidasPorDia { get; set; }
    public DbSet<FnDistribuicaoDfeEntradasResult> FnDistribuicaoDfeEntradasResults { get; set; }

    public DbSet<TotalPorGrupo> cdsTotalPorGrupo { get; set; }
    public DbSet<ProcReg50EntradaResult> ProcReg50EntradaResults { get; set; }
    public DbSet<ProcReg54EntradaResult> ProcReg54EntradaResults { get; set; }
    public DbSet<ProcReg50SaidaResult> ProcReg50SaidaResults { get; set; }
    public DbSet<ProcReg54SaidaResult> ProcReg54SaidaResults { get; set; }
    public DbSet<ProcReg75SaidaResult> ProcReg75SaidaResults { get; set; }
    public DbSet<ProcReg75EntradaResult> ProcReg75EntradaResults { get; set; }

    public DbSet<TotalDiaResult> cdsTotalDia { get; set; }
    public DbSet<TotalPeriodoResult> cdsTotalPeriodo { get; set; }

    public DbSet<FormaPagamentoResult> cdsFormasPagamento { get; set; }

    public DbSet<Produto5MaisVendidosResult> cdsProduto5MaisVendidos { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(IniFile.GetConnectionString());
        }
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("pgcrypto")
            .HasPostgresExtension("unaccent")
            .HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("category_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.Categories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("category_fk");
        });

        modelBuilder.Entity<Certificado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("certificados_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.Tipo)
                .HasDefaultValueSql("'H'::character varying")
                .HasComment("H - Homologacao\r\nP - Producao");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Certificados).HasConstraintName("certificados_fk");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.Certificados)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("certificados_fk1");
        });

        modelBuilder.Entity<CestNcm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cest_ncm_pkey");

            entity.Property(e => e.Descricao).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Cfop>(entity =>
        {
            entity.HasKey(e => e.CdCfop).HasName("cfop_pkey");

            entity.Property(e => e.DescNfe).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Descricao).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.TipoCfop).HasDefaultValueSql("''::character varying");
        });

        modelBuilder.Entity<CfopCsosnV2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cfop_csosn_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.CfopCsosnV2s)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cfop_csosn_v2_fk");
        });

        modelBuilder.Entity<CfopImportacao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cfop_importacao_pkey");

            entity.Property(e => e.CfopDentro).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CfopFora).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.CfopImportacaos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cfop_importacao_fk");
        });

        modelBuilder.Entity<Cidade>(entity =>
        {
            entity.HasKey(e => e.CdCidade).HasName("cidade_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.Uf).HasDefaultValueSql("'XX'::character varying");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cliente_idx");

            entity.Property(e => e.Ativo).HasDefaultValueSql("'S'::character varying");
            entity.Property(e => e.Cep).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.ConsumidorFinal).HasDefaultValue(true);
            entity.Property(e => e.DtCadastro).HasDefaultValueSql("('now'::text)::date");
            entity.Property(e => e.EMail).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.IdCteAntigo).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.Mva).HasDefaultValue(false);
            entity.Property(e => e.NmBairro).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NmEndereco).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrDoc).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Numero).HasDefaultValueSql("'0'::character varying");
            entity.Property(e => e.Telefone).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TpDoc).HasDefaultValueSql("'CPF'::character varying");
            entity.Property(e => e.TxtObs).HasDefaultValueSql("''::character varying");

            entity.HasOne(d => d.CdCidadeNavigation).WithMany(p => p.Clientes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cliente_fk1");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.Clientes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cliente_fk");
        });

        modelBuilder.Entity<ConfEmail>(entity =>
        {
            entity.HasKey(e => e.NrLanc).HasName("conf_email_pkey");

            entity.Property(e => e.AssuntoEmail).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.ConexaoSegura).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.EMailCopia).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.EnviaAposEmitir).HasDefaultValue(false);
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.Ssl).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.Tsl).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.TxtPadrao).HasDefaultValueSql("''::text");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.ConfEmails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("conf_email_cd_empresa_fkey");
        });

        modelBuilder.Entity<ConfiguracoesEmpresa>(entity =>
        {
            entity.HasKey(e => new { e.Chave, e.Unity }).HasName("configuracoes_empresa_idx");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.Valor1).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Valor2).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Valor3).HasDefaultValueSql("''::character varying");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.ConfiguracoesEmpresas).HasConstraintName("configuracoes_empresa_fk");
        });

        modelBuilder.Entity<ConfiguracoesUsuario>(entity =>
        {
            entity.HasKey(e => new { e.Chave, e.IdUsuario }).HasName("configuracoes_usuario_idx");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.Valor1).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Valor2).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Valor3).HasDefaultValueSql("''::character varying");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.ConfiguracoesUsuarios)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("configuracoes_usuario_fk");
        });

        modelBuilder.Entity<ContaDoCaixa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("conta_do_caixa_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.LimiteEspecial).HasDefaultValueSql("0");
            entity.Property(e => e.MostrarDadosImpressao).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.NmBanco).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrAgencia).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrChequeInicial).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrContaBanco).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrDigitoAg).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrDigitoConta).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.SaldoAtual).HasDefaultValueSql("0");
            entity.Property(e => e.SaldoInicial).HasDefaultValueSql("0");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.ContaDoCaixas).HasConstraintName("conta_do_caixa_fk");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.ContaDoCaixas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("conta_do_caixa_fk1");
        });

        modelBuilder.Entity<ContasAPagar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("contas_a_pagar_pkey");

            entity.Property(e => e.DtLancamento).HasDefaultValueSql("('now'::text)::date");
            entity.Property(e => e.IdLancPrinc).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.NrEntradaOutraDesp).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.Pagou).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.TxtObs).HasDefaultValueSql("''::text");
            entity.Property(e => e.VlAcrescimo).HasDefaultValueSql("0");
            entity.Property(e => e.VlCheque).HasDefaultValueSql("0");
            entity.Property(e => e.VlDinheiro).HasDefaultValueSql("0");
            entity.Property(e => e.VlPagoFinal).HasDefaultValueSql("0");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.ContasAPagars)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contas_a_pagar_fk2");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.ContasAPagars)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contas_a_pagar_fk1");

            entity.HasOne(d => d.Fornecedor).WithMany(p => p.ContasAPagars)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contas_a_pagar_fk4");

            entity.HasOne(d => d.Entrada).WithMany(p => p.ContasAPagars)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("contas_a_pagar_fk3");

            entity.HasOne(d => d.HistoricoCaixa).WithMany(p => p.ContasAPagars)
                .HasPrincipalKey(p => new { p.Unity, p.CdSubPlano, p.CdPlano })
                .HasForeignKey(d => new { d.Unity, d.CdHistoricoCaixa, d.CdPlanoCaixa })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contas_a_pagar_fk");
        });

        modelBuilder.Entity<ContasAReceber>(entity =>
        {
            entity.HasKey(e => e.NrConta).HasName("contas_a_receber_pkey");

            entity.Property(e => e.Alteradodtvenc).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.Cancelado).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.CdProjeto).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.DataLanc).HasDefaultValueSql("('now'::text)::date");
            entity.Property(e => e.IdAluno).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.IdGrupo).HasDefaultValue(0);
            entity.Property(e => e.IdLancPrincipal).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.Imprimiu).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.NrContaRenegociado).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.NrFormaPagt).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.NrOs).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.NrSaida).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.QtParcela).HasDefaultValue(1);
            entity.Property(e => e.Quantidade).HasDefaultValue(1);
            entity.Property(e => e.Recebeu).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.Status).HasDefaultValueSql("'01'::character varying");
            entity.Property(e => e.TxtBoleto).HasDefaultValueSql("''::text");
            entity.Property(e => e.TxtObs).HasDefaultValueSql("''::text");
            entity.Property(e => e.UtilizouLimite).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.VenceuPrazo).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.Vinculado).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.VlBruto).HasDefaultValueSql("0");
            entity.Property(e => e.VlDesconto).HasDefaultValueSql("0");
            entity.Property(e => e.VlIrrf).HasDefaultValueSql("0");
            entity.Property(e => e.VlIss).HasDefaultValueSql("0");
            entity.Property(e => e.VlJuros).HasDefaultValueSql("0");
            entity.Property(e => e.VlPago).HasDefaultValueSql("0");

            entity.HasOne(d => d.CdClienteNavigation).WithMany(p => p.ContasARecebers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contas_a_receber_fk1");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.ContasARecebers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contas_a_receber_fk");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.ContasARecebers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contas_a_receber_fk3");

            entity.HasOne(d => d.HistoricoCaixa).WithMany(p => p.ContasARecebers)
                .HasPrincipalKey(p => new { p.Unity, p.CdSubPlano, p.CdPlano })
                .HasForeignKey(d => new { d.Unity, d.CdHistoricoCaixa, d.CdPlanoCaixa })
                .HasConstraintName("contas_a_receber_fk2");
        });

        modelBuilder.Entity<ControleNumeracaoNfe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("controle_numeracao_nfe_pkey");

            entity.HasIndex(e => e.IdEmpresa, "idx_unico_padrao_por_empresa")
                .IsUnique()
                .HasFilter("(padrao = true)");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.IdEmpresaNavigation).WithOne(p => p.ControleNumeracaoNfe).HasConstraintName("controle_numeracao_nfe_fk");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.ControleNumeracaoNves)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("controle_numeracao_nfe_fk1");
        });

        modelBuilder.Entity<Csosn>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("csosn_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Cst>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("cst_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Cte>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cte_pkey");

            entity.Property(e => e.AliqIcms).HasDefaultValueSql("0");
            entity.Property(e => e.AliqInterestUft).HasDefaultValueSql("0");
            entity.Property(e => e.AliqInternaUft).HasDefaultValueSql("0");
            entity.Property(e => e.CaractAdServico).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CaractAdTransp).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdDestinatario).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.CdExpedidor).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.CdModeloTomador).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdNumerico).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdPartUft).HasDefaultValueSql("0");
            entity.Property(e => e.CdRecebedor).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.CdRemetente).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.CdRotaEntrega).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdSituacaoCte).HasDefaultValueSql("'01'::character varying");
            entity.Property(e => e.CdStIcms).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdTomadorServico).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.ChaveAcesso).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.ChaveAcessoReferenc).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.ChaveCteAnulacao).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.ChaveCteSubst).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.ChaveCteTomador).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.ChaveNfeTomador).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Ciot).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CnpjTomador).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CpfTomador).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.DadosRetirada).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.DestinoFluxoCaixa).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Detalhe).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.DtFimPrevisao).HasDefaultValueSql("('now'::text)::date");
            entity.Property(e => e.DtHrEmissao).HasDefaultValueSql("now()");
            entity.Property(e => e.DtInicioPrevisao).HasDefaultValueSql("('now'::text)::date");
            entity.Property(e => e.DtPrevEntrega).HasDefaultValueSql("('now'::text)::date");
            entity.Property(e => e.DtTomador).HasDefaultValueSql("('now'::text)::date");
            entity.Property(e => e.FinalidadeEmissao).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.FormaEmissao).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.FormaPagto).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.FuncEmissorCte).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.HrFimPrevisao).HasDefaultValueSql("('now'::text)::time with time zone");
            entity.Property(e => e.HrInicioPrevisao).HasDefaultValueSql("('now'::text)::time with time zone");
            entity.Property(e => e.IcmsUfTermino).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.IdEmpresa).HasDefaultValue(1);
            entity.Property(e => e.IndicadorLot).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.InfFiscoIcms).HasDefaultValueSql("0");
            entity.Property(e => e.Inss).HasDefaultValueSql("0");
            entity.Property(e => e.Modelo).HasDefaultValueSql("'57'::character varying");
            entity.Property(e => e.MunicipioDestinoCalcFrete).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.MunicipioEmissao).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.MunicipioFimPrestacao).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.MunicipioInicioPrestacao).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.MunicipioOrigemCalcFrete).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrAutorizacaoCte).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrCteReferenciado).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrFatura).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrProtoCancelamento).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrRegEstadual).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrTaf).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NumeroTomador).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Obs).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.OrigemFluxoCaixa).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.OutrasCaractProd).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.PorcIcmsFcpUft).HasDefaultValueSql("0");
            entity.Property(e => e.PorcPartUft).HasDefaultValueSql("0");
            entity.Property(e => e.PorcRedBcIcms).HasDefaultValueSql("0");
            entity.Property(e => e.PrevisaoData).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.PrevisaoHora).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.ProdPredominante).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.QtPassageiro).HasDefaultValue(0);
            entity.Property(e => e.RetemInss).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.Rntcr).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Serie).HasDefaultValueSql("'001'::character varying");
            entity.Property(e => e.SerieTomador).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Status).HasDefaultValueSql("'01'::character varying");
            entity.Property(e => e.Subserie).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TomadorCteSubst).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TomadorNc).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TpDestinatario).HasDefaultValueSql("'C'::character varying");
            entity.Property(e => e.TpDocTomador).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TpExpedidor).HasDefaultValueSql("'S'::character varying");
            entity.Property(e => e.TpRecebedor).HasDefaultValueSql("'S'::character varying");
            entity.Property(e => e.TpRemetente).HasDefaultValueSql("'C'::character varying");
            entity.Property(e => e.TpServico).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.TpTomadorServico).HasDefaultValueSql("'RM'::character varying");
            entity.Property(e => e.TxtJustificativaCancelamento).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TxtdescServicoPrestado).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.VlBcIcms).HasDefaultValueSql("0");
            entity.Property(e => e.VlBcIcmsUft).HasDefaultValueSql("0");
            entity.Property(e => e.VlCarga).HasDefaultValueSql("0");
            entity.Property(e => e.VlCredPresumido).HasDefaultValueSql("0");
            entity.Property(e => e.VlDescFatura).HasDefaultValueSql("0");
            entity.Property(e => e.VlIcms).HasDefaultValueSql("0");
            entity.Property(e => e.VlIcmsFcpUf).HasDefaultValueSql("0");
            entity.Property(e => e.VlIcmsPartUfi).HasDefaultValueSql("0");
            entity.Property(e => e.VlIcmsPartUft).HasDefaultValueSql("0");
            entity.Property(e => e.VlLiqFatura).HasDefaultValueSql("0");
            entity.Property(e => e.VlOriginalFatura).HasDefaultValueSql("0");
            entity.Property(e => e.VlPrestServico).HasDefaultValueSql("0");
            entity.Property(e => e.VlReceberPrestServico).HasDefaultValueSql("0");
            entity.Property(e => e.VlTomador).HasDefaultValueSql("0");
            entity.Property(e => e.VlTribtPrestServico).HasDefaultValueSql("0");
            entity.Property(e => e.XmlCte).HasDefaultValueSql("''::text");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Ctes).HasConstraintName("cte_fk");
        });

        modelBuilder.Entity<CteCompPrestacao>(entity =>
        {
            entity.HasKey(e => e.IdCompPrestacao).HasName("cte_comp_prestacao_pkey");

            entity.Property(e => e.IdCompPrestacao).HasDefaultValueSql("nextval('cte.cte_comp_prestacao_id_seq'::regclass)");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.CteCompPrestacaos).HasConstraintName("cte_comp_prestacao_fk");
        });

        modelBuilder.Entity<CteDocAnterior>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("cte_doc_anterior_pkey");

            entity.Property(e => e.Cnpj).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Cpf).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.InscEstadual).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Uf).HasDefaultValueSql("''::character varying");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.CteDocAnteriors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cte_doc_anterior_fkey");
        });

        modelBuilder.Entity<CteDocAnteriorNfe>(entity =>
        {
            entity.HasKey(e => e.IdCteDocAnteriorNfe).HasName("cte_doc_anterior_nfe_pkey");

            entity.HasOne(d => d.IdDocAnteriorNavigation).WithMany(p => p.CteDocAnteriorNves).HasConstraintName("cte_doc_anterior_nfe_fk");
        });

        modelBuilder.Entity<CteDuplicatum>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("cte_duplicata_pkey");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.CteDuplicata)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cte_duplicata_fkey");
        });

        modelBuilder.Entity<CteInutilizarNumero>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cte_inutilizar_numero_pkey");

            entity.Property(e => e.Status).HasDefaultValueSql("'01'::character varying");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.CteInutilizarNumeros).HasConstraintName("cte_inutilizar_numero_fk");
        });

        modelBuilder.Entity<CteNf>(entity =>
        {
            entity.HasKey(e => e.IdNf).HasName("cte_nf_pkey");

            entity.Property(e => e.IdNf).HasDefaultValueSql("nextval('cte.cte_nf_id_seq'::regclass)");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.CteNfs).HasConstraintName("cte_nf_fk");
        });

        modelBuilder.Entity<CteNfe>(entity =>
        {
            entity.HasKey(e => e.IdNfe).HasName("cte_nfe_pkey");

            entity.Property(e => e.IdNfe).HasDefaultValueSql("nextval('cte.cte_nfe_id_seq'::regclass)");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.CteNves).HasConstraintName("cte_nfe_fk");
        });

        modelBuilder.Entity<CteOb>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("cte_obs_contrib_pkey");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.CteObs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cte_obs_fkey");
        });

        modelBuilder.Entity<CteOrdemColetum>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("cte_ordem_coleta_pkey");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.CteOrdemColeta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cte_ordem_coleta_fkey");
        });

        modelBuilder.Entity<CteOutrosDoc>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("cte_outros_doc_pkey");

            entity.Property(e => e.Descricao).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Numero).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.VlDocumento).HasDefaultValueSql("0");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.CteOutrosDocs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cte_outros_doc_fkey");
        });

        modelBuilder.Entity<CtePassagem>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("cte_passagem_pkey");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.CtePassagems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cte_passagem_fkey");
        });

        modelBuilder.Entity<CteProdPerigoso>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("cte_prod_perigoso_pkey");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.CteProdPerigosos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cte_prod_perigoso_fkey");
        });

        modelBuilder.Entity<CteQtdCarga>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("cte_qtd_carga_pkey");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.CteQtdCargas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cte_qtd_carga_fkey");
        });

        modelBuilder.Entity<CteRodMotoristum>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("cte_rod_motorista_pkey");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.CteRodMotorista)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cte_rod_motorista_fkey");
        });

        modelBuilder.Entity<CteRodVeiculo>(entity =>
        {
            entity.HasKey(e => e.IdRod).HasName("cte_rod_veiculo_pkey");

            entity.Property(e => e.IdRod).HasDefaultValueSql("nextval('cte.cte_rod_veiculo_id_seq'::regclass)");
            entity.Property(e => e.Ativo).HasDefaultValueSql("'S'::character varying");
            entity.Property(e => e.IdEmpresa).HasDefaultValue(1);

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.CteRodVeiculos).HasConstraintName("cte_rod_veiculo_fk");
        });

        modelBuilder.Entity<CteSeguro>(entity =>
        {
            entity.HasKey(e => e.IdSeguro).HasName("cte_seguro_pkey");

            entity.Property(e => e.IdSeguro).HasDefaultValueSql("nextval('cte.cte_seguro_id_seq'::regclass)");
            entity.Property(e => e.IdEmpresa).HasDefaultValue(1);
            entity.Property(e => e.NmSeguradoura).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrApolice).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrAverbacao).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.VlMercadoria).HasDefaultValueSql("0");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.CteSeguros).HasConstraintName("cte_seguro_fk");
        });

        modelBuilder.Entity<CteValePedagio>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("cte_vale_pedagio_pkey");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.CteValePedagios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cte_vale_pedagio_fkey");
        });

        modelBuilder.Entity<CteVeiculo>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("cte_veiculo_pkey");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.CteVeiculos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cte_veiculo_fkey");
        });

        modelBuilder.Entity<DeletedRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("deleted_records_pkey");

            entity.Property(e => e.DtDeleted).HasDefaultValueSql("now()");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
        });

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("departamento_pkey");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.Departamentos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("departamento_fk");
        });

        modelBuilder.Entity<DistribuicaoDfe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("distribuicao_dfe_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.DtInclusao).HasDefaultValueSql("CURRENT_DATE");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.Xml).HasDefaultValueSql("''::text");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.DistribuicaoDves)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("distribuicao_dfe_fk");
        });

        modelBuilder.Entity<Diverso>(entity =>
        {
            entity.HasKey(e => e.CdDiv).HasName("diversos_pkey");

            entity.Property(e => e.CdHistorico).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdPlanoCaixa).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdPlanoEstoque).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.NrConta).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.Observacao).HasDefaultValueSql("''::character varying");
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.CdEmpresa).HasName("empresa_pkey");

            entity.Property(e => e.AutorizoXml).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.CdCnpj).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CpfcnpfAutorizado).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.EMail).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.NmBairro).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrInscrEstadual).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrInscrMunicipal).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Telefone).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TipoRegime).HasDefaultValue(1);
            entity.Property(e => e.TxtObs).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.UairangoVinculado).HasDefaultValue(false);

            entity.HasOne(d => d.CdCidadeNavigation).WithMany(p => p.Empresas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("empresa_fk");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.Empresas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("empresa_fk1");
        });

        modelBuilder.Entity<Entrada>(entity =>
        {
            entity.HasKey(e => new { e.Nr, e.CdEmpresa }).HasName("entradas_idx");

            entity.Property(e => e.Nr).ValueGeneratedOnAdd();
            entity.Property(e => e.CdClienteDevolucao).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.CdSituacao).HasDefaultValueSql("'01'::character varying");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.TxtObs).HasDefaultValueSql("''::text");
            entity.Property(e => e.VIcmsDeson).HasDefaultValueSql("0.0");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.Entrada).HasConstraintName("entradas_fk1");

            entity.HasOne(d => d.CdGrupoEstoqueNavigation).WithMany(p => p.Entrada)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("entradas_fk2");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.Entrada)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("entradas_fk");

            entity.HasOne(d => d.Fornecedor).WithMany(p => p.Entrada)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("entradas_fk3");
        });

        modelBuilder.Entity<EntradaOutrasDesp>(entity =>
        {
            entity.HasKey(e => e.NrEntrada).HasName("entrada_outras_desp_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.EntradaOutrasDesps).HasConstraintName("entrada_outras_desp_fk1");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.EntradaOutrasDesps)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("entrada_outras_desp_fk");

            entity.HasOne(d => d.Fornecedor).WithMany(p => p.EntradaOutrasDesps)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("entrada_outras_desp_fk2");
        });

        modelBuilder.Entity<EntregaNfe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("entrega_nfe_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.Uf).IsFixedLength();

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.EntregaNves)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("entrega_nfe_fk");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.EntregaNves)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("entrega_nfe_fk1");
        });

        modelBuilder.Entity<EnvioEmailAutomatico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("envio_email_automatico_pkey");

            entity.Property(e => e.DtLanc).HasDefaultValueSql("('now'::text)::date");
            entity.Property(e => e.HrLanc).HasDefaultValueSql("(now())::time without time zone");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Featured>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Unity }).HasName("featured_pkey");

            entity.Property(e => e.Excluiu).HasDefaultValue(false);
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Category).WithMany(p => p.Featureds).HasConstraintName("featured_category_id_fkey");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.Featureds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("featured_fk");
        });

        modelBuilder.Entity<FormaPagt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("forma_pagt_pkey");

            entity.Property(e => e.CdHistoricoCaixa).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdHistoricoCaixaD).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdPlanoCaixa).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdPlanoCaixaD).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Integrado).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.Prz02).HasDefaultValue((short)0);
            entity.Property(e => e.Prz03).HasDefaultValue((short)0);
            entity.Property(e => e.Prz04).HasDefaultValue((short)0);
            entity.Property(e => e.Prz05).HasDefaultValue((short)0);
            entity.Property(e => e.Prz06).HasDefaultValue((short)0);
            entity.Property(e => e.Prz07).HasDefaultValue((short)0);
            entity.Property(e => e.Prz08).HasDefaultValue((short)0);
            entity.Property(e => e.Prz09).HasDefaultValue((short)0);
            entity.Property(e => e.Prz10).HasDefaultValue((short)0);
            entity.Property(e => e.TipoPrazo).HasDefaultValueSql("'D'::character varying");
            entity.Property(e => e.TxtObs).HasDefaultValueSql("''::text");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.FormaPagts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("forma_pagt_fk");
        });

        modelBuilder.Entity<Fornecedor>(entity =>
        {
            entity.HasKey(e => new { e.CdForn, e.Unity }).HasName("fornecedor_idx1");

            entity.Property(e => e.CdForn).ValueGeneratedOnAdd();
            entity.Property(e => e.Bairro).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdCep).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Cnpj).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Complemento).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Cpf).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Email).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.IdCliente).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.NmEndereco).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NmFantasia).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NmRepresentante).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrInscrEstadual).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Numero).HasDefaultValue(0);
            entity.Property(e => e.Parceiro).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.Ramo).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TelefoneEmpresa).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TelefoneRepresentante).HasDefaultValueSql("''::character varying");

            entity.HasOne(d => d.CdCidadeNavigation).WithMany(p => p.Fornecedors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fornecedor_fk1");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.Fornecedors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fornecedor_fk");
        });

        modelBuilder.Entity<FotosProduto>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Unity }).HasName("fotos_produto_pkey");

            entity.Property(e => e.Excluiu).HasDefaultValue(false);
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.FotosProdutos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fotos_produto_fk");

            entity.HasOne(d => d.ProdutoEstoque).WithMany(p => p.FotosProdutos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fotos_produto_fk6");
        });

        modelBuilder.Entity<Frete>(entity =>
        {
            entity.HasKey(e => e.NrLanc).HasName("frete_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.Quant).HasDefaultValueSql("0");

            entity.HasOne(d => d.NrSaidaNavigation).WithMany(p => p.Fretes).HasConstraintName("frete_fk1");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.Fretes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("frete_fk3");

            entity.HasOne(d => d.Transportadora).WithMany(p => p.Fretes).HasConstraintName("frete_fk2");
        });

        modelBuilder.Entity<Funcionario>(entity =>
        {
            entity.HasKey(e => new { e.CdFuncionario, e.CdEmpresa }).HasName("funcionario_idx");

            entity.Property(e => e.CdFuncionario).ValueGeneratedOnAdd();
            entity.Property(e => e.Ativo).HasDefaultValueSql("'S'::character varying");
            entity.Property(e => e.Bairro).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdCbo).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdInterno).HasDefaultValueSql("'-1'::character varying");
            entity.Property(e => e.Cep).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Cidade).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Color).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.DtNascimento).HasDefaultValueSql("('now'::text)::date");
            entity.Property(e => e.Endereco).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.EstadoCivil).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.IdServCentral).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Integrado).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.Mecanico).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.NrCpf).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrRg).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrTelefone).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrTelefone2).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Numero).HasDefaultValue(0);
            entity.Property(e => e.Sequence).ValueGeneratedOnAdd();
            entity.Property(e => e.Sexo).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TxtObs).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Vendedor).HasDefaultValueSql("'N'::character varying");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.Funcionarios).HasConstraintName("funcionario_fk");

            entity.HasOne(d => d.CidadeNavigation).WithMany(p => p.Funcionarios).HasConstraintName("funcionario_fk1");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.Funcionarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("funcionario_fk2");
        });

        modelBuilder.Entity<GrupoEstoque>(entity =>
        {
            entity.HasKey(e => e.CdGrupo).HasName("grupo_estoque_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.GrupoEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("grupo_estoque_fk");
        });

        modelBuilder.Entity<HistoricoCaixa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("historico_caixa_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.HistoricoCaixas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("historico_caixa_fk1");

            entity.HasOne(d => d.PlanoDeCaixa).WithMany(p => p.HistoricoCaixas)
                .HasPrincipalKey(p => new { p.CdClassificacao, p.Unity })
                .HasForeignKey(d => new { d.CdPlano, d.Unity })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("historico_caixa_fk");
        });

        modelBuilder.Entity<Ibpt>(entity =>
        {
            entity.HasKey(e => e.NrLanc).HasName("ibpt_pkey");

            entity.Property(e => e.Aliqestadual).HasDefaultValueSql("0");
            entity.Property(e => e.Aliqimp).HasDefaultValueSql("0");
            entity.Property(e => e.Aliqmunicipal).HasDefaultValueSql("0");
            entity.Property(e => e.Aliqnac).HasDefaultValueSql("0");
            entity.Property(e => e.Descricao).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Ex).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.Tabela).HasDefaultValueSql("''::character varying");
        });

        modelBuilder.Entity<Icm>(entity =>
        {
            entity.HasKey(e => e.NrLanc).HasName("icms_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.Icms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("icms_fk");
        });

        modelBuilder.Entity<Impcabnfe>(entity =>
        {
            entity.HasKey(e => e.ChNfe).HasName("pkimpcabnfe");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Impdupnfe>(entity =>
        {
            entity.HasKey(e => new { e.ChNfe, e.NrDup }).HasName("pkimpdupnfe");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Impitensnfe>(entity =>
        {
            entity.HasKey(e => new { e.ChNfe, e.NrItem }).HasName("pkimpitensnfe_ch_nfenr_item");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Imptotalnfe>(entity =>
        {
            entity.HasKey(e => e.ChNfe).HasName("pkimptotalnfe");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Impxml>(entity =>
        {
            entity.HasKey(e => new { e.IdEmpresa, e.ChaveAcesso }).HasName("impxml_idx");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.Type).HasDefaultValue(0);

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Impxmls)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("impxml_fk");
        });

        modelBuilder.Entity<ItemDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("item_details_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.ItemDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("item_details_fk");

            entity.HasOne(d => d.IdProductDetailsNavigation).WithMany(p => p.ItemDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("item_details_fk1");
        });

        modelBuilder.Entity<LivroCaixa>(entity =>
        {
            entity.HasKey(e => e.NrLanc).HasName("livro_caixa_pkey");

            entity.Property(e => e.DtLanc).HasDefaultValueSql("now()");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.TxtObs).HasDefaultValueSql("''::text");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.LivroCaixas).HasConstraintName("livro_caixa_fk");

            entity.HasOne(d => d.NrContaNavigation).WithMany(p => p.LivroCaixas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("livro_caixa_fk1");

            entity.HasOne(d => d.NrCpNavigation).WithMany(p => p.LivroCaixas).HasConstraintName("livro_caixa_fk4");

            entity.HasOne(d => d.NrCrNavigation).WithMany(p => p.LivroCaixas).HasConstraintName("livro_caixa_fk3");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.LivroCaixas).HasConstraintName("livro_caixa_fk5");

            entity.HasOne(d => d.HistoricoCaixa).WithMany(p => p.LivroCaixas)
                .HasPrincipalKey(p => new { p.Unity, p.CdSubPlano, p.CdPlano })
                .HasForeignKey(d => new { d.Unity, d.CdHistorico, d.CdPlano })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("livro_caixa_fk2");
        });

        modelBuilder.Entity<Mdfe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mdfe_pkey");

            entity.Property(e => e.Chnfe).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.DtLanc).HasDefaultValueSql("('now'::text)::date");
            entity.Property(e => e.Hora).HasDefaultValueSql("('now'::text)::time with time zone");
            entity.Property(e => e.NrAutorizacaoMdfe).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrCmdf).HasDefaultValue(0);
            entity.Property(e => e.NrDmfe).HasDefaultValueSql("'-1'::character varying");
            entity.Property(e => e.NrProtoCancelamento).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrProtoEncerramento).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Obs).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.ProdCean).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.ProdCepcar).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.ProdCepdes).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.ProdDescricao).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.ProdNcm).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.ProdTpCarga).HasDefaultValue(0);
            entity.Property(e => e.Status).HasDefaultValueSql("'01'::character varying");
            entity.Property(e => e.Tptransp).HasDefaultValueSql("'2'::character varying");
            entity.Property(e => e.TxtJustificativaCancelamento).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.XmlEventoCancelamento).HasDefaultValueSql("''::text");
            entity.Property(e => e.XmlEventoEncerramento).HasDefaultValueSql("''::text");
            entity.Property(e => e.XmlMdfe).HasDefaultValueSql("''::text");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Mdves).HasConstraintName("mdfe_fk");
        });

        modelBuilder.Entity<MdfeChafe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mdfe_chaves_pkey");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.MdfeChaves).HasConstraintName("mdfe_chaves_fk");
        });

        modelBuilder.Entity<MdfeChavesDfe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mdfe_chaves_dfe_pkey");

            entity.HasOne(d => d.IdChaveNavigation).WithMany(p => p.MdfeChavesDves)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("mdfe_chaves_dfe_fk");
        });

        modelBuilder.Entity<MdfeCondutor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mdfe_condutor_pkey");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.MdfeCondutors).HasConstraintName("mdfe_condutor_fk");
        });

        modelBuilder.Entity<MdfeInfcarregamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mdfe_infcarregamento_pkey");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.MdfeInfcarregamentos).HasConstraintName("mdfe_infcarregamento_fk");
        });

        modelBuilder.Entity<MdfePercurso>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany().HasConstraintName("mdfe_percurso_fk");
        });

        modelBuilder.Entity<MdfeReboque>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mdfe_reboque_pkey");

            entity.Property(e => e.Capkg).HasDefaultValueSql("0");
            entity.Property(e => e.Capm3).HasDefaultValueSql("0");
            entity.Property(e => e.Tara).HasDefaultValueSql("0");
            entity.Property(e => e.Tpcar).HasDefaultValueSql("'00'::character varying");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.MdfeReboques).HasConstraintName("mdfe_reboque_fk");
        });

        modelBuilder.Entity<MdfeRodoviario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mdfe_rodoviario_pkey");

            entity.Property(e => e.Capkg).HasDefaultValue(0);
            entity.Property(e => e.Capm3).HasDefaultValue(0);
            entity.Property(e => e.PropCpfcnpj).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.PropIe).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.PropNome).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.PropRntrc).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.PropTipo).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.PropUf).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Tara).HasDefaultValue(0);
            entity.Property(e => e.Tpcar).HasDefaultValueSql("'00'::character varying");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.MdfeRodoviarios).HasConstraintName("mdfe_rodoviario_fk");
        });

        modelBuilder.Entity<MdfeSeguro>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany().HasConstraintName("mdfe_seguro_fk");
        });

        modelBuilder.Entity<Ncm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ncm_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<NcmProtocoloEstado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ncm_protocolo_estado_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.IdCabProtocoloNavigation).WithMany(p => p.NcmProtocoloEstados).HasConstraintName("ncm_protocolo_estado_fk");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.NcmProtocoloEstados)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ncm_protocolo_estado_fk1");
        });

        modelBuilder.Entity<NfceAberturaCaixa>(entity =>
        {
            entity.HasKey(e => new { e.NrLanc, e.CdEmpresa }).HasName("nfce_abertura_caixa_pkey");

            entity.Property(e => e.NrLanc).ValueGeneratedOnAdd();
            entity.Property(e => e.DataLanc).HasDefaultValueSql("now()");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.Sequence).ValueGeneratedOnAdd();
            entity.Property(e => e.VlBaixaFiado).HasDefaultValueSql("0");
            entity.Property(e => e.VlMoedas).HasDefaultValueSql("0");
            entity.Property(e => e.VlSangria).HasDefaultValueSql("0");
            entity.Property(e => e.VlVendaFinal).HasDefaultValueSql("0");
            entity.Property(e => e.VlVendaFinalCart).HasDefaultValueSql("0");
            entity.Property(e => e.VlVendaFinalCartDeb).HasDefaultValueSql("0");
            entity.Property(e => e.VlVendaFinalChq).HasDefaultValueSql("0");
            entity.Property(e => e.VlVendaFinalPix).HasDefaultValueSql("0");
            entity.Property(e => e.VlVendaFinalPrazo).HasDefaultValueSql("0");
            entity.Property(e => e.VlVendaTicket).HasDefaultValueSql("0");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.NfceAberturaCaixas).HasConstraintName("nfce_abertura_caixa_fk");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.NfceAberturaCaixas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("nfce_abertura_caixa_fk2");
        });

        modelBuilder.Entity<NfceFormaPgt>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CdEmpresa }).HasName("nfce_forma_pgt_idx");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Caixa).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.NrAberturaCaixa).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.Sequence).ValueGeneratedOnAdd();
            entity.Property(e => e.Troco).HasDefaultValueSql("0");
            entity.Property(e => e.Valor).HasDefaultValueSql("0");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.NfceFormaPgts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("nfce_forma_pgt_fk");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.NfceFormaPgts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("nfce_forma_pgt_fk2");

            entity.HasOne(d => d.NfceSaida).WithMany(p => p.NfceFormaPgts).HasConstraintName("nfce_forma_pgt_fk1");
        });

        modelBuilder.Entity<NfceProdutoSaidum>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CdEmpresa }).HasName("nfce_produto_saida_pkey");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Cancelou).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.CdInterno).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdPlanoSecundario).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.CdProdutoOriginal).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.Cor).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CstCofins).HasDefaultValueSql("0");
            entity.Property(e => e.CstPis).HasDefaultValueSql("0");
            entity.Property(e => e.Genero).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.IcmsSubstituto).HasDefaultValueSql("0");
            entity.Property(e => e.IdOs).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.IdPrevenda).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.MvaSt).HasDefaultValueSql("0");
            entity.Property(e => e.Pagou).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.Pfcpufdest).HasDefaultValueSql("0");
            entity.Property(e => e.Picmsinter).HasDefaultValueSql("0");
            entity.Property(e => e.Picmsinterpart).HasDefaultValueSql("0");
            entity.Property(e => e.Picmsufdest).HasDefaultValueSql("0");
            entity.Property(e => e.PocIcms).HasDefaultValueSql("0");
            entity.Property(e => e.PocReducao).HasDefaultValueSql("0");
            entity.Property(e => e.PorcCofins).HasDefaultValueSql("0");
            entity.Property(e => e.PorcCofinsSt).HasDefaultValueSql("0");
            entity.Property(e => e.PorcIbpt).HasDefaultValueSql("0");
            entity.Property(e => e.PorcIpi).HasDefaultValueSql("0");
            entity.Property(e => e.PorcPis).HasDefaultValueSql("0");
            entity.Property(e => e.PorcPisSt).HasDefaultValueSql("0");
            entity.Property(e => e.PorcSt).HasDefaultValueSql("0");
            entity.Property(e => e.QtTotal).HasDefaultValueSql("1");
            entity.Property(e => e.Quant).HasDefaultValueSql("1");
            entity.Property(e => e.QuantEstorno).HasDefaultValueSql("0");
            entity.Property(e => e.Sequence).ValueGeneratedOnAdd();
            entity.Property(e => e.SequenciaItem).HasDefaultValue(0);
            entity.Property(e => e.St).HasDefaultValueSql("0");
            entity.Property(e => e.Tamanho).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Un).HasDefaultValueSql("'UN'::character varying");
            entity.Property(e => e.Vbcfcpufdest).HasDefaultValueSql("0");
            entity.Property(e => e.Vbcufdest).HasDefaultValueSql("0");
            entity.Property(e => e.Vfcpufdest).HasDefaultValueSql("0");
            entity.Property(e => e.Vicmsufdest).HasDefaultValueSql("0");
            entity.Property(e => e.Vicmsufremt).HasDefaultValueSql("0");
            entity.Property(e => e.VlAproxImposto).HasDefaultValueSql("0");
            entity.Property(e => e.VlBaseCofins).HasDefaultValueSql("0");
            entity.Property(e => e.VlBaseCofinsSt).HasDefaultValueSql("0");
            entity.Property(e => e.VlBaseIcms).HasDefaultValueSql("0");
            entity.Property(e => e.VlBaseIpi).HasDefaultValueSql("0");
            entity.Property(e => e.VlBasePis).HasDefaultValueSql("0");
            entity.Property(e => e.VlBasePisSt).HasDefaultValueSql("0");
            entity.Property(e => e.VlBaseRetido).HasDefaultValueSql("0");
            entity.Property(e => e.VlBaseSt).HasDefaultValueSql("0");
            entity.Property(e => e.VlCofins).HasDefaultValueSql("0");
            entity.Property(e => e.VlCofinsSt).HasDefaultValueSql("0");
            entity.Property(e => e.VlComissao).HasDefaultValueSql("0");
            entity.Property(e => e.VlCreditoIcms).HasDefaultValueSql("0");
            entity.Property(e => e.VlCusto).HasDefaultValueSql("0");
            entity.Property(e => e.VlIcms).HasDefaultValueSql("0");
            entity.Property(e => e.VlIcmsRet).HasDefaultValueSql("0");
            entity.Property(e => e.VlIpi).HasDefaultValueSql("0");
            entity.Property(e => e.VlPis).HasDefaultValueSql("0");
            entity.Property(e => e.VlPisSt).HasDefaultValueSql("0");
            entity.Property(e => e.VlSt).HasDefaultValueSql("0");
            entity.Property(e => e.VlUnid).HasDefaultValueSql("0");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.NfceProdutoSaida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("nfce_produto_saida_fk2");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.NfceProdutoSaida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("nfce_produto_saida_fk1");

            entity.HasOne(d => d.ProdutoEstoque).WithMany(p => p.NfceProdutoSaida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("nfce_produto_saida_fk6");

            entity.HasOne(d => d.NfceSaida).WithMany(p => p.NfceProdutoSaida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("nfce_produto_saida_fk");
        });

        modelBuilder.Entity<NfceSaida>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Empresa }).HasName("nfce_saidas_pkey");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Alterado).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.Caixa).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.CdCarga).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.CdSituacao).HasDefaultValueSql("'01'::character varying");
            entity.Property(e => e.CdUf).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdVendedor).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.Cfop).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.ChaveAcessoNfe).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.ChaveNfeSaida).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Data).HasDefaultValueSql("('now'::text)::date");
            entity.Property(e => e.Delivery).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.EnviouNaoCancelada).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.Frete).HasDefaultValueSql("0");
            entity.Property(e => e.IdConvenio).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.IdMedico).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.IdPaciente).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.IdPontoVenda).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.LocalSalvoNota).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NmOperador).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrAutorizacaoNfe).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrNotaFiscal).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrProtoCancelamento).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrProtocoloInutilizacao).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Observacao).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.PagaComissao).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.Pagou).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.Requisicao).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Sequence).ValueGeneratedOnAdd();
            entity.Property(e => e.TabelaVenda).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TxtJustificativaCancelamento).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TxtObsNf).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.VlDescGlobal).HasDefaultValueSql("0");
            entity.Property(e => e.VlOutro).HasDefaultValueSql("0");
            entity.Property(e => e.XmNf).HasDefaultValueSql("''::text");

            entity.HasOne(d => d.ClienteNavigation).WithMany(p => p.NfceSaida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("nfce_saidas_fk2");

            entity.HasOne(d => d.EmpresaNavigation).WithMany(p => p.NfceSaida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("nfce_saidas_fk1");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.NfceSaida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("nfce_saidas_fk");
        });

        modelBuilder.Entity<ObsNf>(entity =>
        {
            entity.HasKey(e => e.NrLanc).HasName("obs_nf_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.TxtObs).HasDefaultValueSql("''::character varying(1)");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.ObsNfs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("obs_nf_fk");
        });

        modelBuilder.Entity<Older>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("older_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Olders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("older_fk");
        });

        modelBuilder.Entity<OlderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("older_items_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Older).WithMany(p => p.OlderItems)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("older_items_older_id_fkey");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.OlderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("older_items_fk");

            entity.HasOne(d => d.ProdutoEstoque).WithMany(p => p.OlderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("older_items_fk6");
        });

        modelBuilder.Entity<OrcamentoCab>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orcamento_cab_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Gerado).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.Sequencia).ValueGeneratedOnAdd();

            entity.HasOne(d => d.CdPlanoNavigation).WithMany(p => p.OrcamentoCabs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orcamento_cab_fk4");

            entity.HasOne(d => d.EmpresaNavigation).WithMany(p => p.OrcamentoCabs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orcamento_cab_fk3");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.OrcamentoCabs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orcamento_cab_fk1");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.OrcamentoCabs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orcamento_cab_fk");

            entity.HasOne(d => d.Funcionario).WithMany(p => p.OrcamentoCabs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orcamento_cab_fk2");
        });

        modelBuilder.Entity<OrcamentoIten>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orcamento_itens_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Qtde).HasDefaultValueSql("1");
            entity.Property(e => e.Sequencia).ValueGeneratedOnAdd();

            entity.HasOne(d => d.CdPlanoNavigation).WithMany(p => p.OrcamentoItens)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orcamento_itens_fk2");

            entity.HasOne(d => d.ProdutoEstoque).WithMany(p => p.OrcamentoItens)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orcamento_itens_fk1");

            entity.HasOne(d => d.OrcamentoCab).WithMany(p => p.OrcamentoItens)
                .HasPrincipalKey(p => new { p.Id, p.Sequencia, p.Unity, p.Empresa, p.IdCliente, p.Gerado, p.IdFuncionario, p.PercentualComissao, p.CdPlano })
                .HasForeignKey(d => new { d.IdCab, d.SequenciaCab, d.Unity, d.Empresa, d.IdCliente, d.Gerado, d.IdFuncionario, d.PercentualComissao, d.CdPlano })
                .HasConstraintName("orcamento_itens_fk");
        });

        modelBuilder.Entity<OrcamentoServico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orcamento_servicos_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Lado).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Qtde).HasDefaultValueSql("1");
            entity.Property(e => e.Sequencia).ValueGeneratedOnAdd();

            entity.HasOne(d => d.IdServicoNavigation).WithMany(p => p.OrcamentoServicos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orcamento_servicos_fk1");

            entity.HasOne(d => d.OrcamentoCab).WithMany(p => p.OrcamentoServicos)
                .HasPrincipalKey(p => new { p.Id, p.Sequencia, p.Unity, p.Empresa, p.IdCliente, p.Gerado, p.IdFuncionario, p.PercentualComissao })
                .HasForeignKey(d => new { d.IdCab, d.SequenciaCab, d.Unity, d.Empresa, d.IdCliente, d.Gerado, d.IdFuncionario, d.PercentualComissao })
                .HasConstraintName("orcamento_servicos_fk");
        });

        modelBuilder.Entity<OrigemCst>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("origem_cst_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<OsTabelaPreco>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("os_tabela_preco_pkey");

            entity.Property(e => e.DtUltAlteracao).HasDefaultValueSql("now()");

            entity.HasOne(d => d.CdServicoNavigation).WithMany(p => p.OsTabelaPrecos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("os_tabela_preco_fk1");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.OsTabelaPrecos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("os_tabela_preco_fk");
        });

        modelBuilder.Entity<PagtosParciaisCp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pagtos_parciais_cp_pkey");

            entity.Property(e => e.Acrescimo).HasDefaultValueSql("0.00");
            entity.Property(e => e.Desconto).HasDefaultValueSql("0.00");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.ValorPago).HasDefaultValueSql("0");
            entity.Property(e => e.ValorRestante).HasDefaultValueSql("0");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.PagtosParciaisCps)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pagtos_parciais_cp_fk");

            entity.HasOne(d => d.IdContasPagarNavigation).WithMany(p => p.PagtosParciaisCps).HasConstraintName("pagtos_parciais_cp_fk1");
        });

        modelBuilder.Entity<PagtosParciaisCr>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pagtos_parciais_cr_pkey");

            entity.Property(e => e.Acrescimo).HasDefaultValueSql("0.00");
            entity.Property(e => e.Desconto).HasDefaultValueSql("0.00");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.ValorPago).HasDefaultValueSql("0");
            entity.Property(e => e.ValorRestante).HasDefaultValueSql("0");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.PagtosParciaisCrs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pagtos_parciais_cr_fk");

            entity.HasOne(d => d.NrContaNavigation).WithMany(p => p.PagtosParciaisCrs).HasConstraintName("pagtos_parciais_cr_fk1");
        });

        modelBuilder.Entity<PerfilLoja>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("perfil_loja_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.PerfilLojas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("perfil_loja_fk");
        });

        modelBuilder.Entity<Permissao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("permissao_pkey");

            entity.Property(e => e.Descricao).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<PlanoDeCaixa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("plano_de_caixa_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.PlanoDeCaixas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plano_de_caixa_fk");
        });

        modelBuilder.Entity<PlanoEstoque>(entity =>
        {
            entity.HasKey(e => e.CdPlano).HasName("plano_estoque_pkey");

            entity.Property(e => e.Ativo).HasDefaultValue(true);
            entity.Property(e => e.EFiscal).HasDefaultValue(false);
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.PlanoEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plano_estoque_fk");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.PlanoEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plano_estoque_fk1");
        });

        modelBuilder.Entity<PlanoSimultaneo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("plano_simultaneos_pkey");

            entity.HasOne(d => d.CdPlanoPrincNavigation).WithMany(p => p.PlanoSimultaneoCdPlanoPrincNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plano_simultaneos_fk1");

            entity.HasOne(d => d.CdPlanoReplicaNavigation).WithMany(p => p.PlanoSimultaneoCdPlanoReplicaNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plano_simultaneos_fk2");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.PlanoSimultaneos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plano_simultaneos_fk");
        });

        modelBuilder.Entity<ProductDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_details_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.ProductDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_details_fk");

            entity.HasOne(d => d.ProdutoEstoque).WithMany(p => p.ProductDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("older_items_fk6");
        });

        modelBuilder.Entity<ProdutoEntradum>(entity =>
        {
            entity.HasKey(e => e.Nr).HasName("produto_entrada_pkey");

            entity.Property(e => e.AnoFabVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.AnoVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.BIcms).HasDefaultValueSql("0");
            entity.Property(e => e.BaseIpi).HasDefaultValueSql("0");
            entity.Property(e => e.CapcMaxLotVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CapcMaxTracVeic).HasDefaultValueSql("0");
            entity.Property(e => e.ChasiVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CilindradasVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CondVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Cor).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CorVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.DescCorVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.DistEixosVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.EspecVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.FcpBase).HasDefaultValueSql("0");
            entity.Property(e => e.FcpPorc).HasDefaultValueSql("0");
            entity.Property(e => e.FcpValor).HasDefaultValueSql("0");
            entity.Property(e => e.FreteProduto).HasDefaultValueSql("0");
            entity.Property(e => e.FreteRateio).HasDefaultValueSql("0");
            entity.Property(e => e.Genero).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.IdCorVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.IdMarcaVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.IdVinVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.ImpBaseIcmsStRet).HasDefaultValueSql("0");
            entity.Property(e => e.ImpBaseStRet).HasDefaultValueSql("0");
            entity.Property(e => e.ImpIcmsPropSubs).HasDefaultValueSql("0");
            entity.Property(e => e.ImpPst).HasDefaultValueSql("0");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.MovEstoque).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.NrMotorVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.PesoBrutoVeic).HasDefaultValueSql("0");
            entity.Property(e => e.PesoLiquidoVeic).HasDefaultValueSql("0");
            entity.Property(e => e.PorcCofinsSt).HasDefaultValueSql("0");
            entity.Property(e => e.PorcConfins).HasDefaultValueSql("0");
            entity.Property(e => e.PorcIcms).HasDefaultValueSql("0");
            entity.Property(e => e.PorcIpi).HasDefaultValueSql("0");
            entity.Property(e => e.PorcPis).HasDefaultValueSql("0");
            entity.Property(e => e.PorcPisSt).HasDefaultValueSql("0");
            entity.Property(e => e.PorcSt).HasDefaultValueSql("0");
            entity.Property(e => e.PotenciaMotorVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.QtTotal).HasDefaultValueSql("0");
            entity.Property(e => e.RestricaoVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.SerialVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Tamanho).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TpCombustVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TpOperacaoVeic).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.TpPinturaVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TpVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Transferiu).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.Unidade).HasDefaultValueSql("'UN'::character varying");
            entity.Property(e => e.VIcmsDeson).HasDefaultValueSql("0.0");
            entity.Property(e => e.VlBaseCofinsSt).HasDefaultValueSql("0");
            entity.Property(e => e.VlBaseConfins).HasDefaultValueSql("0");
            entity.Property(e => e.VlBasePis).HasDefaultValueSql("0");
            entity.Property(e => e.VlBasePisSt).HasDefaultValueSql("0");
            entity.Property(e => e.VlBaseSt).HasDefaultValueSql("0");
            entity.Property(e => e.VlCofinsSt).HasDefaultValueSql("0");
            entity.Property(e => e.VlConfins).HasDefaultValueSql("0");
            entity.Property(e => e.VlDespAcess).HasDefaultValueSql("0");
            entity.Property(e => e.VlIcmsSt).HasDefaultValueSql("0");
            entity.Property(e => e.VlOutras).HasDefaultValueSql("0");
            entity.Property(e => e.VlPis).HasDefaultValueSql("0");
            entity.Property(e => e.VlPisSt).HasDefaultValueSql("0");
            entity.Property(e => e.VlSeguro).HasDefaultValueSql("0");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.ProdutoEntrada).HasConstraintName("produto_entrada_fk2");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.ProdutoEntrada)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produto_entrada_fk1");

            entity.HasOne(d => d.ProdutoEstoque).WithMany(p => p.ProdutoEntrada)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produto_entrada_fk6");

            entity.HasOne(d => d.Entrada).WithMany(p => p.ProdutoEntrada)
                .HasPrincipalKey(p => new { p.Nr, p.CdEmpresa, p.CdGrupoEstoque, p.TpEntrada })
                .HasForeignKey(d => new { d.NrEntrada, d.CdEmpresa, d.CdPlano, d.TpEntrada })
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("produto_entrada_fk");
        });

        modelBuilder.Entity<ProdutoEstoque>(entity =>
        {
            entity.HasKey(e => new { e.CdProduto, e.Unity }).HasName("produto_estoque_idx");

            entity.Property(e => e.CdProduto).ValueGeneratedOnAdd();
            entity.Property(e => e.Ativo).HasDefaultValueSql("'S'::character varying");
            entity.Property(e => e.Balanca).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Capacidade).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdClassFiscal).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdCsosn).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdGenero).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdInterno).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdSeq).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.CdUni).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Cest).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CfoDentro).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CfoFora).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.ClasseTerapeutica).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CodEspecie).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CodMargem).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CodigoBalanca).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CodigoDcb).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.ControlaEstoque).HasDefaultValueSql("'S'::character varying");
            entity.Property(e => e.Corredor).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CstCofins).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CstDentro1).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CstDentro2).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CstFora1).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CstFora2).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CstIpi).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CstPis).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.DescRef).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.DescricaoProduto).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.DtAltPreco).HasDefaultValueSql("now()");
            entity.Property(e => e.DtCadastro).HasDefaultValueSql("now()");
            entity.Property(e => e.EcfIcmSt).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Embalagem).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.EntBcSt).HasDefaultValueSql("0");
            entity.Property(e => e.EntIcmsSt).HasDefaultValueSql("0");
            entity.Property(e => e.EntMva).HasDefaultValueSql("0");
            entity.Property(e => e.EntPorcSt).HasDefaultValueSql("0");
            entity.Property(e => e.EntReducaoBc).HasDefaultValueSql("0");
            entity.Property(e => e.ExTipi).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Frete).HasDefaultValueSql("0");
            entity.Property(e => e.Iat).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.IcmsDentro).HasDefaultValueSql("0");
            entity.Property(e => e.IcmsFora).HasDefaultValueSql("0");
            entity.Property(e => e.IcmsSubsAliq).HasDefaultValueSql("0");
            entity.Property(e => e.IcmsSubsReducao).HasDefaultValueSql("0");
            entity.Property(e => e.IcmsSubsReducaoAliq).HasDefaultValueSql("0");
            entity.Property(e => e.IdMarca).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.Ippt).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.LancLivro).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.LetraCurvaabc).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.LucroPor).HasDefaultValueSql("0");
            entity.Property(e => e.Mva).HasDefaultValueSql("0");
            entity.Property(e => e.Mvaajustado).HasDefaultValueSql("0");
            entity.Property(e => e.NomeImagem).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.OperacionalPor).HasDefaultValueSql("0");
            entity.Property(e => e.PesoBruto).HasDefaultValueSql("0");
            entity.Property(e => e.PesoLiquido).HasDefaultValueSql("0");
            entity.Property(e => e.PorcAliqInterna).HasDefaultValueSql("0");
            entity.Property(e => e.PorcIpi).HasDefaultValueSql("0");
            entity.Property(e => e.PorcLimiteAprazo).HasDefaultValueSql("100");
            entity.Property(e => e.PorcLimiteAvista).HasDefaultValueSql("100");
            entity.Property(e => e.PorcLimiteBoleto).HasDefaultValueSql("100");
            entity.Property(e => e.PorcLimiteCheque).HasDefaultValueSql("100");
            entity.Property(e => e.PorcLimiteCredito).HasDefaultValueSql("100");
            entity.Property(e => e.PorcLimiteCreditoparc).HasDefaultValueSql("100");
            entity.Property(e => e.PorcLimiteDebito).HasDefaultValueSql("100");
            entity.Property(e => e.PorcSaida).HasDefaultValueSql("0");
            entity.Property(e => e.PorcSubst).HasDefaultValueSql("0");
            entity.Property(e => e.PorcVendaAPrazo).HasDefaultValueSql("0");
            entity.Property(e => e.PorcVendaCc).HasDefaultValueSql("0");
            entity.Property(e => e.PorcVendaCd).HasDefaultValueSql("0");
            entity.Property(e => e.Prateleira).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Principal).HasDefaultValueSql("'S'::character varying");
            entity.Property(e => e.QtDiasVenc).HasDefaultValue((short)0);
            entity.Property(e => e.QtTotal).HasDefaultValueSql("1");
            entity.Property(e => e.QtUnitario).HasDefaultValueSql("1");
            entity.Property(e => e.QtdeMax).HasDefaultValueSql("0");
            entity.Property(e => e.RegMs).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Sequence).ValueGeneratedOnAdd();
            entity.Property(e => e.StEcf).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Suspenso).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.TaxaCofins).HasDefaultValueSql("0");
            entity.Property(e => e.TaxaIssqn).HasDefaultValueSql("0");
            entity.Property(e => e.TaxaPis).HasDefaultValueSql("0");
            entity.Property(e => e.TipoItemSped).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TotalizadorParcial).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TpItem).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Transferiu).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.TxtObs).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.VlAVista).HasDefaultValueSql("0");
            entity.Property(e => e.VlAtacado).HasDefaultValueSql("0");
            entity.Property(e => e.VlBoleto).HasDefaultValueSql("0");
            entity.Property(e => e.VlCc).HasDefaultValueSql("0");
            entity.Property(e => e.VlCheque).HasDefaultValueSql("0");
            entity.Property(e => e.VlComanda).HasDefaultValueSql("0");
            entity.Property(e => e.VlCreditoParcelado).HasDefaultValueSql("0");
            entity.Property(e => e.VlCusto).HasDefaultValueSql("0");
            entity.Property(e => e.VlCustoVariavel).HasDefaultValueSql("0");
            entity.Property(e => e.VlDeb).HasDefaultValueSql("0");
            entity.Property(e => e.VlMedia).HasDefaultValueSql("0");
            entity.Property(e => e.VlPequena).HasDefaultValueSql("0");
            entity.Property(e => e.VlPrazo).HasDefaultValueSql("0");
            entity.Property(e => e.VlTabelaGov).HasDefaultValueSql("0");

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.ProdutoEstoques).HasConstraintName("produto_estoque_fk3");

            entity.HasOne(d => d.CdGrupoNavigation).WithMany(p => p.ProdutoEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produto_estoque_fk");

            entity.HasOne(d => d.CdRefNavigation).WithMany(p => p.ProdutoEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produto_estoque_fk1");

            entity.HasOne(d => d.Section).WithMany(p => p.ProdutoEstoques)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_section_id");

            entity.HasOne(d => d.SectionItem).WithMany(p => p.ProdutoEstoques)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_section_item_id");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.ProdutoEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produto_estoque_fk2");
        });

        modelBuilder.Entity<ProdutoSaidum>(entity =>
        {
            entity.HasKey(e => e.Nr).HasName("produto_saida_pkey");

            entity.Property(e => e.AnoFabVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.AnoVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.BaixaEstoque).HasDefaultValueSql("'S'::character varying");
            entity.Property(e => e.Cancelou).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.CapcMaxLotVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CapcMaxTracVeic).HasDefaultValueSql("0");
            entity.Property(e => e.CdInterno).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.ChasiVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CilindradasVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CondVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Cor).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CorVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CstCofins).HasDefaultValueSql("0");
            entity.Property(e => e.CstPis).HasDefaultValueSql("0");
            entity.Property(e => e.DescComplementoNome).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.DescCorVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.DescRateio).HasDefaultValueSql("0");
            entity.Property(e => e.DistEixosVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.EspecVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Genero).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.IcmsSubstituto).HasDefaultValueSql("0");
            entity.Property(e => e.IdCorVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.IdMarcaVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.IdSerieKit).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.IdSerieProduto).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.IdVinVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.MvaSt).HasDefaultValueSql("0");
            entity.Property(e => e.NrMotorVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Pagou).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.PesoBrutoVeic).HasDefaultValueSql("0");
            entity.Property(e => e.PesoLiquidoVeic).HasDefaultValueSql("0");
            entity.Property(e => e.Pfcpufdest).HasDefaultValueSql("0");
            entity.Property(e => e.Picmsinter).HasDefaultValueSql("0");
            entity.Property(e => e.Picmsinterpart).HasDefaultValueSql("0");
            entity.Property(e => e.Picmsufdest).HasDefaultValueSql("0");
            entity.Property(e => e.PocIcms).HasDefaultValueSql("0");
            entity.Property(e => e.PocReducao).HasDefaultValueSql("0");
            entity.Property(e => e.PorcCofins).HasDefaultValueSql("0");
            entity.Property(e => e.PorcCofinsSt).HasDefaultValueSql("0");
            entity.Property(e => e.PorcIbpt).HasDefaultValueSql("0");
            entity.Property(e => e.PorcIpi).HasDefaultValueSql("0");
            entity.Property(e => e.PorcPis).HasDefaultValueSql("0");
            entity.Property(e => e.PorcPisSt).HasDefaultValueSql("0");
            entity.Property(e => e.PorcSt).HasDefaultValueSql("0");
            entity.Property(e => e.PotenciaMotorVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.QtTotal).HasDefaultValueSql("1");
            entity.Property(e => e.Quant).HasDefaultValueSql("1");
            entity.Property(e => e.QuantEstorno).HasDefaultValueSql("0");
            entity.Property(e => e.RestricaoVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.SequenciaItem).HasDefaultValue(0);
            entity.Property(e => e.SerialVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.St).HasDefaultValueSql("0");
            entity.Property(e => e.Tamanho).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TpCombustVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TpOperacaoVeic).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.TpPinturaVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TpVeic).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Un).HasDefaultValueSql("'UN'::character varying");
            entity.Property(e => e.UtilizarIdInternoNfe).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.Vbcfcpufdest).HasDefaultValueSql("0");
            entity.Property(e => e.Vbcufdest).HasDefaultValueSql("0");
            entity.Property(e => e.Vfcpufdest).HasDefaultValueSql("0");
            entity.Property(e => e.Vicmsufdest).HasDefaultValueSql("0");
            entity.Property(e => e.Vicmsufremt).HasDefaultValueSql("0");
            entity.Property(e => e.VlAproxImposto).HasDefaultValueSql("0");
            entity.Property(e => e.VlBaseCofins).HasDefaultValueSql("0");
            entity.Property(e => e.VlBaseCofinsSt).HasDefaultValueSql("0");
            entity.Property(e => e.VlBaseIcms).HasDefaultValueSql("0");
            entity.Property(e => e.VlBaseIpi).HasDefaultValueSql("0");
            entity.Property(e => e.VlBasePis).HasDefaultValueSql("0");
            entity.Property(e => e.VlBasePisSt).HasDefaultValueSql("0");
            entity.Property(e => e.VlBaseRetido).HasDefaultValueSql("0");
            entity.Property(e => e.VlBaseSt).HasDefaultValueSql("0");
            entity.Property(e => e.VlCofins).HasDefaultValueSql("0");
            entity.Property(e => e.VlCofinsSt).HasDefaultValueSql("0");
            entity.Property(e => e.VlComissao).HasDefaultValueSql("0");
            entity.Property(e => e.VlCreditoIcms).HasDefaultValueSql("0");
            entity.Property(e => e.VlCusto).HasDefaultValueSql("0");
            entity.Property(e => e.VlIcms).HasDefaultValueSql("0");
            entity.Property(e => e.VlIcmsRet).HasDefaultValueSql("0");
            entity.Property(e => e.VlIpi).HasDefaultValueSql("0");
            entity.Property(e => e.VlPis).HasDefaultValueSql("0");
            entity.Property(e => e.VlPisSt).HasDefaultValueSql("0");
            entity.Property(e => e.VlSt).HasDefaultValueSql("0");
            entity.Property(e => e.VlUnid).HasDefaultValueSql("0");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.ProdutoSaida).HasConstraintName("produto_saida_fk1");

            entity.HasOne(d => d.CdPlanoNavigation).WithMany(p => p.ProdutoSaida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produto_saida_fk3");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.ProdutoSaida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produto_saida_fk");

            entity.HasOne(d => d.ProdutoEstoque).WithMany(p => p.ProdutoSaida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produto_saida_fk4");

            entity.HasOne(d => d.Saida).WithMany(p => p.ProdutoSaida)
                .HasPrincipalKey(p => new { p.NrLanc, p.Empresa, p.CdGrupoEstoque, p.TpSaida, p.CdSituacao })
                .HasForeignKey(d => new { d.NrSaida, d.CdEmpresa, d.CdPlano, d.TpSaida, d.CdSituacao })
                .HasConstraintName("produto_saida_fk2");
        });

        modelBuilder.Entity<ProdutosForn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("produtos_forn_pkey");

            entity.Property(e => e.CdBarra).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.IdProdutoExterno).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.ProdutosForns)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produtos_forn_fk");

            entity.HasOne(d => d.Fornecedor).WithMany(p => p.ProdutosForns)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produtos_forn_fk3");

            entity.HasOne(d => d.ProdutoEstoque).WithMany(p => p.ProdutosForns)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produtos_forn_fk6");
        });

        modelBuilder.Entity<ProtocoloEstadoNcm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("protocolo_estado_ncm_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.Iva).HasDefaultValueSql("0");
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.RedIcms).HasDefaultValueSql("0");
            entity.Property(e => e.RedSt).HasDefaultValueSql("0");
            entity.Property(e => e.St).HasDefaultValueSql("0");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.ProtocoloEstadoNcms)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("protocolo_estado_ncm_fk");
        });

        modelBuilder.Entity<ReferenciaEstoque>(entity =>
        {
            entity.HasKey(e => e.CdRef).HasName("referencia_estoque_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.ReferenciaEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("referencia_estoque_fk");
        });

        modelBuilder.Entity<RetiradaNfe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("retirada_nfe_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.Uf).IsFixedLength();

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.RetiradaNves)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("retirada_nfe_fk");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.RetiradaNves)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("retirada_nfe_fk1");
        });

        modelBuilder.Entity<Saida>(entity =>
        {
            entity.HasKey(e => e.NrLanc).HasName("saidas_pkey");

            entity.Property(e => e.CdCarga).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.CdSituacao).HasDefaultValueSql("'01'::character varying");
            entity.Property(e => e.CdUf).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdVendedor).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.Cfop).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.ChaveAcessoNfe).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.ChaveNfeSaida).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CnpjMarket).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Data).HasDefaultValueSql("('now'::text)::date");
            entity.Property(e => e.IdConvenio).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.IdEndEntrega).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.IdEndRetirada).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.IdMedico).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.IdPaciente).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.IdPontoVenda).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.IdTipoIndicador).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.IdTipoOperacaoIntermediador).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.LocalSalvoNota).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NmMarket).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrAutorizacaoNfe).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrNotaFiscal).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrProtoCancelamento).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Observacao).HasDefaultValueSql("''::text");
            entity.Property(e => e.PagaComissao).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.Pagou).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.Requisicao).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.SerieNf).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TabelaVenda).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TpOperacao).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.TxtJustificativaCancelamento).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TxtObsNf).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.VlDescGlobal).HasDefaultValueSql("0");
            entity.Property(e => e.VlOutro).HasDefaultValueSql("0");
            entity.Property(e => e.VlSeguro).HasDefaultValueSql("0");
            entity.Property(e => e.XmNf).HasDefaultValueSql("''::text");

            entity.HasOne(d => d.CdGrupoEstoqueNavigation).WithMany(p => p.Saida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("saidas_fk2");

            entity.HasOne(d => d.ClienteNavigation).WithMany(p => p.Saida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("saidas_fk1");

            entity.HasOne(d => d.EmpresaNavigation).WithMany(p => p.Saida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("saidas_fk");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.Saida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("saidas_fk3");
        });

        modelBuilder.Entity<SaidaNotasDevolucao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("saida_notas_devolucao_pkey");

            entity.Property(e => e.ChaveAcesso).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.NrSaidaNavigation).WithMany(p => p.SaidaNotasDevolucaos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("saida_notas_devolucao_fk");
        });

        modelBuilder.Entity<SaidasVolume>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("saidas_volumes_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.NrSaidaNavigation).WithMany(p => p.SaidasVolumes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("saidas_volumes_fk1");
        });

        modelBuilder.Entity<SaldoEstoque>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("saldo_estoque_pk");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.QuantE).HasDefaultValueSql("0");
            entity.Property(e => e.QuantF).HasDefaultValueSql("0");
            entity.Property(e => e.QuantV).HasDefaultValueSql("0");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.SaldoEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("saldo_estoque_fk");

            entity.HasOne(d => d.CdPlanoNavigation).WithMany(p => p.SaldoEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("saldo_estoque_fk2");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.SaldoEstoques).HasConstraintName("saldo_estoque_fk1");

            entity.HasOne(d => d.ProdutoEstoque).WithMany(p => p.SaldoEstoques)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("saldo_estoque_fk6");
        });

        modelBuilder.Entity<SangriaCaixa>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CdEmpresa }).HasName("sangria_caixa_pkey");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Data).HasDefaultValueSql("('now'::text)::date");
            entity.Property(e => e.Hora).HasDefaultValueSql("('now'::text)::time with time zone");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.Sequence).ValueGeneratedOnAdd();
            entity.Property(e => e.Tipo).HasDefaultValueSql("''::character varying");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.SangriaCaixas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sangria_caixa_fk");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.SangriaCaixas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sangria_caixa_fk2");

            entity.HasOne(d => d.NfceAberturaCaixa).WithMany(p => p.SangriaCaixas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sangria_caixa_fk1");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sections_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Category).WithMany(p => p.Sections)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("sections_category_id_fkey");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.Sections).HasConstraintName("sections_id_empresa_fkey");
        });

        modelBuilder.Entity<SectionItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("section_items_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Section).WithMany(p => p.SectionItems)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("section_items_section_id_fkey");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.SectionItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("section_items_fk");
        });

        modelBuilder.Entity<Servico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("servicos_pkey");

            entity.Property(e => e.PagaComissao).HasDefaultValue(true);

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.Servicos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("servicos_fk1");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.Servicos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("servicos_fk");
        });

        modelBuilder.Entity<TabelaAnp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tabela_anp_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval(('public.gen_tabela_anp_id'::text)::regclass)");
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<TipoNf>(entity =>
        {
            entity.HasKey(e => e.CdTipoNf).HasName("tipo_nf_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Transportadora>(entity =>
        {
            entity.HasKey(e => new { e.CdTransportadora, e.Unity }).HasName("transportadora_idx1");

            entity.Property(e => e.CdTransportadora).ValueGeneratedOnAdd();
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.CdCidadeNavigation).WithMany(p => p.Transportadoras).HasConstraintName("transportadora_fk1");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.Transportadoras)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("transportadora_fk");
        });

        modelBuilder.Entity<UairangoConfiguraco>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("uairango_configuracoes_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.UairangoConfiguracos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("uairango_configuracoes_fk");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.UairangoConfiguracos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("uairango_configuracoes_fk1");
        });

        modelBuilder.Entity<UairangoCulinaria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("uairango_culinarias_pkey");
        });

        modelBuilder.Entity<UairangoEmpresaCategorium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("uairango_empresa_categoria_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.UairangoEmpresaCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("uairango_empresa_categoria_fk1");

            entity.HasOne(d => d.CdGrupoNavigation).WithMany(p => p.UairangoEmpresaCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("uairango_empresa_categoria_fk2");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.UairangoEmpresaCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("uairango_empresa_categoria_fk");
        });

        modelBuilder.Entity<UairangoFormasPagamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("uairango_formas_pagamento_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.EmpresaNavigation).WithMany(p => p.UairangoFormasPagamentos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("uairango_formas_pagamento_fk1");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.UairangoFormasPagamentos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("uairango_formas_pagamento_fk");
        });

        modelBuilder.Entity<UairangoOpcoesCategorium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("uairango_opcoes_categoria_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.CdGrupoNavigation).WithMany(p => p.UairangoOpcoesCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("uairango_opcoes_categoria_fk");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.UairangoOpcoesCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("uairango_opcoes_categoria_fk1");
        });

        modelBuilder.Entity<UairangoOpcoesProduto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("uairango_opcoes_produto_pkey");

            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.ProdutoEstoque).WithMany(p => p.UairangoOpcoesProdutos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("uairango_opcoes_produto_fk");
        });

        modelBuilder.Entity<UairangoPrazo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("uairango_prazos_pkey");

            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<UairangoRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("uairango_requests_pkey");

            entity.Property(e => e.Datahora).HasDefaultValueSql("now()");

            entity.HasOne(d => d.EmpresaNavigation).WithMany(p => p.UairangoRequests).HasConstraintName("uairango_requests_fk");
        });

        modelBuilder.Entity<UairangoToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("uairango_tokens_pkey");

            entity.Property(e => e.DataHoraGeracao).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<UnidadeMedida>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("unidade_medida_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.UnidadeMedida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("unidade_medida_fk2");
        });

        modelBuilder.Entity<Unity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("unity_pkey");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.NmUsuario).HasName("usuario_pkey");

            entity.Property(e => e.Ativo).HasDefaultValueSql("'S'::character varying");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.Usuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuario_fk1");
        });

        modelBuilder.Entity<UsuarioEmpresa>(entity =>
        {
            entity.HasKey(e => new { e.CdUsuario, e.CdEmpresa }).HasName("usuario_empresa_pkey");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.UsuarioEmpresas).HasConstraintName("usuario_empresa_fk1");

            entity.HasOne(d => d.CdUsuarioNavigation).WithMany(p => p.UsuarioEmpresas).HasConstraintName("usuario_empresa_fk");
        });

        modelBuilder.Entity<UsuarioFuncionario>(entity =>
        {
            entity.HasKey(e => new { e.CdFuncionario, e.NmUsuario }).HasName("usuario_funcionario_idx");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.NmUsuarioNavigation).WithMany(p => p.UsuarioFuncionarios).HasConstraintName("usuario_funcionario_fk1");

            entity.HasOne(d => d.Funcionario).WithMany(p => p.UsuarioFuncionarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuario_funcionario_fk");
        });

        modelBuilder.Entity<UsuarioPermissao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usuario_permissao_pkey");

            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");

            entity.HasOne(d => d.IdPermissaoNavigation).WithMany(p => p.UsuarioPermissaos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuario_permissao_fk1");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioPermissaos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuario_permissao_fk");
        });

        modelBuilder.Entity<Vendedor>(entity =>
        {
            entity.HasKey(e => new { e.CdFuncionario, e.CdEmpresa }).HasName("vendedor_pkey");

            entity.Property(e => e.ComissaoAPrazo).HasDefaultValueSql("0");
            entity.Property(e => e.ComissaoAVista).HasDefaultValueSql("0");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Integrated).HasDefaultValue(0);
            entity.Property(e => e.LastUpdate).HasDefaultValueSql("now()");
            entity.Property(e => e.TipoPagamento).HasDefaultValueSql("'V'::character varying");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.Vendedors).HasConstraintName("vendedor_fk_empresa");

            entity.HasOne(d => d.UnityNavigation).WithMany(p => p.Vendedors).HasConstraintName("vendedor_fk1");

            entity.HasOne(d => d.Funcionario).WithOne(p => p.VendedorNavigation).HasConstraintName("vendedor_fk");
        });
        modelBuilder.HasSequence("cte_doc_anterior_nfe_id_cte_doc_anterior_nfe_seq", "cte");
        modelBuilder.HasSequence("mdfe_chaves_id_seq", "mdfe");
        modelBuilder.HasSequence("mdfe_condutor_id_seq", "mdfe");
        modelBuilder.HasSequence("mdfe_id_seq", "mdfe");
        modelBuilder.HasSequence("mdfe_infcarregamento_id_seq", "mdfe");
        modelBuilder.HasSequence("mdfe_percurso_id_seq", "mdfe");
        modelBuilder.HasSequence("mdfe_reboque_id_seq", "mdfe");
        modelBuilder.HasSequence("mdfe_rodoviario_id_seq", "mdfe");
        modelBuilder.HasSequence("mdfe_seguro_id_seq", "mdfe");
        modelBuilder.HasSequence("seq_entrada_geral_1");
        modelBuilder.HasSequence("seq_entrada_geral_10");
        modelBuilder.HasSequence("seq_entrada_geral_11");
        modelBuilder.HasSequence("seq_entrada_geral_12");
        modelBuilder.HasSequence("seq_entrada_geral_13");
        modelBuilder.HasSequence("seq_entrada_geral_15");
        modelBuilder.HasSequence("seq_entradas_geral_1");
        modelBuilder.HasSequence("seq_entradas_geral_11");
        modelBuilder.HasSequence("seq_entradas_geral_2");
        modelBuilder.HasSequence("seq_fornecedor_geral_1");
        modelBuilder.HasSequence("seq_fornecedor_geral_10");
        modelBuilder.HasSequence("seq_fornecedor_geral_11");
        modelBuilder.HasSequence("seq_fornecedor_geral_12");
        modelBuilder.HasSequence("seq_fornecedor_geral_13");
        modelBuilder.HasSequence("seq_fornecedor_geral_15");
        modelBuilder.HasSequence("seq_fornecedor_geral_2");
        modelBuilder.HasSequence("seq_fotos_geral_1");
        modelBuilder.HasSequence("seq_fotos_geral_10");
        modelBuilder.HasSequence("seq_fotos_geral_11");
        modelBuilder.HasSequence("seq_fotos_geral_12");
        modelBuilder.HasSequence("seq_fotos_geral_13");
        modelBuilder.HasSequence("seq_fotos_geral_15");
        modelBuilder.HasSequence("seq_funcionario_geral_1");
        modelBuilder.HasSequence("seq_funcionario_geral_10");
        modelBuilder.HasSequence("seq_funcionario_geral_11");
        modelBuilder.HasSequence("seq_funcionario_geral_12");
        modelBuilder.HasSequence("seq_funcionario_geral_13");
        modelBuilder.HasSequence("seq_funcionario_geral_15");
        modelBuilder.HasSequence("seq_funcionario_geral_2");
        modelBuilder.HasSequence("seq_nfce_abertura_caixa_geral_10");
        modelBuilder.HasSequence("seq_nfce_abertura_caixa_geral_11");
        modelBuilder.HasSequence("seq_nfce_abertura_caixa_geral_12");
        modelBuilder.HasSequence("seq_nfce_abertura_caixa_geral_13");
        modelBuilder.HasSequence("seq_nfce_abertura_caixa_geral_15");
        modelBuilder.HasSequence("seq_nfce_forma_pgt_geral_13");
        modelBuilder.HasSequence("seq_nfce_produto_saida_geral_13");
        modelBuilder.HasSequence("seq_nfce_saidas_geral_1");
        modelBuilder.HasSequence("seq_nfce_saidas_geral_10");
        modelBuilder.HasSequence("seq_nfce_saidas_geral_11");
        modelBuilder.HasSequence("seq_nfce_saidas_geral_12");
        modelBuilder.HasSequence("seq_nfce_saidas_geral_13");
        modelBuilder.HasSequence("seq_nfce_saidas_geral_15");
        modelBuilder.HasSequence("seq_produto_estoque_geral_1");
        modelBuilder.HasSequence("seq_produto_estoque_geral_3");
        modelBuilder.HasSequence("seq_produto_geral_1").StartsAt(832L);
        modelBuilder.HasSequence("seq_produto_geral_10");
        modelBuilder.HasSequence("seq_produto_geral_11");
        modelBuilder.HasSequence("seq_produto_geral_12");
        modelBuilder.HasSequence("seq_produto_geral_13");
        modelBuilder.HasSequence("seq_produto_geral_15");
        modelBuilder.HasSequence("seq_produto_geral_2");
        modelBuilder.HasSequence("seq_sangria_caixa_geral_13");
        modelBuilder.HasSequence("seq_transportadora_geral_1");
        modelBuilder.HasSequence("seq_transportadora_geral_10");
        modelBuilder.HasSequence("seq_transportadora_geral_11");
        modelBuilder.HasSequence("seq_transportadora_geral_12");
        modelBuilder.HasSequence("seq_transportadora_geral_13");
        modelBuilder.HasSequence("seq_transportadora_geral_15");

        /************************************************************/
        /* MODELOS PERSONALIZADOS => Triggers & Views => NAO APAGAR */
        /************************************************************/
        ConfigFunctionGetDashboardEstoqueTotalEntradas(modelBuilder);
        ConfigFunctionGetDashboardEstoqueTotalSaidas(modelBuilder);
        modelBuilder.Entity<DashboardEstoqueTotalSaidasPorMes>(entity =>
        {
            entity.HasNoKey();
            entity.ToFunction("get_dashboard_estoque_total_saidas_por_mes");
        });
        modelBuilder.Entity<DashboardEstoqueTotalEntradasPorMes>(entity =>
        {
            entity.HasNoKey();
            entity.ToFunction("get_dashboard_estoque_total_entradas_por_mes");
        });
        modelBuilder.Entity<DashboardEstoqueTotalEntradasPorDia>(entity =>
        {
            entity.HasNoKey();
            entity.ToFunction("get_dashboard_estoque_total_entradas_por_dia");
        });

        modelBuilder.Entity<DashboardEstoqueTotalSaidasPorDia>(entity =>
        {
            entity.HasNoKey();
            entity.ToFunction("get_dashboard_estoque_total_saidas_por_dia");
        });

        modelBuilder.Entity<TotalPorGrupo>(entity =>
        {
            entity.HasNoKey();
            entity.ToFunction("get_total_por_grupo");
        });
        modelBuilder.Entity<FnDistribuicaoDfeEntradasResult>(entity =>
        {
            entity.HasNoKey();
            entity.ToView(null);
        });

        setProcReg50(modelBuilder);
        setProcReg54(modelBuilder);
        modelBuilder.Entity<ProcReg50SaidaResult>().HasNoKey();
        modelBuilder.Entity<ProcReg54SaidaResult>().HasNoKey();
        modelBuilder.Entity<ProcReg75SaidaResult>().HasNoKey();
        modelBuilder.Entity<ProcReg75EntradaResult>().HasNoKey();

        modelBuilder.Entity<TotalDiaResult>(entity =>
        {
            entity.HasNoKey();
            entity.ToFunction("f_nfce_get_total_dia");
        });

        modelBuilder.Entity<TotalPeriodoResult>(entity =>
        {
            entity.HasNoKey();
            entity.ToFunction("f_nfce_get_total_periodo");
        });

        modelBuilder.Entity<FormaPagamentoResult>(entity =>
        {
            entity.HasNoKey();
            entity.ToFunction("f_nfce_get_formas_pagamento");
        });

        modelBuilder.Entity<Produto5MaisVendidosResult>(entity =>
        {
            entity.HasNoKey();
            entity.ToFunction("f_nfce_get_produto_5mais_vendidos");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    private void setProcReg54(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProcReg54EntradaResult>().HasNoKey();
    }

    private void setProcReg50(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProcReg50EntradaResult>().HasNoKey();
    }

    private void ConfigFunctionGetDashboardEstoqueTotalEntradas(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DashboardEstoqueTotalEntradas>(entity =>
        {
            entity.HasNoKey();
            entity.ToFunction("get_dashboard_estoque_total_entradas");
        });
    }

    private void ConfigFunctionGetDashboardEstoqueTotalSaidas(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DashboardEstoqueTotalSaidas>(entity =>
        {
            entity.HasNoKey();
            entity.ToFunction("get_dashboard_estoque_total_saidas");
        });
    }

    public IQueryable<DashboardEstoqueTotalEntradas> GetDashboardEstoqueTotalEntradas(int punity, int? p_month = null, int? p_year = null, int? pid_empresa = -1)
    {
        p_month ??= DateTime.Now.Month;
        p_year ??= DateTime.Now.Year;

        return cdsDashboardEstoqueTotalEntradas
            .FromSqlInterpolated($"SELECT * FROM public.get_dashboard_estoque_total_entradas({punity}, {p_month}, {p_year},{pid_empresa})");
    }

    public IQueryable<DashboardEstoqueTotalSaidas> GetDashboardEstoqueTotalSaidas(int punity, int? p_month = null, int? p_year = null, int? pid_empresa = -1)
    {
        p_month ??= DateTime.Now.Month;
        p_year ??= DateTime.Now.Year;

        return cdsDashboardEstoqueTotalSaidas
            .FromSqlInterpolated($"SELECT * FROM public.get_dashboard_estoque_total_saidas({punity}, {p_month}, {p_year},{pid_empresa})");
    }

    public IQueryable<DashboardEstoqueTotalSaidasPorMes> GetDashboardEstoqueTotalSaidasPorMes(int punity, int? pid_empresa = -1)
    {
        return cdsDashboardEstoqueTotalSaidasPorMes
            .FromSqlInterpolated($"SELECT * FROM public.get_dashboard_estoque_total_saidas_por_mes({punity},{pid_empresa})");
    }

    public IQueryable<DashboardEstoqueTotalEntradasPorMes> GetDashboardEstoqueTotalEntradasPorMes(int punity, int? pid_empresa = -1)
    {
        return cdsDashboardEstoqueTotalEntradasPorMes
            .FromSqlInterpolated($"SELECT * FROM public.get_dashboard_estoque_total_entradas_por_mes({punity},{pid_empresa})");
    }

    public IQueryable<DashboardEstoqueTotalEntradasPorDia> GetDashboardEstoqueTotalEntradasPorDia(int punity, int? pid_empresa = -1)
    {
        return cdsDashboardEstoqueTotalEntradasPorDia
            .FromSqlInterpolated($"SELECT * FROM public.get_dashboard_estoque_total_entradas_por_dia({punity},{pid_empresa})");
    }

    public IQueryable<DashboardEstoqueTotalSaidasPorDia> GetDashboardEstoqueTotalSaidasPorDia(int punity, int? pid_empresa = -1)
    {
        return cdsDashboardEstoqueTotalSaidasPorDia
            .FromSqlInterpolated($"SELECT * FROM public.get_dashboard_estoque_total_saidas_por_dia({punity},{pid_empresa})");
    }

    public IQueryable<TotalPorGrupo> GetTotalPorGrupo(int punity, int? pid_empresa = -1)
    {
        return cdsTotalPorGrupo
            .FromSqlInterpolated($"SELECT * FROM public.get_total_por_grupo({punity},{pid_empresa})");
    }

    public IQueryable<FnDistribuicaoDfeEntradasResult> GetDistribuicaoDfeEntradas(
        int empresaId,
        string? nrNotaFiscal = null,
        string? nome = null,
        string? cnpj = null
    )
    {
        return FnDistribuicaoDfeEntradasResults
            .FromSqlInterpolated(
                $@"SELECT * 
               FROM fn_distribuicao_dfe_entradas(
                   {empresaId}, 
                   {nrNotaFiscal}, 
                   {nome},
                   {cnpj}
               )"
            );
    }

    public IQueryable<TotalDiaResult> GetTotalDia(int punity, int pempresa = -1, DateOnly? pdata = null)
    {
        return cdsTotalDia
            .FromSqlInterpolated($"SELECT * FROM public.f_nfce_get_total_dia({punity}, {pempresa}, {pdata})");
    }

    public IQueryable<TotalPeriodoResult> GetTotalPeriodo(int punity, DateOnly pdata1, DateOnly pdata2, int pempresa = -1)
    {
        return cdsTotalPeriodo
            .FromSqlInterpolated($"SELECT * FROM public.f_nfce_get_total_periodo({punity}, {pdata1}, {pdata2}, {pempresa})");
    }

    public IQueryable<FormaPagamentoResult> GetFormasPagamento(int punity, DateOnly pdata, int pempresa = -1)
    {
        var dataFormatada = pdata.ToString("yyyy-MM-dd");

        return cdsFormasPagamento
            .FromSqlRaw($"SELECT * FROM public.f_nfce_get_formas_pagamento({punity}, '{dataFormatada}'::date, {pempresa})");
    }

    public IQueryable<Produto5MaisVendidosResult> GetProduto5MaisVendidos(int punity, DateOnly pdate, int pempresa = -1)
    {
        var dataFormatada = pdate.ToString("yyyy-MM-dd");

        return cdsProduto5MaisVendidos
            .FromSqlRaw($"SELECT * FROM public.f_nfce_get_produto_5mais_vendidos({punity}, '{dataFormatada}'::date, {pempresa})");
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
