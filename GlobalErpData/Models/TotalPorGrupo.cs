using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Models
{
    public class TotalPorGrupo
    {
        [Column("nm_grupo")]
        public string NmGrupo { get; set; }

        [Column("total")]
        public decimal Total { get; set; }
    }
}
