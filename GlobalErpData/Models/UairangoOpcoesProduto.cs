﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("uairango_opcoes_produto")]
public partial class UairangoOpcoesProduto
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("uairango_id_preco")]
    public int? UairangoIdPreco { get; set; }

    [Column("uairango_id_opcao")]
    public int? UairangoIdOpcao { get; set; }

    [Column("uairango_codigo")]
    [StringLength(50)]
    public string? UairangoCodigo { get; set; }

    [Column("uairango_nome")]
    [StringLength(255)]
    public string? UairangoNome { get; set; }

    [Column("uairango_valor")]
    [Precision(20, 4)]
    public decimal? UairangoValor { get; set; }

    [Column("uairango_valor_atual")]
    [Precision(20, 4)]
    public decimal? UairangoValorAtual { get; set; }

    [Column("uairango_valor2")]
    [Precision(20, 4)]
    public decimal? UairangoValor2 { get; set; }

    [Column("uairango_status")]
    public int? UairangoStatus { get; set; }

    [Column("cd_produto")]
    public long CdProduto { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [ConcurrencyCheck]
    [Column("last_update", TypeName = "timestamp(0) without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [JsonIgnore]
    [ForeignKey("CdProduto")]
    [InverseProperty("UairangoOpcoesProdutos")]
    public virtual ProdutoEstoque CdProdutoNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("UairangoOpcoesProdutos")]
    public virtual Unity UnityNavigation { get; set; } = null!;

}
