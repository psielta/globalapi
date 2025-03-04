using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("cidade")]
public partial class Cidade : IIdentifiable<string>
{
    [Key]
    [Column("cd_cidade")]
    [StringLength(10)]
    public string CdCidade { get; set; } = null!;

    [Column("nm_cidade")]
    [StringLength(150)]
    public string NmCidade { get; set; } = null!;
    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }
    [Column("uf")]
    [StringLength(2)]
    public string Uf { get; set; } = null!;
    [JsonIgnore]
    [InverseProperty("CdCidadeNavigation")]
    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    [JsonIgnore]
    [InverseProperty("CdCidadeNavigation")]
    public virtual ICollection<Empresa> Empresas { get; set; } = new List<Empresa>();

    [JsonIgnore]
    [InverseProperty("CdCidadeNavigation")]
    public virtual ICollection<Fornecedor> Fornecedors { get; set; } = new List<Fornecedor>();

    [JsonIgnore]
    [InverseProperty("CdCidadeNavigation")]
    public virtual ICollection<Transportadora> Transportadoras { get; set; } = new List<Transportadora>();

    [JsonIgnore]
    [InverseProperty("CidadeNavigation")]
    public virtual ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();

    [GraphQLIgnore]
    public string GetId()
    {
        return this.CdCidade;
    }

    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "CdCidade";
    }
}
