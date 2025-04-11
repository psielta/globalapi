using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("uairango_culinarias")]
public partial class UairangoCulinaria
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nm_culinaria")]
    [StringLength(512)]
    public string? NmCulinaria { get; set; }

    [Column("meio_meio")]
    public int? MeioMeio { get; set; }

    [Column("id_culinaria_uairango")]
    [StringLength(255)]
    public string? IdCulinariaUairango { get; set; }
}
