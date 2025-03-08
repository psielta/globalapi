using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("unity")]
public partial class Unity : IIdentifiable<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string? Name { get; set; }

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Empresa> Empresas { get; set; } = new List<Empresa>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

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
