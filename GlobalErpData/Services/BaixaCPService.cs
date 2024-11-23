using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalErpData.Repository.PagedRepositories;
using GlobalLib.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalErpData.Services
{
    public class BaixaCPService
    {
        private readonly GlobalErpFiscalBaseContext _context;
        private readonly IQueryRepository<ContasAPagar, int, ContasAPagarDto> repCp;
        private readonly IQueryRepository<LivroCaixa, long, LivroCaixaDto> repLivro;
        private readonly IQueryRepository<PagtosParciaisCp, int, PagtosParciaisCpDto> repPgt;

        public BaixaCPService(GlobalErpFiscalBaseContext context,
            IQueryRepository<ContasAPagar, int, ContasAPagarDto> repCp,
            IQueryRepository<PagtosParciaisCp, int, PagtosParciaisCpDto> repPgt,
            IQueryRepository<LivroCaixa, long, LivroCaixaDto> repLivro)
        {
            _context = context;
            this.repCp = repCp;
            this.repPgt = repPgt;
            this.repLivro = repLivro;
        }

        /// <summary>
        /// Método principal para processar o pagamento de contas a pagar.
        /// </summary>
        public async Task<List<ContasAPagar>> Core(ListCRDto listCRDto)
        {
            List<ContasAPagar> contasAPagars = null;
            // Passo 1: Recupere as contas selecionadas, ordenadas por data de vencimento.
            contasAPagars = await _context.ContasAPagars
                .Where(c => listCRDto.Contas.Contains(c.Id) && c.CdEmpresa == listCRDto.CdEmpresa)
                .OrderBy(c => c.DtVencimento)
                .ToListAsync();

            if (contasAPagars == null || !contasAPagars.Any())
            {
                throw new Exception("Nenhuma conta a pagar encontrada.");
            }

            // Passo 2: Verifique se há contas que já foram pagas.
            bool containsContasAPagarJaPagas = contasAPagars.Any(c => c.Pagou == "S");
            if (containsContasAPagarJaPagas)
            {
                throw new Exception("Uma ou mais contas a pagar já foram pagas.");
            }

            // Passo 3: Calcular o valor total das contas selecionadas.
            decimal valorTotalContas = contasAPagars.Sum(c => c.VlTotal);

            // Passo 4: Obtenha o valor total do pagamento, desconto global e acréscimo.
            decimal valorPago = listCRDto.ValorPago;
            decimal valorDescontoGlobal = listCRDto.ValorDesconto;
            decimal valorAcrescimoGlobal = listCRDto.ValorAcrescimo;

            // Passo 5: Ajuste o valor total considerando o desconto global e o acréscimo.
            decimal valorTotalComDescontoAcrescimo = valorTotalContas - valorDescontoGlobal + valorAcrescimoGlobal;

            if (valorPago > valorTotalComDescontoAcrescimo)
            {
                throw new Exception("Valor pago é maior que o valor total das contas com desconto e acréscimo.");
            }

            // Passo 6: Distribua o desconto e o acréscimo global proporcionalmente entre as contas.
            foreach (var cp in contasAPagars)
            {
                decimal proporcao = cp.VlTotal / valorTotalContas;

                decimal descontoProporcional = valorDescontoGlobal * proporcao;
                decimal acrescimoProporcional = valorAcrescimoGlobal * proporcao;

                cp.VlDesconto = cp.VlDesconto + descontoProporcional;
                cp.VlAcrescimo = (cp.VlAcrescimo ?? 0) + acrescimoProporcional;

                cp.VlTotal = cp.VlCp - cp.VlDesconto + (cp.VlAcrescimo ?? 0);

                // Atualiza o cache de ContasAPagar.
                ((ContasAPagarRepository)repCp).UpdateCache(cp.Id, cp);
            }

            // Passo 7: Aplique o valor do pagamento às contas.
            decimal valorRestante = valorPago;

            foreach (var cp in contasAPagars)
            {
                if (valorRestante <= 0)
                {
                    break;
                }

                decimal valorConta = cp.VlTotal - (cp.VlPagoFinal ?? 0);

                if (valorRestante >= valorConta)
                {
                    // Pagamento total.
                    await BaixaTotal(cp, valorConta, listCRDto);
                    valorRestante -= valorConta;
                }
                else
                {
                    // Pagamento parcial.
                    await BaixaParcial(cp, valorRestante, listCRDto);
                    valorRestante = 0;
                }
            }

            // Passo 8: Salve as alterações no banco de dados.
            await _context.SaveChangesAsync();
            return contasAPagars;
        }

        /// <summary>
        /// Processa um pagamento total para a conta especificada.
        /// </summary>
        private async Task BaixaTotal(ContasAPagar cp, decimal valorPago, ListCRDto listCRDto)
        {
            // Atualiza ContasAPagar.
            cp.VlPagoFinal = cp.VlTotal;
            cp.DtPagou = listCRDto.DataPagamento;
            cp.Pagou = "S";

            // Atualiza o cache de ContasAPagar.
            ((ContasAPagarRepository)repCp).UpdateCache(cp.Id, cp);

            // Cria entrada no LivroCaixa.
            LivroCaixa livroCaixa = new LivroCaixa
            {
                DtLanc = DateTime.Now,
                CdEmpresa = cp.CdEmpresa,
                CdHistorico = cp.CdHistoricoCaixa,
                VlLancamento = cp.VlTotal,// - cp.VlTotal, // Negativo, pois é uma despesa.
                NrCp = cp.Id,
                TxtObs = cp.TxtObs,
                NrConta = listCRDto.NrContaCaixa,
                CdPlano = cp.CdPlanoCaixa,
            };
            _context.LivroCaixas.Add(livroCaixa);
            await _context.SaveChangesAsync(); // Salva para gerar NrLanc

            // Atualiza o cache de LivroCaixa.
            ((LivroCaixaRepository)repLivro).UpdateCache(livroCaixa.NrLanc, livroCaixa);
        }

        /// <summary>
        /// Processa um pagamento parcial para a conta especificada.
        /// </summary>
        private async Task BaixaParcial(ContasAPagar cp, decimal valorPago, ListCRDto listCRDto)
        {
            // Atualiza ContasAPagar.
            cp.VlPagoFinal = (cp.VlPagoFinal ?? 0) + valorPago;

            // Atualiza o cache de ContasAPagar.
            ((ContasAPagarRepository)repCp).UpdateCache(cp.Id, cp);

            // Cria entrada em PagtosParciaisCp.
            PagtosParciaisCp pagamentoParcial = new PagtosParciaisCp
            {
                IdContasPagar = cp.Id,
                DtPagto = listCRDto.DataPagamento,
                ValorPago = valorPago,
                ValorRestante = cp.VlTotal - cp.VlPagoFinal,
                Acrescimo = cp.VlAcrescimo ?? 0,
                Desconto = cp.VlDesconto,
                CdEmpresa = cp.CdEmpresa
            };
            _context.PagtosParciaisCps.Add(pagamentoParcial);
            await _context.SaveChangesAsync(); // Salva para gerar Id

            // Atualiza o cache de PagtosParciaisCp.
            ((PagtosParciaisCpRepository)repPgt).UpdateCache(pagamentoParcial.Id, pagamentoParcial);

            // Cria entrada no LivroCaixa.
            LivroCaixa livroCaixa = new LivroCaixa
            {
                DtLanc = DateTime.Now,
                CdEmpresa = cp.CdEmpresa,
                CdHistorico = cp.CdHistoricoCaixa,
                VlLancamento = valorPago,// -valorPago, // Negativo, pois é uma despesa.
                NrCp = cp.Id,
                TxtObs = cp.TxtObs,
                NrConta = listCRDto.NrContaCaixa,
                CdPlano = cp.CdPlanoCaixa,
            };
            _context.LivroCaixas.Add(livroCaixa);
            await _context.SaveChangesAsync(); // Salva para gerar NrLanc

            // Atualiza o cache de LivroCaixa.
            ((LivroCaixaRepository)repLivro).UpdateCache(livroCaixa.NrLanc, livroCaixa);

            // Verifica se a conta foi totalmente paga após o pagamento parcial.
            if (cp.VlPagoFinal >= cp.VlTotal)
            {
                cp.Pagou = "S";
                cp.DtPagou = listCRDto.DataPagamento;

                // Atualiza o cache de ContasAPagar novamente.
                ((ContasAPagarRepository)repCp).UpdateCache(cp.Id, cp);
            }
        }

        /// <summary>
        /// Reverte o pagamento para uma conta específica.
        /// </summary>
        public async Task Revert(int id)
        {
            var cp = await _context.ContasAPagars
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cp == null)
            {
                throw new Exception("Conta a pagar não encontrada.");
            }

            if (cp.VlPagoFinal == null || cp.VlPagoFinal == 0)
            {
                throw new Exception("Esta conta não possui pagamentos para reverter.");
            }

            // Remove entradas de PagtosParciaisCp.
            var pagamentosParciais = await _context.PagtosParciaisCps
                .Where(p => p.IdContasPagar == id)
                .ToListAsync();

            // Remover do cache de PagtosParciaisCp.
            var repPgtConcrete = (PagtosParciaisCpRepository)repPgt;
            foreach (var pagamento in pagamentosParciais)
            {
                _context.PagtosParciaisCps.Remove(pagamento);
                // Remover do cache
                repPgtConcrete.RemoveFromCache(pagamento.Id);
            }

            // Remove entradas de LivroCaixa.
            var livroCaixas = await _context.LivroCaixas
                .Where(lc => lc.NrCp == id)
                .ToListAsync();

            // Remover do cache de LivroCaixa.
            var repLivroConcrete = (LivroCaixaRepository)repLivro;
            foreach (var livro in livroCaixas)
            {
                _context.LivroCaixas.Remove(livro);
                // Remover do cache
                repLivroConcrete.RemoveFromCache(livro.NrLanc);
            }

            // Reseta ContasAPagar.
            cp.VlPagoFinal = 0;
            cp.Pagou = "N";
            cp.DtPagou = null;

            // Reseta VlDesconto e VlAcrescimo.
            cp.VlDesconto = 0;
            cp.VlAcrescimo = 0;
            cp.VlTotal = cp.VlCp;

            // Atualiza o cache de ContasAPagar.
            ((ContasAPagarRepository)repCp).UpdateCache(cp.Id, cp);

            // Salva as alterações.
            await _context.SaveChangesAsync();
        }
    }
}
