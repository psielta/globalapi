using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[PrimaryKey("Chave", "CdEmpresa")]
[Table("configuracoes_empresa")]
public partial class ConfiguracoesEmpresa : IIdentifiableMultiKey<int, string>
{
    [Key]
    [Column("chave")]
    [StringLength(256)]
    public string Chave { get; set; } = null!;

    [Column("valor_1")]
    [StringLength(16384)]
    public string? Valor1 { get; set; }

    [Column("valor_2")]
    [StringLength(16384)]
    public string? Valor2 { get; set; }

    [Column("valor_3")]
    [StringLength(16384)]
    public string? Valor3 { get; set; }

    [Key]
    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("valor_4")]
    [StringLength(16384)]
    public string? Valor4 { get; set; }

    [Column("valor_5")]
    [StringLength(16384)]
    public string? Valor5 { get; set; }

    [Column("valor_6")]
    [StringLength(16384)]
    public string? Valor6 { get; set; }

    [Column("valor_7")]
    [StringLength(16384)]
    public string? Valor7 { get; set; }

    [Column("valor_8")]
    [StringLength(16384)]
    public string? Valor8 { get; set; }

    [Column("valor_9")]
    [StringLength(16384)]
    public string? Valor9 { get; set; }
    
    [Column("valor_10")]
    [StringLength(16384)]
    public string? Valor10 { get; set; }

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("ConfiguracoesEmpresas")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [GraphQLIgnore]
    public (int, string) GetId()
    {
        return (CdEmpresa, Chave);
    }

    [GraphQLIgnore]
    public string GetKeyName1()
    {
        return "CdEmpresa";
    }

    [GraphQLIgnore]
    public string GetKeyName2()
    {
        return "Chave";
    }
}
