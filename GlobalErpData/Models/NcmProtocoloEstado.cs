﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("ncm_protocolo_estado")]
public partial class NcmProtocoloEstado : IIdentifiable<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("id_cab_protocolo")]
    public int IdCabProtocolo { get; set; }

    [Column("id_ncm")]
    [StringLength(8)]
    public string IdNcm { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdCabProtocolo")]
    [InverseProperty("NcmProtocoloEstados")]
    public virtual ProtocoloEstadoNcm IdCabProtocoloNavigation { get; set; } = null!;

    [JsonPropertyName("nmProtocolo")]
    [NotMapped]
    public string NmProtocolo => IdCabProtocoloNavigation?.Nome ?? "";

    [JsonIgnore]
    [ForeignKey("IdEmpresa")]
    [InverseProperty("NcmProtocoloEstados")]
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