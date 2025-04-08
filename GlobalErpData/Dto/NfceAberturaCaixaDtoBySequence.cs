using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;
using GlobalErpData.Models;

namespace GlobalErpData.Dto
{
    public class NfceAberturaCaixaDtoBySequence
    {
        public NfceAberturaCaixa abertura { get; set; }
        public ICollection<SangriaCaixa> sangrias { get; set; }
    }
}
