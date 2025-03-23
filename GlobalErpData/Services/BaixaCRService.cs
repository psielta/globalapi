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
    public class BaixaCRService
    {
        private readonly GlobalErpFiscalBaseContext _context;
        private readonly IQueryRepository<ContasAReceber, int, ContasAReceberDto> repCr;
        private readonly IQueryRepository<LivroCaixa, long, LivroCaixaDto> repLivro;
        private readonly IQueryRepository<PagtosParciaisCr, int, PagtosParciaisCrDto> repPgt;

        public BaixaCRService(GlobalErpFiscalBaseContext context,
            IQueryRepository<ContasAReceber, int, ContasAReceberDto> repCr,
            IQueryRepository<PagtosParciaisCr, int, PagtosParciaisCrDto> repPgt,
            IQueryRepository<LivroCaixa, long, LivroCaixaDto> repLivro)
        {
            _context = context;
            this.repCr = repCr;
            this.repPgt = repPgt;
            this.repLivro = repLivro;
        }

        /// <summary>
        /// Método principal para processar o pagamento de contas a receber.
        /// </summary>
        public async Task<List<ContasAReceber>> Core(ListCRDto listCRDto)
        {
            List<ContasAReceber> contasARecebers = null;
            // Passo 1: Recupere as contas selecionadas, ordenadas por data de vencimento.
            contasARecebers = await _context.ContasARecebers
                .Where(c => listCRDto.Contas.Contains(c.NrConta) && c.CdEmpresa == listCRDto.CdEmpresa)
                .OrderBy(c => c.DtVencimento)
                .ToListAsync();

            if (contasARecebers == null || !contasARecebers.Any())
            {
                throw new Exception("Nenhuma conta a receber encontrada.");
            }

            // Passo 2: Verifique se há contas que já foram recebidas.
            bool containsContasAReceberJaRecebido = contasARecebers.Any(c => c.Recebeu == "S");
            if (containsContasAReceberJaRecebido)
            {
                throw new Exception("Uma ou mais contas a receber já foram recebidas.");
            }

            // Passo 3: Calcular o valor total das contas selecionadas.
            decimal valorTotalContas = contasARecebers.Sum(c => c.VlTotal);

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
            foreach (var cr in contasARecebers)
            {
                decimal proporcao = cr.VlTotal / valorTotalContas;

                decimal descontoProporcional = valorDescontoGlobal * proporcao;
                decimal acrescimoProporcional = valorAcrescimoGlobal * proporcao;

                cr.VlDesconto = (cr.VlDesconto ?? 0) + descontoProporcional;
                cr.VlAcrescimo = (cr.VlAcrescimo) + acrescimoProporcional;

                cr.VlTotal = cr.VlConta - (cr.VlDesconto ?? 0) + cr.VlAcrescimo;

                // Atualiza o cache de ContasAReceber.
                ((ContasAReceberRepository)repCr).UpdateCache(cr.NrConta, cr);
            }

            // Passo 7: Aplique o valor do pagamento às contas.
            decimal valorRestante = valorPago;

            foreach (var cr in contasARecebers)
            {
                if (valorRestante <= 0)
                {
                    break;
                }

                decimal valorConta = cr.VlTotal - (cr.VlPago ?? 0);

                if (valorRestante >= valorConta)
                {
                    // Pagamento total.
                    await BaixaTotal(cr, valorConta,listCRDto);
                    valorRestante -= valorConta;
                }
                else
                {
                    // Pagamento parcial.
                    await BaixaParcial(cr, valorRestante, listCRDto);
                    valorRestante = 0;
                }
            }

            // Passo 8: Salve as alterações no banco de dados.
            await _context.SaveChangesAsync();
            return contasARecebers;
        }

        /// <summary>
        /// Processa um pagamento total para a conta especificada.
        /// </summary>
        private async Task BaixaTotal(ContasAReceber cr, decimal valorPago, ListCRDto listCRDto)
        {
            // Atualiza ContasAReceber.
            cr.VlPago = cr.VlTotal;
            cr.DtPagamento = listCRDto.DataPagamento;
            cr.Recebeu = "S";

            // Atualiza o cache de ContasAReceber.
            ((ContasAReceberRepository)repCr).UpdateCache(cr.NrConta, cr);

            // Cria entrada no LivroCaixa.
            LivroCaixa livroCaixa = new LivroCaixa
            {
                DtLanc = DateTime.UtcNow,
                CdEmpresa = cr.CdEmpresa,
                Unity = cr.Unity,
                CdHistorico = cr.CdHistoricoCaixa,
                VlLancamento = cr.VlTotal,
                NrCr = cr.NrConta,
                TxtObs = cr.TxtObs,
                NrConta = listCRDto.NrContaCaixa,
                CdPlano = cr.CdPlanoCaixa,
            };
            _context.LivroCaixas.Add(livroCaixa);
            await _context.SaveChangesAsync(); // Salva para gerar NrLanc

            // Atualiza o cache de LivroCaixa.
            ((LivroCaixaRepository)repLivro).UpdateCache(livroCaixa.NrLanc, livroCaixa);
        }

        /// <summary>
        /// Processa um pagamento parcial para a conta especificada.
        /// </summary>
        private async Task BaixaParcial(ContasAReceber cr, decimal valorPago, ListCRDto listCRDto)
        {
            // Atualiza ContasAReceber.
            cr.VlPago = (cr.VlPago ?? 0) + valorPago;

            // Atualiza o cache de ContasAReceber.
            ((ContasAReceberRepository)repCr).UpdateCache(cr.NrConta, cr);

            // Cria entrada em PagtosParciaisCr.
            PagtosParciaisCr pagamentoParcial = new PagtosParciaisCr
            {
                NrConta = cr.NrConta,
                DtPagto = listCRDto.DataPagamento,
                ValorPago = valorPago,
                ValorRestante = cr.VlTotal - cr.VlPago,
                Acrescimo = cr.VlAcrescimo,
                Desconto = cr.VlDesconto,
                CdEmpresa = cr.CdEmpresa
            };
            _context.PagtosParciaisCrs.Add(pagamentoParcial);
            await _context.SaveChangesAsync(); // Salva para gerar Id

            // Atualiza o cache de PagtosParciaisCr.
            ((PagtosParciaisCrRepository)repPgt).UpdateCache(pagamentoParcial.Id, pagamentoParcial);

            // Cria entrada no LivroCaixa.
            LivroCaixa livroCaixa = new LivroCaixa
            {
                DtLanc = DateTime.UtcNow,
                CdEmpresa = cr.CdEmpresa,
                CdHistorico = cr.CdHistoricoCaixa,
                Unity = cr.Unity,
                VlLancamento = valorPago,
                NrCr = cr.NrConta,
                TxtObs = cr.TxtObs,
                NrConta = listCRDto.NrContaCaixa,
                CdPlano = cr.CdPlanoCaixa,
            };
            _context.LivroCaixas.Add(livroCaixa);
            await _context.SaveChangesAsync(); // Salva para gerar NrLanc

            // Atualiza o cache de LivroCaixa.
            ((LivroCaixaRepository)repLivro).UpdateCache(livroCaixa.NrLanc, livroCaixa);

            // Verifica se a conta foi totalmente paga após o pagamento parcial.
            if (cr.VlPago >= cr.VlTotal)
            {
                cr.Recebeu = "S";
                cr.DtPagamento = listCRDto.DataPagamento;

                // Atualiza o cache de ContasAReceber novamente.
                ((ContasAReceberRepository)repCr).UpdateCache(cr.NrConta, cr);
            }
        }

        /// <summary>
        /// Reverte o pagamento para uma conta específica.
        /// </summary>
        public async Task Revert(int nrConta)
        {
            var cr = await _context.ContasARecebers
                .FirstOrDefaultAsync(c => c.NrConta == nrConta);

            if (cr == null)
            {
                throw new Exception("Conta a receber não encontrada.");
            }

            if (cr.VlPago == null || cr.VlPago == 0)
            {
                throw new Exception("Esta conta não possui pagamentos para reverter.");
            }

            // Remove entradas de PagtosParciaisCr.
            var pagamentosParciais = await _context.PagtosParciaisCrs
                .Where(p => p.NrConta == nrConta)
                .ToListAsync();

            // Remover do cache de PagtosParciaisCr.
            var repPgtConcrete = (PagtosParciaisCrRepository)repPgt;
            foreach (var pagamento in pagamentosParciais)
            {
                _context.PagtosParciaisCrs.Remove(pagamento);
                // Remover do cache
                repPgtConcrete.RemoveFromCache(pagamento.Id);
            }

            // Remove entradas de LivroCaixa.
            var livroCaixas = await _context.LivroCaixas
                .Where(lc => lc.NrCr == nrConta)
                .ToListAsync();

            // Remover do cache de LivroCaixa.
            var repLivroConcrete = (LivroCaixaRepository)repLivro;
            foreach (var livro in livroCaixas)
            {
                _context.LivroCaixas.Remove(livro);
                // Remover do cache
                repLivroConcrete.RemoveFromCache(livro.NrLanc);
            }

            // Reseta ContasAReceber.
            cr.VlPago = 0;
            cr.Recebeu = "N";
            cr.DtPagamento = null;

            // Reseta VlDesconto e VlAcrescimo.
            cr.VlDesconto = 0;
            cr.VlAcrescimo = 0;
            cr.VlTotal = cr.VlConta;

            // Atualiza o cache de ContasAReceber.
            ((ContasAReceberRepository)repCr).UpdateCache(cr.NrConta, cr);

            // Salva as alterações.
            await _context.SaveChangesAsync();
        }
    }
}
