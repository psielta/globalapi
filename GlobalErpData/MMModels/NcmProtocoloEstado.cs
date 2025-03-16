using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("ncm_protocolo_estado")]
public partial class NcmProtocoloEstado
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("id_cab_protocolo")]
    public int IdCabProtocolo { get; set; }

    [Column("id_ncm")]
    [StringLength(8)]
    public string IdNcm { get; set; } = null!;

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [ForeignKey("IdCabProtocolo")]
    [InverseProperty("NcmProtocoloEstados")]
    public virtual ProtocoloEstadoNcm IdCabProtocoloNavigation { get; set; } = null!;

    [ForeignKey("IdEmpresa")]
    [InverseProperty("NcmProtocoloEstados")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}
