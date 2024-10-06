using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class ItemDetailDto
    {
        public int IdEmpresa { get; set; }

        public int IdProductDetails { get; set; }

        public string Value { get; set; } = null!;
    }
}
