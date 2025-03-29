using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalErpData.MMModels;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("unity")]
public partial class Unity : IIdentifiable<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string? Name { get; set; }

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<ContasAPagar> ContasAPagars { get; set; } = new List<ContasAPagar>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Empresa> Empresas { get; set; } = new List<Empresa>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Entrada> Entrada { get; set; } = new List<Entrada>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<EntradaOutrasDesp> EntradaOutrasDesps { get; set; } = new List<EntradaOutrasDesp>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<EntregaNfe> EntregaNves { get; set; } = new List<EntregaNfe>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Featured> Featureds { get; set; } = new List<Featured>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Fornecedor> Fornecedors { get; set; } = new List<Fornecedor>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<FotosProduto> FotosProdutos { get; set; } = new List<FotosProduto>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<GrupoEstoque> GrupoEstoques { get; set; } = new List<GrupoEstoque>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<NfceProdutoSaidum> NfceProdutoSaida { get; set; } = new List<NfceProdutoSaidum>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<OlderItem> OlderItems { get; set; } = new List<OlderItem>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<ProdutoEntradum> ProdutoEntrada { get; set; } = new List<ProdutoEntradum>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<ProdutoEstoque> ProdutoEstoques { get; set; } = new List<ProdutoEstoque>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<ProdutoSaidum> ProdutoSaida { get; set; } = new List<ProdutoSaidum>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<ProdutosForn> ProdutosForns { get; set; } = new List<ProdutosForn>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<ReferenciaEstoque> ReferenciaEstoques { get; set; } = new List<ReferenciaEstoque>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<RetiradaNfe> RetiradaNves { get; set; } = new List<RetiradaNfe>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<SaldoEstoque> SaldoEstoques { get; set; } = new List<SaldoEstoque>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<SectionItem> SectionItems { get; set; } = new List<SectionItem>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<UnidadeMedida> UnidadeMedida { get; set; } = new List<UnidadeMedida>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    
    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<PlanoEstoque> PlanoEstoques { get; set; } = new List<PlanoEstoque>();
    
    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Transportadora> Transportadoras { get; set; } = new List<Transportadora>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Certificado> Certificados { get; set; } = new List<Certificado>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<CfopImportacao> CfopImportacaos { get; set; } = new List<CfopImportacao>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<HistoricoCaixa> HistoricoCaixas { get; set; } = new List<HistoricoCaixa>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Saida> Saida { get; set; } = new List<Saida>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<ConfiguracoesEmpresa> ConfiguracoesEmpresas { get; set; } = new List<ConfiguracoesEmpresa>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<PlanoDeCaixa> PlanoDeCaixas { get; set; } = new List<PlanoDeCaixa>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<ContaDoCaixa> ContaDoCaixas { get; set; } = new List<ContaDoCaixa>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<CfopCsosnV2> CfopCsosnV2s { get; set; } = new List<CfopCsosnV2>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<ControleNumeracaoNfe> ControleNumeracaoNves { get; set; } = new List<ControleNumeracaoNfe>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<ObsNf> ObsNfs { get; set; } = new List<ObsNf>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Icm> Icms { get; set; } = new List<Icm>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<ContasAReceber> ContasARecebers { get; set; } = new List<ContasAReceber>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<NcmProtocoloEstado> NcmProtocoloEstados { get; set; } = new List<NcmProtocoloEstado>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<ProtocoloEstadoNcm> ProtocoloEstadoNcms { get; set; } = new List<ProtocoloEstadoNcm>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Frete> Fretes { get; set; } = new List<Frete>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<LivroCaixa> LivroCaixas { get; set; } = new List<LivroCaixa>();

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

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Departamento> Departamentos { get; set; } = new List<Departamento>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<OrcamentoCab> OrcamentoCabs { get; set; } = new List<OrcamentoCab>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<OsTabelaPreco> OsTabelaPrecos { get; set; } = new List<OsTabelaPreco>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Servico> Servicos { get; set; } = new List<Servico>();

    [JsonIgnore]
    [InverseProperty("UnityNavigation")]
    public virtual ICollection<PlanoSimultaneo> PlanoSimultaneos { get; set; } = new List<PlanoSimultaneo>();
}
