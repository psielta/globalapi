using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("ibpt")]
[Index("Codigo", Name = "idx_ibpt_codigo")]
public partial class Ibpt : IIdentifiable<int>
{
    [Key]
    [Column("nr_lanc")]
    public int NrLanc { get; set; }

    [Column("codigo")]
    [StringLength(8)]
    public string Codigo { get; set; } = null!;

    [Column("ex")]
    [StringLength(15)]
    public string? Ex { get; set; }

    [Column("tabela")]
    [StringLength(15)]
    public string? Tabela { get; set; }

    [Column("aliqnac")]
    [Precision(18, 2)]
    public decimal? Aliqnac { get; set; }

    [Column("aliqimp")]
    [Precision(18, 2)]
    public decimal? Aliqimp { get; set; }

    [Column("descricao")]
    [StringLength(150)]
    public string? Descricao { get; set; }

    [Column("aliqestadual")]
    [Precision(18, 2)]
    public decimal? Aliqestadual { get; set; }

    [Column("aliqmunicipal")]
    [Precision(18, 2)]
    public decimal? Aliqmunicipal { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [GraphQLIgnore]
    public int GetId()
    {
        return NrLanc;
    }

    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "NrLanc";
    }
}
