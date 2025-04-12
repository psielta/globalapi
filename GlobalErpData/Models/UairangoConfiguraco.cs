using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("uairango_configuracoes")]
public partial class UairangoConfiguraco
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [Column("chave")]
    [StringLength(255)]
    public string Chave { get; set; } = null!;

    [Column("valor")]
    [StringLength(512)]
    public string? Valor { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [Column("last_update", TypeName = "timestamp(6) without time zone")]
    public DateTime? LastUpdate { get; set; }

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("UairangoConfiguracos")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("UairangoConfiguracos")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}
