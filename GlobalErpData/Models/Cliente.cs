using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalErpData.MMModels;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("cliente")]
public partial class Cliente : IIdentifiable<int>
{ 
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nm_cliente")]
    [StringLength(256)]
    public string NmCliente { get; set; } = null!;

    [Column("ativo")]
    [StringLength(1)]
    public string? Ativo { get; set; }

    [Column("nm_endereco")]
    [StringLength(256)]
    public string? NmEndereco { get; set; }

    [Column("numero")]
    [StringLength(6)]
    public string? Numero { get; set; }

    [Column("cd_cidade")]
    [StringLength(10)]
    public string CdCidade { get; set; } = null!;

    [Column("nm_bairro")]
    [StringLength(62)]
    public string? NmBairro { get; set; }

    [Column("cep")]
    [StringLength(9)]
    public string? Cep { get; set; }

    [Column("telefone")]
    [StringLength(18)]
    public string? Telefone { get; set; }

    [Column("e_mail")]
    [StringLength(256)]
    public string? EMail { get; set; }

    [Column("tp_doc")]
    [StringLength(3)]
    public string? TpDoc { get; set; }

    [Column("nr_doc")]
    [StringLength(19)]
    public string? NrDoc { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("txt_obs")]
    [StringLength(16384)]
    public string? TxtObs { get; set; }

    [Column("dt_cadastro")]
    public DateOnly? DtCadastro { get; set; }

    [Column("id_usuario_cad")]
    public int IdUsuarioCad { get; set; }

    [Column("id_cte_antigo")]
    public int? IdCteAntigo { get; set; }

    [Column("inscricao_estadual")]
    [StringLength(20)]
    public string? InscricaoEstadual { get; set; }

    [JsonPropertyName("nmCidade")]
    [NotMapped]
    public string NmCidade => CdCidadeNavigation?.NmCidade ?? string.Empty;

    [JsonPropertyName("uf")]
    [NotMapped]
    public string Uf => CdCidadeNavigation?.Uf ?? string.Empty;

    [JsonIgnore]
    [ForeignKey("CdCidade")]
    [InverseProperty("Clientes")]
    public virtual Cidade CdCidadeNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdEmpresa")]
    [InverseProperty("Clientes")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
    [JsonIgnore]
    [ForeignKey("IdUsuarioCad")]
    [InverseProperty("Clientes")]
    public virtual Usuario IdUsuarioCadNavigation { get; set; } = null!;

    [GraphQLIgnore]
    public int GetId()
    {
        return this.Id;
    }

    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "Id";
    }

    [JsonIgnore]
    [InverseProperty("CdClienteNavigation")]
    public virtual ICollection<ContasAReceber> ContasARecebers { get; set; } = new List<ContasAReceber>();

    [JsonIgnore]
    [InverseProperty("ClienteNavigation")]
    public virtual ICollection<Saida> Saida { get; set; } = new List<Saida>();
}
