using GlobalErpData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Services
{
    public class EntradaCalculationService
    {
        public void CalculateTotals(Entrada entrada)
        {
            if (entrada.ProdutoEntrada == null || !entrada.ProdutoEntrada.Any())
            {
                entrada.SubTotal = 0;
                entrada.ValorTotalDesconto = ((double)(entrada.VlDescontoNf ?? 0));
                entrada.ValorTotalProdutos = 0;
                entrada.ValorTotalNfe = entrada.ValorTotalProdutos
                                    + (double)(entrada.VlFrete ?? 0)
                                    + (double)(entrada.VlSeguro ?? 0)
                                    + (double)(entrada.VlAcrescimoNf ?? 0)
                                    - (double)(entrada.VlDescontoNf ?? 0);
                return;
            }

            entrada.SubTotal = entrada.ProdutoEntrada.Sum(pe => (double)(pe.Quant * pe.VlUnitario));
            entrada.ValorTotalDesconto =
                ((double)(entrada.VlDescontoNf ?? 0)) +
                entrada.ProdutoEntrada.Sum(pe => (double)(pe.VlOutras ?? 0));
            entrada.ValorTotalProdutos = entrada.ProdutoEntrada.Sum(pe => (double)(pe.Quant * pe.VlUnitario - (pe.VlOutras ?? 0)));

            entrada.ValorTotalNfe = entrada.ValorTotalProdutos
                                    + (double)(entrada.VlFrete ?? 0)
                                    + (double)(entrada.VlSeguro ?? 0)
                                    + entrada.ProdutoEntrada.Sum(pe => (double)((pe.VlDespAcess ?? 0) + (pe.VlIcmsSt ?? 0) + pe.VlIpi + (pe.FcpValor ?? 0)) - (double)(pe.VIcmsDeson ?? 0))
                                    + (double)(entrada.VlAcrescimoNf ?? 0)
                                    - (double)(entrada.VlDescontoNf ?? 0);
            double diferencaModularTotal = Math.Abs((entrada.ValorTotalNfe ?? 0) - ((double)(entrada.IcmstotVNf ?? 0)));
            double diferencaModularIcmsDeson = Math.Abs(diferencaModularTotal - ((double)(entrada.IcmstotVIcmsDeson ?? 0)));
            if (entrada.IcmstotVNf != null && entrada.IcmstotVNf != 0 &&
                (
                ((entrada.ValorTotalNfe ?? 0) < (double)(entrada.IcmstotVNf ?? 0))
                && diferencaModularIcmsDeson < 0.05
                ))
            {
                entrada.ValorTotalNfe = (double)(entrada.IcmstotVNf ?? 0);
            }
        }
    }
}
