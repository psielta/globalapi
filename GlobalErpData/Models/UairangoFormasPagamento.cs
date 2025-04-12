using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("uairango_formas_pagamento")]
public partial class UairangoFormasPagamento
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_forma_uairango")]
    [StringLength(20)]
    public string IdFormaUairango { get; set; } = null!;

    [Column("nome")]
    [StringLength(255)]
    public string? Nome { get; set; }

    [Column("ativo")]
    public int? Ativo { get; set; }

    [Column("tipo_entrega")]
    [StringLength(20)]
    public string? TipoEntrega { get; set; }

    [Column("empresa")]
    public int Empresa { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [Column("last_update", TypeName = "timestamp(6) without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [JsonIgnore]
    [ForeignKey("Empresa")]
    [InverseProperty("UairangoFormasPagamentos")]
    public virtual Empresa EmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("UairangoFormasPagamentos")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}
