using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("mdfe_chaves_dfe", Schema = "mdfe")]
public partial class MdfeChavesDfe
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_chave")]
    public int IdChave { get; set; }

    [Column("chave_dfe")]
    [StringLength(44)]
    public string ChaveDfe { get; set; } = null!;

    [Column("tipo")]
    [StringLength(1)]
    public string? Tipo { get; set; }

    [Column("cnpj")]
    [StringLength(20)]
    public string? Cnpj { get; set; }

    [ForeignKey("IdChave")]
    [InverseProperty("MdfeChavesDves")]
    public virtual MdfeChafe IdChaveNavigation { get; set; } = null!;
}
