﻿using System;
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

    public string GetId()
    {
        return Codigo;
    }

    public string GetKeyName()
    {
        return "Codigo";
    }
}