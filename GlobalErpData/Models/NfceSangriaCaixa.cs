using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("nfce_sangria_caixa")]
public partial class NfceSangriaCaixa : IIdentifiable<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("data")]
    public DateOnly? Data { get; set; }

    [Column("hora")]
    public TimeOnly? Hora { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("nr_abertura_caixa")]
    public long NrAberturaCaixa { get; set; }

    [Column("id_caixa")]
    public int IdCaixa { get; set; }

    [Column("valor")]
    [Precision(15, 2)]
    public decimal Valor { get; set; }

    [Column("tipo")]
    [StringLength(4)]
    public string? Tipo { get; set; }

    [Column("obs")]
    [StringLength(256)]
    public string? Obs { get; set; }

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("NfceSangriaCaixas")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdUsuario")]
    [InverseProperty("NfceSangriaCaixas")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("NrAberturaCaixa")]
    [InverseProperty("NfceSangriaCaixas")]
    public virtual NfceAberturaCaixa NrAberturaCaixaNavigation { get; set; } = null!;

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
