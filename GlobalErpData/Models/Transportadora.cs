﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[PrimaryKey("CdTransportadora", "Unity")]
[Table("transportadora")]
public partial class Transportadora : IIdentifiableMultiKey<int, int>
{
    [Key]
    [Column("cd_transportadora")]
    public int CdTransportadora { get; set; }

    [Column("nm_transportadora")]
    [StringLength(62)]
    public string? NmTransportadora { get; set; }

    [Column("nm_endereco")]
    [StringLength(62)]
    public string? NmEndereco { get; set; }

    [Column("numero")]
    public int? Numero { get; set; }

    [Column("nm_bairro")]
    [StringLength(62)]
    public string? NmBairro { get; set; }

    [Column("cd_cidade")]
    [StringLength(10)]
    public string? CdCidade { get; set; }

    [Column("cd_cnpj")]
    [StringLength(18)]
    public string? CdCnpj { get; set; }

    [Column("cd_ie")]
    [StringLength(18)]
    public string? CdIe { get; set; }

    [Column("placa_veiculo")]
    [StringLength(11)]
    public string? PlacaVeiculo { get; set; }

    [Column("nr_telefone")]
    [StringLength(15)]
    public string? NrTelefone { get; set; }

    [Column("nr_telefone2")]
    [StringLength(15)]
    public string? NrTelefone2 { get; set; }

    [Column("email")]
    [StringLength(250)]
    public string? Email { get; set; }

    [Column("cep")]
    [StringLength(9)]
    public string? Cep { get; set; }

    [Key]
    [Column("unity")]
    public int Unity { get; set; }

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("Transportadoras")]
    public virtual Unity UnityNavigation { get; set; } = null!;

    [JsonPropertyName("nmCidade")]
    [NotMapped]
    public string NmCidade => CdCidadeNavigation?.NmCidade ?? string.Empty;

    [JsonPropertyName("uf")]
    [NotMapped]
    public string Uf => CdCidadeNavigation?.Uf ?? string.Empty;

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [JsonIgnore]
    [ForeignKey("CdCidade")]
    [InverseProperty("Transportadoras")]
    public virtual Cidade? CdCidadeNavigation { get; set; }

    [GraphQLIgnore]
    public (int, int) GetId()
    {
        return (Unity,CdTransportadora);
    }

    [GraphQLIgnore]
    public string GetKeyName1()
    {
        return "Unity";
    }

    [GraphQLIgnore]
    public string GetKeyName2()
    {
        return "CdTransportadora";
    }

    [JsonIgnore]
    [InverseProperty("Transportadora")]
    public virtual ICollection<Frete> Fretes { get; set; } = new List<Frete>();
}
