using AutoMapper;
using FiscalBr.Common;
using FiscalBr.Common.Sintegra;
using FiscalBr.Sintegra;
using GlobalAPI_ACBrNFe.Lib;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Strings;
using GlobalLib.Utils;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text;
namespace GlobalAPI_ACBrNFe.Services
{
    public class SintegraService
    {
        protected GlobalErpFiscalBaseContext db;
        protected readonly ILogger<SintegraService> logger;
        protected IMapper mapper;
        private readonly IHubContext<ImportProgressHub> _hubContext;

        private string sessionId;
        private Empresa empresa;

        private int idPlano;
        private int mes;
        private int ano;
        private DateTime dataInicial;
        private string dataInicialPtbrFormat;
        private DateTime dataFinal;
        private string dataFinalPtbrFormat;
        private int CodFinalidadeArquivo;

        public SintegraService(GlobalErpFiscalBaseContext db, ILogger<SintegraService> logger, IMapper mapper, IHubContext<ImportProgressHub> hubContext)
        {
            this.db = db;
            this.logger = logger;
            this.mapper = mapper;
            _hubContext = hubContext;
        }

        public async Task<string> Core(SintegraDto sintegraDto)
        {
            await SetDados(sintegraDto);
            string SintegraTxt = string.Empty;
            SintegraTxt += await GetRegistro10();
            SintegraTxt += await GetRegistro11();
            SintegraTxt += await GetRegistro50EntradasAsync();
            SintegraTxt += await GetRegistro50SaidasAsync();
            SintegraTxt += await GetRegistro54EntradaAsync();
            SintegraTxt += await GetRegistro54SaidaAsync();
            return SintegraTxt;
        }

        private async Task<string> GetRegistro54SaidaAsync()
        {
            await _hubContext.Clients.Group(sessionId).SendAsync("ReceiveProgress", "Gerando registro 54 Saidas.");
            int pcd_empresa = empresa.CdEmpresa;
            int pcd_plano = this.idPlano;

            var resultados = await db.ProcReg54SaidaResults
                .FromSqlRaw($"SELECT * FROM public.proc_reg_54_saida({dataInicialPtbrFormat}, {dataFinalPtbrFormat}, {pcd_empresa}, {pcd_plano})")
                .ToListAsync();

            string resultString = string.Empty;
            if (resultados == null || resultados.Count <= 0)
                return resultString;
            var listRegistro54 = new List<Registro54>();
            foreach (var item in resultados)
            {
                var r54 = new Registro54();
                r54.Cnpj = UtlStrings.OnlyInteger(item.scnpj ?? "");
                r54.Modelo = int.Parse(item.ssmodelo);
                r54.Serie = item.sserie_nf;
                r54.Numero = int.Parse(item.snr_nota_fiscal);
                r54.NumeroItem = item.snr_item;
                if (string.IsNullOrEmpty(item.scfop))
                    throw new Exception($"CFOP não informado (Saida Nr: {item.snr_nota_fiscal}, serie {item.sserie_nf})");
                r54.Cfop = int.Parse(item.scfop);
                if (string.IsNullOrEmpty(item.scst))
                    throw new Exception($"CST não informado (Saida Nr: {item.snr_nota_fiscal}, serie {item.sserie_nf})");
                if (item.scst.Length > 3)
                    r54.Cst = item.scst.Substring(1, 3);
                else
                    r54.Cst = item.scst;
                r54.CodProdutoServico = item.scd_produto.ToString();
                r54.Quantidade = item.squant;
                r54.VlProdutoServico = item.svl_total;
                r54.VlIpi = item.svl_ipi;
                if (item.scst.Equals("060"))
                {
                    r54.BaseCalculoIcms = 0;
                    r54.BaseCalculoIcmsSt = 0;
                    r54.AliquotaIcms = 0;

                }
                else
                {
                    r54.BaseCalculoIcms = item.svl_base_icms;
                    r54.BaseCalculoIcmsSt = item.svl_base_st;
                    if (item.svl_base_st > 0)
                        r54.AliquotaIcms = item.sporc_st;
                    else
                        r54.AliquotaIcms = item.spoc_icms;
                    if (item.svl_total == 0)
                        r54.AliquotaIcms = 0;

                }
            }

            foreach (var r54 in listRegistro54)
            {
                resultString += r54.EscreverCampos();
            }
            return resultString;
        }

