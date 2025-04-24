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

    [Column("unity")]
    public int Unity { get; set; }

    [ConcurrencyCheck]
    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [JsonIgnore]
    [InverseProperty("CdGrupoNavigation")]
    public virtual ICollection<ProdutoEstoque> ProdutoEstoques { get; set; } = new List<ProdutoEstoque>();

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("GrupoEstoques")]
    public virtual Unity UnityNavigation { get; set; } = null!;

    [GraphQLIgnore]
    public int GetId()
    {
        return this.CdGrupo;
    }

    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "CdGrupo";
    }

    [Column("uairango_id_categoria")]
    public int? UairangoIdCategoria { get; set; }

    [Column("uairango_order")]
    [StringLength(256)]
    public string? UairangoOrder { get; set; }

    [Column("uairango_codigo")]
    [StringLength(256)]
    public string? UairangoCodigo { get; set; }

    [Column("uairango_descricao")]
    [StringLength(512)]
    public string? UairangoDescricao { get; set; }

    [Column("uairango_inicio")]
    public TimeOnly? UairangoInicio { get; set; }

    [Column("uairango_fim")]
    public TimeOnly? UairangoFim { get; set; }

    [Column("uairango_ativo")]
    public int? UairangoAtivo { get; set; }

    [Column("uairango_opcao_meia")]
    [StringLength(20)]
    public string? UairangoOpcaoMeia { get; set; }

    [Column("uairango_disponivel_domingo")]
    public int? UairangoDisponivelDomingo { get; set; }

    [Column("uairango_disponivel_segunda")]
    public int? UairangoDisponivelSegunda { get; set; }

    [Column("uairango_disponivel_terca")]
    public int? UairangoDisponivelTerca { get; set; }

    [Column("uairango_disponivel_quarta")]
    public int? UairangoDisponivelQuarta { get; set; }

    [Column("uairango_disponivel_quinta")]
    public int? UairangoDisponivelQuinta { get; set; }

    [Column("uairango_disponivel_sexta")]
    public int? UairangoDisponivelSexta { get; set; }

    [Column("uairango_disponivel_sabado")]
    public int? UairangoDisponivelSabado { get; set; }

    [Column("uairango_id_culinaria")]
    public int? UairangoIdCulinaria { get; set; }

    [JsonIgnore]
    [InverseProperty("CdGrupoNavigation")]
    public virtual ICollection<UairangoEmpresaCategorium> UairangoEmpresaCategoria { get; set; } = new List<UairangoEmpresaCategorium>();

    [JsonIgnore]
    [InverseProperty("CdGrupoNavigation")]
    public virtual ICollection<UairangoOpcoesCategorium> UairangoOpcoesCategoria { get; set; } = new List<UairangoOpcoesCategorium>();

    [JsonIgnore]
    [InverseProperty("CdGrupoNavigation")]
    public virtual ICollection<UairangoAdicionalCab> UairangoAdicionalCabs { get; set; } = new List<UairangoAdicionalCab>();

}
