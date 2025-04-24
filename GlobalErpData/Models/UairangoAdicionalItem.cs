using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("uairango_adicional_item")]
public partial class UairangoAdicionalItem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_adicional")]
    public int? IdAdicional { get; set; }

    [Column("id_cab")]
    public int IdCab { get; set; }

    [Column("id_tipo")]
    public int? IdTipo { get; set; }

    [Column("codigo")]
    [StringLength(255)]
    public string? Codigo { get; set; }

    [Column("nome")]
    [StringLength(255)]
    public string? Nome { get; set; }

    [Column("valor")]
    [Precision(20, 4)]
    public decimal? Valor { get; set; }

    [Column("status")]
    public int? Status { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [Column("last_update", TypeName = "timestamp(6) without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [JsonIgnore]
    [ForeignKey("IdCab")]
    [InverseProperty("UairangoAdicionalItems")]
    public virtual UairangoAdicionalCab IdCabNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("UairangoAdicionalItems")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}
