using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("grupo_estoque")]
public partial class GrupoEstoque
{
    [Key]
    [Column("cd_grupo")]
    public int CdGrupo { get; set; }

    [Column("nm_grupo")]
    [StringLength(62)]
    public string NmGrupo { get; set; } = null!;

    [Column("unity")]
    public int Unity { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [InverseProperty("CdGrupoNavigation")]
    public virtual ICollection<ProdutoEstoque> ProdutoEstoques { get; set; } = new List<ProdutoEstoque>();

    [ForeignKey("Unity")]
    [InverseProperty("GrupoEstoques")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}
