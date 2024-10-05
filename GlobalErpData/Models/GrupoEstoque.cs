using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("grupo_estoque")]
public partial class GrupoEstoque : IIdentifiable<int>
{
    [Key]
    [Column("cd_grupo")]
    public int CdGrupo { get; set; }

    [Column("nm_grupo")]
    [StringLength(62)]
    public string NmGrupo { get; set; } = null!;

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("GrupoEstoques")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [InverseProperty("CdGrupoNavigation")]
    public virtual ICollection<ProdutoEstoque> ProdutoEstoques { get; set; } = new List<ProdutoEstoque>();

    [JsonIgnore]
    [InverseProperty("Category")]
    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();

    [JsonIgnore]
    [InverseProperty("Category")]
    public virtual ICollection<Featured> Featureds { get; set; } = new List<Featured>();

    public int GetId()
    {
        return this.CdGrupo;
    }

    public string GetKeyName()
    {
        return "CdGrupo";
    }
}
