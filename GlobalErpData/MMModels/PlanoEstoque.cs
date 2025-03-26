using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("plano_estoque")]
public partial class PlanoEstoque
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

    [Column("unity")]
    public int Unity { get; set; }

    [ForeignKey("CdEmpresa")]
    [InverseProperty("PlanoEstoques")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [InverseProperty("CdGrupoEstoqueNavigation")]
    public virtual ICollection<Entrada> Entrada { get; set; } = new List<Entrada>();

    [InverseProperty("CdPlanoNavigation")]
    public virtual ICollection<OrcamentoCab> OrcamentoCabs { get; set; } = new List<OrcamentoCab>();

    [InverseProperty("CdPlanoNavigation")]
    public virtual ICollection<OrcamentoIten> OrcamentoItens { get; set; } = new List<OrcamentoIten>();

    [InverseProperty("CdPlanoNavigation")]
    public virtual ICollection<ProdutoSaidum> ProdutoSaida { get; set; } = new List<ProdutoSaidum>();

    [InverseProperty("CdGrupoEstoqueNavigation")]
    public virtual ICollection<Saida> Saida { get; set; } = new List<Saida>();

    [InverseProperty("CdPlanoNavigation")]
    public virtual ICollection<SaldoEstoque> SaldoEstoques { get; set; } = new List<SaldoEstoque>();

    [ForeignKey("Unity")]
    [InverseProperty("PlanoEstoques")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}
