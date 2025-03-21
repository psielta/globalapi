using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("unity")]
public partial class Unity
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string? Name { get; set; }

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Certificado> Certificados { get; set; } = new List<Certificado>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<CfopImportacao> CfopImportacaos { get; set; } = new List<CfopImportacao>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<ContasAPagar> ContasAPagars { get; set; } = new List<ContasAPagar>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Empresa> Empresas { get; set; } = new List<Empresa>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Entrada> Entrada { get; set; } = new List<Entrada>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<EntradaOutrasDesp> EntradaOutrasDesps { get; set; } = new List<EntradaOutrasDesp>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<EntregaNfe> EntregaNves { get; set; } = new List<EntregaNfe>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Featured> Featureds { get; set; } = new List<Featured>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Fornecedor> Fornecedors { get; set; } = new List<Fornecedor>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<FotosProduto> FotosProdutos { get; set; } = new List<FotosProduto>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<GrupoEstoque> GrupoEstoques { get; set; } = new List<GrupoEstoque>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<HistoricoCaixa> HistoricoCaixas { get; set; } = new List<HistoricoCaixa>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<PlanoDeCaixa> PlanoDeCaixas { get; set; } = new List<PlanoDeCaixa>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<NfceProdutoSaidum> NfceProdutoSaida { get; set; } = new List<NfceProdutoSaidum>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<OlderItem> OlderItems { get; set; } = new List<OlderItem>();

    

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<PlanoEstoque> PlanoEstoques { get; set; } = new List<PlanoEstoque>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<ProdutoEntradum> ProdutoEntrada { get; set; } = new List<ProdutoEntradum>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<ProdutoEstoque> ProdutoEstoques { get; set; } = new List<ProdutoEstoque>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<ProdutoSaidum> ProdutoSaida { get; set; } = new List<ProdutoSaidum>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<ProdutosForn> ProdutosForns { get; set; } = new List<ProdutosForn>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<ReferenciaEstoque> ReferenciaEstoques { get; set; } = new List<ReferenciaEstoque>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<RetiradaNfe> RetiradaNves { get; set; } = new List<RetiradaNfe>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<SaldoEstoque> SaldoEstoques { get; set; } = new List<SaldoEstoque>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<SectionItem> SectionItems { get; set; } = new List<SectionItem>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Transportadora> Transportadoras { get; set; } = new List<Transportadora>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<UnidadeMedidum> UnidadeMedida { get; set; } = new List<UnidadeMedidum>();

    [InverseProperty("UnityNavigation")]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
