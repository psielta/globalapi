using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class EmpresaDto
    {
        public string NmEmpresa { get; set; } = null!;

        public string NmEndereco { get; set; } = null!;

        public int Numero { get; set; }

        public string CdCidade { get; set; } = null!;

        public string CdCep { get; set; } = null!;

        public string? CdCnpj { get; set; }

        public string? NmBairro { get; set; }

        public string? Telefone { get; set; }

        public string? NrInscrMunicipal { get; set; }

        public string? NrInscrEstadual { get; set; }

        public string? TxtObs { get; set; }

        public string? EMail { get; set; }

        public string? Idcsc { get; set; }
        public string? Csc { get; set; }
        public string? AutorizoXml { get; set; }
        public string? CpfcnpfAutorizado { get; set; }
        public string? NomeFantasia { get; set; }
        public int? TipoRegime { get; set; }
        public string? MailContador { get; set; }
        public string? Iest { get; set; }
        public string? Complemento { get; set; }
        public string? Cnae { get; set; }
        public int Unity { get; set; }

        public bool? UairangoVinculado { get; set; }

        public string? UairangoIdEstabelecimento { get; set; }

        public string? UairangoTokenVinculo { get; set; }
    }
}
