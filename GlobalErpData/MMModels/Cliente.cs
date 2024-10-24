using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("cliente")]
public partial class Cliente
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nm_cliente")]
    [StringLength(256)]
    public string NmCliente { get; set; } = null!;

    [Column("ativo")]
    [StringLength(1)]
    public string? Ativo { get; set; }

    [Column("nm_endereco")]
    [StringLength(256)]
    public string? NmEndereco { get; set; }

    [Column("numero")]
    [StringLength(6)]
    public string? Numero { get; set; }

    [Column("cd_cidade")]
    [StringLength(10)]
    public string CdCidade { get; set; } = null!;

    [Column("nm_bairro")]
    [StringLength(62)]
    public string? NmBairro { get; set; }

    [Column("cep")]
    [StringLength(9)]
    public string? Cep { get; set; }

    [Column("telefone")]
    [StringLength(18)]
    public string? Telefone { get; set; }

    [Column("e_mail")]
    [StringLength(256)]
    public string? EMail { get; set; }

    [Column("tp_doc")]
    [StringLength(3)]
    public string? TpDoc { get; set; }

    [Column("nr_doc")]
    [StringLength(19)]
    public string? NrDoc { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("txt_obs")]
    [StringLength(16384)]
    public string? TxtObs { get; set; }

    [Column("dt_cadastro")]
    public DateOnly? DtCadastro { get; set; }

    [Column("id_usuario_cad")]
    public int IdUsuarioCad { get; set; }

    [Column("id_cte_antigo")]
    public int? IdCteAntigo { get; set; }

    [Column("inscricao_estadual")]
    [StringLength(20)]
    public string? InscricaoEstadual { get; set; }

    [ForeignKey("CdCidade")]
    [InverseProperty("Clientes")]
    public virtual Cidade CdCidadeNavigation { get; set; } = null!;

    [ForeignKey("IdEmpresa")]
    [InverseProperty("Clientes")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    [ForeignKey("IdUsuarioCad")]
    [InverseProperty("Clientes")]
    public virtual Usuario IdUsuarioCadNavigation { get; set; } = null!;
}
