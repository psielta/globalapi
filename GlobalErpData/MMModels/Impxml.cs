using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[PrimaryKey("IdEmpresa", "ChaveAcesso")]
[Table("impxml")]
public partial class Impxml
{
    [Key]
    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Key]
    [Column("chave_acesso")]
    [StringLength(44)]
    public string ChaveAcesso { get; set; } = null!;

    [Column("type")]
    public int Type { get; set; }

    [Column("xml", TypeName = "xml")]
    public string Xml { get; set; } = null!;

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("Impxmls")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}
