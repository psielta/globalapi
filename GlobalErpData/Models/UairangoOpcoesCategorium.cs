using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("uairango_opcoes_categoria")]
public partial class UairangoOpcoesCategorium
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("cd_grupo")]
    public int CdGrupo { get; set; }

    [Column("uairango_id_categoria")]
    public int? UairangoIdCategoria { get; set; }

    [Column("uairango_id_opcao")]
    public int? UairangoIdOpcao { get; set; }

    [Column("uairango_nome")]
    [StringLength(256)]
    public string? UairangoNome { get; set; }

    [Column("uairango_status")]
    public int? UairangoStatus { get; set; }

    [Column("uairango_codigo_opcao")]
    [StringLength(50)]
    public string? UairangoCodigoOpcao { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [Column("last_update", TypeName = "timestamp(0) without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [JsonIgnore]
    [ForeignKey("CdGrupo")]
    [InverseProperty("UairangoOpcoesCategoria")]
    public virtual GrupoEstoque CdGrupoNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("UairangoOpcoesCategoria")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}
