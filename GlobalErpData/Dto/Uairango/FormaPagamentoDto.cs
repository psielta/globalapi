using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GlobalErpData.Dto.Uairango
{
    public class FormaPagamentoDto
    {
        [JsonProperty("id_forma")]
        public int IdForma { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("ativo")]
        public int Ativo { get; set; }
    }
}
