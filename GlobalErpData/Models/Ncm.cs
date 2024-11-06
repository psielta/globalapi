using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("ncm")]
[Index("Ncm1", Name = "idx_ncm_codigo")]
public partial class Ncm : IIdentifiable<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("ncm")]
    [StringLength(8)]
    public string? Ncm1 { get; set; }

    [Column("dt_vigencia")]
    public DateOnly? DtVigencia { get; set; }

    [Column("unidade")]
    [StringLength(10)]
    public string? Unidade { get; set; }

    [Column("descricao")]
    [StringLength(255)]
    public string? Descricao { get; set; }

    [GraphQLIgnore]
    public int GetId()
    {
        return Id;
    }

    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "Id";
    }
}
