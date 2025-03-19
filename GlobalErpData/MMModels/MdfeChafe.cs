using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("mdfe_chaves", Schema = "mdfe")]
public partial class MdfeChafe
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nr_mdfe")]
    [StringLength(10)]
    public string NrMdfe { get; set; } = null!;

    [Column("id_municipio")]
    [StringLength(10)]
    public string? IdMunicipio { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("MdfeChaves")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    [InverseProperty("IdChaveNavigation")]
    public virtual ICollection<MdfeChavesDfe> MdfeChavesDves { get; set; } = new List<MdfeChavesDfe>();
}
