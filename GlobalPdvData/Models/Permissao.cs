using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalPdvData.Models;

[Table("permissao")]
public partial class Permissao : IIdentifiable<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("chave")]
    [StringLength(255)]
    public string Chave { get; set; } = null!;

    [Column("modulo")]
    [StringLength(255)]
    public string Modulo { get; set; } = null!;

    [Column("descricao")]
    [StringLength(255)]
    public string? Descricao { get; set; }

    [JsonIgnore]
    [InverseProperty("IdPermissaoNavigation")]
    public virtual ICollection<UsuarioPermissao> UsuarioPermissaos { get; set; } = new List<UsuarioPermissao>();
    
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
