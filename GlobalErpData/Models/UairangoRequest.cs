using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("uairango_requests")]
public partial class UairangoRequest
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("datahora", TypeName = "timestamp(0) without time zone")]
    public DateTime Datahora { get; set; }

    [Column("endpoint")]
    [StringLength(255)]
    public string? Endpoint { get; set; }

    [Column("metodo")]
    [StringLength(10)]
    public string? Metodo { get; set; }

    [Column("status")]
    public int? Status { get; set; }

    [Column("empresa")]
    public int? Empresa { get; set; }

    [Column("obs")]
    [StringLength(4096)]
    public string? Obs { get; set; }

    [JsonIgnore]
    [ForeignKey("Empresa")]
    [InverseProperty("UairangoRequests")]
    public virtual Empresa? EmpresaNavigation { get; set; }
}
