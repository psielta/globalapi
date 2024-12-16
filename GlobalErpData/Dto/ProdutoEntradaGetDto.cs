using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class ProdutoEntradaGetDto
    {
        public int Nr { get; set; }

        public int NrEntrada { get; set; }

        public int? CdEmpresa { get; set; }

        public int CdProduto { get; set; }

        public string? CdBarra { get; set; }

        public decimal Quant { get; set; }

        public decimal VlUnitario { get; set; }

        public decimal? BIcms { get; set; }

        public decimal? PorcIcms { get; set; }

        public decimal VlIcms { get; set; }

        public int CdPlano { get; set; }

        public string? Lote { get; set; }

        public DateOnly? DtValidade { get; set; }

        public decimal? BaseIpi { get; set; }

        public decimal? PorcIpi { get; set; }

        public decimal VlIpi { get; set; }

        public decimal? VlOutras { get; set; }

        public string? Transferiu { get; set; }

        public string? Unidade { get; set; }

        public string? CdClassFiscal { get; set; }

        public string? CodSubTrib { get; set; }

        public string? CdCfop { get; set; }

        public short? NrItem { get; set; }

        public decimal? VlPis { get; set; }

        public decimal? VlConfins { get; set; }

        public string? Cst { get; set; }

        public decimal? VlIcmsSt { get; set; }

        public decimal? QtTotal { get; set; }

        public decimal? VlBaseSt { get; set; }

        public decimal? PorcSt { get; set; }

        public decimal? PorcPis { get; set; }

        public decimal? PorcConfins { get; set; }

        public decimal? VlBasePis { get; set; }

        public decimal? VlBaseConfins { get; set; }

        public string? CstPis { get; set; }

        public string? CstConfins { get; set; }

        public string? CdTipSt { get; set; }

        public string? CdContSocialApPis { get; set; }

        public string? CdContSocialApCofins { get; set; }

        public decimal? VlBasePisSt { get; set; }

        public decimal? VlPisSt { get; set; }

        public decimal? PorcPisSt { get; set; }

        public decimal? VlBaseCofinsSt { get; set; }

        public decimal? VlCofinsSt { get; set; }

        public decimal? PorcCofinsSt { get; set; }

        public string? CstIpi { get; set; }

        public string? CdEnquadraIpi { get; set; }

        public decimal? FreteProduto { get; set; }

        public string? MovEstoque { get; set; }

        public decimal? VlDespAcess { get; set; }

        public decimal? FcpBase { get; set; }

        public decimal? FcpPorc { get; set; }

        public decimal? FcpValor { get; set; }

        public decimal? ImpBaseStRet { get; set; }

        public decimal? ImpBaseIcmsStRet { get; set; }

        public decimal? ImpPst { get; set; }

        public decimal? ImpIcmsPropSubs { get; set; }

        public decimal? FreteRateio { get; set; }

        public int? TpOperacaoVeic { get; set; }

        public string? ChasiVeic { get; set; }

        public string? CorVeic { get; set; }

        public string? DescCorVeic { get; set; }

        public string? PotenciaMotorVeic { get; set; }

        public string? CilindradasVeic { get; set; }

        public decimal? PesoLiquidoVeic { get; set; }

        public decimal? PesoBrutoVeic { get; set; }

        public string? SerialVeic { get; set; }

        public string? TpCombustVeic { get; set; }

        public string? NrMotorVeic { get; set; }

        public decimal? CapcMaxTracVeic { get; set; }

        public string? DistEixosVeic { get; set; }

        public string? AnoVeic { get; set; }

        public string? AnoFabVeic { get; set; }

        public string? TpPinturaVeic { get; set; }

        public string? TpVeic { get; set; }

        public string? EspecVeic { get; set; }

        public string? IdVinVeic { get; set; }

        public string? CondVeic { get; set; }

        public string? IdMarcaVeic { get; set; }

        public string? IdCorVeic { get; set; }

        public string? CapcMaxLotVeic { get; set; }

        public string? RestricaoVeic { get; set; }

        public decimal? VIcmsDeson { get; set; }

        public string? Tamanho { get; set; }

        public string? Cor { get; set; }

        public string? Genero { get; set; }

        public decimal? VlSeguro { get; set; }

        public decimal? CustoAtualizado { get; set; }

        public decimal? PrecoAtualizado { get; set; }

        public string? NmProduto { get; set; }

        public decimal? VlCustoAntigo { get; set; }

        public decimal? VlPrecoAntigo { get; set; }

        public decimal? PercentualImpostos { get; set; }

        public decimal? PercentualComissao { get; set; }

        public decimal? PercentualCustoFixo { get; set; }

        public decimal? PercentualLucrroLiquidoFiscal { get; set; }

        public decimal? IndiceMarkupFiscal { get; set; }

        public decimal? LucroPor { get; set; }

        public decimal? VlTotal { get; set; }
    }
}