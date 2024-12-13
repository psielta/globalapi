using AutoMapper;
using FiscalBr.Common;
using FiscalBr.Common.Sintegra;
using FiscalBr.Sintegra;
using GlobalAPI_ACBrNFe.Lib;
using GlobalErpData.Data;
using GlobalErpData.Dto;
using GlobalErpData.Models;
using GlobalLib.Strings;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Serilog;
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

        private int mes;
        private int ano;
        private DateTime dataInicial;
        private DateTime dataFinal;
        private int CodFinalidadeArquivo;

        public SintegraService(GlobalErpFiscalBaseContext db, ILogger<SintegraService> logger, IMapper mapper, IHubContext<ImportProgressHub> hubContext)
        {
            this.db = db;
            this.logger = logger;
            this.mapper = mapper;
            _hubContext = hubContext;
        }

        public async Task<string> Core(SintegraDto sintegraDto) {
            await SetDados(sintegraDto);
            var registro10 = await GetRegistro10();
            return $"{registro10}\n";
        }

        private async Task SetDados(SintegraDto sintegraDto)
        {
            this.sessionId = sintegraDto.SessionId;
            this.mes = sintegraDto.Mes;
            this.ano = sintegraDto.Ano;
            dataInicial = new DateTime(ano, mes, 1);
            dataFinal = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));
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
    }
}
