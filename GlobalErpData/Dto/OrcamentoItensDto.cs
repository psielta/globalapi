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
    public class OrcamentoItensDto
    {
        public int Unity { get; set; }
        public int Empresa { get; set; }
        public int IdCliente { get; set; }
        public string Gerado { get; set; } = null!;
        public int IdFuncionario { get; set; }
        public decimal PercentualComissao { get; set; }
        public int IdProduto { get; set; }
        public decimal Qtde { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorAcrescimo { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorComissao { get; set; }
        public decimal ValorTotal { get; set; }
        public Guid IdCab { get; set; }
        public long SequenciaCab { get; set; }
        public int CdPlano { get; set; }
    }
    
    
}
