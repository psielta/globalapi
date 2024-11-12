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

        public async Task RealizarCalculoImpostoSaida(ProdutoSaidum produtoSaidum, ProdutoEstoque produto, Cliente cliente)
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
                        await Tributados10(produtoSaidum, cliente, empresa, produto);
                        break;
                    case "20":
                        await Tributados20(produtoSaidum);
                        break;
                    case "30":
                        await Tributados30(produtoSaidum, cliente, empresa, produto);
                        break;
                    case "40":
                        Tributados40(produtoSaidum);
                        break;
                    case "41":
                        Tributados41(produtoSaidum);
                        break;
                    case "50":
                        Tributados50(produtoSaidum);
                        break;
                    case "60":
                        Tributados60(produtoSaidum);
                        break;
                    case "70":
                        await Tributados70(produtoSaidum, cliente, empresa, produto);
                        break;
                    case "90":
                        await Tributados90(produtoSaidum, cliente, empresa, produto);
                        break;
                    default:
                        break;
                }
            }
            if (!possuiProtocoloNcm)
            {
                await CalcularICMSAutomaticamente(produtoSaidum, cliente, empresa, produto);
            }

            CalcularPisCofins(produtoSaidum);
            VerificarCstPis(produtoSaidum);
            VerificarCstCofins(produtoSaidum);

        }

        private void VerificarCstCofins(ProdutoSaidum? produtoSaidum)
        {
            if (!string.IsNullOrEmpty(produtoSaidum.CstCofins))
            {
                int cstCofins = Convert.ToInt32(produtoSaidum.CstCofins);
                switch (cstCofins)
                {
                    case 6:
                        produtoSaidum.VlCofins = 0;
                        produtoSaidum.VlBaseCofins = 0;
                        break;
                    default:
                        break;
                }
            }
        }

        private void VerificarCstPis(ProdutoSaidum? produtoSaidum)
        {
            if (!string.IsNullOrEmpty(produtoSaidum.CstPis))
            {
                int cstPis = Convert.ToInt32(produtoSaidum.CstPis);
                switch (cstPis)
                {
                    case 6:
                        produtoSaidum.VlPis = 0;
                        produtoSaidum.VlBasePis = 0;
                        break;
                    default:
                        break;
                }
            }

        }

        private void CalcularPisCofins(ProdutoSaidum? produtoSaidum)
        {
            produtoSaidum.VlBaseCofins = produtoSaidum.VlTotal - produtoSaidum.VlIcms - (produtoSaidum.DescRateio ?? 0);
            decimal vValorAux = Math.Round((produtoSaidum.VlBaseCofins ?? 0) * ((produtoSaidum.PorcCofins ?? 0) / 100), 2);
            produtoSaidum.VlCofins = vValorAux;
            produtoSaidum.VlBasePis = produtoSaidum.VlTotal - produtoSaidum.VlIcms - (produtoSaidum.DescRateio ?? 0);
            vValorAux = Math.Round((produtoSaidum.VlBasePis ?? 0) * ((produtoSaidum.PorcPis ?? 0) / 100), 2);
            produtoSaidum.VlPis = vValorAux;
        }

        private async Task CalcularICMSAutomaticamente(ProdutoSaidum? produtoSaidum, Cliente cliente, Empresa empresa, ProdutoEstoque produto)
        {
            decimal icmsDentro = produto.IcmsDentro ?? 0;
            decimal icmsFora = produto.IcmsFora ?? 0;
            decimal icmsInterno = produto.PorcAliqInterna ?? 0;

            if (cliente.CdCidadeNavigation == null)
            {
                Cidade? cidade = await _context.Cidades.FindAsync(cliente.CdCidade);
                if (cidade == null)
                {
                    throw new Exception("Cidade não encontrada");
                }
                cliente.CdCidadeNavigation = cidade;
            }
            string ufCliente = cliente.CdCidadeNavigation?.Uf ?? "";

            if (empresa.CdCidadeNavigation == null)
            {
                empresa.CdCidadeNavigation = await _context.Cidades.FindAsync(empresa.CdCidade);
            }
            string ufEmpresa = empresa.CdCidadeNavigation?.Uf ?? "";

            if (icmsInterno > 0)
            {
                produtoSaidum.VlBaseIcms = produtoSaidum.VlTotal - produtoSaidum.DescRateio;
                produtoSaidum.PocIcms = icmsInterno;
                produtoSaidum.VlIcms = Math.Round((produtoSaidum.VlTotal - (produtoSaidum.DescRateio ?? 0)) * (icmsInterno / 100), 2);
            }
            else if (ufCliente.Equals(ufEmpresa))
            {
                produtoSaidum.VlBaseIcms = produtoSaidum.VlTotal - produtoSaidum.DescRateio;
                produtoSaidum.PocIcms = icmsDentro;
                produtoSaidum.VlIcms = Math.Round((produtoSaidum.VlTotal - (produtoSaidum.DescRateio ?? 0)) * (icmsDentro / 100), 2);
            }
            else
            {
                produtoSaidum.VlBaseIcms = produtoSaidum.VlTotal - produtoSaidum.DescRateio;
                produtoSaidum.PocIcms = icmsFora;
                produtoSaidum.VlIcms = Math.Round((produtoSaidum.VlTotal - (produtoSaidum.DescRateio ?? 0)) * (icmsFora / 100), 2);
            }
        }

        private async Task Tributados90(ProdutoSaidum? produtoSaidum, Cliente cliente, Empresa empresa, ProdutoEstoque produto)
        {
            decimal pocReducao = produtoSaidum.PocReducao ?? 0;
            produtoSaidum.PocReducao = pocReducao;
            produtoSaidum.VlBaseIcms = produtoSaidum.VlTotal - produtoSaidum.VlTotal * (pocReducao / 100);
            produtoSaidum.VlIcms = Math.Round((produtoSaidum.VlBaseIcms ?? 0) * ((produtoSaidum.PocIcms ?? 0) / 100), 2);
            produtoSaidum.VlBaseSt = produtoSaidum.VlBaseIcms;
            produtoSaidum.PorcSt = produto.PorcSubst ?? 0;
            produtoSaidum.VlSt = Math.Round((produtoSaidum.VlBaseSt ?? 0) * ((produtoSaidum.PorcSt ?? 0) / 100), 2);
        }

        private async Task Tributados70(ProdutoSaidum? produtoSaidum, Cliente cliente, Empresa empresa, ProdutoEstoque produto)
        {
            if (cliente.Mva ?? false)
            {
                decimal? porcIcms = await GetIcmsEstado(empresa);
                decimal pIcms = porcIcms ?? 0;
                int tipoRegimeCliente = cliente.TpRegime ?? 0;
                produtoSaidum.MvaSt = 0;
                if (tipoRegimeCliente == 1 || tipoRegimeCliente == 3)
                {
                    produtoSaidum.MvaSt = produto.Mva ?? 0;
                }
                decimal pocReducao = produtoSaidum.PocReducao ?? 0;
                produtoSaidum.PocReducao = pocReducao;
                produtoSaidum.PorcSt = produto.PorcSubst ?? 0;
                produtoSaidum.VlBaseSt = (produtoSaidum.VlTotal + produtoSaidum.VlIpi)
                   * ((100 + produtoSaidum.MvaSt) / 100);
                produtoSaidum.VlBaseIcms = produtoSaidum.VlBaseSt * ((100 - pocReducao) / 100);
                produtoSaidum.VlIcms = Math.Round((produtoSaidum.VlBaseIcms ?? 0) * ((produto.PorcAliqInterna ?? 0) / 100), 2);
                produtoSaidum.VlSt = Math.Round((produtoSaidum.VlBaseIcms ?? 0) * ((produtoSaidum.PorcSt ?? 0) / 100) - (produtoSaidum.VlIcms ?? 0), 2);
            }
            else
            {
                decimal pocReducao = produtoSaidum.PocReducao ?? 0;
                produtoSaidum.PocReducao = pocReducao;
                produtoSaidum.VlBaseSt = produtoSaidum.VlTotal;
                produtoSaidum.VlSt = Math.Round(produtoSaidum.VlTotal * (pocReducao / 100), 2);
            }
        }

        private void Tributados40(ProdutoSaidum? produtoSaidum)
        {
            produtoSaidum.VlBaseIcms = 0;
            produtoSaidum.VlIcms = 0;
            produtoSaidum.VlBaseSt = 0;
            produtoSaidum.PorcSt = 0;
            produtoSaidum.VlSt = 0;
            produtoSaidum.PocReducao = 0;
            produtoSaidum.MvaSt = 0;
        }
        private void Tributados41(ProdutoSaidum? produtoSaidum)
        {
            produtoSaidum.VlBaseIcms = 0;
            produtoSaidum.VlIcms = 0;
            produtoSaidum.VlBaseSt = 0;
            produtoSaidum.PorcSt = 0;
            produtoSaidum.VlSt = 0;
            produtoSaidum.PocReducao = 0;
            produtoSaidum.MvaSt = 0;
        }
        private void Tributados50(ProdutoSaidum? produtoSaidum)
        {
            produtoSaidum.VlBaseIcms = 0;
            produtoSaidum.VlIcms = 0;
            produtoSaidum.VlBaseSt = 0;
            produtoSaidum.PorcSt = 0;
            produtoSaidum.VlSt = 0;
            produtoSaidum.PocReducao = 0;
            produtoSaidum.MvaSt = 0;
        }
        private void Tributados60(ProdutoSaidum? produtoSaidum)
        {
            produtoSaidum.VlBaseIcms = 0;
            produtoSaidum.VlIcms = 0;
            produtoSaidum.VlBaseSt = 0;
            produtoSaidum.PorcSt = 0;
            produtoSaidum.VlSt = 0;
            produtoSaidum.PocReducao = 0;
            produtoSaidum.MvaSt = 0;
        }

        private async Task Tributados30(ProdutoSaidum? produtoSaidum, Cliente cliente, Empresa empresa, ProdutoEstoque produto)
        {
            if (cliente.Mva ?? false)
            {
                decimal? porcIcms = await GetIcmsEstado(empresa);
                decimal pIcms = porcIcms ?? 0;
                int tipoRegimeCliente = cliente.TpRegime ?? 0;
                produtoSaidum.MvaSt = 0;
                if (tipoRegimeCliente == 1 || tipoRegimeCliente == 3)
                {
                    produtoSaidum.MvaSt = produto.Mva ?? 0;
                }
                decimal pocReducao = produtoSaidum.PocReducao ?? 0;
                produtoSaidum.PocReducao = pocReducao;
                produtoSaidum.VlBaseSt = (produtoSaidum.VlTotal + produtoSaidum.VlIpi)
                   * ((100 + produtoSaidum.MvaSt) / 100);
                produtoSaidum.PorcSt = produto.PorcSubst ?? 0;
                decimal valorIcms = Math.Round((produtoSaidum.VlTotal) * (pocReducao / 100), 2);
                produtoSaidum.VlSt = Math.Round((produtoSaidum.VlBaseSt ?? 0)
                    * ((produto.PorcSubst ?? 0) / 100), 2) - valorIcms;

            }
            else
            {
                decimal pocReducao = produtoSaidum.PocReducao ?? 0;
                produtoSaidum.PocReducao = pocReducao;
                produtoSaidum.VlBaseSt = produtoSaidum.VlTotal;// - produtoSaidum.VlTotal * (pocReducao / 100);
                produtoSaidum.VlSt = Math.Round((produtoSaidum.VlTotal) * ((produtoSaidum.PocIcms ?? 0) / 100), 2);

            }
        }

        private async Task Tributados20(ProdutoSaidum? produtoSaidum)
        {
            decimal pocReducao = produtoSaidum.PocReducao ?? 0;
            produtoSaidum.PocReducao = pocReducao;
            produtoSaidum.VlBaseIcms = produtoSaidum.VlTotal - produtoSaidum.VlTotal * (pocReducao / 100);
            produtoSaidum.VlIcms = Math.Round((produtoSaidum.VlBaseIcms ?? 0) * ((produtoSaidum.PocIcms ?? 0) / 100), 2);
        }

        private async Task Tributados10(ProdutoSaidum? produtoSaidum, Cliente cliente, Empresa empresa, ProdutoEstoque produto)
        {
            if (cliente.Mva ?? false)
            {
                decimal? porcIcms = await GetIcmsEstado(empresa);
                decimal pIcms = porcIcms ?? 0;
                int tipoRegimeCliente = cliente.TpRegime ?? 0;
                produtoSaidum.MvaSt = 0;
                if (tipoRegimeCliente == 1 || tipoRegimeCliente == 3)
                {
                    produtoSaidum.MvaSt = produto.Mva ?? 0;
                }
                produtoSaidum.PorcSt = produto.PorcSubst ?? 0;

                produtoSaidum.VlBaseSt = (produtoSaidum.VlTotal + produtoSaidum.VlIpi)
                    * ((100 + produtoSaidum.MvaSt) / 100);

                decimal valorIcms = Math.Round((produtoSaidum.VlTotal) * (pIcms / 100), 2);
                decimal vBase = 0;
                if ((produto.VlTabelaGov ?? 0) > 0)
                {
                    vBase = produto.VlTabelaGov ?? 0;
                }
                else
                {
                    vBase = produtoSaidum.VlBaseSt ?? 0;
                }
                produtoSaidum.VlSt = Math.Round((vBase) * ((produto.PorcAliqInterna ?? 0) / 100), 2) - valorIcms;
            }
            else
            {
                if (!possuiProtocoloNcm)
                {
                    produtoSaidum.VlBaseSt = produtoSaidum.VlTotal - produtoSaidum.DescRateio;
                }
                produtoSaidum.VlSt = Math.Round((produtoSaidum.VlBaseSt ?? 0) * ((produtoSaidum.PorcSt ?? 0) / 100), 2);
            }
        }

        private void Tributados00(ProdutoSaidum? produtoSaidum)
        {
            if (!possuiProtocoloNcm)
            {
                produtoSaidum.VlBaseIcms = produtoSaidum.VlTotal - produtoSaidum.DescRateio;
            }
            produtoSaidum.VlIcms = Math.Round((produtoSaidum.VlBaseIcms ?? 0) * ((produtoSaidum.PocIcms ?? 0) / 100), 2);
        }

        private async Task<decimal?> GetIcmsEstado(Empresa empresa)
        {
            Icm? icm = await _context.Icms.AsNoTracking().Where(p => p.CdEmpresa == empresa.CdEmpresa).FirstAsync();
            if (icm == null)
            {
                throw new Exception("ICMS não encontrado");
            }
            if (empresa.CdCidadeNavigation == null)
            {
                empresa.CdCidadeNavigation = await _context.Cidades.FindAsync(empresa.CdCidade);
            }
            string ufEmpresa = empresa.CdCidadeNavigation?.Uf ?? "";

            switch (ufEmpresa)
            {
                case "AC":
                    return icm.Ac;
                case "AL":
                    return icm.Al;
                case "AM":
                    return icm.Am;
                case "AP":
                    return icm.Ap;
                case "BA":
                    return icm.Ba;
                case "CE":
                    return icm.Ce;
                case "DF":
                    return icm.Df;
                case "ES":
                    return icm.Es;
                case "GO":
                    return icm.Go;
                case "MA":
                    return icm.Ma;
                case "MG":
                    return icm.Mg;
                case "MS":
                    return icm.Ms;
                case "MT":
                    return icm.Mt;
                case "PA":
                    return icm.Pa;
                case "PB":
                    return icm.Pb;
                case "PE":
                    return icm.Pe;
                case "PI":
                    return icm.Pi;
                case "PR":
                    return icm.Pr;
                case "RJ":
                    return icm.Rj;
                case "RN":
                    return icm.Rn;
                case "RO":
                    return icm.Ro;
                case "RR":
                    return icm.Rr;
                case "RS":
                    return icm.Rs;
                case "SC":
                    return icm.Sc;
                case "SE":
                    return icm.Se;
                case "SP":
                    return icm.Sp;
                case "TO":
                    return icm.To;
                default:
                    throw new Exception("Estado não encontrado");
            }
        }
    }
}
