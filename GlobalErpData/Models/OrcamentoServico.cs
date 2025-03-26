using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("orcamento_servicos")]
[Index("Sequencia", Name = "orcamento_servicos_sequencia_key", IsUnique = true)]
public partial class OrcamentoServico : IIdentifiable<Guid>
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("sequencia")]
    public long Sequencia { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [Column("empresa")]
    public int Empresa { get; set; }

    [Column("id_cliente")]
    public int IdCliente { get; set; }

    [Column("gerado")]
    [StringLength(1)]
    public string Gerado { get; set; } = null!;

    [Column("id_funcionario")]
    public int IdFuncionario { get; set; }

    [Column("percentual_comissao")]
    [Precision(18, 4)]
    public decimal PercentualComissao { get; set; }

    [Column("cd_mecanico")]
    public int? CdMecanico { get; set; }

    [Column("cd_mecanico2")]
    public int? CdMecanico2 { get; set; }

    [Column("txt_relato_cliente")]
    [StringLength(16384)]
    public string? TxtRelatoCliente { get; set; }

    [Column("txt_divergencia")]
    [StringLength(16384)]
    public string? TxtDivergencia { get; set; }

    [Column("txt_aval_tecnico")]
    [StringLength(16384)]
    public string? TxtAvalTecnico { get; set; }

    [Column("lado")]
    [StringLength(20)]
    public string? Lado { get; set; }

    [Column("id_servico")]
    public long IdServico { get; set; }

    [Column("qtde")]
    [Precision(18, 4)]
    public decimal Qtde { get; set; }

    [Column("valor_unitario")]
    [Precision(18, 4)]
    public decimal ValorUnitario { get; set; }

    [Column("valor_acrescimo")]
    [Precision(18, 4)]
    public decimal ValorAcrescimo { get; set; }

    [Column("valor_desconto")]
    [Precision(18, 4)]
    public decimal ValorDesconto { get; set; }

    [Column("valor_comissao")]
    [Precision(18, 4)]
    public decimal ValorComissao { get; set; }

    [Column("valor_total")]
    [Precision(18, 4)]
    public decimal ValorTotal { get; set; }

    [Column("id_cab")]
    public Guid IdCab { get; set; }

    [Column("sequencia_cab")]
    public long SequenciaCab { get; set; }

    [JsonIgnore]
    [ForeignKey("IdServico")]
    [InverseProperty("OrcamentoServicos")]
    public virtual Servico IdServicoNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdCab, SequenciaCab, Unity, Empresa, IdCliente, Gerado, IdFuncionario, PercentualComissao")]
    [InverseProperty("OrcamentoServicos")]
    public virtual OrcamentoCab OrcamentoCab { get; set; } = null!;

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
