using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("contas_a_pagar")]
public partial class ContasAPagar : IIdentifiable<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("dt_lancamento")]
    public DateOnly DtLancamento { get; set; }

    [Column("dt_vencimento")]
    public DateOnly DtVencimento { get; set; }

    [Column("dt_pagou")]
    public DateOnly? DtPagou { get; set; }

    [Column("cd_fornecedor")]
    public int CdFornecedor { get; set; }

    [Column("nr_entrada")]
    public int? NrEntrada { get; set; }

    [Column("nr_duplicata")]
    [StringLength(25)]
    public string? NrDuplicata { get; set; }

    [Column("vl_cp")]
    [Precision(18, 4)]
    public decimal VlCp { get; set; }

    [Column("vl_desconto")]
    [Precision(18, 4)]
    public decimal VlDesconto { get; set; }

    [Column("vl_total")]
    [Precision(18, 4)]
    public decimal VlTotal { get; set; }

    [Column("vl_acrescimo")]
    [Precision(18, 4)]
    public decimal? VlAcrescimo { get; set; }

    [Column("vl_pago_final")]
    [Precision(18, 4)]
    public decimal? VlPagoFinal { get; set; }

    [Column("pagou")]
    [StringLength(1)]
    public string Pagou { get; set; } = null!;

    [Column("txt_obs")]
    [StringLength(16384)]
    public string? TxtObs { get; set; }

    [Column("tp_forma_pagt")]
    [StringLength(3)]
    public string? TpFormaPagt { get; set; }

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("nr_cheque")]
    [StringLength(10)]
    public string? NrCheque { get; set; }

    [Column("nr_conta")]
    public int? NrConta { get; set; }

    [Column("pago_a")]
    [StringLength(62)]
    public string? PagoA { get; set; }

    [Column("vl_cheque")]
    [Precision(18, 4)]
    public decimal? VlCheque { get; set; }

    [Column("nr_nf")]
    [StringLength(15)]
    public string? NrNf { get; set; }

    [Column("cd_plano_caixa")]
    [StringLength(25)]
    public string CdPlanoCaixa { get; set; } = null!;

    [Column("cd_historico_caixa")]
    [StringLength(25)]
    public string CdHistoricoCaixa { get; set; } = null!;

    [Column("nr_forma_pagt")]
    public int? NrFormaPagt { get; set; }

    [Column("vl_dinheiro")]
    [Precision(18, 4)]
    public decimal? VlDinheiro { get; set; }

    [Column("id_lanc_princ")]
    [StringLength(10)]
    public string? IdLancPrinc { get; set; }

    [Column("nr_entrada_outra_desp")]
    public int? NrEntradaOutraDesp { get; set; }

    [Column("ref_quant_entrega")]
    [Precision(18, 0)]
    public decimal? RefQuantEntrega { get; set; }

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
    [ForeignKey("CdEmpresa")]
    [InverseProperty("ContasAPagars")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("CdFornecedor, CdEmpresa")]
    [InverseProperty("ContasAPagars")]
    public virtual Fornecedor Fornecedor { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("NrEntrada, CdEmpresa")]
    [InverseProperty("ContasAPagars")]
    public virtual Entrada? Entrada { get; set; }

    [JsonPropertyName("nmForn")]
    [NotMapped]
    public string NmForn => Fornecedor?.NmForn ?? "";

    [JsonIgnore]
    [ForeignKey("CdEmpresa, CdHistoricoCaixa, CdPlanoCaixa")]
    [InverseProperty("ContasAPagars")]
    public virtual HistoricoCaixa HistoricoCaixa { get; set; } = null!;

    [JsonPropertyName("nmPlanoCaixa")]
    [NotMapped]
    public string NmPlanoCaixa => HistoricoCaixa?.PlanoDeCaixa?.Descricao ?? "";

    [JsonPropertyName("nmHistoricoCaixa")]
    [NotMapped]
    public string NmHistoricoCaixa => HistoricoCaixa?.Descricao ?? "";

    [JsonIgnore]
    [InverseProperty("NrCpNavigation")]
    public virtual ICollection<LivroCaixa> LivroCaixas { get; set; } = new List<LivroCaixa>();

    [JsonIgnore]
    [InverseProperty("IdContasPagarNavigation")]
    public virtual ICollection<PagtosParciaisCp> PagtosParciaisCps { get; set; } = new List<PagtosParciaisCp>();


    [GraphQLIgnore]
    public int GetId()
    {
        return Id;
    }

    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "Id";
    }
}
