﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("older_items")]
public partial class OlderItem : IIdentifiable<Guid>
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("older_id")]
    public Guid? OlderId { get; set; }

    [Column("item_number")]
    public int ItemNumber { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("price")]
    [Precision(10, 2)]
    public decimal Price { get; set; }

    [Column("subtotal")]
    [Precision(10, 2)]
    public decimal Subtotal { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("cd_produto")]
    public int CdProduto { get; set; }

    [JsonIgnore]
    [ForeignKey("OlderId")]
    [InverseProperty("OlderItems")]
    public virtual Older? Older { get; set; }

    [JsonIgnore]
    [ForeignKey("CdProduto, IdEmpresa")]
    [InverseProperty("OlderItems")]
    public virtual ProdutoEstoque ProdutoEstoque { get; set; } = null!;

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