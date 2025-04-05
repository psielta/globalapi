using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class FotosProdutoDto
    {
        public int Id { get; set; }
        public int Unity { get; set; }
        public int CdProduto { get; set; }
        public string CaminhoFoto { get; set; } = null!;
        public bool Excluiu { get; set; }
        public string? DescricaoFoto { get; set; }
        public byte[]? Foto { get; set; }
    }
}
