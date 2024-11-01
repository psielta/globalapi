using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("historico_caixa")]
[Index("CdEmpresa", "CdSubPlano", "CdPlano", Name = "historico_caixa_idx1", IsUnique = true)]
public partial class HistoricoCaixa : IIdentifiable<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("cd_sub_plano")]
    [StringLength(25)]
    public string CdSubPlano { get; set; } = null!;

    [Column("cd_plano")]
    [StringLength(25)]
    public string CdPlano { get; set; } = null!;

    [Column("tipo")]
    [StringLength(1)]
    public string Tipo { get; set; } = null!;

    [Column("descricao")]
    [StringLength(62)]
    public string Descricao { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("HistoricoCaixas")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("CdPlano, CdEmpresa")]
    [InverseProperty("HistoricoCaixas")]
    public virtual PlanoDeCaixa PlanoDeCaixa { get; set; } = null!;

    [JsonPropertyName("nmPlanoCaixa")]
    [NotMapped]
    public string NmPlanoCaixa => PlanoDeCaixa?.Descricao ?? string.Empty;

    [JsonIgnore]
    [InverseProperty("HistoricoCaixa")]
    public virtual ICollection<ContasAPagar> ContasAPagars { get; set; } = new List<ContasAPagar>();

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

    [JsonIgnore]
    [InverseProperty("HistoricoCaixa")]
    public virtual ICollection<ContasAReceber> ContasARecebers { get; set; } = new List<ContasAReceber>();
}
