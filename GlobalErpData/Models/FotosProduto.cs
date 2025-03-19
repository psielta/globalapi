using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[PrimaryKey("Id", "Unity")]
[Table("fotos_produto")]
public partial class FotosProduto : IIdentifiableMultiKey<int, int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Key]
    [Column("unity")]
    public int Unity { get; set; }

    [Column("cd_produto")]
    public int CdProduto { get; set; }

    [Column("caminho_foto")]
    public string CaminhoFoto { get; set; } = null!;

    [Column("excluiu")]
    public bool Excluiu { get; set; }

    [Column("descricao_foto")]
    [StringLength(255)]
    public string? DescricaoFoto { get; set; }
    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }
    [JsonIgnore]
    [ForeignKey("CdProduto, Unity")]
    [InverseProperty("FotosProdutos")]
    public virtual ProdutoEstoque ProdutoEstoque { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("FotosProdutos")]
    public virtual Unity UnityNavigation { get; set; } = null!;

    [GraphQLIgnore]
    public (int, int) GetId()
    {
        return (Unity, Id);
    }

    [GraphQLIgnore]
    public string GetKeyName1()
    {
        return "Unity";
    }

    [GraphQLIgnore]
    public string GetKeyName2()
    {
        return "Id";
    }
}
