using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("plano_de_caixa")]
[Index("CdClassificacao", "CdEmpresa", Name = "plano_de_caixa_idx", IsUnique = true)]
public partial class PlanoDeCaixa : IIdentifiable<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("cd_classificacao")]
    [StringLength(25)]
    public string CdClassificacao { get; set; } = null!;

    [Column("descricao")]
    [StringLength(62)]
    public string Descricao { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("PlanoDeCaixas")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [InverseProperty("PlanoDeCaixa")]
    public virtual ICollection<HistoricoCaixa> HistoricoCaixas { get; set; } = new List<HistoricoCaixa>();

    
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
}
