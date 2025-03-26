using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("departamento")]
public partial class Departamento
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [Column("nm_departamento")]
    [StringLength(255)]
    public string NmDepartamento { get; set; } = null!;

    [ForeignKey("Unity")]
    [InverseProperty("Departamentos")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}
