using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[PrimaryKey("Id", "IdEmpresa")]
[Table("fotos_produto")]
public partial class FotosProduto
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Key]
    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("cd_produto")]
    public int CdProduto { get; set; }

    [Column("caminho_foto")]
    public string CaminhoFoto { get; set; } = null!;

    [Column("excluiu")]
    public bool Excluiu { get; set; }

    [Column("descricao_foto")]
    [StringLength(255)]
    public string? DescricaoFoto { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("FotosProdutos")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    [ForeignKey("CdProduto, IdEmpresa")]
    [InverseProperty("FotosProdutos")]
    public virtual ProdutoEstoque ProdutoEstoque { get; set; } = null!;
}
