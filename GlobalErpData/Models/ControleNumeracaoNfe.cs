﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("controle_numeracao_nfe")]
[Index("IdEmpresa", "Serie", Name = "controle_numeracao_nfe_idx", IsUnique = true)]
public partial class ControleNumeracaoNfe : IIdentifiable<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("serie")]
    public int Serie { get; set; }

    [Column("proximo_numero")]
    public long ProximoNumero { get; set; }

    [Column("padrao")]
    public bool Padrao { get; set; }

    [JsonIgnore]
    [ForeignKey("IdEmpresa")]
    [InverseProperty("ControleNumeracaoNfe")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
    
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