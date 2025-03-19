using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("cte_doc_anterior", Schema = "cte")]
public partial class CteDocAnterior
{
    [Key]
    [Column("codigo")]
    public int Codigo { get; set; }

    [Column("tp_documento")]
    [StringLength(2)]
    public string TpDocumento { get; set; } = null!;

    [Column("cpf")]
    [StringLength(11)]
    public string? Cpf { get; set; }

    [Column("cnpj")]
    [StringLength(14)]
    public string? Cnpj { get; set; }

    [Column("insc_estadual")]
    [StringLength(64)]
    public string? InscEstadual { get; set; }

    [Column("uf")]
    [StringLength(2)]
    public string? Uf { get; set; }

    [Column("nome")]
    [StringLength(60)]
    public string Nome { get; set; } = null!;

    [Column("nr_cte")]
    public int NrCte { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [InverseProperty("IdDocAnteriorNavigation")]
    public virtual ICollection<CteDocAnteriorNfe> CteDocAnteriorNves { get; set; } = new List<CteDocAnteriorNfe>();

    [ForeignKey("IdEmpresa")]
    [InverseProperty("CteDocAnteriors")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}
