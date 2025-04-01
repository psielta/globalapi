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
    public class SangriaCaixaDto
    {
        public int? Id { get; set; }
        public int CdEmpresa { get; set; }
        public DateOnly? Data { get; set; }
        public TimeOnly? Hora { get; set; }
        public int IdUsuario { get; set; }
        public int IdAberturaCaixa { get; set; }
        public int IdCaixa { get; set; }
        public decimal Valor { get; set; }
        public string? Tipo { get; set; }
        public string? Obs { get; set; }
        public DateTime? LastUpdate { get; set; }
        public int? Integrated { get; set; }
        public int Unity { get; set; }
    }
}
