using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("permissao")]
public partial class Permissao
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("chave")]
    [StringLength(255)]
    public string Chave { get; set; } = null!;

    [Column("modulo")]
    [StringLength(255)]
    public string Modulo { get; set; } = null!;

    [Column("descricao")]
    [StringLength(255)]
    public string? Descricao { get; set; }

    [InverseProperty("IdPermissaoNavigation")]
    public virtual ICollection<UsuarioPermissao> UsuarioPermissaos { get; set; } = new List<UsuarioPermissao>();
}
