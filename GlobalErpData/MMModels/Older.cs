using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("older")]
public partial class Older
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }

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

    [Column("status")]
    public int Status { get; set; }

    [Column("customer_city")]
    [StringLength(7)]
    public string CustomerCity { get; set; } = null!;

    [Column("customer_neighborhood")]
    [StringLength(100)]
    public string CustomerNeighborhood { get; set; } = null!;

    [Column("customer_number")]
    [StringLength(10)]
    public string CustomerNumber { get; set; } = null!;

    [Column("customer_zip")]
    [StringLength(20)]
    public string CustomerZip { get; set; } = null!;

    [Column("customer_complement")]
    [StringLength(255)]
    public string? CustomerComplement { get; set; }

    [Column("customer_reference")]
    [StringLength(255)]
    public string? CustomerReference { get; set; }

    [Column("customer_address")]
    [StringLength(255)]
    public string CustomerAddress { get; set; } = null!;

    [Column("integrated")]
    public bool? Integrated { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("Olders")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    [InverseProperty("Older")]
    public virtual ICollection<OlderItem> OlderItems { get; set; } = new List<OlderItem>();
}
