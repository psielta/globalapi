using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[PrimaryKey("ChNfe", "NrItem")]
[Table("impitensnfe")]
public partial class Impitensnfe
{
    [Key]
    [Column("ch_nfe")]
    [StringLength(150)]
    public string ChNfe { get; set; } = null!;

    [Key]
    [Column("nr_item")]
    [StringLength(10)]
    public string NrItem { get; set; } = null!;

    [Column("cean")]
    [StringLength(15)]
    public string? Cean { get; set; }

    [Column("nome")]
    [StringLength(150)]
    public string? Nome { get; set; }

    [Column("ncm")]
    [StringLength(62)]
    public string? Ncm { get; set; }

    [Column("extipi")]
    [StringLength(62)]
    public string? Extipi { get; set; }

    [Column("cfop")]
    [StringLength(6)]
    public string? Cfop { get; set; }

    [Column("un")]
    [StringLength(6)]
    public string? Un { get; set; }

    [Column("quant")]
    [StringLength(50)]
    public string? Quant { get; set; }

    [Column("vl_unit")]
    [StringLength(50)]
    public string? VlUnit { get; set; }

    [Column("vl_total")]
    [StringLength(50)]
    public string? VlTotal { get; set; }

    [Column("pis")]
    [StringLength(4)]
    public string? Pis { get; set; }

    [Column("confins")]
    [StringLength(4)]
    public string? Confins { get; set; }

    [Column("cst")]
    [StringLength(6)]
    public string? Cst { get; set; }

    [Column("imp_origem")]
    [StringLength(50)]
    public string? ImpOrigem { get; set; }

    [Column("mod_bc")]
    [StringLength(50)]
    public string? ModBc { get; set; }

    [Column("predbc")]
    [StringLength(50)]
    public string? Predbc { get; set; }

    [Column("vbc")]
    [StringLength(50)]
    public string? Vbc { get; set; }

    [Column("picms")]
    [StringLength(50)]
    public string? Picms { get; set; }

    [Column("vicms")]
    [StringLength(50)]
    public string? Vicms { get; set; }

    [Column("modbcst")]
    [StringLength(50)]
    public string? Modbcst { get; set; }

    [Column("pmvast")]
    [StringLength(50)]
    public string? Pmvast { get; set; }

    [Column("predbcst")]
    [StringLength(50)]
    public string? Predbcst { get; set; }

    [Column("vbcst")]
    [StringLength(50)]
    public string? Vbcst { get; set; }

    [Column("picmsst")]
    [StringLength(50)]
    public string? Picmsst { get; set; }

    [Column("vicmsst")]
    [StringLength(50)]
    public string? Vicmsst { get; set; }

    [Column("vbcstret")]
    [StringLength(50)]
    public string? Vbcstret { get; set; }

    [Column("vicmsstret")]
    [StringLength(50)]
    public string? Vicmsstret { get; set; }

    [Column("pcred")]
    [StringLength(50)]
    public string? Pcred { get; set; }

    [Column("vl_cred_icms")]
    [StringLength(50)]
    public string? VlCredIcms { get; set; }

    [Column("dt_valid", TypeName = "timestamp without time zone")]
    public DateTime? DtValid { get; set; }

    [Column("lote")]
    [StringLength(60)]
    public string? Lote { get; set; }

    [Column("iss")]
    [StringLength(1)]
    public string? Iss { get; set; }

    [Column("cd_produto_global")]
    public int? CdProdutoGlobal { get; set; }

    [Column("bciss")]
    [StringLength(50)]
    public string? Bciss { get; set; }

    [Column("valiq")]
    [StringLength(50)]
    public string? Valiq { get; set; }

    [Column("vissqn")]
    [StringLength(50)]
    public string? Vissqn { get; set; }

    [Column("cmunfg")]
    [StringLength(50)]
    public string? Cmunfg { get; set; }

    [Column("clistserv")]
    [StringLength(50)]
    public string? Clistserv { get; set; }

    [Column("csosn")]
    [StringLength(50)]
    public string? Csosn { get; set; }

    [Column("ipclenq")]
    [StringLength(50)]
    public string? Ipclenq { get; set; }

