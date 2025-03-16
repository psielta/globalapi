using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("distribuicao_dfe")]
public partial class DistribuicaoDfe
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("serie")]
    [StringLength(3)]
    public string Serie { get; set; } = null!;

    [Column("nr_nota_fiscal")]
    [StringLength(20)]
    public string NrNotaFiscal { get; set; } = null!;

    [Column("chave_acesso_nfe")]
    [StringLength(50)]
    public string ChaveAcessoNfe { get; set; } = null!;

    [Column("cnpj")]
    [StringLength(20)]
    public string Cnpj { get; set; } = null!;

    [Column("nome")]
    [StringLength(255)]
    public string Nome { get; set; } = null!;

    [Column("ie")]
    [StringLength(30)]
    public string? Ie { get; set; }

    [Column("tp_nfe")]
    [StringLength(20)]
    public string? TpNfe { get; set; }

    [Column("nsu")]
    [StringLength(20)]
    public string? Nsu { get; set; }

    [Column("emissao")]
    [StringLength(20)]
    public string? Emissao { get; set; }

    [Column("valor")]
    [Precision(18, 2)]
    public decimal? Valor { get; set; }

    [Column("impresso")]
    [StringLength(20)]
    public string? Impresso { get; set; }

    [Column("tp_resposta")]
    [MaxLength(1)]
    public char? TpResposta { get; set; }

    [Column("manifesto")]
    [MaxLength(1)]
    public char? Manifesto { get; set; }

    [Column("transferiu")]
    [MaxLength(1)]
    public char? Transferiu { get; set; }

    [Column("dt_recebimento")]
    public DateOnly? DtRecebimento { get; set; }

    [Column("xml")]
    public string? Xml { get; set; }

    [Column("dt_inclusao")]
    public DateOnly DtInclusao { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("cstat_ciencia")]
    public int? CstatCiencia { get; set; }

    [Column("xmotivo_ciencia")]
    [StringLength(512)]
    public string? XmotivoCiencia { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("DistribuicaoDves")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}
