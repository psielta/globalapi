using System;
using System.Collections.Generic;
using GlobalErpData.Models;
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
    public virtual DbSet<FormaPagt> FormaPagts { get; set; }
    public virtual DbSet<HistoricoCaixa> HistoricoCaixas { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<PerfilLoja> PerfilLojas { get; set; }
    public virtual DbSet<ItemDetail> ItemDetails { get; set; }
    public virtual DbSet<ProductDetail> ProductDetails { get; set; }
    public virtual DbSet<FotosProduto> FotosProdutos { get; set; }
    public virtual DbSet<Featured> Featureds { get; set; }
    public virtual DbSet<Section> Sections { get; set; }
    public virtual DbSet<SectionItem> SectionItems { get; set; }
    public virtual DbSet<ContaDoCaixa> ContaDoCaixas { get; set; }
    public virtual DbSet<Certificado> Certificados { get; set; }
    public virtual DbSet<ConfiguracoesEmpresa> ConfiguracoesEmpresas { get; set; }
    public virtual DbSet<ConfiguracoesUsuario> ConfiguracoesUsuarios { get; set; }
    public virtual DbSet<Entrada> Entradas { get; set; }
    public virtual DbSet<PlanoEstoque> PlanoEstoques { get; set; }
    public virtual DbSet<SaldoEstoque> SaldoEstoques { get; set; }
    public virtual DbSet<Cidade> Cidades { get; set; }
    public virtual DbSet<Impcabnfe> Impcabnves { get; set; }
    public virtual DbSet<ProdutoEntradum> ProdutoEntrada { get; set; }

    public virtual DbSet<Impdupnfe> Impdupnves { get; set; }

    public virtual DbSet<Impitensnfe> Impitensnves { get; set; }

    public virtual DbSet<Imptotalnfe> Imptotalnves { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<CfopImportacao> CfopImportacaos { get; set; }
    public virtual DbSet<ConfEmail> ConfEmails { get; set; }

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

    public virtual DbSet<Transportadora> Transportadoras { get; set; }

    public virtual DbSet<CteSeguro> CteSeguros { get; set; }

    public virtual DbSet<CteValePedagio> CteValePedagios { get; set; }

    public virtual DbSet<CteVeiculo> CteVeiculos { get; set; }

    public virtual DbSet<Diverso> Diversos { get; set; }

    public virtual DbSet<Empresa> Empresas { get; set; }

    public virtual DbSet<EnvioEmailAutomatico> EnvioEmailAutomaticos { get; set; }

    public virtual DbSet<GrupoEstoque> GrupoEstoques { get; set; }

    public virtual DbSet<Fornecedor> Fornecedors { get; set; }
    public virtual DbSet<Mdfe> Mdves { get; set; }

    public virtual DbSet<MdfeChafe> MdfeChaves { get; set; }

    public virtual DbSet<MdfeChavesDfe> MdfeChavesDves { get; set; }

    public virtual DbSet<MdfeCondutor> MdfeCondutors { get; set; }

    public virtual DbSet<MdfeInfcarregamento> MdfeInfcarregamentos { get; set; }

    public virtual DbSet<MdfePercurso> MdfePercursos { get; set; }

    public virtual DbSet<MdfeReboque> MdfeReboques { get; set; }

    public virtual DbSet<MdfeRodoviario> MdfeRodoviarios { get; set; }

    public virtual DbSet<MdfeSeguro> MdfeSeguros { get; set; }

    public virtual DbSet<Permissao> Permissaos { get; set; }

    public virtual DbSet<ProdutoEstoque> ProdutoEstoques { get; set; }

    public virtual DbSet<ProdutosForn> ProdutosForns { get; set; }
    public virtual DbSet<ReferenciaEstoque> ReferenciaEstoques { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioPermissao> UsuarioPermissaos { get; set; }

    public virtual DbSet<UnidadeMedida> UnidadeMedidas { get; set; }

    public virtual DbSet<Older> Olders { get; set; }

    public virtual DbSet<OlderItem> OlderItems { get; set; }


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
            .HasPostgresExtension("unaccent")
            .HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("category_pkey");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Categories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("category_fk");
        });

        modelBuilder.Entity<Certificado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("certificados_pkey");

            entity.Property(e => e.Tipo)
                .HasDefaultValueSql("'H'::character varying")
                .HasComment("H - Homologacao\r\nP - Producao");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Certificados).HasConstraintName("certificados_fk");
        });

        modelBuilder.Entity<CfopImportacao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cfop_importacao_pkey");

            entity.Property(e => e.CfopDentro).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CfopFora).HasDefaultValueSql("''::character varying");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.CfopImportacaos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cfop_importacao_fk");
        });

        modelBuilder.Entity<Cidade>(entity =>
        {
            entity.HasKey(e => e.CdCidade).HasName("cidade_pkey");

            entity.Property(e => e.Uf).HasDefaultValueSql("'XX'::character varying");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cliente_idx");

            entity.Property(e => e.Ativo).HasDefaultValueSql("'S'::character varying");
            entity.Property(e => e.Cep).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.DtCadastro).HasDefaultValueSql("('now'::text)::date");
            entity.Property(e => e.EMail).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.IdCteAntigo).HasDefaultValueSql("'-1'::integer");
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

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Clientes).HasConstraintName("cliente_fk");

            entity.HasOne(d => d.IdUsuarioCadNavigation).WithMany(p => p.Clientes)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.IdUsuarioCad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cliente_fk2");
        });

        modelBuilder.Entity<ConfEmail>(entity =>
        {
            entity.HasKey(e => e.NrLanc).HasName("conf_email_pkey");

            entity.Property(e => e.AssuntoEmail).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.ConexaoSegura).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.EMailCopia).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.EnviaAposEmitir).HasDefaultValue(false);
            entity.Property(e => e.Ssl).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.Tsl).HasDefaultValueSql("'N'::character varying");
            entity.Property(e => e.TxtPadrao).HasDefaultValueSql("''::text");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.ConfEmails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("conf_email_cd_empresa_fkey");
        });

        modelBuilder.Entity<ConfiguracoesEmpresa>(entity =>
        {
            entity.HasKey(e => new { e.Chave, e.CdEmpresa }).HasName("configuracoes_empresa_idx");

            entity.Property(e => e.Valor1).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Valor2).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Valor3).HasDefaultValueSql("''::character varying");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.ConfiguracoesEmpresas).HasConstraintName("configuracoes_empresa_fk");
        });

        modelBuilder.Entity<ConfiguracoesUsuario>(entity =>
        {
            entity.HasKey(e => new { e.Chave, e.IdUsuario }).HasName("configuracoes_usuario_idx");

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

        modelBuilder.Entity<Diverso>(entity =>
        {
            entity.HasKey(e => e.CdDiv).HasName("diversos_pkey");

            entity.Property(e => e.CdHistorico).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdPlanoCaixa).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdPlanoEstoque).HasDefaultValueSql("'-1'::integer");
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
            entity.Property(e => e.NmBairro).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrInscrEstadual).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.NrInscrMunicipal).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Telefone).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.TipoRegime).HasDefaultValue(1);
            entity.Property(e => e.TxtObs).HasDefaultValueSql("''::character varying");

            entity.HasOne(d => d.CdCidadeNavigation).WithMany(p => p.Empresas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("empresa_fk");
        });

        modelBuilder.Entity<Entrada>(entity =>
        {
            entity.HasKey(e => new { e.Nr, e.CdEmpresa }).HasName("entradas_idx");

            entity.Property(e => e.Nr).ValueGeneratedOnAdd();
            entity.Property(e => e.CdClienteDevolucao).HasDefaultValueSql("'-1'::integer");
            entity.Property(e => e.CdSituacao).HasDefaultValueSql("'01'::character varying");
            entity.Property(e => e.TxtObs).HasDefaultValueSql("''::text");
            entity.Property(e => e.VIcmsDeson).HasDefaultValueSql("0.0");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.Entrada).HasConstraintName("entradas_fk1");

            entity.HasOne(d => d.CdGrupoEstoqueNavigation).WithMany(p => p.Entrada)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("entradas_fk2");

            entity.HasOne(d => d.Fornecedor).WithMany(p => p.Entrada)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("entradas_fk");
        });

        modelBuilder.Entity<EnvioEmailAutomatico>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("envio_email_automatico_pkey");

            entity.Property(e => e.DtLanc).HasDefaultValueSql("('now'::text)::date");
            entity.Property(e => e.HrLanc).HasDefaultValueSql("(now())::time without time zone");
        });

        modelBuilder.Entity<Featured>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.IdEmpresa }).HasName("featured_pkey");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Excluiu).HasDefaultValue(false);

            entity.HasOne(d => d.Category).WithMany(p => p.Featureds).HasConstraintName("featured_category_id_fkey");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Featureds).HasConstraintName("featured_id_empresa_fkey");
        });

        modelBuilder.Entity<FormaPagt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("forma_pagt_pkey");

            entity.Property(e => e.CdHistoricoCaixa).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdHistoricoCaixaD).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdPlanoCaixa).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdPlanoCaixaD).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Integrado).HasDefaultValueSql("'N'::character varying");
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

            entity.HasOne(d => d.PlanoDeCaixa).WithMany(p => p.FormaPagtPlanoDeCaixas)
                .HasPrincipalKey(p => new { p.CdClassificacao, p.CdEmpresa })
                .HasForeignKey(d => new { d.CdPlanoCaixa, d.CdEmpresa })
                .HasConstraintName("forma_pagt_fk1");

            entity.HasOne(d => d.PlanoDeCaixaNavigation).WithMany(p => p.FormaPagtPlanoDeCaixaNavigations)
                .HasPrincipalKey(p => new { p.CdClassificacao, p.CdEmpresa })
                .HasForeignKey(d => new { d.CdPlanoCaixaD, d.CdEmpresa })
                .HasConstraintName("forma_pagt_fk2");

            entity.HasOne(d => d.HistoricoCaixa).WithMany(p => p.FormaPagtHistoricoCaixas)
                .HasPrincipalKey(p => new { p.CdEmpresa, p.CdSubPlano, p.CdPlano })
                .HasForeignKey(d => new { d.CdEmpresa, d.CdHistoricoCaixa, d.CdPlanoCaixa })
                .HasConstraintName("forma_pagt_fk3");

            entity.HasOne(d => d.HistoricoCaixaNavigation).WithMany(p => p.FormaPagtHistoricoCaixaNavigations)
                .HasPrincipalKey(p => new { p.CdEmpresa, p.CdSubPlano, p.CdPlano })
                .HasForeignKey(d => new { d.CdEmpresa, d.CdHistoricoCaixaD, d.CdPlanoCaixaD })
                .HasConstraintName("forma_pagt_fk4");
        });

        modelBuilder.Entity<Fornecedor>(entity =>
        {
            entity.HasKey(e => new { e.CdForn, e.IdEmpresa }).HasName("fornecedor_idx");

            entity.Property(e => e.CdForn).ValueGeneratedOnAdd();
            entity.Property(e => e.Bairro).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CdCep).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Cnpj).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Complemento).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Cpf).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Email).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.IdCliente).HasDefaultValueSql("'-1'::integer");
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

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Fornecedors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fornecedor_fk");
        });

        modelBuilder.Entity<FotosProduto>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.IdEmpresa }).HasName("fotos_produto_pkey");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Excluiu).HasDefaultValue(false);

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.FotosProdutos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_empresa");

            entity.HasOne(d => d.ProdutoEstoque).WithMany(p => p.FotosProdutos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_produto");
        });

        modelBuilder.Entity<GrupoEstoque>(entity =>
        {
            entity.HasKey(e => e.CdGrupo).HasName("grupo_estoque_pkey");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.GrupoEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("grupo_estoque_fk");
        });

        modelBuilder.Entity<HistoricoCaixa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("historico_caixa_pkey");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.HistoricoCaixas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("historico_caixa_fk1");

            entity.HasOne(d => d.PlanoDeCaixa).WithMany(p => p.HistoricoCaixas)
                .HasPrincipalKey(p => new { p.CdClassificacao, p.CdEmpresa })
                .HasForeignKey(d => new { d.CdPlano, d.CdEmpresa })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("historico_caixa_fk");
        });

        modelBuilder.Entity<Impcabnfe>(entity =>
        {
            entity.HasKey(e => e.ChNfe).HasName("pkimpcabnfe");
        });

        modelBuilder.Entity<Impdupnfe>(entity =>
        {
            entity.HasKey(e => new { e.ChNfe, e.NrDup }).HasName("pkimpdupnfe");
        });

        modelBuilder.Entity<Impitensnfe>(entity =>
        {
            entity.HasKey(e => new { e.ChNfe, e.NrItem }).HasName("pkimpitensnfe_ch_nfenr_item");
        });

        modelBuilder.Entity<Imptotalnfe>(entity =>
        {
            entity.HasKey(e => e.ChNfe).HasName("pkimptotalnfe");
        });

        modelBuilder.Entity<ItemDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("item_details_pkey");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.ItemDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("item_details_fk");

            entity.HasOne(d => d.IdProductDetailsNavigation).WithMany(p => p.ItemDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("item_details_fk1");
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

        modelBuilder.Entity<Older>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("older_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Olders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("older_fk");
        });

        modelBuilder.Entity<OlderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("older_items_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");

            entity.HasOne(d => d.Older).WithMany(p => p.OlderItems)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("older_items_older_id_fkey");

            entity.HasOne(d => d.ProdutoEstoque).WithMany(p => p.OlderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("older_items_fk");
        });

        modelBuilder.Entity<PerfilLoja>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("perfil_loja_pkey");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.PerfilLojas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("perfil_loja_fk");
        });

        modelBuilder.Entity<Permissao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("permissao_pkey");

            entity.Property(e => e.Descricao).HasDefaultValueSql("''::character varying");
        });

        modelBuilder.Entity<PlanoDeCaixa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("plano_de_caixa_pkey");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.PlanoDeCaixas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plano_de_caixa_fk");
        });

        modelBuilder.Entity<PlanoEstoque>(entity =>
        {
            entity.HasKey(e => e.CdPlano).HasName("plano_estoque_pkey");

            entity.Property(e => e.Ativo).HasDefaultValue(true);

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.PlanoEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plano_estoque_fk");
        });

        modelBuilder.Entity<ProductDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_details_pkey");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.ProductDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_details_fk1");

            entity.HasOne(d => d.ProdutoEstoque).WithMany(p => p.ProductDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_details_fk");
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

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.ProdutoEntrada).HasConstraintName("produto_entrada_fk2");

            entity.HasOne(d => d.ProdutoEstoque).WithMany(p => p.ProdutoEntrada).HasConstraintName("produto_entrada_fk1");

            entity.HasOne(d => d.Entrada).WithMany(p => p.ProdutoEntrada).HasConstraintName("produto_entrada_fk");
        });

        modelBuilder.Entity<ProdutoEstoque>(entity =>
        {
            entity.HasKey(e => new { e.CdProduto, e.IdEmpresa }).HasName("pk_produtos");

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
            entity.Property(e => e.Ippt).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.LancLivro).HasDefaultValueSql("'N'::character varying");
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

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.ProdutoEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("produto_estoque_fk2");

            entity.HasOne(d => d.Section).WithMany(p => p.ProdutoEstoques)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_section_id");

            entity.HasOne(d => d.SectionItem).WithMany(p => p.ProdutoEstoques)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_section_item_id");

            entity.HasOne(d => d.Featured).WithMany(p => p.ProdutoEstoques)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_featured_id");
        });

        modelBuilder.Entity<ProdutosForn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("produtos_forn_pkey");

            entity.Property(e => e.CdBarra).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.IdProdutoExterno).HasDefaultValueSql("''::character varying");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.ProdutosForns).HasConstraintName("produtos_forn_fk1");

            entity.HasOne(d => d.Fornecedor).WithMany(p => p.ProdutosForns).HasConstraintName("produtos_forn_fk3");

            entity.HasOne(d => d.ProdutoEstoque).WithMany(p => p.ProdutosForns).HasConstraintName("produtos_forn_fk2");
        });

        modelBuilder.Entity<ReferenciaEstoque>(entity =>
        {
            entity.HasKey(e => e.CdRef).HasName("referencia_estoque_pkey");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.ReferenciaEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("referencia_estoque_fk");
        });

        modelBuilder.Entity<SaldoEstoque>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("saldo_estoque_pk");

            entity.Property(e => e.QuantE).HasDefaultValueSql("0");
            entity.Property(e => e.QuantF).HasDefaultValueSql("0");
            entity.Property(e => e.QuantV).HasDefaultValueSql("0");

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.SaldoEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("saldo_estoque_fk");

            entity.HasOne(d => d.CdPlanoNavigation).WithMany(p => p.SaldoEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("saldo_estoque_fk2");

            entity.HasOne(d => d.ProdutoEstoque).WithMany(p => p.SaldoEstoques)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("saldo_estoque_fk1");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sections_pkey");

            entity.HasOne(d => d.Category).WithMany(p => p.Sections)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("sections_category_id_fkey");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Sections).HasConstraintName("sections_id_empresa_fkey");
        });

        modelBuilder.Entity<SectionItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("section_items_pkey");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.SectionItems).HasConstraintName("section_items_id_empresa_fkey");

            entity.HasOne(d => d.Section).WithMany(p => p.SectionItems)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("section_items_section_id_fkey");
        });

        modelBuilder.Entity<Transportadora>(entity =>
        {
            entity.HasKey(e => new { e.CdTransportadora, e.IdEmpresa }).HasName("transportadora_idx1");

            entity.Property(e => e.CdTransportadora).ValueGeneratedOnAdd();

            entity.HasOne(d => d.CdCidadeNavigation).WithMany(p => p.Transportadoras).HasConstraintName("transportadora_fk1");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Transportadoras)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("transportadora_fk");
        });

        modelBuilder.Entity<UnidadeMedida>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("unidade_medida_pkey");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.UnidadeMedida)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("unidade_medida_fk2");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.NmUsuario).HasName("usuario_pkey");

            entity.Property(e => e.Ativo).HasDefaultValueSql("'S'::character varying");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity.HasOne(d => d.CdEmpresaNavigation).WithMany(p => p.Usuarios).HasConstraintName("usuario_fk");
        });

        modelBuilder.Entity<UsuarioPermissao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usuario_permissao_pkey");

            entity.HasOne(d => d.IdPermissaoNavigation).WithMany(p => p.UsuarioPermissaos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuario_permissao_fk1");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.UsuarioPermissaos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuario_permissao_fk");
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
        modelBuilder.HasSequence("seq_fornecedor_geral_1");
        modelBuilder.HasSequence("seq_fornecedor_geral_2");
        modelBuilder.HasSequence("seq_fotos_geral_1");
        modelBuilder.HasSequence("seq_produto_geral_1").StartsAt(832L);
        modelBuilder.HasSequence("seq_produto_geral_2");
        modelBuilder.HasSequence("seq_transportadora_geral_1");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
