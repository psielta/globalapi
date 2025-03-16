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

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [ForeignKey("CdEmpresa")]
    [InverseProperty("GrupoEstoques")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [InverseProperty("CdGrupoNavigation")]
    public virtual ICollection<ProdutoEstoque> ProdutoEstoques { get; set; } = new List<ProdutoEstoque>();
}
