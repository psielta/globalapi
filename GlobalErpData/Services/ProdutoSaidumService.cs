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
        private bool possuiProtocoloNcm;
        public ProdutoSaidumService(GlobalErpFiscalBaseContext context)
        {
            _context = context;
            possuiProtocoloNcm = false;
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
                .AsNoTracking()
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

        public async Task RealizarCalculoImpostoSaida(ProdutoSaidum produtoSaidum, Cliente cliente)
        {
            Empresa? empresa = await _context.Empresas
                .AsNoTracking()
                .Include(e => e.CdCidadeNavigation)
                .Where(e => e.CdEmpresa == produtoSaidum.CdEmpresa).FirstOrDefaultAsync();

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
            if (produtoSaidum?.Cfop?.Length > 0)
            {
                CfopCsosnV2? cfopCsosnV2 = await _context.CfopCsosnV2s
                    .Where(c => c.Cfop.Equals(produtoSaidum.Cfop)).FirstOrDefaultAsync();
                if (cfopCsosnV2 != null)
                {
                    produtoSaidum.CdCsosn = cfopCsosnV2.Csosn;
                }
            }

            if (cliente.CdCidadeNavigation == null)
            {
                Cidade? cidade = await _context.Cidades.FindAsync(cliente.CdCidade);
                if (cidade == null)
                {
                    throw new Exception("Cidade não encontrada");
                }
                cliente.CdCidadeNavigation = cidade;
            }

            if (produtoSaidum?.Ncm?.Length > 0)
            {
                ProtocoloEstadoNcm? protocoloEstadoNcm = await _context.ProtocoloEstadoNcms
                    .FromSqlRaw($@"
                        select  p.* from protocolo_estado_ncm p
                        where p.id in (select id_cab_protocolo from ncm_protocolo_estado where id_ncm = '{produtoSaidum?.Ncm}' LIMIT 1)
                        and p.ativo = 'S' and p.uf = '{cliente.CdCidadeNavigation.Uf}'
                    ").FirstOrDefaultAsync();

                if (protocoloEstadoNcm != null)
                {
                    possuiProtocoloNcm = true;
                    produtoSaidum.VlBaseIcms = produtoSaidum.VlTotal - produtoSaidum.DescRateio;
                    produtoSaidum.PorcSt = protocoloEstadoNcm.St;
                    produtoSaidum.MvaSt = protocoloEstadoNcm.Iva;
                    if (protocoloEstadoNcm.RedIcms > 0)
                    {
                        decimal? vValorBase = (produtoSaidum.VlBaseIcms) - (produtoSaidum.VlBaseIcms * (protocoloEstadoNcm.RedIcms / 100));
                        produtoSaidum.VlBaseIcms = Math.Round(vValorBase ?? 0, 2);
                    }
                    if (protocoloEstadoNcm.RedSt > 0)
                    {
                        produtoSaidum.VlBaseSt = produtoSaidum.VlTotal - produtoSaidum.DescRateio;
                        decimal? vValorBase = (produtoSaidum.VlBaseSt) - (produtoSaidum.VlBaseSt * (protocoloEstadoNcm.RedSt / 100));
                        produtoSaidum.VlBaseSt = Math.Round(vValorBase ?? 0, 2);
                    }
                }
            }

            if (empresa.TipoRegime > 1)
            {

                string onlyCst = produtoSaidum?.Cst.Length > 0 ? produtoSaidum.Cst?.Substring(1, 2) : "";

                switch (onlyCst)
                {
                    case "00":
                        Tributados00(produtoSaidum);
                        break;
                    case "10":
                        Tributados10(produtoSaidum);
                        break;
                    default:
                        break;
                }
            }


            /*********************************************************/
        }

        private void Tributados10(ProdutoSaidum? produtoSaidum)
        {
            throw new NotImplementedException();
        }

        private void Tributados00(ProdutoSaidum? produtoSaidum)
        {
            if (!possuiProtocoloNcm)
            {
                produtoSaidum.VlBaseIcms = produtoSaidum.VlTotal - produtoSaidum.DescRateio;
            }
            produtoSaidum.VlIcms = Math.Round((produtoSaidum.VlBaseIcms ?? 0) * ((produtoSaidum.PocIcms ?? 0) / 100), 2);
        }
    }
}
