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
    public class OlderItemDto
    {
        public Guid? OlderId { get; set; }

        public int ItemNumber { get; set; }

        public string Name { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal Subtotal { get; set; }

        public int IdEmpresa { get; set; }

    public int Unity { get; set; }
        public int CdProduto { get; set; }
    }
}
