using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("uairango_empresa_categoria")]
public partial class UairangoEmpresaCategorium
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("cd_grupo")]
    public int CdGrupo { get; set; }

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("last_update", TypeName = "timestamp(6) without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [ForeignKey("CdEmpresa")]
    [InverseProperty("UairangoEmpresaCategoria")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [ForeignKey("CdGrupo")]
    [InverseProperty("UairangoEmpresaCategoria")]
    public virtual GrupoEstoque CdGrupoNavigation { get; set; } = null!;

    [ForeignKey("Unity")]
    [InverseProperty("UairangoEmpresaCategoria")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}
