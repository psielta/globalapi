using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GlobalErpData.Dto.Uairango
{
    public class EstabelecimentoConfigDto
    {
        [JsonProperty("id_estabelecimento")]
        public int IdEstabelecimento { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("status_estabelecimento")]
        public int StatusEstabelecimento { get; set; } // 0 = fechado, 1 = aberto

        [JsonProperty("status_delivery")]
        public int StatusDelivery { get; set; } // 0 = não aceita, 1 = aceita

        [JsonProperty("id_tempo_delivery")]
        public int? IdTempoDelivery { get; set; }

        [JsonProperty("prazo_delivery")]
        public int? PrazoDelivery { get; set; }

        [JsonProperty("status_retirada")]
        public int StatusRetirada { get; set; } // 0 = não aceita, 1 = aceita

        [JsonProperty("id_tempo_retirada")]
        public int? IdTempoRetirada { get; set; }

        [JsonProperty("prazo_retirada")]
        public int? PrazoRetirada { get; set; }
    }
}
