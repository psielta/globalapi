﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[PrimaryKey("Chave", "IdUsuario")]
[Table("configuracoes_usuario")]
public partial class ConfiguracoesUsuario : IIdentifiableMultiKey<int, string>
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
    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }
    [Key]
    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [JsonIgnore]
    [ForeignKey("IdUsuario")]
    [InverseProperty("ConfiguracoesUsuarios")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    [GraphQLIgnore]
    public (int, string) GetId()
    {
        return (IdUsuario, Chave);
    }

    [GraphQLIgnore]
    public string GetKeyName1()
    {
        return "IdUsuario";
    }

    [GraphQLIgnore]
    public string GetKeyName2()
    {
        return "Chave";
    }
}
