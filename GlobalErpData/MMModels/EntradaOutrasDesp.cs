using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("entrada_outras_desp")]
public partial class EntradaOutrasDesp
{
    [Key]
    [Column("nr_entrada")]
    public int NrEntrada { get; set; }

    [Column("dt_entrada")]
    public DateOnly DtEntrada { get; set; }

    [Column("cd_fornecedor")]
    public int CdFornecedor { get; set; }

    [Column("nr_nota_fiscal")]
    [StringLength(25)]
    public string NrNotaFiscal { get; set; } = null!;

    [Column("modelo_nf")]
    [StringLength(2)]
    public string? ModeloNf { get; set; }

    [Column("serie")]
    [StringLength(4)]
    public string? Serie { get; set; }

    [Column("subserie")]
    [StringLength(2)]
    public string? Subserie { get; set; }

    [Column("nr_nota_fiscal_entrada")]
    [StringLength(25)]
    public string? NrNotaFiscalEntrada { get; set; }

    [Column("cfop")]
    [StringLength(4)]
    public string Cfop { get; set; } = null!;

    [Column("valor_total")]
    [Precision(18, 4)]
    public decimal ValorTotal { get; set; }

    [Column("base_icms")]
    [Precision(18, 4)]
    public decimal BaseIcms { get; set; }

    [Column("porc_icms")]
    [Precision(18, 2)]
    public decimal PorcIcms { get; set; }

    [Column("valor_icms")]
    [Precision(18, 4)]
    public decimal ValorIcms { get; set; }

    [Column("vl_isenta_n_trib")]
    [Precision(18, 4)]
    public decimal VlIsentaNTrib { get; set; }

    [Column("vl_outras")]
    [Precision(18, 4)]
    public decimal VlOutras { get; set; }

    [Column("tp_frete")]
    public int TpFrete { get; set; }

    [Column("cd_tipo_nf")]
    [StringLength(4)]
    public string? CdTipoNf { get; set; }

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("chave_cte")]
    [StringLength(44)]
    public string? ChaveCte { get; set; }

    [Column("cd_cidade_origem")]
    [StringLength(10)]
    public string? CdCidadeOrigem { get; set; }

    [Column("cd_cidade_destino")]
    [StringLength(10)]
    public string? CdCidadeDestino { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [ForeignKey("CdEmpresa")]
    [InverseProperty("EntradaOutrasDesps")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [ForeignKey("CdFornecedor, Unity")]
    [InverseProperty("EntradaOutrasDesps")]
    public virtual Fornecedor Fornecedor { get; set; } = null!;

    [ForeignKey("Unity")]
    [InverseProperty("EntradaOutrasDesps")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}
