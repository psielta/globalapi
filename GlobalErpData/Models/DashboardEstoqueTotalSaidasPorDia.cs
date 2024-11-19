using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Models
{
    public class DashboardEstoqueTotalSaidasPorDia
    {
        [Column("data")]
        public DateOnly Data { get; set; }

        [Column("vl_total")]
        public decimal VlTotal { get; set; }
    }
}
