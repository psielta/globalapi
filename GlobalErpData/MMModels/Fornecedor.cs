﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[PrimaryKey("CdForn", "IdEmpresa")]
[Table("fornecedor")]
public partial class Fornecedor
{
    [Key]
    [Column("cd_forn")]
    public int CdForn { get; set; }

    [Column("nm_forn")]
    [StringLength(150)]
    public string NmForn { get; set; } = null!;

    [Column("nm_endereco")]
    [StringLength(62)]
    public string? NmEndereco { get; set; }

    [Column("numero")]
    public int? Numero { get; set; }

    [Column("cd_cidade")]
    [StringLength(10)]
    public string CdCidade { get; set; } = null!;

    [Column("cd_cep")]
    [StringLength(9)]
    public string? CdCep { get; set; }

    [Column("nm_representante")]
    [StringLength(62)]
    public string? NmRepresentante { get; set; }

    [Column("telefone_empresa")]
    [StringLength(15)]
    public string? TelefoneEmpresa { get; set; }

    [Column("telefone_representante")]
    [StringLength(15)]
    public string? TelefoneRepresentante { get; set; }

    [Column("cnpj")]
    [StringLength(18)]
    public string? Cnpj { get; set; }

    [Column("bairro")]
    [StringLength(62)]
    public string? Bairro { get; set; }

    [Column("ramo")]
    [StringLength(62)]
    public string? Ramo { get; set; }

    [Column("email")]
    [StringLength(250)]
    public string? Email { get; set; }

    [Column("nr_inscr_estadual")]
    [StringLength(18)]
    public string? NrInscrEstadual { get; set; }

    [Column("parceiro")]
    [StringLength(1)]
    public string? Parceiro { get; set; }

    [Column("nm_fantasia")]
    [StringLength(150)]
    public string? NmFantasia { get; set; }

    [Column("complemento")]
    [StringLength(62)]
    public string? Complemento { get; set; }

    [Column("id_cliente")]
    public int? IdCliente { get; set; }

    [Column("cpf")]
    [StringLength(18)]
    public string? Cpf { get; set; }

    [Key]
    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("CdCidade")]
    [InverseProperty("Fornecedors")]
    public virtual Cidade CdCidadeNavigation { get; set; } = null!;

    [InverseProperty("Fornecedor")]
    public virtual ICollection<ContasAPagar> ContasAPagars { get; set; } = new List<ContasAPagar>();

    [InverseProperty("Fornecedor")]
    public virtual ICollection<Entrada> Entrada { get; set; } = new List<Entrada>();

    [ForeignKey("IdEmpresa")]
    [InverseProperty("Fornecedors")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    [InverseProperty("Fornecedor")]
    public virtual ICollection<ProdutosForn> ProdutosForns { get; set; } = new List<ProdutosForn>();
}