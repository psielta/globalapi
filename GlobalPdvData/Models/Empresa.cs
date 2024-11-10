using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalPdvData.Models;

[Table("empresa")]
public partial class Empresa : IIdentifiable<int>
{
    [Key]
    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("nm_empresa")]
    [StringLength(256)]
    public string NmEmpresa { get; set; } = null!;

    [Column("nm_endereco")]
    [StringLength(62)]
    public string NmEndereco { get; set; } = null!;

    [Column("numero")]
    public int Numero { get; set; }

    [Column("cd_cidade")]
    [StringLength(10)]
    public string CdCidade { get; set; } = null!;

    [Column("cd_cep")]
    [StringLength(9)]
    public string CdCep { get; set; } = null!;

    [Column("cd_cnpj")]
    [StringLength(18)]
    public string? CdCnpj { get; set; }

    [Column("nm_bairro")]
    [StringLength(25)]
    public string? NmBairro { get; set; }

    [Column("telefone")]
    [StringLength(15)]
    public string? Telefone { get; set; }

    [Column("nr_inscr_municipal")]
    [StringLength(18)]
    public string? NrInscrMunicipal { get; set; }

    [Column("nr_inscr_estadual")]
    [StringLength(18)]
    public string? NrInscrEstadual { get; set; }

    [Column("txt_obs")]
    [StringLength(16384)]
    public string? TxtObs { get; set; }

    [Column("e_mail")]
    [StringLength(100)]
    public string? EMail { get; set; }

    [Column("idcsc")]
    [StringLength(6)]
    public string? Idcsc { get; set; }

    [Column("csc")]
    [StringLength(36)]
    public string? Csc { get; set; }

    [Column("autorizo_xml")]
    [StringLength(1)]
    public string? AutorizoXml { get; set; }

    [Column("cpfcnpf_autorizado")]
    [StringLength(18)]
    public string? CpfcnpfAutorizado { get; set; }

    [Column("nome_fantasia")]
    [StringLength(128)]
    public string? NomeFantasia { get; set; }

    [Column("tipo_regime")]
    public int? TipoRegime { get; set; }

    [Column("mail_contador")]
    [StringLength(255)]
    public string? MailContador { get; set; }

    [JsonIgnore]
    [ForeignKey("CdCidade")]
    [InverseProperty("Empresas")]
    public virtual Cidade CdCidadeNavigation { get; set; } = null!;

    [JsonIgnore]
    [InverseProperty("CdEmpresaNavigation")]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "CdEmpresa";
    }

    [GraphQLIgnore]
    public int GetId()
    {
        return this.CdEmpresa;
    }
}