    [Column("ipicnpjprod")]
    [StringLength(18)]
    public string? Ipicnpjprod { get; set; }

    [Column("ipicselo")]
    [StringLength(50)]
    public string? Ipicselo { get; set; }

    [Column("ipiqselo")]
    public int? Ipiqselo { get; set; }

    [Column("ipicenq")]
    [StringLength(50)]
    public string? Ipicenq { get; set; }

    [Column("ipivbc")]
    [StringLength(50)]
    public string? Ipivbc { get; set; }

    [Column("ipi")]
    [StringLength(1)]
    public string? Ipi { get; set; }

    [Column("ipiqunid")]
    [StringLength(50)]
    public string? Ipiqunid { get; set; }

    [Column("ipivunid")]
    [StringLength(50)]
    public string? Ipivunid { get; set; }

    [Column("ipipipi")]
    [StringLength(50)]
    public string? Ipipipi { get; set; }

    [Column("ipivipi")]
    [StringLength(50)]
    public string? Ipivipi { get; set; }

    [Column("ii")]
    [StringLength(1)]
    public string? Ii { get; set; }

    [Column("iivbc")]
    [StringLength(50)]
    public string? Iivbc { get; set; }

    [Column("iivdespadu")]
    [StringLength(50)]
    public string? Iivdespadu { get; set; }

    [Column("iivii")]
    [StringLength(50)]
    public string? Iivii { get; set; }

    [Column("iiviof")]
    [StringLength(50)]
    public string? Iiviof { get; set; }

    [Column("ipicst")]
    [StringLength(6)]
    public string? Ipicst { get; set; }

    [Column("ipiclenq")]
    [StringLength(50)]
    public string? Ipiclenq { get; set; }

    [Column("pis_cst")]
    [StringLength(6)]
    public string? PisCst { get; set; }

    [Column("pisvbc")]
    [StringLength(50)]
    public string? Pisvbc { get; set; }

    [Column("pisppis")]
    [StringLength(50)]
    public string? Pisppis { get; set; }

    [Column("pisvpis")]
    [StringLength(50)]
    public string? Pisvpis { get; set; }

    [Column("pisqbcprod")]
    [StringLength(50)]
    public string? Pisqbcprod { get; set; }

    [Column("pisvaliqprod")]
    [StringLength(50)]
    public string? Pisvaliqprod { get; set; }

    [Column("pisst")]
    [StringLength(1)]
    public string? Pisst { get; set; }

    [Column("pisstvbc")]
    [StringLength(50)]
    public string? Pisstvbc { get; set; }

    [Column("pisstppis")]
    [StringLength(50)]
    public string? Pisstppis { get; set; }

    [Column("pisstqbcprod")]
    [StringLength(50)]
    public string? Pisstqbcprod { get; set; }

    [Column("pisstvaliqprod")]
    [StringLength(50)]
    public string? Pisstvaliqprod { get; set; }

    [Column("pisstvpis")]
    [StringLength(50)]
    public string? Pisstvpis { get; set; }

    [Column("cof_cst")]
    [StringLength(50)]
    public string? CofCst { get; set; }

    [Column("cof_vbc")]
    [StringLength(50)]
    public string? CofVbc { get; set; }

    [Column("cof_pcofins")]
    [StringLength(50)]
    public string? CofPcofins { get; set; }

    [Column("cof_vcofins")]
    [StringLength(50)]
    public string? CofVcofins { get; set; }

    [Column("cof_qbcprod")]
    [StringLength(50)]
    public string? CofQbcprod { get; set; }

    [Column("cof_valiqprod")]
    [StringLength(50)]
    public string? CofValiqprod { get; set; }

    [Column("cofinsst")]
    [StringLength(1)]
    public string? Cofinsst { get; set; }

    [Column("cofst_vbc")]
    [StringLength(50)]
    public string? CofstVbc { get; set; }

    [Column("cofst_pcofins")]
    [StringLength(50)]
    public string? CofstPcofins { get; set; }

    [Column("cofst_qbcprod")]
    [StringLength(50)]
    public string? CofstQbcprod { get; set; }

