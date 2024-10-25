using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[PrimaryKey("Nr", "CdEmpresa")]
[Table("entradas")]
public partial class Entrada : IIdentifiableMultiKey<int, int>
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

    [JsonPropertyName("nmPlano")]
    [NotMapped]
    public string NmPlano => CdGrupoEstoqueNavigation?.NmPlano ?? string.Empty;

    [JsonPropertyName("nmForn")]
    [NotMapped]
    public string NmForn => Fornecedor?.NmForn ?? "";

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("Entrada")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("CdGrupoEstoque")]
    [InverseProperty("Entrada")]
    public virtual PlanoEstoque CdGrupoEstoqueNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("CdForn, CdEmpresa")]
    [InverseProperty("Entrada")]
    public virtual Fornecedor Fornecedor { get; set; } = null!;

    [JsonIgnore]
    [InverseProperty("Entrada")]
    public virtual ICollection<ProdutoEntradum> ProdutoEntrada { get; set; } = new List<ProdutoEntradum>();

    [JsonIgnore]
    [InverseProperty("Entrada")]
    public virtual ICollection<ContasAPagar> ContasAPagars { get; set; } = new List<ContasAPagar>();

    [GraphQLIgnore]
    public (int, int) GetId()
    {
        return (CdEmpresa, Nr);
    }

    [GraphQLIgnore]
    public string GetKeyName1()
    {
        return "CdEmpresa";
    }

    [GraphQLIgnore]
    public string GetKeyName2()
    {
        return "Nr";
    }
}
