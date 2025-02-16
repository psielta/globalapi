using AutoMapper;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Enum;
using GlobalLib.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Repository.PagedRepositories
{
    public class CteRepository : GenericPagedRepository<Cte, GlobalErpFiscalBaseContext, int, CteDto>
    {
        public CteRepository(GlobalErpFiscalBaseContext injectedContext, IMapper mapper, ILogger<GenericRepositoryDto<Cte, GlobalErpFiscalBaseContext, int, CteDto>> logger) : base(injectedContext, mapper, logger)
        {
        }

        /*
            string vsql = $@"
                    select 
                        c.*, 
                        cast(case c.cd_situacao_cte 
                            when '01' then 'Normal' 
                            when '02' then 'Transmitida' 
                            when '11' then 'Cancelado' 
                            else ' ' 
                        end as Character Varying(12)) as txt_sit, 
                        cl1.nm_cliente as nm_tomador_servico 
                    from 
                        cte.cte c 
                    left join 
                        cliente cl1 on {(modelosNFe == GGUModelosNFe.GGUCtePassageiros ? "c.cd_tomador_servico" : "c.cd_remetente")} = cl1.id 
                    where 
                        cast(c.dt_hr_emissao as date) between '{pDataInicial}' and '{pDataFinal}' 
                        and (c.nr_cte = '{pIdCte}' or ('*' = '{pIdCte}')) 
                        and (c.id = {pId} or (-1 = {pId})) 
                        and ({(modelosNFe == GGUModelosNFe.GGUCtePassageiros ? "c.cd_tomador_servico" : "c.cd_remetente")} = {pId_Cliente} or (-1 = {pId_Cliente})) 
                        and c.modelo = '{(int)modelosNFe}' 
                        and c.id_empresa = {pId_Empresa} 
                    order by 
                        c.id desc";*/
        public IQueryable<Cte> Get_Pesquisa_Nome(string pId_Empresa, string pDataInicial, string pDataFinal,
         string pIdCte, string pId, string pId_Cliente, GGUModelosNFe modelosNFe)
        {
            if (string.IsNullOrEmpty(pIdCte))
            {
                pIdCte = "*";
            }
            if (string.IsNullOrEmpty(pId))
            {
                pId = "-1";
            }
            if (string.IsNullOrEmpty(pId_Cliente))
            {
                pId_Cliente = "-1";
            }
            if (string.IsNullOrEmpty(pDataInicial))
            {
                pDataInicial = DateTime.MinValue.ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(pDataFinal))
            {
                pDataFinal = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(pId_Empresa))
            {
                throw new Exception("Empresa não informada");
            }

            DateTime dataInicial = DateTime.Parse(pDataInicial);
            DateTime dataFinal = DateTime.Parse(pDataFinal).AddDays(1); // para incluir o dia final

            var query = from c in db.Ctes
                        join cl in db.Clientes on (modelosNFe == GGUModelosNFe.GGUCtePassageiros ? c.CdTomadorServico : c.CdRemetente)
                            equals cl.Id into clJoin
                        from cliente in clJoin.DefaultIfEmpty()
                        where c.DtHrEmissao >= dataInicial && c.DtHrEmissao < dataFinal
                           && (c.NrCte == pIdCte || pIdCte == "*")
                           && (c.Id == int.Parse(pId) || int.Parse(pId) == -1)
                           && (((modelosNFe == GGUModelosNFe.GGUCtePassageiros ? c.CdTomadorServico : c.CdRemetente)
                                  == int.Parse(pId_Cliente)) || int.Parse(pId_Cliente) == -1)
                           && c.Modelo == ((int)modelosNFe).ToString()
                           && c.IdEmpresa == int.Parse(pId_Empresa)
                        orderby c.Id descending
                        select new Cte
                        {
                            Id = c.Id,
                            NrCte = c.NrCte,
                            Serie = c.Serie,
                            DtHrEmissao = c.DtHrEmissao,
                            Modelo = c.Modelo,
                            Status = c.Status,
                            CdNumerico = c.CdNumerico,
                            Cfop = c.Cfop,
                            Modal = c.Modal,
                            TpServico = c.TpServico,
                            FormaPagto = c.FormaPagto,
                            FinalidadeEmissao = c.FinalidadeEmissao,
                            FormaEmissao = c.FormaEmissao,
                            ChaveAcesso = c.ChaveAcesso,
                            ChaveAcessoReferenc = c.ChaveAcessoReferenc,
                            MunicipioEmissao = c.MunicipioEmissao,
                            MunicipioInicioPrestacao = c.MunicipioInicioPrestacao,
                            MunicipioFimPrestacao = c.MunicipioFimPrestacao,
                            DadosRetirada = c.DadosRetirada,
                            Detalhe = c.Detalhe,
                            CaractAdTransp = c.CaractAdTransp,
                            CaractAdServico = c.CaractAdServico,
                            FuncEmissorCte = c.FuncEmissorCte,
                            MunicipioOrigemCalcFrete = c.MunicipioOrigemCalcFrete,
                            MunicipioDestinoCalcFrete = c.MunicipioDestinoCalcFrete,
                            CdRotaEntrega = c.CdRotaEntrega,
                            OrigemFluxoCaixa = c.OrigemFluxoCaixa,
                            DestinoFluxoCaixa = c.DestinoFluxoCaixa,
                            PrevisaoData = c.PrevisaoData,
                            PrevisaoHora = c.PrevisaoHora,
                            DtInicioPrevisao = c.DtInicioPrevisao,
                            DtFimPrevisao = c.DtFimPrevisao,
                            HrInicioPrevisao = c.HrInicioPrevisao,
                            HrFimPrevisao = c.HrFimPrevisao,
                            TpTomadorServico = c.TpTomadorServico,
                            CdTomadorServico = c.CdTomadorServico,
                            TpRemetente = c.TpRemetente,
                            CdRemetente = c.CdRemetente,
                            TpExpedidor = c.TpExpedidor,
                            CdExpedidor = c.CdExpedidor,
                            TpRecebedor = c.TpRecebedor,
                            CdRecebedor = c.CdRecebedor,
                            TpDestinatario = c.TpDestinatario,
                            CdDestinatario = c.CdDestinatario,
                            VlPrestServico = c.VlPrestServico,
                            VlReceberPrestServico = c.VlReceberPrestServico,
                            VlTribtPrestServico = c.VlTribtPrestServico,
                            CdStIcms = c.CdStIcms,
                            PorcRedBcIcms = c.PorcRedBcIcms,
                            VlBcIcms = c.VlBcIcms,
                            AliqIcms = c.AliqIcms,
                            VlIcms = c.VlIcms,
                            VlCredPresumido = c.VlCredPresumido,
                            InfFiscoIcms = c.InfFiscoIcms,
                            IcmsUfTermino = c.IcmsUfTermino,
                            VlBcIcmsUft = c.VlBcIcmsUft,
                            AliqInternaUft = c.AliqInternaUft,
                            AliqInterestUft = c.AliqInterestUft,
                            CdPartUft = c.CdPartUft,
                            PorcPartUft = c.PorcPartUft,
                            VlIcmsPartUfi = c.VlIcmsPartUfi,
                            VlIcmsPartUft = c.VlIcmsPartUft,
                            PorcIcmsFcpUft = c.PorcIcmsFcpUft,
                            VlIcmsFcpUf = c.VlIcmsFcpUf,
                            VlCarga = c.VlCarga,
                            ProdPredominante = c.ProdPredominante,
                            OutrasCaractProd = c.OutrasCaractProd,
                            ChaveCteSubst = c.ChaveCteSubst,
                            TomadorCteSubst = c.TomadorCteSubst,
                            TomadorNc = c.TomadorNc,
                            ChaveCteTomador = c.ChaveCteTomador,
                            ChaveNfeTomador = c.ChaveNfeTomador,
                            TpDocTomador = c.TpDocTomador,
                            CpfTomador = c.CpfTomador,
                            CnpjTomador = c.CnpjTomador,
                            CdModeloTomador = c.CdModeloTomador,
                            SerieTomador = c.SerieTomador,
                            Subserie = c.Subserie,
                            NumeroTomador = c.NumeroTomador,
                            VlTomador = c.VlTomador,
                            DtTomador = c.DtTomador,
                            NrFatura = c.NrFatura,
                            VlOriginalFatura = c.VlOriginalFatura,
                            VlDescFatura = c.VlDescFatura,
                            VlLiqFatura = c.VlLiqFatura,
                            Rntcr = c.Rntcr,
                            DtPrevEntrega = c.DtPrevEntrega,
                            IndicadorLot = c.IndicadorLot,
                            Ciot = c.Ciot,
                            ChaveCteAnulacao = c.ChaveCteAnulacao,
                            Obs = c.Obs,
                            NrAutorizacaoCte = c.NrAutorizacaoCte,
                            CdSituacaoCte = c.CdSituacaoCte,
                            XmlCte = c.XmlCte,
                            TxtJustificativaCancelamento = c.TxtJustificativaCancelamento,
                            NrProtoCancelamento = c.NrProtoCancelamento,
                            TxtdescServicoPrestado = c.TxtdescServicoPrestado,
                            QtPassageiro = c.QtPassageiro,
                            NrTaf = c.NrTaf,
                            NrRegEstadual = c.NrRegEstadual,
                            DataViagem = c.DataViagem,
                            HoraViagem = c.HoraViagem,
                            TipoViagem = c.TipoViagem,
                            NrCnf = c.NrCnf,
                            Inss = c.Inss,
                            NrCteReferenciado = c.NrCteReferenciado,
                            RetemInss = c.RetemInss,
                            IdEmpresa = c.IdEmpresa,
                            TxtSit = c.CdSituacaoCte == "01" ? "Normal" :
                                     c.CdSituacaoCte == "02" ? "Transmitida" :
                                     c.CdSituacaoCte == "11" ? "Cancelado" : " ",
                            NmTomadorServico = cliente != null ? cliente.NmCliente : string.Empty
                        };

            return query;
        }

    }
}