    [Column("cofst_valiqprod")]
    [StringLength(50)]
    public string? CofstValiqprod { get; set; }

    [Column("cofst_vcofins")]
    [StringLength(50)]
    public string? CofstVcofins { get; set; }

    [Column("vldesc")]
    [StringLength(50)]
    public string? Vldesc { get; set; }

    [Column("frete_produto")]
    [StringLength(255)]
    public string? FreteProduto { get; set; }

    [Column("vl_outros")]
    public decimal? VlOutros { get; set; }

    [Column("c_prod")]
    [StringLength(255)]
    public string? CProd { get; set; }

    [Column("fcp_base")]
    [StringLength(255)]
    public string? FcpBase { get; set; }

    [Column("fcp_porc")]
    [StringLength(255)]
    public string? FcpPorc { get; set; }

    [Column("fcp_valor")]
    [StringLength(255)]
    public string? FcpValor { get; set; }

    [Column("pst")]
    [StringLength(255)]
    public string? Pst { get; set; }

    [Column("veic_tpop")]
    [StringLength(255)]
    public string? VeicTpop { get; set; }

    [Column("veic_chassi")]
    [StringLength(255)]
    public string? VeicChassi { get; set; }

    [Column("veci_ccor")]
    [StringLength(255)]
    public string? VeciCcor { get; set; }

    [Column("veic_xcor")]
    [StringLength(255)]
    public string? VeicXcor { get; set; }

    [Column("veic_pot")]
    [StringLength(255)]
    public string? VeicPot { get; set; }

    [Column("veic_cilin")]
    [StringLength(255)]
    public string? VeicCilin { get; set; }

    [Column("veic_pesol")]
    [StringLength(255)]
    public string? VeicPesol { get; set; }

    [Column("veic_pesob")]
    [StringLength(255)]
    public string? VeicPesob { get; set; }

    [Column("veic_nserie")]
    [StringLength(255)]
    public string? VeicNserie { get; set; }

    [Column("veic_tpcomb")]
    [StringLength(255)]
    public string? VeicTpcomb { get; set; }

    [Column("veic_nmotor")]
    [StringLength(255)]
    public string? VeicNmotor { get; set; }

    [Column("veic_cmt")]
    [StringLength(255)]
    public string? VeicCmt { get; set; }

    [Column("veic_dist")]
    [StringLength(255)]
    public string? VeicDist { get; set; }

    [Column("veic_anomod")]
    [StringLength(255)]
    public string? VeicAnomod { get; set; }

    [Column("veic_anofab")]
    [StringLength(255)]
    public string? VeicAnofab { get; set; }

    [Column("veic_tppint")]
    [StringLength(255)]
    public string? VeicTppint { get; set; }

    [Column("veic_tpveic")]
    [StringLength(255)]
    public string? VeicTpveic { get; set; }

    [Column("veic_espveic")]
    [StringLength(255)]
    public string? VeicEspveic { get; set; }

    [Column("veic_vin")]
    [StringLength(255)]
    public string? VeicVin { get; set; }

    [Column("veic_condveic")]
    [StringLength(255)]
    public string? VeicCondveic { get; set; }

    [Column("veic_cmod")]
    [StringLength(255)]
    public string? VeicCmod { get; set; }

    [Column("veic_ccordenatran")]
    [StringLength(255)]
    public string? VeicCcordenatran { get; set; }

    [Column("veic_lota")]
    [StringLength(255)]
    public string? VeicLota { get; set; }

    [Column("veic_tprest")]
    [StringLength(255)]
    public string? VeicTprest { get; set; }

    [Column("cest")]
    [StringLength(255)]
    public string? Cest { get; set; }

    [Column("qtrib")]
    [StringLength(255)]
    public string? Qtrib { get; set; }

    [Column("vuntrib")]
    [StringLength(255)]
    public string? Vuntrib { get; set; }

    [Column("utrib")]
    [StringLength(255)]
    public string? Utrib { get; set; }

    [Column("vicmsdeson")]
    [StringLength(255)]
    public string? Vicmsdeson { get; set; }
}
