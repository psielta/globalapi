using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("perfil_loja")]
public partial class PerfilLoja : IIdentifiable<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("nome")]
    [StringLength(100)]
    public string Nome { get; set; } = null!;

    [Column("descricao")]
    [StringLength(255)]
    public string Descricao { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdEmpresa")]
    [InverseProperty("PerfilLojas")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    public int GetId()
    {
        return Id;
    }

    public string GetKeyName()
    {
        return "Id";
    }
}
