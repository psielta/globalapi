using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("plano_simultaneos")]
public partial class PlanoSimultaneo : IIdentifiable<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [Column("cd_plano_princ")]
    public int CdPlanoPrinc { get; set; }

    [Column("cd_plano_replica")]
    public int CdPlanoReplica { get; set; }

    [JsonIgnore]
    [ForeignKey("CdPlanoPrinc")]
    [InverseProperty("PlanoSimultaneoCdPlanoPrincNavigations")]
    public virtual PlanoEstoque CdPlanoPrincNavigation { get; set; } = null!;

    [JsonPropertyName("nmPlanoPrinc")]
    [NotMapped]
    public string? NmPlanoPrinc => (CdPlanoPrincNavigation == null) ? "" : CdPlanoPrincNavigation.NmPlano;

    [JsonPropertyName("cdEmpresaPlanoPrinc")]
    [NotMapped]
    public int? CdEmpresaPlanoPrinc => (CdPlanoPrincNavigation == null) ? null : CdPlanoPrincNavigation.CdEmpresa;

    [JsonPropertyName("nmEmpresaPlanoPrinc")]
    [NotMapped]
    public string? NmEmpresaPlanoPrinc => (CdPlanoPrincNavigation == null || CdPlanoPrincNavigation.CdEmpresaNavigation == null) ? "" : CdPlanoPrincNavigation.CdEmpresaNavigation.NmEmpresa;

    [JsonIgnore]
    [ForeignKey("CdPlanoReplica")]
    [InverseProperty("PlanoSimultaneoCdPlanoReplicaNavigations")]
    public virtual PlanoEstoque CdPlanoReplicaNavigation { get; set; } = null!;

    [JsonPropertyName("nmPlanoReplica")]
    [NotMapped]
    public string? NmPlanoReplica => (CdPlanoReplicaNavigation == null) ? "" : CdPlanoReplicaNavigation.NmPlano;
    
    [JsonPropertyName("cdEmpresaPlanoReplica")]
    [NotMapped]
    public int? CdEmpresaPlanoReplica => (CdPlanoReplicaNavigation == null) ? null : CdPlanoReplicaNavigation.CdEmpresa;

    [JsonPropertyName("nmEmpresaPlanoReplica")]
    [NotMapped]
    public string? NmEmpresaPlanoReplica => (CdPlanoReplicaNavigation == null || CdPlanoReplicaNavigation.CdEmpresaNavigation == null) ? "" : CdPlanoReplicaNavigation.CdEmpresaNavigation.NmEmpresa;

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("PlanoSimultaneos")]
    public virtual Unity UnityNavigation { get; set; } = null!;

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
