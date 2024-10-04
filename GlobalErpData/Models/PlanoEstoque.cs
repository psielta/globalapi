using System;
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

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("PlanoEstoques")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;
    
    [JsonIgnore]
    [InverseProperty("CdPlanoNavigation")]
    public virtual ICollection<SaldoEstoque> SaldoEstoques { get; set; } = new List<SaldoEstoque>();

    [JsonIgnore]
    [InverseProperty("CdGrupoEstoqueNavigation")]
    public virtual ICollection<Entrada> Entrada { get; set; } = new List<Entrada>();

    public int GetId()
    {
        return CdPlano;
    }

    public string GetKeyName()
    {
        return "CdPlano";
    }
}
