﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class PlanoCaixaDto
    {
        public int Unity { get; set; }

        public string CdClassificacao { get; set; } = null!;

        public string Descricao { get; set; } = null!;
    }
}
