using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class ConfiguracoesEmpresaDto
    {
        public string Chave { get; set; } = null!;

        public string? Valor1 { get; set; }

        public string? Valor2 { get; set; }

        public string? Valor3 { get; set; }

        public int CdEmpresa { get; set; }
    }
}
