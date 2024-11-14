using GlobalLib.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public partial class ClienteDto
    {
        public string NmCliente { get; set; } = null!;

        public string? Ativo { get; set; }

        public string? NmEndereco { get; set; }

        public string? Numero { get; set; }

        public string CdCidade { get; set; } = null!;

        public string? NmBairro { get; set; }

        public string? Cep { get; set; }

        public string? Telefone { get; set; }

        public string? EMail { get; set; }

        public string? TpDoc { get; set; }

        public string? NrDoc { get; set; }

        public int IdEmpresa { get; set; }

        public string? TxtObs { get; set; }

        public int IdUsuarioCad { get; set; }

        public int? IdCteAntigo { get; set; }

        public string? InscricaoEstadual { get; set; }
        public bool? Mva { get; set; }

        public int? TpRegime { get; set; }
        public bool ConsumidorFinal { get; set; }


    }

}
