using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("os_tabela_preco")]
[Index("CdServico", "IdTabelaPreco", Name = "os_tabela_preco_idx", IsUnique = true)]
public partial class OsTabelaPreco : IIdentifiable<long>
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("cd_servico")]
    public long CdServico { get; set; }

    [Column("id_tabela_preco")]
    public int IdTabelaPreco { get; set; }

    [Column("preco_venda")]
    [Precision(20, 4)]
    public decimal PrecoVenda { get; set; }

    [Column("dt_ult_alteracao", TypeName = "timestamp(6) without time zone")]
    public DateTime DtUltAlteracao { get; set; }

    [Column("descricao")]
    [StringLength(255)]
    public string? Descricao { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [JsonIgnore]
    [ForeignKey("CdServico")]
    [InverseProperty("OsTabelaPrecos")]
    public virtual Servico CdServicoNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("OsTabelaPrecos")]
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
