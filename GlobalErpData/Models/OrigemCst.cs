using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("origem_cst")]
public partial class OrigemCst : IIdentifiable<string>
{
    [Key]
    [Column("codigo")]
    [StringLength(1)]
    public string Codigo { get; set; } = null!;

    [Column("nome")]
    [StringLength(256)]
    public string Nome { get; set; } = null!;

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
