﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("unidade_medida")]
[Index("CdUnidade", "IdEmpresa", Name = "unidade_medida_idx", IsUnique = true)]
public partial class UnidadeMedidum
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("cd_unidade")]
    [StringLength(6)]
    public string CdUnidade { get; set; } = null!;

    [Column("descricao")]
    [StringLength(62)]
    public string Descricao { get; set; } = null!;

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("UnidadeMedida")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}
