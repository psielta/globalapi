using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class NfceAberturaCaixaDto
    {
        public int? NrLanc { get; set; }
        public DateTime? DataLanc { get; set; }
        public int CdEmpresa { get; set; }
        public DateOnly DataAbertura { get; set; }
        public TimeOnly HoraAbertura { get; set; }
        public DateOnly? DataEncerramento { get; set; }
        public TimeOnly? HoraEncerramento { get; set; }
        public string CdOperador { get; set; } = null!;
        public string Status { get; set; } = null!;
        public decimal VlSuprimento { get; set; }
        public decimal? VlVendaFinal { get; set; }
        public string? CdGerente { get; set; }
        public decimal? VlVendaFinalCart { get; set; }
        public decimal? VlVendaFinalChq { get; set; }
        public decimal? VlVendaFinalPrazo { get; set; }
        public decimal? VlVendaFinalCartDeb { get; set; }
        public decimal? VlSangria { get; set; }
        public decimal? VlVendaFinalPix { get; set; }
        public decimal? VlVendaTicket { get; set; }
        public decimal? VlMoedas { get; set; }
        public decimal? VlBaixaFiado { get; set; }
        public decimal? IDinheiro { get; set; }
        public decimal? ICc { get; set; }
        public decimal? ICd { get; set; }
        public decimal? IPix { get; set; }
        public decimal? ICheque { get; set; }
        public decimal? IPrazo { get; set; }
        public decimal? IVale { get; set; }
        public decimal? ISangria { get; set; }
        public decimal? ISuprimento { get; set; }
        public decimal? ITotal { get; set; }
        public DateTime? LastUpdate { get; set; }
        public int? Integrated { get; set; }
        public int Unity { get; set; }
    }
}
