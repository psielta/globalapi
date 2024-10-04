using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("cte_rod_veiculo", Schema = "cte")]
public partial class CteRodVeiculo
{
    [Key]
    [Column("id_rod")]
    public int IdRod { get; set; }

    [Column("cd_interno")]
    [StringLength(10)]
    public string? CdInterno { get; set; }

    [Column("renavam")]
    [StringLength(11)]
    public string Renavam { get; set; } = null!;

    [Column("placa")]
    [StringLength(7)]
    public string Placa { get; set; } = null!;

    [Column("tara")]
    public int Tara { get; set; }

    [Column("capacidade_kg")]
    public int CapacidadeKg { get; set; }

    [Column("capacidade_m3")]
    public int CapacidadeM3 { get; set; }

    [Column("tp_rodado")]
    [StringLength(2)]
    public string TpRodado { get; set; } = null!;

    [Column("tp_carroceria")]
    [StringLength(2)]
    public string TpCarroceria { get; set; } = null!;

    [Column("tp_veiculo")]
    [StringLength(2)]
    public string TpVeiculo { get; set; } = null!;

    [Column("tp_proprietario")]
    [StringLength(2)]
    public string TpProprietario { get; set; } = null!;

    [Column("uf")]
    [StringLength(2)]
    public string Uf { get; set; } = null!;

    [Column("pertence_emitente")]
    [MaxLength(1)]
    public string? PertenceEmitente { get; set; }

    [Column("prop_tipo_documento")]
    [StringLength(2)]
    public string? PropTipoDocumento { get; set; }

    [Column("prop_cpf")]
    [StringLength(14)]
    public string? PropCpf { get; set; }

    [Column("prop_cnpj")]
    [StringLength(14)]
    public string? PropCnpj { get; set; }

    [Column("prop_rntrc")]
    [StringLength(8)]
    public string? PropRntrc { get; set; }

    [Column("prop_insc_estadual")]
    [StringLength(14)]
    public string? PropInscEstadual { get; set; }

    [Column("prop_uf")]
    [StringLength(2)]
    public string? PropUf { get; set; }

    [Column("prop_tp_proprietario")]
    [StringLength(2)]
    public string? PropTpProprietario { get; set; }

    [Column("prop_razao_social")]
    [StringLength(60)]
    public string? PropRazaoSocial { get; set; }

    [Column("responsavel")]
    [StringLength(2)]
    public string? Responsavel { get; set; }

    [Column("nm_seguradora")]
    [StringLength(30)]
    public string? NmSeguradora { get; set; }

    [Column("nr_apolice")]
    [StringLength(20)]
    public string? NrApolice { get; set; }

    [Column("nr_averbacao")]
    [StringLength(20)]
    public string? NrAverbacao { get; set; }

    [Column("vl_mercadoria")]
    [Precision(18, 2)]
    public decimal? VlMercadoria { get; set; }

    [Column("ativo")]
    [StringLength(1)]
    public string? Ativo { get; set; }

    [Column("nr_cte")]
    [StringLength(10)]
    public string NrCte { get; set; } = null!;

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("CteRodVeiculos")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}
