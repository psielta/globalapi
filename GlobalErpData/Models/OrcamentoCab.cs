using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("orcamento_cab")]
[Index("Id", "Sequencia", "Unity", "Empresa", "IdCliente", "Gerado", "IdFuncionario", "PercentualComissao", Name = "orcamento_cab_index", IsUnique = true)]
[Index("Id", "Sequencia", "Unity", "Empresa", "IdCliente", "Gerado", "IdFuncionario", "PercentualComissao", "CdPlano", Name = "orcamento_cab_index2", IsUnique = true)]
[Index("Sequencia", Name = "orcamento_cab_sequencia_key", IsUnique = true)]
public partial class OrcamentoCab : IIdentifiable<Guid>
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

    [Column("gerado", TypeName = "character varying")]
    public string Gerado { get; set; } = null!;

    [Column("id_funcionario")]
    public int IdFuncionario { get; set; }

    [Column("percentual_comissao")]
    [Precision(18, 4)]
    public decimal PercentualComissao { get; set; }

    [Column("valor_produtos")]
    [Precision(18, 4)]
    public decimal ValorProdutos { get; set; }

    [Column("valor_acrescimo")]
    [Precision(18, 4)]
    public decimal ValorAcrescimo { get; set; }

    [Column("valor_desconto")]
    [Precision(18, 4)]
    public decimal ValorDesconto { get; set; }

    [Column("valor_comissao")]
    [Precision(18, 4)]
    public decimal ValorComissao { get; set; }

    [Column("valor_servicos")]
    [Precision(18, 4)]
    public decimal ValorServicos { get; set; }

    [Column("valor_total")]
    [Precision(18, 4)]
    public decimal ValorTotal { get; set; }

    [Column("cd_plano")]
    public int CdPlano { get; set; }

    [JsonIgnore]
    [ForeignKey("CdPlano")]
    [InverseProperty("OrcamentoCabs")]
    public virtual PlanoEstoque CdPlanoNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("Empresa")]
    [InverseProperty("OrcamentoCabs")]
    public virtual Empresa EmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdFuncionario, Empresa")]
    [InverseProperty("OrcamentoCabs")]
    public virtual Funcionario Funcionario { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdCliente")]
    [InverseProperty("OrcamentoCabs")]
    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    [InverseProperty("OrcamentoCab")]
    public virtual ICollection<OrcamentoIten> OrcamentoItens { get; set; } = new List<OrcamentoIten>();

    [InverseProperty("OrcamentoCab")]
    public virtual ICollection<OrcamentoServico> OrcamentoServicos { get; set; } = new List<OrcamentoServico>();

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("OrcamentoCabs")]
    public virtual Unity UnityNavigation { get; set; } = null!;

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
