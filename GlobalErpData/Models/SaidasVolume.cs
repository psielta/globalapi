using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("saidas_volumes")]
public partial class SaidasVolume : IIdentifiable<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nr_saida")]
    public int NrSaida { get; set; }

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("q_vol")]
    public int? QVol { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [Column("esp")]
    [StringLength(60)]
    public string? Esp { get; set; }

    [Column("marca")]
    [StringLength(60)]
    public string? Marca { get; set; }

    [Column("n_vol")]
    [StringLength(60)]
    public string? NVol { get; set; }

    [Column("peso_l")]
    [Precision(18, 3)]
    public decimal? PesoL { get; set; }

    [Column("peso_b")]
    [Precision(18, 3)]
    public decimal? PesoB { get; set; }

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("SaidasVolumes")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("NrSaida")]
    [InverseProperty("SaidasVolumes")]
    public virtual Saida NrSaidaNavigation { get; set; } = null!;

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
