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

    [Column("unity")]
    public int Unity { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [InverseProperty("CdRefNavigation")]
    public virtual ICollection<ProdutoEstoque> ProdutoEstoques { get; set; } = new List<ProdutoEstoque>();

    [ForeignKey("Unity")]
    [InverseProperty("ReferenciaEstoques")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}
