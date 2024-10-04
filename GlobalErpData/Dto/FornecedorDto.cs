using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class FornecedorDto
    {
        public string NmForn { get; set; } = null!;

        public string? NmEndereco { get; set; }

        public int? Numero { get; set; }
        public string CdCidade { get; set; } = null!;

        public string? CdCep { get; set; }

        public string? NmRepresentante { get; set; }

        public string? TelefoneEmpresa { get; set; }

        public string? TelefoneRepresentante { get; set; }

        public string? Cnpj { get; set; }

        public string? Bairro { get; set; }

        public string? Ramo { get; set; }

        public string? Email { get; set; }

        public string? NrInscrEstadual { get; set; }

        public string? Parceiro { get; set; }

        public string? NmFantasia { get; set; }

        public string? Complemento { get; set; }

        public int? IdCliente { get; set; }

        public string? Cpf { get; set; }

        public int IdEmpresa { get; set; }
    }
}
