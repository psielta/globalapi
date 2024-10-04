using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("mdfe", Schema = "mdfe")]
public partial class Mdfe
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("dt_lanc")]
    public DateOnly? DtLanc { get; set; }

    [Column("nr_dmfe")]
    [StringLength(10)]
    public string NrDmfe { get; set; } = null!;

    [Column("hora")]
    public TimeOnly? Hora { get; set; }

    [Column("status")]
    [StringLength(3)]
    public string? Status { get; set; }

    [Column("tpemit")]
    [StringLength(2)]
    public string? Tpemit { get; set; }

    [Column("modelo")]
    [StringLength(2)]
    public string? Modelo { get; set; }

    [Column("serie")]
    [StringLength(2)]
    public string? Serie { get; set; }

    [Column("modal")]
    [StringLength(2)]
    public string? Modal { get; set; }

    [Column("tpemis")]
    [StringLength(2)]
    public string? Tpemis { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("chnfe")]
    [StringLength(44)]
    public string? Chnfe { get; set; }

    [Column("xml_mdfe")]
    public string? XmlMdfe { get; set; }

    [Column("obs")]
    [StringLength(16384)]
    public string? Obs { get; set; }

    [Column("nr_autorizacao_mdfe")]
    [StringLength(62)]
    public string? NrAutorizacaoMdfe { get; set; }

    [Column("uf_saida")]
    [StringLength(2)]
    public string UfSaida { get; set; } = null!;

    [Column("uf_final")]
    [StringLength(2)]
    public string UfFinal { get; set; } = null!;

    [Column("tptransp")]
    [StringLength(2)]
    public string? Tptransp { get; set; }

    [Column("nr_proto_cancelamento")]
    [StringLength(62)]
    public string? NrProtoCancelamento { get; set; }

    [Column("txt_justificativa_cancelamento")]
    [StringLength(62)]
    public string? TxtJustificativaCancelamento { get; set; }

    [Column("nr_proto_encerramento")]
    [StringLength(62)]
    public string? NrProtoEncerramento { get; set; }

    [Column("nr_cmdf")]
    public int? NrCmdf { get; set; }

    [Column("prod_tp_carga")]
    public int? ProdTpCarga { get; set; }

    [Column("prod_descricao")]
    [StringLength(128)]
    public string? ProdDescricao { get; set; }

    [Column("prod_cean")]
    [StringLength(20)]
    public string? ProdCean { get; set; }

    [Column("prod_ncm")]
    [StringLength(20)]
    public string? ProdNcm { get; set; }

    [Column("prod_cepcar")]
    [StringLength(20)]
    public string? ProdCepcar { get; set; }

    [Column("prod_cepdes")]
    [StringLength(20)]
    public string? ProdCepdes { get; set; }

    [Column("xml_evento_cancelamento")]
    public string? XmlEventoCancelamento { get; set; }

    [Column("xml_evento_encerramento")]
    public string? XmlEventoEncerramento { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("Mdves")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}
