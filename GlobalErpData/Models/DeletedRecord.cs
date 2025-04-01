using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("deleted_records")]
public partial class DeletedRecord
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("table_name")]
    [StringLength(100)]
    public string TableName { get; set; } = null!;

    [Column("record_id")]
    public long RecordId { get; set; }

    [Column("dt_deleted", TypeName = "timestamp without time zone")]
    public DateTime DtDeleted { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [Column("aux_1")]
    [StringLength(255)]
    public string? Aux1 { get; set; }

    [Column("aux_2")]
    [StringLength(255)]
    public string? Aux2 { get; set; }

    [Column("aux_3")]
    [StringLength(255)]
    public string? Aux3 { get; set; }
}
