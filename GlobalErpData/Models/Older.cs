using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

public enum StatusOlder
{
    Pendente = 0,
    Cancelado = 1,
    Aprovado = 2,
    EmPreparo = 3,
    EmEntrega = 4,
    Entregue = 5
}

[Table("older")]
public partial class Older : IIdentifiable<Guid>
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

    [Column("status")]
    public StatusOlder Status { get; set; }


    [Column("customer_id")]
    public Guid CustomerId { get; set; }

    [Column("customer_name")]
    [StringLength(255)]
    public string CustomerName { get; set; } = null!;

    [Column("customer_phone")]
    [StringLength(20)]
    public string? CustomerPhone { get; set; }

    [Column("customer_email")]
    [StringLength(255)]
    public string? CustomerEmail { get; set; }

    [Column("subtotal")]
    [Precision(10, 2)]
    public decimal Subtotal { get; set; }

    [Column("discount")]
    [Precision(10, 2)]
    public decimal Discount { get; set; }

    [Column("shipping")]
    [Precision(10, 2)]
    public decimal Shipping { get; set; }

    [Column("taxes")]
    [Precision(10, 2)]
    public decimal Taxes { get; set; }

    [Column("total")]
    [Precision(10, 2)]
    public decimal Total { get; set; }

    [Column("items", TypeName = "jsonb")]
    public string? Items { get; set; }

    [JsonIgnore]
    [ForeignKey("IdEmpresa")]
    [InverseProperty("Olders")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [InverseProperty("Older")]
    public virtual ICollection<OlderItem> OlderItems { get; set; } = new List<OlderItem>();

    [GraphQLIgnore]
    public Guid GetId()
    {
        return Id;
    }

    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "Id";
    }
}
