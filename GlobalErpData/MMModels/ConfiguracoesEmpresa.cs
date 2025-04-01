using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[PrimaryKey("Chave", "Unity")]
[Table("configuracoes_empresa")]
public partial class ConfiguracoesEmpresa
{
    [Key]
    [Column("chave")]
    [StringLength(256)]
    public string Chave { get; set; } = null!;

    [Column("valor_1")]
    [StringLength(16384)]
    public string? Valor1 { get; set; }

    [Column("valor_2")]
    [StringLength(16384)]
    public string? Valor2 { get; set; }

    [Column("valor_3")]
    [StringLength(16384)]
    public string? Valor3 { get; set; }

    [Key]
    [Column("unity")]
    public int Unity { get; set; }

    [Column("valor_4")]
    [StringLength(16384)]
    public string? Valor4 { get; set; }

    [Column("valor_5")]
    [StringLength(16384)]
    public string? Valor5 { get; set; }

    [Column("valor_6")]
    [StringLength(16384)]
    public string? Valor6 { get; set; }

    [Column("valor_7")]
    [StringLength(16384)]
    public string? Valor7 { get; set; }

    [Column("valor_8")]
    [StringLength(16384)]
    public string? Valor8 { get; set; }

    [Column("valor_9")]
    [StringLength(16384)]
    public string? Valor9 { get; set; }

    [Column("valor_10")]
    [StringLength(16384)]
    public string? Valor10 { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [ForeignKey("Unity")]
    [InverseProperty("ConfiguracoesEmpresas")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}
