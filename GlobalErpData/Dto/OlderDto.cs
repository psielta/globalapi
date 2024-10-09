using GlobalErpData.Models;
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
    public class OlderDto
    {
        public int IdEmpresa { get; set; }

        public DateTime CreatedAt { get; set; }

        public StatusOlder Status { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerName { get; set; } = null!;
        public string CustomerAddress { get; set; } = null!;

        public string? CustomerPhone { get; set; }

        public string? CustomerEmail { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Discount { get; set; }

        public decimal Shipping { get; set; }

        public decimal Taxes { get; set; }

        public decimal Total { get; set; }

        public string CustomerCity { get; set; } = null!;

        public string CustomerNeighborhood { get; set; } = null!;

        public string CustomerNumber { get; set; } = null!;

        public string CustomerZip { get; set; } = null!;

        public string? CustomerComplement { get; set; }

        public string? CustomerReference { get; set; }
    }
}