        private async Task<string> GetRegistro50SaidasAsync()
        {
            await _hubContext.Clients.Group(sessionId).SendAsync("ReceiveProgress", "Gerando registro 50 Saidas.");
            int cd_empresa = empresa.CdEmpresa;
            int pcd_plano = this.idPlano;

            var resultados = await db.ProcReg50SaidaResults
                .FromSqlRaw($"SELECT * FROM public.proc_reg_50_saida({dataInicialPtbrFormat}, {dataFinalPtbrFormat}, {cd_empresa}, {pcd_plano})")
                .ToListAsync();

            string resultString = string.Empty;
            if (resultados == null || resultados.Count <= 0)
                return resultString;

            var listRegistro50 = new List<Registro50>();
            foreach (var item in resultados)
            {
                Registro50 registro50 = new Registro50();
                registro50.Cnpj = UtlStrings.OnlyInteger(item.scnpj ?? "");
                registro50.InscrEstadual = item.snr_insc_esta;
                registro50.DataEmissaoRecebimento = item.sdata;
                registro50.Modelo = int.Parse(item.smodelo);
                registro50.Serie = item.sserie_nf;
                registro50.Numero = int.Parse(item.snr_nota_fiscal);
                if (string.IsNullOrEmpty(item.scd_cfop))
                    throw new Exception($"CFOP não informado (Saida Nr: {item.snr_nota_fiscal}, serie {item.sserie_nf})");
                registro50.Cfop = int.Parse(item.scd_cfop);
                registro50.Emitente = "P";
                if (item.scd_situacao == "11")
                    registro50.SituacaoNotaFiscal = "S";
                else if (item.scd_situacao == "70")
                    registro50.SituacaoNotaFiscal = "4";
                else if (item.scd_situacao == "04")
                    registro50.SituacaoNotaFiscal = "2";
                else
                    registro50.SituacaoNotaFiscal = "N";

                registro50.ValorTotal = item.svl_total;
                registro50.BaseCalculoIcms = item.sbase;
                registro50.ValorIcms = item.svl_icms;
                registro50.AliquotaIcms = item.spor_icms;
                registro50.ValorIsentaOuNaoTributadas = 0;
                registro50.ValorOutras = 0;
            }

            foreach (var r50 in listRegistro50)
            {
                resultString += r50.EscreverCampos();
            }
            return resultString;
        }

