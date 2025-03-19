using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("pagtos_parciais_cr")]
public partial class PagtosParciaisCr
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nr_conta")]
    public int? NrConta { get; set; }

    [Column("dt_pagto")]
    public DateOnly? DtPagto { get; set; }

    [Column("valor_pago")]
    [Precision(18, 2)]
    public decimal? ValorPago { get; set; }

    [Column("valor_restante")]
    [Precision(18, 2)]
    public decimal? ValorRestante { get; set; }

    [Column("acrescimo")]
    [Precision(18, 4)]
    public decimal? Acrescimo { get; set; }

    [Column("desconto")]
    [Precision(18, 4)]
    public decimal? Desconto { get; set; }

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("pdf_recebimento")]
    public byte[]? PdfRecebimento { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [ForeignKey("CdEmpresa")]
    [InverseProperty("PagtosParciaisCrs")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [ForeignKey("NrConta")]
    [InverseProperty("PagtosParciaisCrs")]
    public virtual ContasAReceber? NrContaNavigation { get; set; }
}
