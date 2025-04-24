using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("uairango_adicional_cab")]
public partial class UairangoAdicionalCab
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_tipo")]
    public int? IdTipo { get; set; }

    [Column("codigo_tipo")]
    [StringLength(255)]
    public string? CodigoTipo { get; set; }

    [Column("nome")]
    [StringLength(255)]
    public string? Nome { get; set; }

    [Column("selecao")]
    [StringLength(20)]
    public string? Selecao { get; set; }

    [Column("limite")]
    public int? Limite { get; set; }

    [Column("status")]
    public int? Status { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [Column("last_update", TypeName = "timestamp(6) without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("cd_grupo")]
    public int CdGrupo { get; set; }

    [Column("uairango_id_categoria")]
    public int? UairangoIdCategoria { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [JsonIgnore]
    [ForeignKey("CdGrupo")]
    [InverseProperty("UairangoAdicionalCabs")]
    public virtual GrupoEstoque CdGrupoNavigation { get; set; } = null!;

    [JsonIgnore]
    [InverseProperty("IdCabNavigation")]
    public virtual ICollection<UairangoAdicionalItem> UairangoAdicionalItems { get; set; } = new List<UairangoAdicionalItem>();

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("UairangoAdicionalCabs")]
    public virtual Unity UnityNavigation { get; set; } = null!;

    [Column("minimo")]
    public int? Minimo { get; set; }
}
