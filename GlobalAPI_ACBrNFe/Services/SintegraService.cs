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
            return SintegraTxt;
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
