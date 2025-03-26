using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("departamento")]
public partial class Departamento : IIdentifiable<long>
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [Column("nm_departamento")]
    [StringLength(255)]
    public string NmDepartamento { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("Departamentos")]
    public virtual Unity UnityNavigation { get; set; } = null!;

    [GraphQLIgnore]
    public long GetId()
    {
        return Id;
    }

    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "Id";
    }
}
