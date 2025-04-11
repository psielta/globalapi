using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GlobalErpData.Dto.Uairango
{
    public class AtualizarFormasPagamentoRequestDto
    {
        [JsonProperty("formas")]
        public List<int> Formas { get; set; }
    }
}
