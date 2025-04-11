using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("uairango_tokens")]
public partial class UairangoToken
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("token_acesso")]
    [StringLength(4096)]
    public string TokenAcesso { get; set; } = null!;

    [Column("data_hora_geracao", TypeName = "timestamp(0) without time zone")]
    public DateTime? DataHoraGeracao { get; set; }
}
