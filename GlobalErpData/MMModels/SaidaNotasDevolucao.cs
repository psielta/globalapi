﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("saida_notas_devolucao")]
[Index("NrSaida", "ChaveAcesso", Name = "saida_notas_devolucao_idx", IsUnique = true)]
public partial class SaidaNotasDevolucao
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nr_saida")]
    public int NrSaida { get; set; }

    [Column("chave_acesso")]
    [StringLength(44)]
    public string? ChaveAcesso { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [ForeignKey("NrSaida")]
    [InverseProperty("SaidaNotasDevolucaos")]
    public virtual Saida NrSaidaNavigation { get; set; } = null!;
}
