using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class OrcamentoCabDto
    {
        public int Unity { get; set; }

        public int Empresa { get; set; }

        public int IdCliente { get; set; }

        public string Gerado { get; set; } = null!;

        public int IdFuncionario { get; set; }

        public decimal PercentualComissao { get; set; }

        public decimal ValorProdutos { get; set; }

        public decimal ValorAcrescimo { get; set; }

        public decimal ValorDesconto { get; set; }

        public decimal ValorComissao { get; set; }

        public decimal ValorServicos { get; set; }

        public decimal ValorTotal { get; set; }

        public int CdPlano { get; set; }
    }
    
    
}
