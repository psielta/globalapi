﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("certificados")]
public partial class Certificado
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

    /// <summary>
    /// H - Homologacao
    /// P - Producao
    /// </summary>
    [Column("tipo")]
    [StringLength(1)]
    public string? Tipo { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("Certificados")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}