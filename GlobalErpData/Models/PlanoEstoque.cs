﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("plano_estoque")]
public partial class PlanoEstoque : IIdentifiable<int>
{
    [Key]
    [Column("cd_plano")]
    public int CdPlano { get; set; }

    [Column("nm_plano")]
    [StringLength(62)]
    public string NmPlano { get; set; } = null!;

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("ativo")]
    public bool Ativo { get; set; }

    [Column("e_fiscal")]
    public bool? EFiscal { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("PlanoEstoques")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [JsonPropertyName("nmEmpresa")]
    [NotMapped]
    public string NmEmpresa => $"{CdEmpresaNavigation?.CdEmpresa} - {CdEmpresaNavigation?.NmEmpresa}" ?? "";



    [JsonIgnore]
    [InverseProperty("CdPlanoNavigation")]
    public virtual ICollection<SaldoEstoque> SaldoEstoques { get; set; } = new List<SaldoEstoque>();

    [JsonIgnore]
    [InverseProperty("CdGrupoEstoqueNavigation")]
    public virtual ICollection<Entrada> Entrada { get; set; } = new List<Entrada>();

    [JsonIgnore]
    [InverseProperty("CdGrupoEstoqueNavigation")]
    public virtual ICollection<Saida> Saida { get; set; } = new List<Saida>();

    [JsonIgnore]
    [InverseProperty("CdPlanoNavigation")]
    public virtual ICollection<ProdutoSaidum> ProdutoSaida { get; set; } = new List<ProdutoSaidum>();

    [GraphQLIgnore]
    public int GetId()
    {
        return CdPlano;
    }

    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "CdPlano";
    }

    [Column("unity")]
    public int Unity { get; set; }

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("PlanoEstoques")]
    public virtual Unity UnityNavigation { get; set; } = null!;

    [JsonIgnore]
    [InverseProperty("CdPlanoNavigation")]
    public virtual ICollection<OrcamentoCab> OrcamentoCabs { get; set; } = new List<OrcamentoCab>();

    [JsonIgnore]
    [InverseProperty("CdPlanoNavigation")]
    public virtual ICollection<OrcamentoIten> OrcamentoItens { get; set; } = new List<OrcamentoIten>();

    [JsonIgnore]
    [InverseProperty("CdPlanoPrincNavigation")]
    public virtual ICollection<PlanoSimultaneo> PlanoSimultaneoCdPlanoPrincNavigations { get; set; } = new List<PlanoSimultaneo>();

    [JsonIgnore]
    [InverseProperty("CdPlanoReplicaNavigation")]
    public virtual ICollection<PlanoSimultaneo> PlanoSimultaneoCdPlanoReplicaNavigations { get; set; } = new List<PlanoSimultaneo>();
}
