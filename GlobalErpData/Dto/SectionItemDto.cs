using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class SectionItemDto
    {
        public int? SectionId { get; set; }

        public string? Name { get; set; }

        public string? Href { get; set; }

        public int Unity { get; set; }
    }
}
