﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("usuario_permissao")]
public partial class UsuarioPermissao : IIdentifiable<int>
{

    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_usuario")]
    [StringLength(255)]
    public string IdUsuario { get; set; } = null!;

    [Column("id_permissao")]
    public int IdPermissao { get; set; }


    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [JsonIgnore]
    [ForeignKey("IdPermissao")]
    [InverseProperty("UsuarioPermissaos")]
    public virtual Permissao IdPermissaoNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdUsuario")]
    [InverseProperty("UsuarioPermissaos")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

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
