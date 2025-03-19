using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class ProdutosFornDto
    {
        public int CdProduto { get; set; }

        public int CdForn { get; set; }

        public string IdProdutoExterno { get; set; } = null!;

        public string CdBarra { get; set; } = null!;

        public int Unity { get; set; }
    }
}
