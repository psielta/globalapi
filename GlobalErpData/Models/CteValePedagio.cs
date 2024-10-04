using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("cte_vale_pedagio", Schema = "cte")]
public partial class CteValePedagio
{
    [Key]
    [Column("codigo")]
    public int Codigo { get; set; }

    [Column("cnpj_fornecedora")]
    [StringLength(14)]
    public string CnpjFornecedora { get; set; } = null!;

    [Column("cnpj_responsavel")]
    [StringLength(14)]
    public string? CnpjResponsavel { get; set; }

    [Column("nr_comprovante")]
    [StringLength(20)]
    public string NrComprovante { get; set; } = null!;

    [Column("valor")]
    [Precision(18, 2)]
    public decimal Valor { get; set; }

    [Column("nr_cte")]
    public int NrCte { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("CteValePedagios")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}
