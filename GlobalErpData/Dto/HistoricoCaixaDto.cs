using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class HistoricoCaixaDto
    {
        public int CdEmpresa { get; set; }
        public string CdSubPlano { get; set; } = null!;
        public string CdPlano { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public string Descricao { get; set; } = null!;
    }
}
