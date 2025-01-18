using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Models
{
    public class FnDistribuicaoDfeEntradasResult
    {

        [Column("id")]
        public Guid Id { get; set; }

        [Column("serie")]
        [StringLength(3)]
        public string? Serie { get; set; }

        [Column("nr_nota_fiscal")]
        [StringLength(20)]
        public string? Nr_Nota_Fiscal { get; set; }

        [Column("chave_acesso_nfe")]
        [StringLength(50)]
        public string? Chave_Acesso_Nfe { get; set; }

        [Column("cnpj")]
        [StringLength(20)]
        public string? Cnpj { get; set; }

        [Column("nome")]
        [StringLength(255)]
        public string? Nome { get; set; }

        [Column("ie")]
        [StringLength(30)]
        public string? Ie { get; set; }

        [Column("tp_nfe")]
        [StringLength(20)]
        public string? Tp_Nfe { get; set; }

        [Column("nsu")]
        [StringLength(20)]
        public string? Nsu { get; set; }

        [Column("emissao")]
        [StringLength(20)]
        public string Emissao { get; set; }

        [Column("valor")]
        public decimal? Valor { get; set; }

        [Column("impresso")]
        [StringLength(20)]
        public string? Impresso { get; set; }

        [Column("tp_resposta")]
        public string? Tp_Resposta { get; set; }

        [Column("manifesto")]
        public string? Manifesto { get; set; }

        [Column("transferiu")]
        public string? Transferiu { get; set; }

        [Column("dt_recebimento")]
        public DateOnly? Dt_Recebimento { get; set; }

        [Column("xml")]
        public string Xml { get; set; }

        [Column("dt_inclusao")]
        public DateOnly? Dt_Inclusao { get; set; }

        [Column("id_empresa")]
        public int Id_Empresa { get; set; }

        [Column("cstat_ciencia")]
        public int? Cstat_Ciencia { get; set; }

        [Column("xmotivo_ciencia")]
        [StringLength(512)]
        public string? Xmotivo_Ciencia { get; set; }

        [Column("tem_entrada")]
        [StringLength(1)]
        public string? Tem_Entrada { get; set; }
    }
}
