using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("icms")]
public partial class Icm : IIdentifiable<int>
{
    [Key]
    [Column("nr_lanc")]
    public int NrLanc { get; set; }

    [Column("am")]
    [Precision(18, 2)]
    public decimal? Am { get; set; }

    [Column("ac")]
    [Precision(18, 2)]
    public decimal? Ac { get; set; }

    [Column("al")]
    [Precision(18, 2)]
    public decimal? Al { get; set; }

    [Column("ap")]
    [Precision(18, 2)]
    public decimal? Ap { get; set; }

    [Column("ba")]
    [Precision(18, 2)]
    public decimal? Ba { get; set; }

    [Column("ce")]
    [Precision(18, 2)]
    public decimal? Ce { get; set; }

    [Column("df")]
    [Precision(18, 2)]
    public decimal? Df { get; set; }

    [Column("es")]
    [Precision(18, 2)]
    public decimal? Es { get; set; }

    [Column("go")]
    [Precision(18, 2)]
    public decimal? Go { get; set; }

    [Column("ma")]
    [Precision(18, 2)]
    public decimal? Ma { get; set; }

    [Column("mg")]
    [Precision(18, 2)]
    public decimal? Mg { get; set; }

    [Column("mt")]
    [Precision(18, 2)]
    public decimal? Mt { get; set; }

    [Column("ms")]
    [Precision(18, 2)]
    public decimal? Ms { get; set; }

    [Column("pa")]
    [Precision(18, 2)]
    public decimal? Pa { get; set; }

    [Column("pb")]
    [Precision(18, 2)]
    public decimal? Pb { get; set; }

    [Column("pi")]
    [Precision(18, 2)]
    public decimal? Pi { get; set; }
    
    [Column("pe")]
    [Precision(18, 2)]
    public decimal? Pe { get; set; }

    [Column("pr")]
    [Precision(18, 2)]
    public decimal? Pr { get; set; }

    [Column("rj")]
    [Precision(18, 2)]
    public decimal? Rj { get; set; }

    [Column("rn")]
    [Precision(18, 2)]
    public decimal? Rn { get; set; }

    [Column("ro")]
    [Precision(18, 2)]
    public decimal? Ro { get; set; }

    [Column("rr")]
    [Precision(18, 2)]
    public decimal? Rr { get; set; }

    [Column("rs")]
    [Precision(18, 2)]
    public decimal? Rs { get; set; }

    [Column("sc")]
    [Precision(18, 2)]
    public decimal? Sc { get; set; }

    [Column("se")]
    [Precision(18, 2)]
    public decimal? Se { get; set; }

    [Column("sp")]
    [Precision(18, 2)]
    public decimal? Sp { get; set; }

    [Column("to")]
    [Precision(18, 2)]
    public decimal? To { get; set; }

    [Column("ex")]
    [Precision(18, 2)]
    public decimal? Ex { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("Icms")]
    public virtual Unity UnityNavigation { get; set; } = null!;

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
