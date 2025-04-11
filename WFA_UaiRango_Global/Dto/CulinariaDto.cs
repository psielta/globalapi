using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WFA_UaiRango_Global.Dto
{
    public class CulinariaDto
    {
        [JsonProperty("id_culinaria")]
        public int IdCulinaria { get; set; }

        [JsonProperty("nome")]
        public string Nome { get; set; }

        [JsonProperty("meio_meio")]
        public int MeioMeio { get; set; }
    }
}
