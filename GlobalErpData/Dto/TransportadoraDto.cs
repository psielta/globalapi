using GlobalErpData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class TransportadoraDto
    {
        public int CdTransportadora { get; set; }
        public string? NmTransportadora { get; set; }
        public string? NmEndereco { get; set; }
        public int? Numero { get; set; }
        public string? NmBairro { get; set; }
        public string? CdCidade { get; set; }
        public string? CdCnpj { get; set; }
        public string? CdIe { get; set; }
        public string? PlacaVeiculo { get; set; }
        public string? NrTelefone { get; set; }
        public string? NrTelefone2 { get; set; }
        public string? Email { get; set; }
        public string? Cep { get; set; }
        public int IdEmpresa { get; set; }
    }
}
