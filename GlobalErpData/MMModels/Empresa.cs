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

    [Column("iest")]
    [StringLength(50)]
    public string? Iest { get; set; }

    [Column("complemento")]
    [StringLength(255)]
    public string? Complemento { get; set; }

    [Column("cnae")]
    [StringLength(50)]
    public string? Cnae { get; set; }

    [Column("ultima_execucao_dfe", TypeName = "timestamp(0) without time zone")]
    public DateTime? UltimaExecucaoDfe { get; set; }

    [Column("ultimo_nsu")]
    [StringLength(255)]
    public string? UltimoNsu { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [ForeignKey("CdCidade")]
    [InverseProperty("Empresas")]
    public virtual Cidade CdCidadeNavigation { get; set; } = null!;

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Certificado> Certificados { get; set; } = new List<Certificado>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<ConfEmail> ConfEmails { get; set; } = new List<ConfEmail>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<ContaDoCaixa> ContaDoCaixas { get; set; } = new List<ContaDoCaixa>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<ContasAPagar> ContasAPagars { get; set; } = new List<ContasAPagar>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<ContasAReceber> ContasARecebers { get; set; } = new List<ContasAReceber>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ControleNumeracaoNfe? ControleNumeracaoNfe { get; set; }

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

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<DistribuicaoDfe> DistribuicaoDves { get; set; } = new List<DistribuicaoDfe>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<Entrada> Entrada { get; set; } = new List<Entrada>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<EntradaOutrasDesp> EntradaOutrasDesps { get; set; } = new List<EntradaOutrasDesp>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<FormaPagt> FormaPagts { get; set; } = new List<FormaPagt>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Impxml> Impxmls { get; set; } = new List<Impxml>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<ItemDetail> ItemDetails { get; set; } = new List<ItemDetail>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<LivroCaixa> LivroCaixas { get; set; } = new List<LivroCaixa>();

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

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<NfceAberturaCaixa> NfceAberturaCaixas { get; set; } = new List<NfceAberturaCaixa>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<NfceFormaPgt> NfceFormaPgts { get; set; } = new List<NfceFormaPgt>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<NfceProdutoSaidum> NfceProdutoSaida { get; set; } = new List<NfceProdutoSaidum>();

    [InverseProperty("EmpresaNavigation")]
    public virtual ICollection<NfceSaida> NfceSaida { get; set; } = new List<NfceSaida>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<Older> Olders { get; set; } = new List<Older>();

    [InverseProperty("EmpresaNavigation")]
    public virtual ICollection<OrcamentoCab> OrcamentoCabs { get; set; } = new List<OrcamentoCab>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<PagtosParciaisCp> PagtosParciaisCps { get; set; } = new List<PagtosParciaisCp>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<PagtosParciaisCr> PagtosParciaisCrs { get; set; } = new List<PagtosParciaisCr>();

    [InverseProperty("IdEmpresaNavigation")]
    public virtual ICollection<PerfilLoja> PerfilLojas { get; set; } = new List<PerfilLoja>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<PlanoEstoque> PlanoEstoques { get; set; } = new List<PlanoEstoque>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<ProdutoEntradum> ProdutoEntrada { get; set; } = new List<ProdutoEntradum>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<ProdutoSaidum> ProdutoSaida { get; set; } = new List<ProdutoSaidum>();

    [InverseProperty("EmpresaNavigation")]
    public virtual ICollection<Saida> Saida { get; set; } = new List<Saida>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<SaldoEstoque> SaldoEstoques { get; set; } = new List<SaldoEstoque>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<SangriaCaixa> SangriaCaixas { get; set; } = new List<SangriaCaixa>();

    [InverseProperty("EmpresaNavigation")]
    public virtual ICollection<UairangoRequest> UairangoRequests { get; set; } = new List<UairangoRequest>();

    [ForeignKey("Unity")]
    [InverseProperty("Empresas")]
    public virtual Unity UnityNavigation { get; set; } = null!;

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<UsuarioEmpresa> UsuarioEmpresas { get; set; } = new List<UsuarioEmpresa>();

    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<Vendedor> Vendedors { get; set; } = new List<Vendedor>();
}
