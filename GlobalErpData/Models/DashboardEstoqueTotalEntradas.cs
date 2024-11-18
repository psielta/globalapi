using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Models
{
    public class DashboardEstoqueTotalEntradas
    {
        [Column("total")]
        public decimal Total { get; set; }
    }
}
