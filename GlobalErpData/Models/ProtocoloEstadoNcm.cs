﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("protocolo_estado_ncm")]
public partial class ProtocoloEstadoNcm : IIdentifiable<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [Column("ativo")]
    [StringLength(1)]
    public string Ativo { get; set; } = null!;

    [Column("nome")]
    [StringLength(62)]
    public string Nome { get; set; } = null!;

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [Column("uf")]
    [StringLength(2)]
    public string Uf { get; set; } = null!;

    [Column("iva")]
    [Precision(18, 4)]
    public decimal? Iva { get; set; }

    [Column("st")]
    [Precision(18, 4)]
    public decimal? St { get; set; }

    [Column("red_st")]
    [Precision(18, 4)]
    public decimal? RedSt { get; set; }

    [Column("red_icms")]
    [Precision(18, 4)]
    public decimal? RedIcms { get; set; }

    [Column("txt_obs")]
    [StringLength(16384)]
    public string? TxtObs { get; set; }

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("ProtocoloEstadoNcms")]
    public virtual Unity UnityNavigation { get; set; } = null!;

    [JsonIgnore]
    [InverseProperty("IdCabProtocoloNavigation")]
    public virtual ICollection<NcmProtocoloEstado> NcmProtocoloEstados { get; set; } = new List<NcmProtocoloEstado>();

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
