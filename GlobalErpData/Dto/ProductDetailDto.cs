using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class ProductDetailDto
    {
        public int Unity { get; set; }

        public int IdProduto { get; set; }

        public string Name { get; set; } = null!;

    }
}
