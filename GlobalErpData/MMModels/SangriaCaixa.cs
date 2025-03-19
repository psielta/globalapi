using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[PrimaryKey("Id", "CdEmpresa")]
[Table("sangria_caixa")]
public partial class SangriaCaixa
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Key]
    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("data")]
    public DateOnly? Data { get; set; }

    [Column("hora")]
    public TimeOnly? Hora { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("id_abertura_caixa")]
    public int IdAberturaCaixa { get; set; }

    [Column("id_caixa")]
    public int IdCaixa { get; set; }

    [Column("valor")]
    [Precision(15, 2)]
    public decimal Valor { get; set; }

    [Column("tipo")]
    [StringLength(4)]
    public string? Tipo { get; set; }

    [Column("obs")]
    [StringLength(256)]
    public string? Obs { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [ForeignKey("CdEmpresa")]
    [InverseProperty("SangriaCaixas")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;
}
