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

    [Column("whatsapp")]
    [StringLength(15)]
    public string? Whatsapp { get; set; }

    [Column("link_instagram")]
    [StringLength(512)]
    public string? LinkInstagram { get; set; }

    [Column("link_facebook")]
    [StringLength(512)]
    public string? LinkFacebook { get; set; }

    [Column("link_whatsapp")]
    [StringLength(512)]
    public string? LinkWhatsapp { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [JsonIgnore]
    [ForeignKey("IdEmpresa")]
    [InverseProperty("PerfilLojas")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    [GraphQLIgnore]
    public int GetId()
    {
        return Id;
    }

    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "Id";
    }
}
