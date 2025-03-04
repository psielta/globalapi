using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("imptotalnfe")]
public partial class Imptotalnfe : IIdentifiable<string>
{
    [Key]
    [Column("ch_nfe")]
    [StringLength(150)]
    public string ChNfe { get; set; } = null!;

    [Column("icms_vbc")]
    [StringLength(50)]
    public string? IcmsVbc { get; set; }

    [Column("icms_valor")]
    [StringLength(50)]
    public string? IcmsValor { get; set; }

    [Column("icms_vbcst")]
    [StringLength(50)]
    public string? IcmsVbcst { get; set; }

    [Column("icms_st")]
    [StringLength(50)]
    public string? IcmsSt { get; set; }

    [Column("icms_vprod")]
    [StringLength(50)]
    public string? IcmsVprod { get; set; }

    [Column("icms_frete")]
    [StringLength(50)]
    public string? IcmsFrete { get; set; }

    [Column("icms_seg")]
    [StringLength(50)]
    public string? IcmsSeg { get; set; }

    [Column("icms_desc")]
    [StringLength(50)]
    public string? IcmsDesc { get; set; }

    [Column("icms_vii")]
    [StringLength(50)]
    public string? IcmsVii { get; set; }

    [Column("icms_vipi")]
    [StringLength(50)]
    public string? IcmsVipi { get; set; }

    [Column("icms_vpis")]
    [StringLength(50)]
    public string? IcmsVpis { get; set; }

    [Column("icms_vconfins")]
    [StringLength(50)]
    public string? IcmsVconfins { get; set; }

    [Column("icms_outros")]
    [StringLength(50)]
    public string? IcmsOutros { get; set; }

    [Column("icms_vnf")]
    [StringLength(50)]
    public string? IcmsVnf { get; set; }

    [Column("ret_pis")]
    [StringLength(50)]
    public string? RetPis { get; set; }

    [Column("ret_confins")]
    [StringLength(50)]
    public string? RetConfins { get; set; }

    [Column("ret_csll")]
    [StringLength(50)]
    public string? RetCsll { get; set; }

    [Column("ret_irrf")]
    [StringLength(50)]
    public string? RetIrrf { get; set; }

    [Column("ret_birrf")]
    [StringLength(50)]
    public string? RetBirrf { get; set; }

    [Column("ret_bcvprev")]
    [StringLength(50)]
    public string? RetBcvprev { get; set; }

    [Column("ret_vprev")]
    [StringLength(50)]
    public string? RetVprev { get; set; }

    [Column("vicmsdeson")]
    [StringLength(50)]
    public string? Vicmsdeson { get; set; }

    [Column("icms_vfcpst")]
    [StringLength(50)]
    public string? IcmsVfcpst { get; set; }

    [Column("icms_vipidevol")]
    [StringLength(50)]
    public string? IcmsVipidevol { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [GraphQLIgnore]
    public string GetId()
    {
        return ChNfe;
    }

    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "ChNfe";
    }
}
