using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("livro_caixa")]
public partial class LivroCaixa : IIdentifiable<long>
{
    [Key]
    [Column("nr_lanc")]
    public long NrLanc { get; set; }

    [Column("dt_lanc", TypeName = "timestamp without time zone")]
    public DateTime DtLanc { get; set; }

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("cd_historico")]
    [StringLength(25)]
    public string CdHistorico { get; set; } = null!;

    [Column("vl_lancamento")]
    [Precision(18, 4)]
    public decimal VlLancamento { get; set; }

    [Column("nr_cp")]
    public int? NrCp { get; set; }

    [Column("nr_cr")]
    public int? NrCr { get; set; }

    [Column("txt_obs")]
    [StringLength(16384)]
    public string? TxtObs { get; set; }

    [Column("nr_conta")]
    public int NrConta { get; set; }

    [Column("cd_plano")]
    [StringLength(25)]
    public string CdPlano { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("LivroCaixas")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("CdEmpresa, CdHistorico, CdPlano")]
    [InverseProperty("LivroCaixas")]
    public virtual HistoricoCaixa HistoricoCaixa { get; set; } = null!;

    [JsonPropertyName("tipo")]
    [NotMapped]
    public string Tipo => HistoricoCaixa.Tipo ?? string.Empty;

    [JsonIgnore]
    [ForeignKey("NrConta")]
    [InverseProperty("LivroCaixas")]
    public virtual ContaDoCaixa NrContaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("NrCp")]
    [InverseProperty("LivroCaixas")]
    public virtual ContasAPagar? NrCpNavigation { get; set; }
    [JsonIgnore]
    [ForeignKey("NrCr")]
    [InverseProperty("LivroCaixas")]
    public virtual ContasAReceber? NrCrNavigation { get; set; }

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
