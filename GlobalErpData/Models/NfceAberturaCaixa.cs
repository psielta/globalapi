using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("nfce_abertura_caixa")]
public partial class NfceAberturaCaixa : IIdentifiable<long>
{
    [Key]
    [Column("nr_lanc")]
    public long NrLanc { get; set; }

    [Column("data_lanc", TypeName = "timestamp without time zone")]
    public DateTime? DataLanc { get; set; }

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

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("NfceAberturaCaixas")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdUsuario")]
    [InverseProperty("NfceAberturaCaixas")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    [JsonIgnore]
    [InverseProperty("NrAberturaCaixaNavigation")]
    public virtual ICollection<NfceSaida> NfceSaida { get; set; } = new List<NfceSaida>();

    [JsonIgnore]
    [InverseProperty("NrAberturaCaixaNavigation")]
    public virtual ICollection<NfceSangriaCaixa> NfceSangriaCaixas { get; set; } = new List<NfceSangriaCaixa>();

    [JsonIgnore]
    [InverseProperty("NrAberturaCaixaNavigation")]
    public virtual ICollection<NfceSuprimentoCaixa> NfceSuprimentoCaixas { get; set; } = new List<NfceSuprimentoCaixa>();

    [GraphQLIgnore]
    public long GetId()
    {
        return NrLanc;
    }

    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "NrLanc";
    }
}