        private async Task<string> GetRegistro54EntradaAsync()
        {
            string resultString = string.Empty;
            await _hubContext.Clients.Group(sessionId).SendAsync("ReceiveProgress", "Gerando registro 54 Entradas.");

            DateTime pdt1 = this.dataInicial;
            DateTime pdt2 = this.dataFinal;
            int pcd_empresa = empresa.CdEmpresa;
            int pcd_plano = idPlano;

            var resultados = await db.ProcReg54EntradaResults
                .FromSqlRaw($"SELECT * FROM public.proc_reg_54_entrada('{dataInicialPtbrFormat}', '{dataFinalPtbrFormat}', {pcd_empresa}, {pcd_plano})")
                .ToListAsync();

            var ListaRegistros54 = new List<Registro54>();

            if (resultados.Count == 0)
            {
                return resultString;
            }

            int i = 0;
            int x = 1; // contador de itens por NF
            double vdesp = 0, vdesp1 = 0, vdesp2 = 0, vdesp3 = 0;
            int num_item_desp_acess = 990;

            string vnr_nf = resultados.First().snr_nf; // pega a primeira NF

            for (int idx = 0; idx < resultados.Count; idx++)
            {
                var item = resultados[idx];

                var wregistro54 = new Registro54
                {
                    Cnpj = UtlStrings.OnlyInteger(item.scnpj ?? ""),
                    Modelo = int.Parse(item.smodelo_nf),
                    Serie = string.IsNullOrEmpty(item.sserie_nf) ? "1" : item.sserie_nf,
                    Numero = int.Parse(item.snr_nf),
                    Cfop = int.Parse(item.scd_cfop),
                };

                string cstAjustado = item.scst;
                if (!string.IsNullOrEmpty(cstAjustado))
                {
                    if (cstAjustado.Length > 3)
                        cstAjustado = cstAjustado.Substring(1, 3);
                }
                wregistro54.Cst = cstAjustado;

                if (vnr_nf != item.snr_nf)
                {
                    x = 1;
                    vnr_nf = item.snr_nf;
                }

                wregistro54.NumeroItem = x++;
                wregistro54.CodProdutoServico = item.scd_produto.ToString();
                wregistro54.Quantidade = item.squant;
                wregistro54.VlProdutoServico = item.svl_total;
                int cstMid = 0;
                if (!string.IsNullOrEmpty(cstAjustado) && cstAjustado.Length >= 3)
                {
                    string middle = cstAjustado.Substring(1, 2);
                    cstMid = int.Parse(middle);
                }

                switch (cstMid)
                {
                    case 00:
                    case 10:
                    case 20:
                    case 30:
                    case 70:
                    case 90:
                        wregistro54.VlDescontoDespesaAc = 0;
                        wregistro54.BaseCalculoIcms = item.sb_icms;
                        wregistro54.BaseCalculoIcmsSt = 0;
                        wregistro54.VlIpi = item.svl_ipi;
                        wregistro54.AliquotaIcms = item.sporc_icms;
                        break;
                    case 40:
                    case 41:
                    case 50:
                    case 60:
                        wregistro54.VlDescontoDespesaAc = 0;
                        wregistro54.BaseCalculoIcms = 0;
                        wregistro54.BaseCalculoIcmsSt = 0;
                        wregistro54.VlIpi = 0;
                        wregistro54.AliquotaIcms = 0;
                        break;
                    default:
                        break;
                }

                ListaRegistros54.Add(wregistro54);

                if (item.svl_outras > 0)
                {
                    if (item.scd_cfop == "1102")
                        vdesp += (double)item.svl_outras;
                    else if (item.scd_cfop == "1403")
                        vdesp1 += (double)item.svl_outras;
                    else if (item.scd_cfop == "2403")
                        vdesp2 += (double)item.svl_outras;
                    else if (item.scd_cfop == "2102")
                        vdesp3 += (double)item.svl_outras;
                }

                bool mudancaDeNota = false;
                if (idx < resultados.Count - 1)
                {
                    var prox = resultados[idx + 1];
                    mudancaDeNota = (prox.snr_nf != item.snr_nf);
                }
                else
                {
                    mudancaDeNota = true;
                }

                if (mudancaDeNota)
                {
                    num_item_desp_acess = 990;
                    if (vdesp > 0)
                    {
                        var regDesp = CriaRegistroDespesa(item, "1102", ++num_item_desp_acess, (decimal)vdesp);
                        ListaRegistros54.Add(regDesp);
                        vdesp = 0;
                    }

                    if (vdesp1 > 0)
                    {
                        var regDesp = CriaRegistroDespesa(item, "1403", ++num_item_desp_acess, (decimal)vdesp1);
                        ListaRegistros54.Add(regDesp);
                        vdesp1 = 0;
                    }

                    if (vdesp2 > 0)
                    {
                        var regDesp = CriaRegistroDespesa(item, "2403", ++num_item_desp_acess, (decimal)vdesp2);
                        ListaRegistros54.Add(regDesp);
                        vdesp2 = 0;
                    }

                    if (vdesp3 > 0)
                    {
                        var regDesp = CriaRegistroDespesa(item, "2102", ++num_item_desp_acess, (decimal)vdesp3);
                        ListaRegistros54.Add(regDesp);
                        vdesp3 = 0;
                    }
                }

                i++;
            }

            foreach (var r54 in ListaRegistros54)
            {
                resultString += r54.EscreverCampos();
            }

            return resultString;

            Registro54 CriaRegistroDespesa(ProcReg54EntradaResult refItem, string cfop, int numItem, decimal valorDesp)
            {
                var reg = new Registro54
                {
                    Cnpj = UtlStrings.OnlyInteger(refItem.scnpj ?? ""),
                    Modelo = int.Parse(refItem.smodelo_nf),
                    Serie = string.IsNullOrEmpty(refItem.sserie_nf) ? "1" : refItem.sserie_nf,
                    Numero = int.Parse(refItem.snr_nf),
                    Cfop = int.Parse(cfop),
                    Cst = "000",
                    NumeroItem = numItem,
                    CodProdutoServico = 0.ToString(),
                    Quantidade = 0,
                    VlProdutoServico = 0,
                    VlDescontoDespesaAc = valorDesp,
                    BaseCalculoIcms = null,
                    BaseCalculoIcmsSt = null,
                    VlIpi = null,
                    AliquotaIcms = null
                };
                return reg;
            }
        }

