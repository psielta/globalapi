using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("impcabnfe")]
public partial class Impcabnfe : IIdentifiable<string>
{
    [Key]
    [Column("ch_nfe")]
    [StringLength(150)]
    public string ChNfe { get; set; } = null!;

    [Column("tipo")]
    [StringLength(1)]
    public string? Tipo { get; set; }

    [Column("nr_nf")]
    [StringLength(150)]
    public string? NrNf { get; set; }

    [Column("modelo")]
    [StringLength(4)]
    public string? Modelo { get; set; }

    [Column("cnfe")]
    [StringLength(150)]
    public string? Cnfe { get; set; }

    [Column("tp_pagt")]
    [StringLength(255)]
    public string? TpPagt { get; set; }

    [Column("dt_emissao", TypeName = "timestamp without time zone")]
    public DateTime? DtEmissao { get; set; }

    [Column("dt_saida", TypeName = "timestamp without time zone")]
    public DateTime? DtSaida { get; set; }

    [Column("cnpj")]
    [StringLength(18)]
    public string? Cnpj { get; set; }

    [Column("ie")]
    [StringLength(18)]
    public string? Ie { get; set; }

    [Column("nome")]
    [StringLength(150)]
    public string? Nome { get; set; }

    [Column("fone")]
    [StringLength(18)]
    public string? Fone { get; set; }

    [Column("cep")]
    [StringLength(9)]
    public string? Cep { get; set; }

    [Column("endereco")]
    [StringLength(150)]
    public string? Endereco { get; set; }

    [Column("numero")]
    [StringLength(4)]
    public string? Numero { get; set; }

    [Column("bairro")]
    [StringLength(62)]
    public string? Bairro { get; set; }

    [Column("cd_cidade")]
    [StringLength(10)]
    public string? CdCidade { get; set; }

    [Column("inf_obs")]
    public string? InfObs { get; set; }

    [Column("tp_frete")]
    [StringLength(4)]
    public string? TpFrete { get; set; }

    [Column("cnpj_transp")]
    [StringLength(18)]
    public string? CnpjTransp { get; set; }

    [Column("nome_transp")]
    [StringLength(150)]
    public string? NomeTransp { get; set; }

    [Column("end_transp")]
    [StringLength(150)]
    public string? EndTransp { get; set; }

    [Column("cidade_transp")]
    [StringLength(150)]
    public string? CidadeTransp { get; set; }

    [Column("uf_transp")]
    [StringLength(2)]
    public string? UfTransp { get; set; }

    [Column("cd_uf_transp")]
    public int? CdUfTransp { get; set; }

    [Column("id_nota")]
    [StringLength(150)]
    public string? IdNota { get; set; }

    [Column("serie")]
    [StringLength(4)]
    public string? Serie { get; set; }

    [Column("caminho")]
    [StringLength(255)]
    public string? Caminho { get; set; }

    [Column("nr_autorizacao")]
    [StringLength(255)]
    public string? NrAutorizacao { get; set; }

    [Column("xml_nota")]
    public string? XmlNota { get; set; }

    [Column("t_pag")]
    [StringLength(255)]
    public string? TPag { get; set; }

    [Column("cnpj_avulsa")]
    [StringLength(255)]
    public string? CnpjAvulsa { get; set; }

    public string GetId()
    {
        return ChNfe;
    }

    public string GetKeyName()
    {
        return "ChNfe";
    }
}
