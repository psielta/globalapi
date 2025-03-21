using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("historico_caixa")]
[Index("Unity", "CdSubPlano", "CdPlano", Name = "historico_caixa_idx1", IsUnique = true)]
public partial class HistoricoCaixa : IIdentifiable<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

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

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("HistoricoCaixas")]
    public virtual Unity UnityNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("CdPlano, Unity")]
    [InverseProperty("HistoricoCaixas")]
    public virtual PlanoDeCaixa PlanoDeCaixa { get; set; } = null!;

    [JsonPropertyName("nmPlanoCaixa")]
    [NotMapped]
    public string NmPlanoCaixa => PlanoDeCaixa?.Descricao ?? string.Empty;

    [JsonIgnore]
    [InverseProperty("HistoricoCaixa")]
    public virtual ICollection<ContasAPagar> ContasAPagars { get; set; } = new List<ContasAPagar>();

    [JsonIgnore]
    [InverseProperty("HistoricoCaixa")]
    public virtual ICollection<LivroCaixa> LivroCaixas { get; set; } = new List<LivroCaixa>();

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
