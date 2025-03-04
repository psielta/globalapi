using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("cst")]
[Index("Descricao", Name = "cst_idx")]
public partial class Cst : IIdentifiable<string>
{
    [Key]
    [Column("codigo")]
    [StringLength(2)]
    public string Codigo { get; set; } = null!;

    [Column("descricao")]
    [StringLength(150)]
    public string Descricao { get; set; } = null!;
    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }
    public string GetId()
    {
        return Codigo;
    }

    public string GetKeyName()
    {
        return "Codigo";
    }
}
