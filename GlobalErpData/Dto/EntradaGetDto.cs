using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class EntradaGetDto
    {
        public int Nr { get; set; }

        public int Unity { get; set; }
        public DateOnly Data { get; set; }

        public int CdForn { get; set; }

        public int CdEmpresa { get; set; }

        public string NrNf { get; set; }

        public DateOnly? DtSaida { get; set; }

        public TimeOnly? HrSaida { get; set; }

        public TimeOnly? HrChegada { get; set; }

        public string CdCfop { get; set; }

        public decimal? VlFrete { get; set; }

        public string Transferiu { get; set; }
        public string? NmEmpresa { get; set; }

        public int? NrPedidoCompra { get; set; }

        public decimal? VlOutras { get; set; }

        public decimal? VlSeguro { get; set; }

        public decimal? VlDescontoNf { get; set; }

        public decimal? VlAcrescimoNf { get; set; }

        public string TxtObs { get; set; }

        public int CdGrupoEstoque { get; set; }

        public string TpPagt { get; set; }

        public int? Transp { get; set; }

        public string CdTipoNf { get; set; }

        public string ModeloNf { get; set; }

        public string SerieNf { get; set; }

        public string EmitendeNf { get; set; }

        public string CdSituacao { get; set; }

        public string CdChaveNfe { get; set; }

        public int? TpFrete { get; set; }

        public string TpEntrada { get; set; }

        public string XmlNf { get; set; }

        public DateOnly? DtEmissao { get; set; }

        public decimal? VlGuiaSt { get; set; }

        public decimal? PorcDifAlig { get; set; }

        public decimal? VlStNf { get; set; }

        public int? CdClienteDevolucao { get; set; }

        public string RespRetIcmsSt { get; set; }

        public string CodModDocArrec { get; set; }

        public string NumDocArrec { get; set; }

        public string TPag { get; set; }

        public decimal? VIcmsDeson { get; set; }

        public decimal? IcmstotVBc { get; set; }

        public decimal? IcmstotVIcms { get; set; }

        public decimal? IcmstotVIcmsDeson { get; set; }

        public decimal? IcmstotVFcp { get; set; }

        public decimal? IcmstotVBcst { get; set; }

        public decimal? IcmstotVSt { get; set; }

        public decimal? IcmstotVFcpst { get; set; }

        public decimal? IcmstotVFcpstRet { get; set; }

        public decimal? IcmstotVProd { get; set; }

        public decimal? IcmstotVFrete { get; set; }

        public decimal? IcmstotVSeg { get; set; }

        public decimal? IcmstotVDesc { get; set; }

        public decimal? IcmstotVIi { get; set; }

        public decimal? IcmstotVIpi { get; set; }

        public decimal? IcmstotVIpiDevol { get; set; }

        public decimal? IcmstotVPis { get; set; }

        public decimal? IcmstotVCofins { get; set; }

        public decimal? IcmstotVOutro { get; set; }

        public decimal? IcmstotVNf { get; set; }

        public string NmPlano { get; set; }

        public string NmForn { get; set; }

        public double? ValorTotalNfe { get; set; }

        public double? SubTotal { get; set; }

        public double? ValorTotalDesconto { get; set; }

        public double? ValorTotalProdutos { get; set; }
    }
}