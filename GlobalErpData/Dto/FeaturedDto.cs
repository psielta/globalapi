using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class FeaturedDto
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Href { get; set; }
        public string? ImageSrc { get; set; }
        public string? ImageAlt { get; set; }
        public int IdEmpresa { get; set; }
    }
}
