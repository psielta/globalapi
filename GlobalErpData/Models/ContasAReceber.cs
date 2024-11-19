using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("contas_a_receber")]
public partial class ContasAReceber : IIdentifiable<int>
{
    [Key]
    [Column("nr_conta")]
    public int NrConta { get; set; }

    [Column("data_lanc")]
    public DateOnly? DataLanc { get; set; }

    [Column("dt_vencimento")]
    public DateOnly DtVencimento { get; set; }

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("cd_cliente")]
    public int CdCliente { get; set; }

    [Column("nr_duplicata")]
    [StringLength(20)]
    public string? NrDuplicata { get; set; }

    [Column("vl_conta")]
    [Precision(18, 4)]
    public decimal VlConta { get; set; }

    [Column("vl_acrescimo")]
    [Precision(18, 4)]
    public decimal VlAcrescimo { get; set; }

    [Column("vl_desconto")]
    [Precision(18, 4)]
    public decimal? VlDesconto { get; set; }

    [Column("vl_total")]
    [Precision(18, 4)]
    public decimal VlTotal { get; set; }

    [Column("vl_pago")]
    [Precision(18, 4)]
    public decimal? VlPago { get; set; }

    [Column("vl_juros")]
    [Precision(18, 4)]
    public decimal? VlJuros { get; set; }

    [Column("dt_pagamento")]
    public DateOnly? DtPagamento { get; set; }

    [Column("nr_usuario")]
    public int? NrUsuario { get; set; }

    [Column("recebeu")]
    [StringLength(1)]
    public string Recebeu { get; set; } = null!;

    [Column("quantidade")]
    public int Quantidade { get; set; }

    [Column("qt_parcela")]
    public int QtParcela { get; set; }

    [Column("txt_obs")]
    [StringLength(16384)]
    public string? TxtObs { get; set; }

    [Column("nr_conta_caixa")]
    public int? NrContaCaixa { get; set; }

    [Column("cd_historico_caixa")]
    [StringLength(25)]
    public string? CdHistoricoCaixa { get; set; }

    [Column("cd_plano_caixa")]
    [StringLength(25)]
    public string? CdPlanoCaixa { get; set; }

    [Column("nr_saida")]
    public int? NrSaida { get; set; }

    [Column("nr_boleto")]
    [StringLength(25)]
    public string? NrBoleto { get; set; }

    [Column("txt_boleto")]
    [StringLength(16384)]
    public string? TxtBoleto { get; set; }

    [Column("cd_projeto")]
    public int? CdProjeto { get; set; }

    [Column("base")]
    [StringLength(7)]
    public string? Base { get; set; }

    [Column("vl_iss")]
    [Precision(18, 4)]
    public decimal? VlIss { get; set; }

    [Column("vl_irrf")]
    [Precision(18, 4)]
    public decimal? VlIrrf { get; set; }

    [Column("nr_os")]
    public int? NrOs { get; set; }

    [Column("nr_forma_pagt")]
    public int? NrFormaPagt { get; set; }

    [Column("imprimiu")]
    [StringLength(1)]
    public string? Imprimiu { get; set; }

    [Column("vl_bruto")]
    [Precision(18, 4)]
    public decimal? VlBruto { get; set; }

    [Column("status")]
    [StringLength(2)]
    public string? Status { get; set; }

    [Column("nr_conta_renegociado")]
    public int? NrContaRenegociado { get; set; }

    [Column("alteradodtvenc")]
    [StringLength(1)]
    public string? Alteradodtvenc { get; set; }

    [Column("id_aluno")]
    public int? IdAluno { get; set; }

    [Column("cancelado")]
    [StringLength(1)]
    public string? Cancelado { get; set; }

    [Column("id_grupo")]
    public int? IdGrupo { get; set; }

    [Column("id_bandeira")]
    public int? IdBandeira { get; set; }

    [Column("vinculado")]
    [StringLength(1)]
    public string? Vinculado { get; set; }

    [Column("id_lanc_principal")]
    public int? IdLancPrincipal { get; set; }

    [Column("utilizou_limite")]
    [StringLength(1)]
    public string? UtilizouLimite { get; set; }

    [Column("nsu")]
    public string? Nsu { get; set; }

    [Column("venceu_prazo")]
    [StringLength(1)]
    public string VenceuPrazo { get; set; } = null!;

    [Column("id_venda_mobile")]
    public int? IdVendaMobile { get; set; }

    [Column("id_extrato")]
    [StringLength(16384)]
    public string? IdExtrato { get; set; }

    [Column("rate")]
    [Precision(18, 4)]
    public decimal Rate { get; set; }

    [Column("number_of_payments")]
    public int NumberOfPayments { get; set; }

    [Column("type")]
    public int Type { get; set; }

    [Column("type_register")]
    public int TypeRegister { get; set; }

    [JsonIgnore]
    [ForeignKey("CdCliente")]
    [InverseProperty("ContasARecebers")]
    public virtual Cliente CdClienteNavigation { get; set; } = null!;

    [JsonPropertyName("nmCliente")]
    [NotMapped]
    public string NmCliente => CdClienteNavigation?.NmCliente ?? "";

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("ContasARecebers")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("CdEmpresa, CdHistoricoCaixa, CdPlanoCaixa")]
    [InverseProperty("ContasARecebers")]
    public virtual HistoricoCaixa? HistoricoCaixa { get; set; }

    [GraphQLIgnore]
    public int GetId()
    {
       return NrConta;
    }

    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "NrConta";
    }

    [JsonIgnore]
    [InverseProperty("NrCrNavigation")]
    public virtual ICollection<LivroCaixa> LivroCaixas { get; set; } = new List<LivroCaixa>();

    [JsonIgnore]
    [InverseProperty("NrContaNavigation")]
    public virtual ICollection<PagtosParciaisCr> PagtosParciaisCrs { get; set; } = new List<PagtosParciaisCr>();

}
