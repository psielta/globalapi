using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;

namespace GlobalErpData.Models;

[Table("empresa")]
public partial class Empresa : IIdentifiable<int>
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

    [Column("iest")]
    [StringLength(50)]
    public string? Iest { get; set; }

    [Column("complemento")]
    [StringLength(255)]
    public string? Complemento { get; set; }

    [Column("cnae")]
    [StringLength(50)]
    public string? Cnae { get; set; }

    [JsonIgnore]
    [ForeignKey("CdCidade")]
    [InverseProperty("Empresas")]
    public virtual Cidade CdCidadeNavigation { get; set; } = null!;
    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Certificado> Certificados { get; set; } = new List<Certificado>();
    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<ObsNf> ObsNfs { get; set; } = new List<ObsNf>();
    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CfopCsosnV2> CfopCsosnV2s { get; set; } = new List<CfopCsosnV2>();
    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<ConfEmail> ConfEmails { get; set; } = new List<ConfEmail>();
    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteCompPrestacao> CteCompPrestacaos { get; set; } = new List<CteCompPrestacao>();
    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteDocAnterior> CteDocAnteriors { get; set; } = new List<CteDocAnterior>();
    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteDuplicatum> CteDuplicata { get; set; } = new List<CteDuplicatum>();
    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteInutilizarNumero> CteInutilizarNumeros { get; set; } = new List<CteInutilizarNumero>();
    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteNf> CteNfs { get; set; } = new List<CteNf>();
    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteNfe> CteNves { get; set; } = new List<CteNfe>();
    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteOb> CteObs { get; set; } = new List<CteOb>();
    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Fornecedor> Fornecedors { get; set; } = new List<Fornecedor>();
    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteOrdemColetum> CteOrdemColeta { get; set; } = new List<CteOrdemColetum>();
    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteOutrosDoc> CteOutrosDocs { get; set; } = new List<CteOutrosDoc>();
    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CtePassagem> CtePassagems { get; set; } = new List<CtePassagem>();
    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteProdPerigoso> CteProdPerigosos { get; set; } = new List<CteProdPerigoso>();
    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteQtdCarga> CteQtdCargas { get; set; } = new List<CteQtdCarga>();
    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteRodMotoristum> CteRodMotorista { get; set; } = new List<CteRodMotoristum>();
    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteRodVeiculo> CteRodVeiculos { get; set; } = new List<CteRodVeiculo>();
    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteSeguro> CteSeguros { get; set; } = new List<CteSeguro>();
    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteValePedagio> CteValePedagios { get; set; } = new List<CteValePedagio>();

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CteVeiculo> CteVeiculos { get; set; } = new List<CteVeiculo>();

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Cte> Ctes { get; set; } = new List<Cte>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<Icm> Icms { get; set; } = new List<Icm>();

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<MdfeChafe> MdfeChaves { get; set; } = new List<MdfeChafe>();

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<MdfeCondutor> MdfeCondutors { get; set; } = new List<MdfeCondutor>();

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<MdfeInfcarregamento> MdfeInfcarregamentos { get; set; } = new List<MdfeInfcarregamento>();

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<MdfeReboque> MdfeReboques { get; set; } = new List<MdfeReboque>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<ContasAPagar> ContasAPagars { get; set; } = new List<ContasAPagar>();

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<MdfeRodoviario> MdfeRodoviarios { get; set; } = new List<MdfeRodoviario>();

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Mdfe> Mdves { get; set; } = new List<Mdfe>();
    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<NcmProtocoloEstado> NcmProtocoloEstados { get; set; } = new List<NcmProtocoloEstado>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<ProdutoEstoque> ProdutoEstoques { get; set; } = new List<ProdutoEstoque>();
    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<CfopImportacao> CfopImportacaos { get; set; } = new List<CfopImportacao>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<GrupoEstoque> GrupoEstoques { get; set; } = new List<GrupoEstoque>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<ReferenciaEstoque> ReferenciaEstoques { get; set; } = new List<ReferenciaEstoque>();

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<UnidadeMedida> UnidadeMedida { get; set; } = new List<UnidadeMedida>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<PlanoEstoque> PlanoEstoques { get; set; } = new List<PlanoEstoque>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<SaldoEstoque> SaldoEstoques { get; set; } = new List<SaldoEstoque>();

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<ProdutosForn> ProdutosForns { get; set; } = new List<ProdutosForn>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<ConfiguracoesEmpresa> ConfiguracoesEmpresas { get; set; } = new List<ConfiguracoesEmpresa>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<Entrada> Entrada { get; set; } = new List<Entrada>();

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Transportadora> Transportadoras { get; set; } = new List<Transportadora>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<ProdutoEntradum> ProdutoEntrada { get; set; } = new List<ProdutoEntradum>();

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<SectionItem> SectionItems { get; set; } = new List<SectionItem>();

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Featured> Featureds { get; set; } = new List<Featured>();

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<ItemDetail> ItemDetails { get; set; } = new List<ItemDetail>();

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Older> Olders { get; set; } = new List<Older>();

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Impxml> Impxmls { get; set; } = new List<Impxml>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<ContasAReceber> ContasARecebers { get; set; } = new List<ContasAReceber>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<LivroCaixa> LivroCaixas { get; set; } = new List<LivroCaixa>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<PagtosParciaisCp> PagtosParciaisCps { get; set; } = new List<PagtosParciaisCp>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<PagtosParciaisCr> PagtosParciaisCrs { get; set; } = new List<PagtosParciaisCr>();

    [GraphQLIgnore]
    public int GetId()
    {
        return this.CdEmpresa;
    }

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<PerfilLoja> PerfilLojas { get; set; } = new List<PerfilLoja>();


    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "CdEmpresa";
    }

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<FotosProduto> FotosProdutos { get; set; } = new List<FotosProduto>();

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<ContaDoCaixa> ContaDoCaixas { get; set; } = new List<ContaDoCaixa>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<PlanoDeCaixa> PlanoDeCaixas { get; set; } = new List<PlanoDeCaixa>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<HistoricoCaixa> HistoricoCaixas { get; set; } = new List<HistoricoCaixa>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<FormaPagt> FormaPagts { get; set; } = new List<FormaPagt>();

    [JsonIgnore]
    [InverseProperty("EmpresaNavigation")]
    public virtual ICollection<Saida> Saida { get; set; } = new List<Saida>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<ProdutoSaidum> ProdutoSaida { get; set; } = new List<ProdutoSaidum>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<Frete> Fretes { get; set; } = new List<Frete>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<SaidasVolume> SaidasVolumes { get; set; } = new List<SaidasVolume>();

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<ProtocoloEstadoNcm> ProtocoloEstadoNcms { get; set; } = new List<ProtocoloEstadoNcm>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<NfceAberturaCaixa> NfceAberturaCaixas { get; set; } = new List<NfceAberturaCaixa>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<NfceProdutoSaidum> NfceProdutoSaida { get; set; } = new List<NfceProdutoSaidum>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<NfceSaida> NfceSaida { get; set; } = new List<NfceSaida>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<NfceSangriaCaixa> NfceSangriaCaixas { get; set; } = new List<NfceSangriaCaixa>();

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<NfceSuprimentoCaixa> NfceSuprimentoCaixas { get; set; } = new List<NfceSuprimentoCaixa>();

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ControleNumeracaoNfe? ControleNumeracaoNfe { get; set; }

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<EntregaNfe> EntregaNves { get; set; } = new List<EntregaNfe>();

    [JsonIgnore]
    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<RetiradaNfe> RetiradaNves { get; set; } = new List<RetiradaNfe>();
}
