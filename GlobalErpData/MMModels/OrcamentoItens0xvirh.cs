using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Keyless]
[Table("orcamento_itens0xvirh", Schema = "pg_temp_3")]
public partial class OrcamentoItens0xvirh
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

    [Column("gerado")]
    [StringLength(1)]
    public string? Gerado { get; set; }

    [Column("id_funcionario")]
    public int? IdFuncionario { get; set; }

    [Column("id_produto")]
    public long? IdProduto { get; set; }

    [Column("qtde")]
    [Precision(18, 4)]
    public decimal? Qtde { get; set; }

    [Column("valor_unitario")]
    [Precision(18, 4)]
    public decimal? ValorUnitario { get; set; }

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

    [Column("id_cab")]
    public Guid? IdCab { get; set; }

    [Column("sequencia_cab")]
    public long? SequenciaCab { get; set; }

    [Column("percentual_comissao")]
    [Precision(18, 4)]
    public decimal? PercentualComissao { get; set; }
}
