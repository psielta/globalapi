using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("pagtos_parciais_cp")]
public partial class PagtosParciaisCp : IIdentifiable<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_contas_pagar")]
    public int? IdContasPagar { get; set; }

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

    [Column("pdf_pagamento")]
    public byte[]? PdfPagamento { get; set; }

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("PagtosParciaisCps")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdContasPagar")]
    [InverseProperty("PagtosParciaisCps")]
    public virtual ContasAPagar? IdContasPagarNavigation { get; set; }

    [GraphQLIgnore]
    public int GetId()
    {
        return Id;
    }
    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "Id";
    }
}
