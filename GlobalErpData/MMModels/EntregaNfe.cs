using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("entrega_nfe")]
public partial class EntregaNfe
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [Column("id_cliente")]
    public int IdCliente { get; set; }

    [Column("cnpjcpf")]
    [StringLength(20)]
    public string? Cnpjcpf { get; set; }

    [Column("ie")]
    [StringLength(20)]
    public string? Ie { get; set; }

    [Column("xnome")]
    [StringLength(255)]
    public string? Xnome { get; set; }

    [Column("xlgr")]
    [StringLength(255)]
    public string? Xlgr { get; set; }

    [Column("nro")]
    [StringLength(20)]
    public string? Nro { get; set; }

    [Column("xcpl")]
    [StringLength(255)]
    public string? Xcpl { get; set; }

    [Column("xbairro")]
    [StringLength(255)]
    public string? Xbairro { get; set; }

    [Column("cmun")]
    public int? Cmun { get; set; }

    [Column("xmun")]
    [StringLength(255)]
    public string? Xmun { get; set; }

    [Column("uf")]
    [StringLength(2)]
    public string? Uf { get; set; }

    [Column("cep")]
    [StringLength(10)]
    public string? Cep { get; set; }

    [Column("fone")]
    [StringLength(20)]
    public string? Fone { get; set; }

    [Column("email")]
    [StringLength(255)]
    public string? Email { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [ForeignKey("IdCliente")]
    [InverseProperty("EntregaNves")]
    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    [ForeignKey("Unity")]
    [InverseProperty("EntregaNves")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}
