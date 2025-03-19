using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("conf_email")]
public partial class ConfEmail
{
    [Key]
    [Column("nr_lanc")]
    public int NrLanc { get; set; }

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("e_mail_copia")]
    [StringLength(150)]
    public string? EMailCopia { get; set; }

    [Column("assunto_email")]
    [StringLength(150)]
    public string? AssuntoEmail { get; set; }

    [Column("smtp")]
    [StringLength(62)]
    public string Smtp { get; set; } = null!;

    [Column("porta")]
    [StringLength(10)]
    public string Porta { get; set; } = null!;

    [Column("usuario")]
    [StringLength(62)]
    public string Usuario { get; set; } = null!;

    [Column("senha")]
    [StringLength(62)]
    public string Senha { get; set; } = null!;

    [Column("conexao_segura")]
    [StringLength(1)]
    public string? ConexaoSegura { get; set; }

    [Column("txt_padrao")]
    [StringLength(16384)]
    public string? TxtPadrao { get; set; }

    [Column("ssl")]
    [StringLength(1)]
    public string? Ssl { get; set; }

    [Column("tsl")]
    [StringLength(1)]
    public string? Tsl { get; set; }

    [Column("envia_apos_emitir")]
    public bool? EnviaAposEmitir { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [ForeignKey("CdEmpresa")]
    [InverseProperty("ConfEmails")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;
}
