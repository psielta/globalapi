using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[PrimaryKey("CdFuncionario", "CdEmpresa")]
[Table("funcionario")]
public partial class Funcionario
{
    [Key]
    [Column("cd_funcionario")]
    public int CdFuncionario { get; set; }

    [Column("nm_funcionario")]
    [StringLength(62)]
    public string NmFuncionario { get; set; } = null!;

    [Column("dt_nascimento")]
    public DateOnly? DtNascimento { get; set; }

    [Column("endereco")]
    [StringLength(62)]
    public string? Endereco { get; set; }

    [Column("numero")]
    public int? Numero { get; set; }

    [Column("bairro")]
    [StringLength(62)]
    public string? Bairro { get; set; }

    [Column("cidade")]
    [StringLength(10)]
    public string? Cidade { get; set; }

    [Column("nr_telefone")]
    [StringLength(15)]
    public string? NrTelefone { get; set; }

    [Column("nr_telefone2")]
    [StringLength(15)]
    public string? NrTelefone2 { get; set; }

    [Column("nr_cpf")]
    [StringLength(18)]
    public string? NrCpf { get; set; }

    [Column("nr_rg")]
    [StringLength(18)]
    public string? NrRg { get; set; }

    [Column("sexo")]
    [StringLength(1)]
    public string? Sexo { get; set; }

    [Column("estado_civil")]
    [StringLength(1)]
    public string? EstadoCivil { get; set; }

    [Column("dt_admissao")]
    public DateOnly DtAdmissao { get; set; }

    [Key]
    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("cargo")]
    [StringLength(62)]
    public string Cargo { get; set; } = null!;

    [Column("salario")]
    [Precision(18, 2)]
    public decimal Salario { get; set; }

    [Column("cep")]
    [StringLength(9)]
    public string? Cep { get; set; }

    [Column("cd_cbo")]
    [StringLength(6)]
    public string? CdCbo { get; set; }

    [Column("mecanico")]
    [StringLength(1)]
    public string? Mecanico { get; set; }

    [Column("vendedor")]
    [StringLength(1)]
    public string? Vendedor { get; set; }

    [Column("ativo")]
    [StringLength(1)]
    public string? Ativo { get; set; }

    [Column("cd_interno")]
    public string? CdInterno { get; set; }

    [Column("color")]
    [StringLength(15)]
    public string? Color { get; set; }

    [Column("txt_obs")]
    [StringLength(16384)]
    public string? TxtObs { get; set; }

    [Column("nr_carteira")]
    [StringLength(20)]
    public string? NrCarteira { get; set; }

    [Column("pasep")]
    [StringLength(20)]
    public string? Pasep { get; set; }

    [Column("data_rescisao")]
    public DateOnly? DataRescisao { get; set; }

    [Column("salario_carteira")]
    public decimal? SalarioCarteira { get; set; }

    [Column("registrado")]
    [StringLength(1)]
    public string? Registrado { get; set; }

    [Column("integrado")]
    [StringLength(1)]
    public string? Integrado { get; set; }

    [Column("id_serv_central")]
    [StringLength(256)]
    public string? IdServCentral { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [Column("percentual_comissao")]
    [Precision(18, 4)]
    public decimal PercentualComissao { get; set; }

    [ForeignKey("CdEmpresa")]
    [InverseProperty("Funcionarios")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [ForeignKey("Cidade")]
    [InverseProperty("Funcionarios")]
    public virtual Cidade? CidadeNavigation { get; set; }

    [InverseProperty("Funcionario")]
    public virtual ICollection<OrcamentoCab> OrcamentoCabs { get; set; } = new List<OrcamentoCab>();

    [ForeignKey("Unity")]
    [InverseProperty("Funcionarios")]
    public virtual Unity UnityNavigation { get; set; } = null!;

    [InverseProperty("Funcionario")]
    public virtual ICollection<UsuarioFuncionario> UsuarioFuncionarios { get; set; } = new List<UsuarioFuncionario>();

    [InverseProperty("Funcionario")]
    public virtual Vendedor? VendedorNavigation { get; set; }
}
