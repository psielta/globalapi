using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Keyless]
[Table("orcamento_cab0eqeti", Schema = "pg_temp_3")]
public partial class OrcamentoCab0eqeti
{
    [Column("id")]
    public Guid? Id { get; set; }

    [Column("sequencia")]
    public long? Sequencia { get; set; }

    [Column("unity")]
    public int? Unity { get; set; }

    [Column("empresa")]
    public int? Empresa { get; set; }

    [Column("id_cliente")]
    public int? IdCliente { get; set; }

    [Column("gerado", TypeName = "character varying")]
    public string? Gerado { get; set; }

    [Column("id_funcionario")]
    public int? IdFuncionario { get; set; }

    [Column("valor_produtos")]
    [Precision(18, 4)]
    public decimal? ValorProdutos { get; set; }

    [Column("valor_acrescimo")]
    [Precision(18, 4)]
    public decimal? ValorAcrescimo { get; set; }

    [Column("valor_desconto")]
    [Precision(18, 4)]
    public decimal? ValorDesconto { get; set; }

    [Column("valor_comissao")]
    [Precision(18, 4)]
    public decimal? ValorComissao { get; set; }

    [Column("valor_total")]
    [Precision(18, 4)]
    public decimal? ValorTotal { get; set; }

    [Column("valor_servicos")]
    [Precision(18, 4)]
    public decimal? ValorServicos { get; set; }

    [Column("percentual_comissao")]
    [Precision(18, 4)]
    public decimal? PercentualComissao { get; set; }
}
