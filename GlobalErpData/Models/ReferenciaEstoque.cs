using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("referencia_estoque")]
public partial class ReferenciaEstoque : IIdentifiable<int>
{
    [Key]
    [Column("cd_ref")]
    public int CdRef { get; set; }

    [Column("nm_ref")]
    [StringLength(62)]
    public string NmRef { get; set; } = null!;

    [Column("unity")]
    public int Unity { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [JsonIgnore]
    [InverseProperty("CdRefNavigation")]
    public virtual ICollection<ProdutoEstoque> ProdutoEstoques { get; set; } = new List<ProdutoEstoque>();

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("ReferenciaEstoques")]
    public virtual Unity UnityNavigation { get; set; } = null!;

    [GraphQLIgnore]
    public int GetId()
    {
        return this.CdRef;
    }

    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "CdRef";
    }
}
