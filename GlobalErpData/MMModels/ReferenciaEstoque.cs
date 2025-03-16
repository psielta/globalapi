using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("referencia_estoque")]
public partial class ReferenciaEstoque
{
    [Key]
    [Column("cd_ref")]
    public int CdRef { get; set; }

    [Column("nm_ref")]
    [StringLength(62)]
    public string NmRef { get; set; } = null!;

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [ForeignKey("CdEmpresa")]
    [InverseProperty("ReferenciaEstoques")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [InverseProperty("CdRefNavigation")]
    public virtual ICollection<ProdutoEstoque> ProdutoEstoques { get; set; } = new List<ProdutoEstoque>();
}
