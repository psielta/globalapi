using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Services
{
    public class ProdutoSaidumService
    {
        private readonly GlobalErpFiscalBaseContext _context;
        public ProdutoSaidumService(GlobalErpFiscalBaseContext context)
        {
            _context = context;
        }
        public async Task InserirProdutoSaidum(
            InsercaoProdutoSaidumEanDto dto,
            ProdutoSaidumDto ProdutoSaidumDto, ProdutoEstoque produto, Cliente cliente)
        {
            ProdutoSaidumDto.NrSaida = dto.NrSaida;
            ProdutoSaidumDto.CdEmpresa = dto.CdEmpresa ?? 0;
            ProdutoSaidumDto.CdProduto = produto.CdProduto;
            ProdutoSaidumDto.CdBarra = produto.CdBarra;
            ProdutoSaidumDto.NmProduto = produto.NmProduto;
            ProdutoSaidumDto.Lote = "-1";
            ProdutoSaidumDto.Desconto = 0;
            ProdutoSaidumDto.DtValidade = DateUtils.DateTimeToDateOnly(DateTime.Now);
            ProdutoSaidumDto.Quant = dto.Quant;
            ProdutoSaidumDto.CdPlano = dto.CdPlano;
            ProdutoSaidumDto.VlVenda = Math.Round(produto.VlAVista ?? 0, 4);
            ProdutoSaidumDto.VlTotal = Math.Round(ProdutoSaidumDto.Quant * ProdutoSaidumDto.VlVenda, 4);
            ProdutoSaidumDto.Cest = produto.Cest;
            ProdutoSaidumDto.CstPis = produto.CstPis;
            ProdutoSaidumDto.CstCofins = produto.CstCofins;
            ProdutoSaidumDto.PorcPis = produto.TaxaPis;
            ProdutoSaidumDto.PorcCofins = produto.TaxaCofins;
            ProdutoSaidumDto.Un = produto.CdUni;
            ProdutoSaidumDto.Ncm = produto.CdClassFiscal;
            ProdutoSaidumDto.CdCsosn = produto.CdCsosn;

            if (cliente.CdCidadeNavigation == null)
            {
                Cidade? cidade = await _context.Cidades.FindAsync(cliente.CdCidade);
                if (cidade == null)
                {
                    throw new Exception("Cidade não encontrada");
                }
                cliente.CdCidadeNavigation = cidade;
            }

            Empresa? empresa = await _context.Empresas
                .Include(e => e.CdCidadeNavigation)
                .Where(e => e.CdEmpresa == dto.CdEmpresa).FirstOrDefaultAsync();
            if (empresa == null)
            {
                throw new Exception("Empresa não encontrada");
            }

            if (cliente.CdCidadeNavigation.Uf.Equals(empresa.CdCidadeNavigation.Uf))
            {
                ProdutoSaidumDto.Cfop = produto.CfoDentro;
                ProdutoSaidumDto.Cst = produto.CstDentro1;
                ProdutoSaidumDto.PocIcms = produto.IcmsDentro;
            }
            else
            {
                ProdutoSaidumDto.Cfop = produto.CfoFora;
                ProdutoSaidumDto.Cst = produto.CstFora1;
                ProdutoSaidumDto.PocIcms = produto.IcmsFora;
            }
        }

        public async Task RealizarCalculoImpostoSaida(ProdutoSaidum produtoSaidum)
        {
            string ncm = produtoSaidum.Ncm ?? "";
            Ibpt? ibpt = await _context.Ibpts.Where(i => i.Codigo.Equals(ncm)).FirstOrDefaultAsync();
            if (ibpt == null)
            {
                produtoSaidum.PorcIbpt = 0;
                produtoSaidum.VlAproxImposto = 0;
            }
            else
            {
                produtoSaidum.PorcIbpt = ibpt?.Aliqnac ?? 0;
                produtoSaidum.VlAproxImposto = Math.Round(produtoSaidum.VlTotal * ((produtoSaidum.PorcIbpt ?? 0) / 100), 4);
            }

            if (produtoSaidum != null && produtoSaidum?.Cst?.Length > 0 && (!produtoSaidum.Cst.Equals("040")))
            {
                if (
                    produtoSaidum.Cfop.Length > 0 &&
                    (produtoSaidum.Cfop.StartsWith("54") || produtoSaidum.Cfop.StartsWith("64")) &&
                    (Convert.ToInt32(produtoSaidum.Cfop ?? "0") < 5410 || Convert.ToInt32(produtoSaidum.Cfop ?? "0") < 6410)
                   )
                {
                    produtoSaidum.VlBaseIcms = 0;
                    produtoSaidum.VlIcms = 0;
                    produtoSaidum.PocIcms = 0;
                    produtoSaidum.Cst = "060";
                }
            }
        }
    }
}
