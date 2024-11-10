using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalPdvData.Models;

[Table("cidade")]
public partial class Cidade : IIdentifiable<string>
{
    [Key]
    [Column("cd_cidade")]
    [StringLength(10)]
    public string CdCidade { get; set; } = null!;

    [Column("nm_cidade")]
    [StringLength(150)]
    public string NmCidade { get; set; } = null!;

    [Column("uf")]
    [StringLength(2)]
    public string Uf { get; set; } = null!;

    [JsonIgnore]
    [InverseProperty("CdCidadeNavigation")]
    public virtual ICollection<Empresa> Empresas { get; set; } = new List<Empresa>();

    [GraphQLIgnore]
    public string GetId()
    {
        return this.CdCidade;
    }

    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "CdCidade";
    }
}
