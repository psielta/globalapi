using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Models
{
    public class DashboardEstoqueTotalEntradasPorMes
    {
        [Column("nome_mes")]
        public string NomeMes { get; set; }

        [Column("mes_numero")]
        public int MesNumero { get; set; }

        [Column("total")]
        public decimal Total { get; set; }
    }
}
