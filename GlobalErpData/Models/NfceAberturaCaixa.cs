using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[PrimaryKey("NrLanc", "CdEmpresa")]
[Table("nfce_abertura_caixa")]
[Index("Sequence", Name = "nfce_abertura_caixa_u_idx1", IsUnique = true)]
public partial class NfceAberturaCaixa : IIdentifiableMultiKey<int, int>
{
    [Key]
    [Column("nr_lanc")]
    public int NrLanc { get; set; }

    [Column("sequence")]
    public long Sequence { get; set; }

    [Column("data_lanc", TypeName = "timestamp without time zone")]
    public DateTime? DataLanc { get; set; }

    [Key]
    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("data_abertura")]
    public DateOnly DataAbertura { get; set; }

    [Column("hora_abertura")]
    public TimeOnly HoraAbertura { get; set; }

    [Column("data_encerramento")]
    public DateOnly? DataEncerramento { get; set; }

    [Column("hora_encerramento")]
    public TimeOnly? HoraEncerramento { get; set; }

    [Column("cd_operador")]
    [StringLength(62)]
    public string CdOperador { get; set; } = null!;

    [Column("status")]
    [StringLength(4)]
    public string Status { get; set; } = null!;

    [Column("vl_suprimento")]
    [Precision(18, 4)]
    public decimal VlSuprimento { get; set; }

    [Column("vl_venda_final")]
    [Precision(18, 4)]
    public decimal? VlVendaFinal { get; set; }

    [Column("cd_gerente")]
    [StringLength(62)]
    public string? CdGerente { get; set; }

    [Column("vl_venda_final_cart")]
    [Precision(18, 4)]
    public decimal? VlVendaFinalCart { get; set; }

    [Column("vl_venda_final_chq")]
    [Precision(18, 4)]
    public decimal? VlVendaFinalChq { get; set; }

    [Column("vl_venda_final_prazo")]
    [Precision(18, 4)]
    public decimal? VlVendaFinalPrazo { get; set; }

    [Column("vl_venda_final_cart_deb")]
    [Precision(18, 4)]
    public decimal? VlVendaFinalCartDeb { get; set; }

    [Column("vl_sangria")]
    [Precision(18, 4)]
    public decimal? VlSangria { get; set; }

    [Column("vl_venda_final_pix")]
    [Precision(18, 4)]
    public decimal? VlVendaFinalPix { get; set; }

    [Column("vl_venda_ticket")]
    [Precision(18, 4)]
    public decimal? VlVendaTicket { get; set; }

    [Column("vl_moedas")]
    [Precision(18, 4)]
    public decimal? VlMoedas { get; set; }

    [Column("vl_baixa_fiado")]
    [Precision(18, 4)]
    public decimal? VlBaixaFiado { get; set; }

    [Column("i_dinheiro")]
    [Precision(18, 4)]
    public decimal? IDinheiro { get; set; }

    [Column("i_cc")]
    [Precision(18, 4)]
    public decimal? ICc { get; set; }

    [Column("i_cd")]
    [Precision(18, 4)]
    public decimal? ICd { get; set; }

    [Column("i_pix")]
    [Precision(18, 4)]
    public decimal? IPix { get; set; }

    [Column("i_cheque")]
    [Precision(18, 4)]
    public decimal? ICheque { get; set; }

    [Column("i_prazo")]
    [Precision(18, 4)]
    public decimal? IPrazo { get; set; }

    [Column("i_vale")]
    [Precision(18, 4)]
    public decimal? IVale { get; set; }

    [Column("i_sangria")]
    [Precision(18, 4)]
    public decimal? ISangria { get; set; }

    [Column("i_suprimento")]
    [Precision(18, 4)]
    public decimal? ISuprimento { get; set; }

    [Column("i_total")]
    [Precision(18, 4)]
    public decimal? ITotal { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("NfceAberturaCaixas")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [GraphQLIgnore]
    public (int, int) GetId()
    {
        return (NrLanc, CdEmpresa);
    }

    [GraphQLIgnore]
    public string GetKeyName1()
    {
        return "NrLanc";
    }

    [GraphQLIgnore]
    public string GetKeyName2()
    {
        return "CdEmpresa";
    }

    [Column("unity")]
    public int Unity { get; set; }

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("NfceAberturaCaixas")]
    public virtual Unity UnityNavigation { get; set; } = null!;

    [NotMapped]
    [JsonPropertyName("nm_empresa")]
    public string? NmEmpresa => CdEmpresaNavigation?.NmEmpresa ?? string.Empty;

    [JsonIgnore]
    [InverseProperty("NfceAberturaCaixa")]
    public virtual ICollection<SangriaCaixa> SangriaCaixas { get; set; } = new List<SangriaCaixa>();
}
