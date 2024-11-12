using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class NfceSuprimentoCaixaDto
    {
        public int CdEmpresa { get; set; }
        public DateOnly? Data { get; set; }
        public TimeOnly? Hora { get; set; }
        public int IdUsuario { get; set; }
        public long NrAberturaCaixa { get; set; }
        public int IdCaixa { get; set; }
        public decimal Valor { get; set; }
        public string? Tipo { get; set; }
        public string? Obs { get; set; }
    }
}
