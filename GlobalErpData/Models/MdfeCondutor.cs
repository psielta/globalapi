using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("mdfe_condutor", Schema = "mdfe")]
public partial class MdfeCondutor
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_mdfe")]
    [StringLength(10)]
    public string IdMdfe { get; set; } = null!;

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("xnome")]
    [StringLength(60)]
    public string? Xnome { get; set; }

    [Column("cpf")]
    [StringLength(18)]
    public string? Cpf { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("MdfeCondutors")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}
