﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("conta_do_caixa")]
public partial class ContaDoCaixa
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nm_conta")]
    [StringLength(62)]
    public string NmConta { get; set; } = null!;

    [Column("nr_conta_banco")]
    [StringLength(15)]
    public string? NrContaBanco { get; set; }

    [Column("nr_agencia")]
    [StringLength(10)]
    public string? NrAgencia { get; set; }

    [Column("nm_banco")]
    [StringLength(62)]
    public string? NmBanco { get; set; }

    [Column("saldo_inicial")]
    [Precision(18, 4)]
    public decimal? SaldoInicial { get; set; }

    [Column("nr_cheque_inicial")]
    [StringLength(10)]
    public string? NrChequeInicial { get; set; }

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("nr_digito_ag")]
    [StringLength(1)]
    public string? NrDigitoAg { get; set; }

    [Column("nr_digito_conta")]
    [StringLength(1)]
    public string? NrDigitoConta { get; set; }

    [Column("saldo_atual")]
    [Precision(18, 4)]
    public decimal? SaldoAtual { get; set; }

    [Column("limite_especial")]
    [Precision(18, 4)]
    public decimal? LimiteEspecial { get; set; }

    [Column("mostrar_dados_impressao")]
    [StringLength(1)]
    public string? MostrarDadosImpressao { get; set; }

    [ForeignKey("CdEmpresa")]
    [InverseProperty("ContaDoCaixas")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;
}