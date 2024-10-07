using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class GetOldersDto
    {
        public Guid Id { get; set; }
        public int IdEmpresa { get; set; }
        public DateTime CreatedAt { get; set; }
        public StatusOlder Status { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerEmail { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Shipping { get; set; }
        public decimal Taxes { get; set; }
        public decimal Total { get; set; }
        public virtual ICollection<GetOlderItemDto> Items { get; set; } = new List<GetOlderItemDto>();
    }

    public class GetOlderItemDto
    {
        public Guid Id { get; set; }
        public Guid? OlderId { get; set; }
        public int ItemNumber { get; set; }
        public string Name { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal { get; set; }
        public int IdEmpresa { get; set; }
        public int CdProduto { get; set; }
    }
}