        private async Task SetDados(SintegraDto sintegraDto)
        {
            this.sessionId = sintegraDto.SessionId;
            this.mes = sintegraDto.Mes;
            this.ano = sintegraDto.Ano;
            dataInicial = new DateTime(ano, mes, 1);
            dataInicialPtbrFormat = dataInicial.ToString("dd/MM/yyyy");
            dataFinal = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));
            dataFinalPtbrFormat = dataFinal.ToString("dd/MM/yyyy");
            this.idPlano = sintegraDto.Plano;
            this.CodFinalidadeArquivo = sintegraDto.CodFinalidadeArquivo;

            Empresa? empresa = await db.Empresas.Where(e => e.CdEmpresa == sintegraDto.IdEmpresa)
                .Include(c => c.CdCidadeNavigation)
                .FirstOrDefaultAsync();

            if (empresa == null)
            {
                throw new Exception("Empresa nao encontrada");
            }

            this.empresa = empresa;
        }

        private async Task<string> GetRegistro10()
        {
            await _hubContext.Clients.Group(sessionId).SendAsync("ReceiveProgress", $"Gerando registro 10.");
            var r10 = new Registro10(
                Cnpj: UtlStrings.OnlyInteger(empresa.CdCnpj ?? ""),
                Ie: UtlStrings.OnlyInteger(empresa.NrInscrEstadual ?? ""),
                RazaoSocial: empresa.NmEmpresa,
                Municipio: empresa.CdCidadeNavigation.NmCidade,
                Uf: empresa.CdCidadeNavigation.Uf,
                Fax: UtlStrings.OnlyInteger(empresa.Telefone ?? ""),
                DataInicial: dataInicial,
                DataFinal: dataFinal,
                CodFin: (FiscalBr.Sintegra.CodFinalidadeArquivo)CodFinalidadeArquivo
             );
            return r10.EscreverCampos();

        }

        private async Task<string> GetRegistro11()
        {
            await _hubContext.Clients.Group(sessionId).SendAsync("ReceiveProgress", $"Gerando registro 11.");
            var r11 = new Registro11(
                Logradouro: empresa.NmEndereco,
                Numero: empresa.Numero.ToString(),
                Complemento: empresa.Complemento,
                Bairro: empresa.NmBairro,
                Cep: empresa.CdCep,
                NomeContato: "",
                NumeroContato: UtlStrings.OnlyInteger(empresa.Telefone ?? "")
                );
            return r11.EscreverCampos();
        }

        private async Task<string> GetRegistro50EntradasAsync()
        {
            string resultString = string.Empty;
            await _hubContext.Clients.Group(sessionId).SendAsync("ReceiveProgress", "Gerando registro 50 Entradas.");

            DateTime pdt1 = this.dataInicial; // Data inicial já definida
            DateTime pdt2 = this.dataFinal;   // Data final já definida
            int pcd_empresa = empresa.CdEmpresa;
            int pcd_plano = idPlano;

            string sql = $"SELECT * FROM public.proc_reg_50_entrada('{this.dataInicialPtbrFormat}', '{this.dataFinalPtbrFormat}', {empresa.CdEmpresa}, {idPlano})";

            var resultados = await db.ProcReg50EntradaResults
                .FromSqlRaw(sql)
                .ToListAsync();

            List<Registro50> registros50 = new List<Registro50>();
            string vnr_ = "-1";
            int i = 0;

            double _vl_desconto = 0;
            double _vl_frete = 0;
            double _vl_seguro = 0;
            double _vl_outras = 0;

            foreach (var item in resultados)
            {
                if (vnr_ != item.snr_nf)
                {
                    ObterValorEntradaDespesas(item.snr_nf, out _vl_desconto, out _vl_frete, out _vl_seguro, out _vl_outras, item);
                    vnr_ = item.snr_nf;
                }
                else
                {
                    _vl_desconto = 0;
                    _vl_frete = 0;
                    _vl_seguro = 0;
                    _vl_outras = 0;
                }

                double vTot = (double)item.svl_total + _vl_frete + _vl_seguro;
                if (_vl_desconto > 0)
                    vTot -= _vl_desconto;
                else
                    vnr_ = "-1";

                var wregistro50 = new Registro50
                {
                    Cnpj = UtlStrings.OnlyInteger(item.scnpj ?? ""),
                    InscrEstadual = string.IsNullOrEmpty(item.snr_inscr_estadual) ? "ISENTO" : item.snr_inscr_estadual,
                    DataEmissaoRecebimento = item.sdata.ToDateTime(TimeOnly.MinValue), // caso sdata seja DateOnly
                    Uf = item.suf,
                    Modelo = int.Parse(UtlStrings.OnlyInteger(item.smodelo_nf)),
                    Serie = string.IsNullOrEmpty(item.sserie_nf) ? "1" : UtlStrings.OnlyInteger(item.sserie_nf),
                    Numero = int.Parse(item.snr_nf),
                    Cfop = int.Parse(GetCfopReg50Ent(item.scd_cfop, item)),
                    Emitente = "T",
                    SituacaoNotaFiscal = "N",
                    ValorTotal = (decimal)vTot
                };

                bool cfopEspecial = (item.scd_cfop == "1102" || item.scd_cfop == "2102");
                string scst = item.scst ?? "";

                if (cfopEspecial)
                {
                    if (scst == "040")
                    {
                        wregistro50.BaseCalculoIcms = 0;
                        wregistro50.ValorIcms = 0;
                        wregistro50.AliquotaIcms = 0;
                        wregistro50.ValorIsentaOuNaoTributadas = (decimal)item.svl_total;
                        wregistro50.ValorOutras = 0;
                    }
                    else if (scst == "000" || scst == "020")
                    {
                        wregistro50.BaseCalculoIcms = (decimal)item.sb_icms;
                        wregistro50.ValorIcms = (decimal)item.svl_icms;
                        wregistro50.AliquotaIcms = (decimal)item.sporc_icms;
                        wregistro50.ValorIsentaOuNaoTributadas = 0;
                        wregistro50.ValorOutras = 0;
                    }
                    else
                    {
                        wregistro50.BaseCalculoIcms = (decimal)item.sb_icms;
                        wregistro50.ValorIcms = (decimal)item.svl_icms;
                        wregistro50.AliquotaIcms = (decimal)item.sporc_icms;
                        wregistro50.ValorIsentaOuNaoTributadas = 0;
                        wregistro50.ValorOutras = (decimal)item.svl_total;
                    }
                }
                else
                {
                    wregistro50.BaseCalculoIcms = (decimal)item.sb_icms;
                    wregistro50.ValorIcms = (decimal)item.svl_icms;
                    wregistro50.AliquotaIcms = (decimal)item.sporc_icms;
                    wregistro50.ValorIsentaOuNaoTributadas = 0;
                    wregistro50.ValorOutras = (decimal)item.svl_total;
                }

                if (i != 0 && registros50.Count > 0)
                {
                    var wregistro50Last = registros50.Last();
                    if (!string.IsNullOrEmpty(wregistro50Last.Cfop.ToString()))
                    {
                        if (wregistro50Last.Cfop == wregistro50.Cfop &&
                            wregistro50Last.AliquotaIcms == wregistro50.AliquotaIcms &&
                            wregistro50Last.Cnpj == wregistro50.Cnpj &&
                            wregistro50Last.InscrEstadual == wregistro50.InscrEstadual &&
                            wregistro50Last.Numero == wregistro50.Numero &&
                            wregistro50Last.Serie == wregistro50.Serie)
                        {
                            wregistro50Last.ValorTotal += wregistro50.ValorTotal;
                            wregistro50Last.BaseCalculoIcms += wregistro50.BaseCalculoIcms;
                            wregistro50Last.ValorIcms += wregistro50.ValorIcms;
                            wregistro50Last.ValorIsentaOuNaoTributadas += wregistro50.ValorIsentaOuNaoTributadas ?? 0;
                            wregistro50Last.ValorOutras += wregistro50.ValorOutras ?? 0;
                        }
                        else
                        {
                            registros50.Add(wregistro50);
                        }
                    }
                    else
                    {
                        registros50.Add(wregistro50);
                    }
                }
                else
                {
                    registros50.Add(wregistro50);
                }

                i++;
            }

            foreach (var r50 in registros50)
            {
                resultString += r50.EscreverCampos();
            }

            return resultString;
        }

        private void ObterValorEntradaDespesas(string nr_nf, out double vl_desconto, out double vl_frete, out double vl_seguro, out double vl_outras, ProcReg50EntradaResult item)
        {
            Entrada? entrada = db.Entradas.FromSqlRaw($@"
                select e.* from entradas e
                inner join fornecedor f on f.cd_forn = e.cd_forn and e.cd_empresa = e.cd_empresa
                where regexp_replace(f.cnpj, '[^0-9]', '', 'g') = '{UtlStrings.OnlyInteger(item.scnpj)}'
                and e.nr_nf = '{item.snr_nf}' and e.serie_nf = '{item.sserie_nf}'
            ").FirstOrDefault();

            vl_desconto = 0;
            vl_frete = 0;
            vl_seguro = 0;
            vl_outras = 0;

            if (entrada != null)
            {
                vl_desconto = Convert.ToDouble(entrada.VlDescontoNf ?? 0);
                vl_frete = Convert.ToDouble(entrada.VlFrete ?? 0);
                vl_seguro = Convert.ToDouble(entrada.VlSeguro ?? 0);
                vl_outras = Convert.ToDouble(entrada.VlOutras ?? 0);
            }
        }

        private ReadOnlySpan<byte> GetCfopReg50Ent(string? scd_cfop, ProcReg50EntradaResult item)
        {
            if (string.IsNullOrEmpty(scd_cfop) || scd_cfop.Length < 4)
            {
                throw new Exception($"Entrada com CFOP invalido (numero {item.snr_nf}, serie {item.sserie_nf})");
            }
            return Encoding.ASCII.GetBytes(scd_cfop);
        }

    }
}
