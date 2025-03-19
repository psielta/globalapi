using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[PrimaryKey("CdFuncionario", "CdEmpresa")]
[Table("vendedor")]
[Index("Id", Name = "vendedor_id_key", IsUnique = true)]
public partial class Vendedor
{
    [Column("id")]
    public int Id { get; set; }

    [Key]
    [Column("cd_funcionario")]
    public int CdFuncionario { get; set; }

    [Key]
    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("comissao_a_prazo")]
    [Precision(18, 2)]
    public decimal? ComissaoAPrazo { get; set; }

    [Column("comissao_a_vista")]
    [Precision(18, 2)]
    public decimal? ComissaoAVista { get; set; }

    [Column("tipo_pagamento")]
    [StringLength(1)]
    public string? TipoPagamento { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [ForeignKey("CdEmpresa")]
    [InverseProperty("Vendedors")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [ForeignKey("CdFuncionario, CdEmpresa")]
    [InverseProperty("VendedorNavigation")]
    public virtual Funcionario Funcionario { get; set; } = null!;
}
