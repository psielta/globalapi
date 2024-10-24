using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("empresa")]
public partial class Empresa
{
    [Key]
    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("nm_empresa")]
    [StringLength(256)]
    public string NmEmpresa { get; set; } = null!;

    [Column("nm_endereco")]
    [StringLength(62)]
    public string NmEndereco { get; set; } = null!;

    [Column("numero")]
    public int Numero { get; set; }

    [Column("cd_cidade")]
    [StringLength(10)]
    public string CdCidade { get; set; } = null!;

    [Column("cd_cep")]
    [StringLength(9)]
    public string CdCep { get; set; } = null!;

    [Column("cd_cnpj")]
    [StringLength(18)]
    public string? CdCnpj { get; set; }

    [Column("nm_bairro")]
    [StringLength(25)]
    public string? NmBairro { get; set; }

    [Column("telefone")]
    [StringLength(15)]
    public string? Telefone { get; set; }

    [Column("nr_inscr_municipal")]
    [StringLength(18)]
    public string? NrInscrMunicipal { get; set; }

    [Column("nr_inscr_estadual")]
    [StringLength(18)]
    public string? NrInscrEstadual { get; set; }

    [Column("txt_obs")]
    [StringLength(16384)]
    public string? TxtObs { get; set; }

    [Column("e_mail")]
    [StringLength(100)]
    public string? EMail { get; set; }

    [Column("idcsc")]
    [StringLength(6)]
    public string? Idcsc { get; set; }

    [Column("csc")]
    [StringLength(36)]
    public string? Csc { get; set; }

    [Column("autorizo_xml")]
    [StringLength(1)]
    public string? AutorizoXml { get; set; }

    [Column("cpfcnpf_autorizado")]
    [StringLength(18)]
    public string? CpfcnpfAutorizado { get; set; }

    [Column("nome_fantasia")]
    [StringLength(128)]
    public string? NomeFantasia { get; set; }

    [Column("tipo_regime")]
    public int? TipoRegime { get; set; }

    [Column("mail_contador")]
    [StringLength(255)]
    public string? MailContador { get; set; }

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    [ForeignKey("CdCidade")]
    [InverseProperty("Empresas")]
    public virtual Cidade CdCidadeNavigation { get; set; } = null!;

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Certificado> Certificados { get; set; } = new List<Certificado>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CfopImportacao> CfopImportacaos { get; set; } = new List<CfopImportacao>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<ConfEmail> ConfEmails { get; set; } = new List<ConfEmail>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<ConfiguracoesEmpresa> ConfiguracoesEmpresas { get; set; } = new List<ConfiguracoesEmpresa>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<ContaDoCaixa> ContaDoCaixas { get; set; } = new List<ContaDoCaixa>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<ContasAPagar> ContasAPagars { get; set; } = new List<ContasAPagar>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteCompPrestacao> CteCompPrestacaos { get; set; } = new List<CteCompPrestacao>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteDocAnterior> CteDocAnteriors { get; set; } = new List<CteDocAnterior>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteDuplicatum> CteDuplicata { get; set; } = new List<CteDuplicatum>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteInutilizarNumero> CteInutilizarNumeros { get; set; } = new List<CteInutilizarNumero>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteNf> CteNfs { get; set; } = new List<CteNf>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteNfe> CteNves { get; set; } = new List<CteNfe>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteOb> CteObs { get; set; } = new List<CteOb>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteOrdemColetum> CteOrdemColeta { get; set; } = new List<CteOrdemColetum>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteOutrosDoc> CteOutrosDocs { get; set; } = new List<CteOutrosDoc>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CtePassagem> CtePassagems { get; set; } = new List<CtePassagem>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteProdPerigoso> CteProdPerigosos { get; set; } = new List<CteProdPerigoso>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteQtdCarga> CteQtdCargas { get; set; } = new List<CteQtdCarga>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteRodMotoristum> CteRodMotorista { get; set; } = new List<CteRodMotoristum>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteRodVeiculo> CteRodVeiculos { get; set; } = new List<CteRodVeiculo>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteSeguro> CteSeguros { get; set; } = new List<CteSeguro>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteValePedagio> CteValePedagios { get; set; } = new List<CteValePedagio>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteVeiculo> CteVeiculos { get; set; } = new List<CteVeiculo>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Cte> Ctes { get; set; } = new List<Cte>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<Entrada> Entrada { get; set; } = new List<Entrada>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Featured> Featureds { get; set; } = new List<Featured>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<FormaPagt> FormaPagts { get; set; } = new List<FormaPagt>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Fornecedor> Fornecedors { get; set; } = new List<Fornecedor>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<FotosProduto> FotosProdutos { get; set; } = new List<FotosProduto>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<GrupoEstoque> GrupoEstoques { get; set; } = new List<GrupoEstoque>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<HistoricoCaixa> HistoricoCaixas { get; set; } = new List<HistoricoCaixa>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<ItemDetail> ItemDetails { get; set; } = new List<ItemDetail>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<MdfeChafe> MdfeChaves { get; set; } = new List<MdfeChafe>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<MdfeCondutor> MdfeCondutors { get; set; } = new List<MdfeCondutor>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<MdfeInfcarregamento> MdfeInfcarregamentos { get; set; } = new List<MdfeInfcarregamento>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<MdfeReboque> MdfeReboques { get; set; } = new List<MdfeReboque>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<MdfeRodoviario> MdfeRodoviarios { get; set; } = new List<MdfeRodoviario>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Mdfe> Mdves { get; set; } = new List<Mdfe>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Older> Olders { get; set; } = new List<Older>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<PerfilLoja> PerfilLojas { get; set; } = new List<PerfilLoja>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<PlanoDeCaixa> PlanoDeCaixas { get; set; } = new List<PlanoDeCaixa>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<PlanoEstoque> PlanoEstoques { get; set; } = new List<PlanoEstoque>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<ProdutoEntradum> ProdutoEntrada { get; set; } = new List<ProdutoEntradum>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<ProdutoEstoque> ProdutoEstoques { get; set; } = new List<ProdutoEstoque>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<ProdutosForn> ProdutosForns { get; set; } = new List<ProdutosForn>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<ReferenciaEstoque> ReferenciaEstoques { get; set; } = new List<ReferenciaEstoque>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<SaldoEstoque> SaldoEstoques { get; set; } = new List<SaldoEstoque>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<SectionItem> SectionItems { get; set; } = new List<SectionItem>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Transportadora> Transportadoras { get; set; } = new List<Transportadora>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<UnidadeMedidum> UnidadeMedida { get; set; } = new List<UnidadeMedidum>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
