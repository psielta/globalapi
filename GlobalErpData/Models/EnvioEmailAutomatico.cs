using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("envio_email_automatico")]
public partial class EnvioEmailAutomatico
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("mes_base")]
    [StringLength(7)]
    public string MesBase { get; set; } = null!;

    [Column("email_envio")]
    [StringLength(255)]
    public string EmailEnvio { get; set; } = null!;

    [Column("email_contador")]
    [StringLength(255)]
    public string EmailContador { get; set; } = null!;

    [Column("hr_lanc")]
    [Precision(0, 0)]
    public TimeOnly HrLanc { get; set; }

    [Column("dt_lanc")]
    public DateOnly DtLanc { get; set; }

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }
}
