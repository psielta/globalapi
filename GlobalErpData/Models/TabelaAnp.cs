using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("tabela_anp")]
public partial class TabelaAnp
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("codigo")]
    public int? Codigo { get; set; }

    [Column("produto")]
    [StringLength(150)]
    public string? Produto { get; set; }

    [Column("familia")]
    [StringLength(80)]
    public string? Familia { get; set; }

    [Column("grupo")]
    [StringLength(80)]
    public string? Grupo { get; set; }

    [Column("sub_grupo")]
    [StringLength(80)]
    public string? SubGrupo { get; set; }

    [Column("sub_subgrupo")]
    [StringLength(80)]
    public string? SubSubgrupo { get; set; }

    [Column("unidade_grandeza")]
    [StringLength(50)]
    public string? UnidadeGrandeza { get; set; }

    [Column("unidade_medida_simp")]
    [StringLength(5)]
    public string? UnidadeMedidaSimp { get; set; }

    [Column("ramo")]
    [StringLength(50)]
    public string? Ramo { get; set; }

    [Column("data_inicio_validade")]
    [StringLength(6)]
    public string? DataInicioValidade { get; set; }

    [Column("data_final_validade")]
    [StringLength(6)]
    public string? DataFinalValidade { get; set; }
}
