using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class SectionDto
    {
        public int? CategoryId { get; set; }

        public string? SectionId { get; set; }

        public string? Name { get; set; }

        public int IdEmpresa { get; set; }
    }
}
