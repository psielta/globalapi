using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("mdfe_infcarregamento", Schema = "mdfe")]
public partial class MdfeInfcarregamento
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_mdfe")]
    [StringLength(10)]
    public string IdMdfe { get; set; } = null!;

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("id_cidade")]
    [StringLength(10)]
    public string IdCidade { get; set; } = null!;

    [ForeignKey("IdEmpresa")]
    [InverseProperty("MdfeInfcarregamentos")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}
