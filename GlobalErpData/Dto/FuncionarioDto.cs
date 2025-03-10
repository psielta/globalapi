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
    public class FuncionarioDto
    {
        public string NmFuncionario { get; set; } = null!;

        public DateOnly? DtNascimento { get; set; }

        public string? Endereco { get; set; }

        public int? Numero { get; set; }

        public string? Bairro { get; set; }

        public string? Cidade { get; set; }

        public string? NrTelefone { get; set; }

        public string? NrTelefone2 { get; set; }

        public string? NrCpf { get; set; }

        public string? NrRg { get; set; }

        public string? Sexo { get; set; }

        public string? EstadoCivil { get; set; }

        public DateOnly DtAdmissao { get; set; }

        public int CdEmpresa { get; set; }

        public string Cargo { get; set; } = null!;

        public decimal Salario { get; set; }

        public string? Cep { get; set; }

        public string? CdCbo { get; set; }

        public string? Mecanico { get; set; }

        public string? Vendedor { get; set; }

        public string? Ativo { get; set; }

        public string? CdInterno { get; set; }

        public string? Color { get; set; }

        public string? TxtObs { get; set; }

        public string? NrCarteira { get; set; }

        public string? Pasep { get; set; }

        public DateOnly? DataRescisao { get; set; }

        public decimal? SalarioCarteira { get; set; }

        public string? Registrado { get; set; }

        public string? Integrado { get; set; }

        public string? IdServCentral { get; set; }

        public DateTime? LastUpdate { get; set; }

        public int? Integrated { get; set; }

        public int Unity { get; set; }
    }
}
