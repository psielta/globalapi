using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("entrega_nfe")]
public partial class EntregaNfe : IIdentifiable<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

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

    [JsonIgnore]
    [ForeignKey("IdCliente")]
    [InverseProperty("EntregaNves")]
    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdEmpresa")]
    [InverseProperty("EntregaNves")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

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
