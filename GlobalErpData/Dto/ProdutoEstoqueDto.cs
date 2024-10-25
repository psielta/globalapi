using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class ProdutoEstoqueDto
    {
        public string NmProduto { get; set; } = null!;

        public decimal QuantMinima { get; set; }

        public string CdBarra { get; set; } = null!;

        public string? CodMargem { get; set; }

        public string? CodEspecie { get; set; }

        public string LancLivro { get; set; } = null!;

        public decimal? PorcVendedor { get; set; }

        public string? CdClassFiscal { get; set; }

        public int CdTribt { get; set; }

        public decimal? PesoBruto { get; set; }

        public decimal? PesoLiquido { get; set; }

        public decimal QtUnitario { get; set; }

        public int CdGrupo { get; set; }

        public int CdRef { get; set; }

        public decimal? QtTotal { get; set; }

        public int? CdSeq { get; set; }

        public string? Suspenso { get; set; }

        public int? CdPlanoEst { get; set; }

        public string? CdUni { get; set; }

        public decimal? PorcSaida { get; set; }

        public DateOnly? DtSuspenso { get; set; }

        public TimeOnly? HrSuspenso { get; set; }

        public DateOnly? DtAtivacao { get; set; }

        public TimeOnly? HrAtivacao { get; set; }

        public DateOnly? DtAtivoLivro { get; set; }

        public DateOnly? DtSuspLivro { get; set; }

        public TimeOnly? HrAtivoLivro { get; set; }

        public TimeOnly? HrSuspLivro { get; set; }

        public string? Ativo { get; set; }
        public string? DescRef { get; set; }
        public int? CdTribtFe { get; set; }
        public decimal? Mva { get; set; }
        public decimal? Mvaajustado { get; set; }
        public string? Foto { get; set; }
        public string? CstDentro1 { get; set; }
        public string? CstDentro2 { get; set; }
        public string? CstFora1 { get; set; }
        public string? CstFora2 { get; set; }
        public string? CstIpi { get; set; }
        public string? CstPis { get; set; }
        public string? CfoDentro { get; set; }
        public string? CfoFora { get; set; }
        public decimal? PorcSubst { get; set; }
        public decimal? PorcIpi { get; set; }
        public decimal? IcmsDentro { get; set; }

        public decimal? IcmsFora { get; set; }

        
        public string? ExTipi { get; set; }

        public string? CdGenero { get; set; }

        public string? TpItem { get; set; }

        public string? LetraCurvaabc { get; set; }

        public decimal? PorcVendaAPrazo { get; set; }

        public decimal? PorcVendaCd { get; set; }

        public decimal? PorcVendaCc { get; set; }

        public decimal? QtdeMax { get; set; }

        public string? Corredor { get; set; }

        public string? Prateleira { get; set; }

        public string? TxtObs { get; set; }

        
        public string? CdInterno { get; set; }

        public string? StEcf { get; set; }

        public string? CdCsosn { get; set; }


        public string? CodigoBalanca { get; set; }


        public string? Iat { get; set; }


        public string? Ippt { get; set; }


        public string? TipoItemSped { get; set; }


        public decimal? TaxaPis { get; set; }


        public decimal? TaxaIssqn { get; set; }


        public decimal? TaxaCofins { get; set; }

        public string? TotalizadorParcial { get; set; }


        public decimal? VlAVista { get; set; }


        public decimal? VlPrazo { get; set; }


        public decimal? VlCc { get; set; }

        public decimal? VlDeb { get; set; }

        public string? EcfIcmSt { get; set; }
        public short? TpCdBalanca { get; set; }

        public string? CstCofins { get; set; }

        public string? Balanca { get; set; }

        public short? QtDiasVenc { get; set; }

        public decimal? VlTabelaGov { get; set; }

        public decimal? PorcAliqInterna { get; set; }

        public decimal? VlComanda { get; set; }

        public string? ClasseTerapeutica { get; set; }

        public string? RegMs { get; set; }

        public string? CodigoDcb { get; set; }

        public string? DescricaoProduto { get; set; }

        public int? CdAnp { get; set; }
        public decimal? VlMedia { get; set; }

        public decimal? VlPequena { get; set; }

        public string? Transferiu { get; set; }

        public string? Cest { get; set; }

        public decimal? VlCusto { get; set; }

        public decimal? VlAtacado { get; set; }

        public int? IdMarca { get; set; }

        public string? Local { get; set; }

        public string? BandejaGaveta { get; set; }

        public decimal? EntMva { get; set; }

        public decimal? EntPorcSt { get; set; }

        public decimal? EntReducaoBc { get; set; }

        public decimal? EntBcSt { get; set; }

        public decimal? EntIcmsSt { get; set; }

        public decimal? IcmsSubsAliq { get; set; }

        public decimal? IcmsSubsReducao { get; set; }

        public decimal? IcmsSubsReducaoAliq { get; set; }

        public decimal? LucroPor { get; set; }

        public decimal? OperacionalPor { get; set; }

        public decimal? Frete { get; set; }

        public string? NomeImagem { get; set; }

        public string? ControlaEstoque { get; set; }

        public decimal? VlCustoVariavel { get; set; }

        public string? Principal { get; set; }

        public string? Embalagem { get; set; }

        public string? Capacidade { get; set; }

        public decimal? VlCheque { get; set; }

        public decimal? VlCreditoParcelado { get; set; }

        public decimal? VlBoleto { get; set; }

        public decimal? PorcLimiteAvista { get; set; }

        public decimal? PorcLimiteAprazo { get; set; }

        public decimal? PorcLimiteCredito { get; set; }

        public decimal? PorcLimiteDebito { get; set; }

        public decimal? PorcLimiteCreditoparc { get; set; }

        public decimal? PorcLimiteBoleto { get; set; }

        public decimal? PorcLimiteCheque { get; set; }

        public decimal? PorcDesgasteEquipamento { get; set; }

        public decimal? CustoAdicional { get; set; }
        public decimal? PorcMaoObra { get; set; }

        public decimal? MargemLucroAtacado { get; set; }

        public decimal? VlNFiscal { get; set; }

        public decimal? OperacionalNFiscalPor { get; set; }

        public decimal? LucroPorNFiscal { get; set; }

        public int IdEmpresa { get; set; }
        public decimal? PercentualImpostos { get; set; }

        public decimal? PercentualComissao { get; set; }

        public decimal? PercentualCustoFixo { get; set; }

        public decimal? PercentualLucroLiquidoFiscal { get; set; }

        public decimal? IndiceMarkupFiscal { get; set; }

        public int? SectionId { get; set; }

        public int? SectionItemId { get; set; }

        public int? FeaturedId { get; set; }

        public int? Category { get; set; }

        public int? CdProdutoErp { get; set; }
    }

    public class ProdutoEstoqueDto2
    {
        public int CdProduto { get; set; }
        public string NmProduto { get; set; } = null!;

        public decimal QuantMinima { get; set; }

        public string CdBarra { get; set; } = null!;

        public string? CodMargem { get; set; }

        public string? CodEspecie { get; set; }

        public string LancLivro { get; set; } = null!;

        public decimal? PorcVendedor { get; set; }

        public string? CdClassFiscal { get; set; }

        public int CdTribt { get; set; }

        public decimal? PesoBruto { get; set; }

        public decimal? PesoLiquido { get; set; }

        public decimal QtUnitario { get; set; }

        public int CdGrupo { get; set; }

        public int CdRef { get; set; }

        public decimal? QtTotal { get; set; }

        public int? CdSeq { get; set; }

        public string? Suspenso { get; set; }

        public int? CdPlanoEst { get; set; }

        public string? CdUni { get; set; }

        public decimal? PorcSaida { get; set; }

        public DateOnly? DtSuspenso { get; set; }

        public TimeOnly? HrSuspenso { get; set; }

        public DateOnly? DtAtivacao { get; set; }

        public TimeOnly? HrAtivacao { get; set; }

        public DateOnly? DtAtivoLivro { get; set; }

        public DateOnly? DtSuspLivro { get; set; }

        public TimeOnly? HrAtivoLivro { get; set; }

        public TimeOnly? HrSuspLivro { get; set; }

        public string? Ativo { get; set; }
        public string? DescRef { get; set; }
        public int? CdTribtFe { get; set; }
        public decimal? Mva { get; set; }
        public decimal? Mvaajustado { get; set; }
        public string? Foto { get; set; }
        public string? CstDentro1 { get; set; }
        public string? CstDentro2 { get; set; }
        public string? CstFora1 { get; set; }
        public string? CstFora2 { get; set; }
        public string? CstIpi { get; set; }
        public string? CstPis { get; set; }
        public string? CfoDentro { get; set; }
        public string? CfoFora { get; set; }
        public decimal? PorcSubst { get; set; }
        public decimal? PorcIpi { get; set; }
        public decimal? IcmsDentro { get; set; }

        public decimal? IcmsFora { get; set; }


        public string? ExTipi { get; set; }

        public string? CdGenero { get; set; }

        public string? TpItem { get; set; }

        public string? LetraCurvaabc { get; set; }

        public decimal? PorcVendaAPrazo { get; set; }

        public decimal? PorcVendaCd { get; set; }

        public decimal? PorcVendaCc { get; set; }

        public decimal? QtdeMax { get; set; }

        public string? Corredor { get; set; }

        public string? Prateleira { get; set; }

        public string? TxtObs { get; set; }


        public string? CdInterno { get; set; }

        public string? StEcf { get; set; }

        public string? CdCsosn { get; set; }


        public string? CodigoBalanca { get; set; }


        public string? Iat { get; set; }


        public string? Ippt { get; set; }


        public string? TipoItemSped { get; set; }


        public decimal? TaxaPis { get; set; }


        public decimal? TaxaIssqn { get; set; }


        public decimal? TaxaCofins { get; set; }

        public string? TotalizadorParcial { get; set; }


        public decimal? VlAVista { get; set; }


        public decimal? VlPrazo { get; set; }


        public decimal? VlCc { get; set; }

        public decimal? VlDeb { get; set; }

        public string? EcfIcmSt { get; set; }
        public short? TpCdBalanca { get; set; }

        public string? CstCofins { get; set; }

        public string? Balanca { get; set; }

        public short? QtDiasVenc { get; set; }

        public decimal? VlTabelaGov { get; set; }

        public decimal? PorcAliqInterna { get; set; }

        public decimal? VlComanda { get; set; }

        public string? ClasseTerapeutica { get; set; }

        public string? RegMs { get; set; }

        public string? CodigoDcb { get; set; }

        public string? DescricaoProduto { get; set; }

        public int? CdAnp { get; set; }
        public decimal? VlMedia { get; set; }

        public decimal? VlPequena { get; set; }

        public string? Transferiu { get; set; }

        public string? Cest { get; set; }

        public decimal? VlCusto { get; set; }

        public decimal? VlAtacado { get; set; }

        public int? IdMarca { get; set; }

        public string? Local { get; set; }

        public string? BandejaGaveta { get; set; }

        public decimal? EntMva { get; set; }

        public decimal? EntPorcSt { get; set; }

        public decimal? EntReducaoBc { get; set; }

        public decimal? EntBcSt { get; set; }

        public decimal? EntIcmsSt { get; set; }

        public decimal? IcmsSubsAliq { get; set; }

        public decimal? IcmsSubsReducao { get; set; }

        public decimal? IcmsSubsReducaoAliq { get; set; }

        public decimal? LucroPor { get; set; }

        public decimal? OperacionalPor { get; set; }

        public decimal? Frete { get; set; }

        public string? NomeImagem { get; set; }

        public string? ControlaEstoque { get; set; }

        public decimal? VlCustoVariavel { get; set; }

        public string? Principal { get; set; }

        public string? Embalagem { get; set; }

        public string? Capacidade { get; set; }

        public decimal? VlCheque { get; set; }

        public decimal? VlCreditoParcelado { get; set; }

        public decimal? VlBoleto { get; set; }

        public decimal? PorcLimiteAvista { get; set; }

        public decimal? PorcLimiteAprazo { get; set; }

        public decimal? PorcLimiteCredito { get; set; }

        public decimal? PorcLimiteDebito { get; set; }

        public decimal? PorcLimiteCreditoparc { get; set; }

        public decimal? PorcLimiteBoleto { get; set; }

        public decimal? PorcLimiteCheque { get; set; }

        public decimal? PorcDesgasteEquipamento { get; set; }

        public decimal? CustoAdicional { get; set; }
        public decimal? PorcMaoObra { get; set; }

        public decimal? MargemLucroAtacado { get; set; }

        public decimal? VlNFiscal { get; set; }

        public decimal? OperacionalNFiscalPor { get; set; }

        public decimal? LucroPorNFiscal { get; set; }

        public int IdEmpresa { get; set; }
        public decimal? PercentualImpostos { get; set; }

        public decimal? PercentualComissao { get; set; }

        public decimal? PercentualCustoFixo { get; set; }

        public decimal? PercentualLucroLiquidoFiscal { get; set; }

        public decimal? IndiceMarkupFiscal { get; set; }

        public int? SectionId { get; set; }

        public int? SectionItemId { get; set; }

        public int? FeaturedId { get; set; }

        public int? Category { get; set; }

        public int? CdProdutoErp { get; set; }
    }
}
