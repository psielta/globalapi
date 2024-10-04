using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class ImpcabnfeDto
    {
        public string ChNfe { get; set; } = null!;
        public string? Tipo { get; set; }
        public string? NrNf { get; set; }
        public string? Modelo { get; set; }
        public string? Cnfe { get; set; }
        public string? TpPagt { get; set; }
        public DateTime? DtEmissao { get; set; }
        public DateTime? DtSaida { get; set; }
        public string? Cnpj { get; set; }
        public string? Ie { get; set; }
        public string? Nome { get; set; }
        public string? Fone { get; set; }
        public string? Cep { get; set; }
        public string? Endereco { get; set; }
        public string? Numero { get; set; }
        public string? Bairro { get; set; }
        public string? CdCidade { get; set; }
        public string? InfObs { get; set; }
        public string? TpFrete { get; set; }
        public string? CnpjTransp { get; set; }
        public string? NomeTransp { get; set; }
        public string? EndTransp { get; set; }
        public string? CidadeTransp { get; set; }
        public string? UfTransp { get; set; }
        public int? CdUfTransp { get; set; }
        public string? IdNota { get; set; }
        public string? Serie { get; set; }
        public string? Caminho { get; set; }
        public string? NrAutorizacao { get; set; }
        public string? XmlNota { get; set; }
        public string? TPag { get; set; }
        public string? CnpjAvulsa { get; set; }
    }
}
