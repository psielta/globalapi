using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("cte_doc_anterior_nfe", Schema = "cte")]
public partial class CteDocAnteriorNfe
{
    [Key]
    [Column("id_cte_doc_anterior_nfe")]
    public int IdCteDocAnteriorNfe { get; set; }

    [Column("id_doc_anterior")]
    public int IdDocAnterior { get; set; }

    [Column("id_chave")]
    [StringLength(162)]
    public string IdChave { get; set; } = null!;

    [ForeignKey("IdDocAnterior")]
    [InverseProperty("CteDocAnteriorNves")]
    public virtual CteDocAnterior IdDocAnteriorNavigation { get; set; } = null!;
}
