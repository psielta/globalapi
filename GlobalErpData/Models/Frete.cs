﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("frete")]
public partial class Frete : IIdentifiable<int>
{
    [Key]
    [Column("nr_lanc")]
    public int NrLanc { get; set; }

    [Column("nr_saida")]
    public int NrSaida { get; set; }

    [Column("frete_por_conta")]
    public int FretePorConta { get; set; }

    [Column("vl_frete")]
    [Precision(18, 4)]
    public decimal VlFrete { get; set; }

    [Column("cd_transp")]
    public int? CdTransp { get; set; }

    [Column("quant")]
    [Precision(18, 4)]
    public decimal? Quant { get; set; }

    [Column("especie")]
    [StringLength(62)]
    public string? Especie { get; set; }

    [Column("marca")]
    [StringLength(62)]
    public string? Marca { get; set; }

    [Column("numeracao")]
    [StringLength(62)]
    public string? Numeracao { get; set; }

    [Column("pbruto")]
    [Precision(18, 4)]
    public decimal? Pbruto { get; set; }

    [Column("pliq")]
    [Precision(18, 4)]
    public decimal? Pliq { get; set; }
    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [JsonIgnore]
    [ForeignKey("NrSaida")]
    [InverseProperty("Fretes")]
    public virtual Saida NrSaidaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("CdTransp, Unity")]
    [InverseProperty("Fretes")]
    public virtual Transportadora? Transportadora { get; set; }

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("Fretes")]
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
