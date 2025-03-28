﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("certificados")]
[Index("IdEmpresa", "Unity", Name = "certificados_idx", IsUnique = true)]
public partial class Certificado: IIdentifiable<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("serial_certificado")]
    [StringLength(62)]
    public string SerialCertificado { get; set; } = null!;

    [Column("senha")]
    [StringLength(25)]
    public string? Senha { get; set; }

    [Column("caminho_certificado")]
    [StringLength(152)]
    public string? CaminhoCertificado { get; set; }

    [Column("validade_cert")]
    public DateOnly? ValidadeCert { get; set; }

    [Column("certificado")]
    public string? Certificado1 { get; set; }
    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    /// <summary>
    /// H - Homologacao
    /// P - Producao
    /// </summary>
    [Column("tipo")]
    [StringLength(1)]
    public string? Tipo { get; set; }

    [Column("certificado_byte")]
    public byte[]? CertificadoByte { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [JsonIgnore]
    [ForeignKey("IdEmpresa")]
    [InverseProperty("Certificados")]
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

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("Certificados")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}
