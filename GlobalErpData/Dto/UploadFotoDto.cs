using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class UploadFotoDto
    {
        [FromForm(Name = "id")]
        public int Id { get; set; }

        [FromForm(Name = "idEmpresa")]
        public int IdEmpresa { get; set; }

        [FromForm(Name = "cdProduto")]
        public int CdProduto { get; set; }

        [FromForm(Name = "descricaoFoto")]
        public string DescricaoFoto { get; set; }

        [FromForm(Name = "foto")]
        public IFormFile Foto { get; set; }
    }

}
