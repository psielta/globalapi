using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class UploadCertificadoRequest
    {
        public int Id { get; set; }
        public CertificadoDto Dados { get; set; }
        public IFormFile ArquivoPfx { get; set; }
    }
}
