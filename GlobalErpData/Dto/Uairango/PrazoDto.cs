using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GlobalErpData.Dto.Uairango
{
    public class PrazoDto
    {
        [JsonProperty("id_tempo")]
        public int IdTempo { get; set; }

        [JsonProperty("min")]
        public int Min { get; set; }

        [JsonProperty("max")]
        public int Max { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; } // 0 = ativo, 1 = desativado
    }
}
