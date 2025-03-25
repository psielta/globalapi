using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[PrimaryKey("Nr", "CdEmpresa")]
[Table("entradas")]
[Index("Nr", "CdEmpresa", "CdGrupoEstoque", Name = "entradas_idx1", IsUnique = true)]
public partial class Entrada
{
    [Key]
    [Column("nr")]
    public int Nr { get; set; }

    [Column("data")]
    public DateOnly Data { get; set; }

    [Column("cd_forn")]
    public int CdForn { get; set; }

    [Key]
    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("nr_nf")]
    [StringLength(25)]
    public string? NrNf { get; set; }

    [Column("dt_saida")]
    public DateOnly? DtSaida { get; set; }

    [Column("hr_saida")]
    public TimeOnly? HrSaida { get; set; }

    [Column("hr_chegada")]
    public TimeOnly? HrChegada { get; set; }

    [Column("cd_cfop")]
    [StringLength(4)]
    public string? CdCfop { get; set; }

    [Column("vl_frete")]
    [Precision(18, 4)]
    public decimal? VlFrete { get; set; }

    [Column("transferiu")]
    [StringLength(1)]
    public string? Transferiu { get; set; }

    [Column("nr_pedido_compra")]
    public int? NrPedidoCompra { get; set; }

    [Column("vl_outras")]
    [Precision(18, 4)]
    public decimal? VlOutras { get; set; }

    [Column("vl_seguro")]
    [Precision(18, 4)]
    public decimal? VlSeguro { get; set; }

    [Column("vl_desconto_nf")]
    [Precision(18, 4)]
    public decimal? VlDescontoNf { get; set; }

    [Column("vl_acrescimo_nf")]
    [Precision(18, 4)]
    public decimal? VlAcrescimoNf { get; set; }

    [Column("txt_obs")]
    [StringLength(16384)]
    public string? TxtObs { get; set; }

    [Column("cd_grupo_estoque")]
    public int CdGrupoEstoque { get; set; }

    [Column("tp_pagt")]
    [StringLength(128)]
    public string? TpPagt { get; set; }

    [Column("transp")]
    public int? Transp { get; set; }

    [Column("cd_tipo_nf")]
    [StringLength(4)]
    public string? CdTipoNf { get; set; }

    [Column("modelo_nf")]
    [StringLength(6)]
    public string? ModeloNf { get; set; }

    [Column("serie_nf")]
    [StringLength(4)]
    public string? SerieNf { get; set; }

    [Column("emitende_nf")]
    [StringLength(1)]
    public string? EmitendeNf { get; set; }

    [Column("cd_situacao")]
    [StringLength(2)]
    public string? CdSituacao { get; set; }

    [Column("cd_chave_nfe")]
    [StringLength(62)]
    public string? CdChaveNfe { get; set; }

    [Column("tp_frete")]
    public int? TpFrete { get; set; }

    [Column("tp_entrada")]
    [StringLength(2)]
    public string? TpEntrada { get; set; }

    [Column("xml_nf")]
    public string? XmlNf { get; set; }

    [Column("dt_emissao")]
    public DateOnly? DtEmissao { get; set; }

    [Column("vl_guia_st")]
    [Precision(18, 4)]
    public decimal? VlGuiaSt { get; set; }

    [Column("porc_dif_alig")]
    [Precision(18, 4)]
    public decimal? PorcDifAlig { get; set; }

    [Column("vl_st_nf")]
    [Precision(18, 4)]
    public decimal? VlStNf { get; set; }

    [Column("cd_cliente_devolucao")]
    public int? CdClienteDevolucao { get; set; }

    [Column("resp_ret_icms_st")]
    [StringLength(30)]
    public string? RespRetIcmsSt { get; set; }

    [Column("cod_mod_doc_arrec")]
    [StringLength(50)]
    public string? CodModDocArrec { get; set; }

    [Column("num_doc_arrec")]
    [StringLength(20)]
    public string? NumDocArrec { get; set; }

    [Column("t_pag")]
    [StringLength(128)]
    public string? TPag { get; set; }

    [Column("v_icms_deson")]
    [Precision(18, 4)]
    public decimal? VIcmsDeson { get; set; }

    [Column("icmstot_v_bc")]
    [Precision(18, 2)]
    public decimal? IcmstotVBc { get; set; }

    [Column("icmstot_v_icms")]
    [Precision(18, 2)]
    public decimal? IcmstotVIcms { get; set; }

    [Column("icmstot_v_icms_deson")]
    [Precision(18, 2)]
    public decimal? IcmstotVIcmsDeson { get; set; }

    [Column("icmstot_v_fcp")]
    [Precision(18, 2)]
    public decimal? IcmstotVFcp { get; set; }

    [Column("icmstot_v_bcst")]
    [Precision(18, 2)]
    public decimal? IcmstotVBcst { get; set; }

    [Column("icmstot_v_st")]
    [Precision(18, 2)]
    public decimal? IcmstotVSt { get; set; }

    [Column("icmstot_v_fcpst")]
    [Precision(18, 2)]
    public decimal? IcmstotVFcpst { get; set; }

    [Column("icmstot_v_fcpst_ret")]
    [Precision(18, 2)]
    public decimal? IcmstotVFcpstRet { get; set; }

    [Column("icmstot_v_prod")]
    [Precision(18, 2)]
    public decimal? IcmstotVProd { get; set; }

    [Column("icmstot_v_frete")]
    [Precision(18, 2)]
    public decimal? IcmstotVFrete { get; set; }

    [Column("icmstot_v_seg")]
    [Precision(18, 2)]
    public decimal? IcmstotVSeg { get; set; }

    [Column("icmstot_v_desc")]
    [Precision(18, 2)]
    public decimal? IcmstotVDesc { get; set; }

    [Column("icmstot_v_ii")]
    [Precision(18, 2)]
    public decimal? IcmstotVIi { get; set; }

    [Column("icmstot_v_ipi")]
    [Precision(18, 2)]
    public decimal? IcmstotVIpi { get; set; }

    [Column("icmstot_v_ipi_devol")]
    [Precision(18, 2)]
    public decimal? IcmstotVIpiDevol { get; set; }

    [Column("icmstot_v_pis")]
    [Precision(18, 2)]
    public decimal? IcmstotVPis { get; set; }

    [Column("icmstot_v_cofins")]
    [Precision(18, 2)]
    public decimal? IcmstotVCofins { get; set; }

    [Column("icmstot_v_outro")]
    [Precision(18, 2)]
    public decimal? IcmstotVOutro { get; set; }

    [Column("icmstot_v_nf")]
    [Precision(18, 2)]
    public decimal? IcmstotVNf { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [ForeignKey("CdEmpresa")]
    [InverseProperty("Entrada")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [ForeignKey("CdGrupoEstoque")]
    [InverseProperty("Entrada")]
    public virtual PlanoEstoque CdGrupoEstoqueNavigation { get; set; } = null!;

    [InverseProperty("Entrada")]
    public virtual ICollection<ContasAPagar> ContasAPagars { get; set; } = new List<ContasAPagar>();

    [ForeignKey("CdForn, Unity")]
    [InverseProperty("Entrada")]
    public virtual Fornecedor Fornecedor { get; set; } = null!;

    [InverseProperty("Entrada")]
    public virtual ICollection<ProdutoEntradum> ProdutoEntrada { get; set; } = new List<ProdutoEntradum>();

    [ForeignKey("Unity")]
    [InverseProperty("Entrada")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}
