using GlobalErpData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Services
{
    public class SaidaCalculationService
    {
        public void CalculateTotals(Saida saida)
        {
            var fretes = saida.Fretes;
            decimal valorFrete = 0;
            if (fretes != null && fretes.Any())
            {
                valorFrete = fretes.Sum(f => f.VlFrete);
            }

            if (saida.ProdutoSaida == null || !saida.ProdutoSaida.Any())
            {
                saida.SubTotal = 0;
                saida.ValorTotalDesconto = ((double)(saida.VlDescGlobal ?? 0));
                saida.ValorTotalProdutos = 0;
                saida.ValorTotalNfe = saida.ValorTotalProdutos
                                    + (double)(saida.VlOutro ?? 0)
                                    + (double)(saida.VlSeguro ?? 0)
                                    + (double)(valorFrete)
                                    - (double)(saida.VlDescGlobal ?? 0);
                return;
            }

            saida.SubTotal = saida.ProdutoSaida.Sum(pe => (double)(pe.Quant * pe.VlVenda));
            saida.ValorTotalDesconto =
                ((double)(saida.VlDescGlobal ?? 0)) +
                saida.ProdutoSaida.Sum(pe => (double)(pe.Desconto));
            saida.ValorTotalProdutos = saida.ProdutoSaida.Sum(pe => (double)(pe.Quant * pe.VlVenda - (pe.Desconto)));

            saida.ValorTotalNfe = saida.ValorTotalProdutos
                                    + (double)(valorFrete)
                                    + (double)(saida.VlSeguro ?? 0)
                                    + (double)(saida.VlOutro ?? 0)
                                    + saida.ProdutoSaida.Sum(pe => (double)((pe.VlIpi ?? 0) + (pe.VlSt ?? 0)))
                                    - (double)(saida.VlDescGlobal ?? 0);
        }
    }
}
