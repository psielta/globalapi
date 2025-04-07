using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlobalErpData.Models;

namespace GlobalErpData.Dto
{
    public class NfceSaidaDtoBySequence
    {
        public NfceSaida cabecalho { get; set; }
        public virtual ICollection<NfceProdutoSaidum> itens { get; set; } 
        public virtual ICollection<NfceFormaPgt> formaPagamento { get; set; }
        public virtual Empresa empresa { get; set; } = null!;
        public virtual Cliente cliente { get; set; } = null!;


    }
}
