using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;


namespace GlobalErpData.Models;

[Table("servicos")]
public partial class Servico : IIdentifiable<long>
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [Column("id_departamento")]
    public long IdDepartamento { get; set; }

    [Column("paga_comissao")]
    public bool PagaComissao { get; set; }

    [Column("valor_unitario")]
    [Precision(18, 4)]
    public decimal ValorUnitario { get; set; }

    [Column("nm_servico")]
    [StringLength(255)]
    public string NmServico { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdDepartamento")]
    [InverseProperty("Servicos")]
    public virtual Departamento IdDepartamentoNavigation { get; set; } = null!;

    [JsonPropertyName("nmDepartamento")]
    [NotMapped]
    public string NmDepartamento => IdDepartamentoNavigation?.NmDepartamento ?? "";

    [JsonIgnore]
    [InverseProperty("IdServicoNavigation")]
    public virtual ICollection<OrcamentoServico> OrcamentoServicos { get; set; } = new List<OrcamentoServico>();

    [JsonIgnore]
    [InverseProperty("CdServicoNavigation")]
    public virtual ICollection<OsTabelaPreco> OsTabelaPrecos { get; set; } = new List<OsTabelaPreco>();

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("Servicos")]
    public virtual Unity UnityNavigation { get; set; } = null!;

    [GraphQLIgnore]
    public long GetId()
    {
        return Id;
    }

    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "Id";
    }
}
