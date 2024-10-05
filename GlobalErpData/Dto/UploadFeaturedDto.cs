using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class UploadFeaturedDto
    {
        [FromForm(Name = "id")]
        public int Id { get; set; }

        [FromForm(Name = "categoryId")]
        public int CategoryId { get; set; }

        [FromForm(Name = "name")]
        public string? Name { get; set; }

        //[FromForm(Name = "href")]
        //public string? Href { get; set; }

        [FromForm(Name = "imageAlt")]
        public string? ImageAlt { get; set; }

        [FromForm(Name = "idEmpresa")]
        public int IdEmpresa { get; set; }

        [FromForm(Name = "excluiu")]
        public bool? Excluiu { get; set; }

        [FromForm(Name = "foto")]
        public IFormFile Foto { get; set; }
    }
}
