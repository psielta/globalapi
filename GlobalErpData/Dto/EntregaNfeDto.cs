using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class EntregaNfeDto
    {
        public int Unity { get; set; }
        public int IdCliente { get; set; }
        public string? Cnpjcpf { get; set; }
        public string? Ie { get; set; }
        public string? Xnome { get; set; }
        public string? Xlgr { get; set; }
        public string? Nro { get; set; }
        public string? Xcpl { get; set; }
        public string? Xbairro { get; set; }
        public int? Cmun { get; set; }
        public string? Xmun { get; set; }
        public string? Uf { get; set; }
        public string? Cep { get; set; }
        public string? Fone { get; set; }
        public string? Email { get; set; }
    }
}
