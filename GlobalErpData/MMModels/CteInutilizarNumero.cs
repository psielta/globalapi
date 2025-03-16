using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("cte_inutilizar_numero", Schema = "cte")]
public partial class CteInutilizarNumero
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("ano")]
    [StringLength(4)]
    public string Ano { get; set; } = null!;

    [Column("modelo")]
    [StringLength(2)]
    public string Modelo { get; set; } = null!;

    [Column("serie")]
    [StringLength(3)]
    public string Serie { get; set; } = null!;

    [Column("nr_inicial")]
    [StringLength(10)]
    public string NrInicial { get; set; } = null!;

    [Column("nr_final")]
    [StringLength(10)]
    public string NrFinal { get; set; } = null!;

    [Column("txt_justificativa")]
    [StringLength(16384)]
    public string? TxtJustificativa { get; set; }

    [Column("status")]
    [StringLength(2)]
    public string? Status { get; set; }

    [Column("txt_retorno")]
    [StringLength(16384)]
    public string? TxtRetorno { get; set; }

    [Column("xml_retorno")]
    [StringLength(16384)]
    public string? XmlRetorno { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("CteInutilizarNumeros")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}
