using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class CertificadoDto
    {
        public int IdEmpresa { get; set; }

        public string SerialCertificado { get; set; } = null!;

        public string? Senha { get; set; }

        public string? CaminhoCertificado { get; set; }

        public DateOnly? ValidadeCert { get; set; }

        public string? Certificado1 { get; set; }

        public string? Tipo { get; set; }

        public byte[]? CertificadoByte { get; set; }
    }
}
