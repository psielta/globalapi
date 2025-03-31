using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class OrcamentItenoPostDto
    {
        public int Unity { get; set; }
        public int Empresa { get; set; }
        public int IdCliente { get; set; }
        public string Gerado { get; set; } = null!;
        public int IdFuncionario { get; set; }
        public decimal PercentualComissao { get; set; }
        public int IdProduto { get; set; }
        public decimal Qtde { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorAcrescimo { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorComissao { get; set; }
        public decimal ValorTotal { get; set; }
        public Guid IdCab { get; set; }
        public long SequenciaCab { get; set; }
        public int CdPlano { get; set; }
    }

    public class OrcamentItenoPutDto
    {
        public Guid? Id { get; set; }
        public long? Sequencia { get; set; }
        public int Unity { get; set; }
        public int Empresa { get; set; }
        public int IdCliente { get; set; }
        public string Gerado { get; set; } = null!;
        public int IdFuncionario { get; set; }
        public decimal PercentualComissao { get; set; }
        public int IdProduto { get; set; }
        public decimal Qtde { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorAcrescimo { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorComissao { get; set; }
        public decimal ValorTotal { get; set; }
        public Guid IdCab { get; set; }
        public long SequenciaCab { get; set; }
        public int CdPlano { get; set; }
    }
    public class OrcamentoServicosPostDto
    {
        public int Unity { get; set; }

        public int Empresa { get; set; }

        public int IdCliente { get; set; }

        public string Gerado { get; set; } = null!;

        public int IdFuncionario { get; set; }

        public decimal PercentualComissao { get; set; }

        public int? CdMecanico { get; set; }

        public int? CdMecanico2 { get; set; }

        public string? TxtRelatoCliente { get; set; }

        public string? TxtDivergencia { get; set; }

        public string? TxtAvalTecnico { get; set; }

        public string? Lado { get; set; }

        public long IdServico { get; set; }

        public decimal Qtde { get; set; }

        public decimal ValorUnitario { get; set; }

        public decimal ValorAcrescimo { get; set; }

        public decimal ValorDesconto { get; set; }

        public decimal ValorComissao { get; set; }

        public decimal ValorTotal { get; set; }

        public Guid IdCab { get; set; }

        public long SequenciaCab { get; set; }
    }

    public class OrcamentoServicosPutDto
    {
        public Guid? Id { get; set; }
        public long? Sequencia { get; set; }
        public int Unity { get; set; }

        public int Empresa { get; set; }

        public int IdCliente { get; set; }

        public string Gerado { get; set; } = null!;

        public int IdFuncionario { get; set; }

        public decimal PercentualComissao { get; set; }

        public int? CdMecanico { get; set; }

        public int? CdMecanico2 { get; set; }

        public string? TxtRelatoCliente { get; set; }

        public string? TxtDivergencia { get; set; }

        public string? TxtAvalTecnico { get; set; }

        public string? Lado { get; set; }

        public long IdServico { get; set; }

        public decimal Qtde { get; set; }

        public decimal ValorUnitario { get; set; }

        public decimal ValorAcrescimo { get; set; }

        public decimal ValorDesconto { get; set; }

        public decimal ValorComissao { get; set; }

        public decimal ValorTotal { get; set; }

        public Guid IdCab { get; set; }

        public long SequenciaCab { get; set; }
    }

    public class OrcamentoPostDto
    {
        public int Unity { get; set; }

        public int Empresa { get; set; }

        public int IdCliente { get; set; }

        public string Gerado { get; set; } = null!;

        public int IdFuncionario { get; set; }

        public decimal PercentualComissao { get; set; }

        public decimal ValorProdutos { get; set; }

        public decimal ValorAcrescimo { get; set; }

        public decimal ValorDesconto { get; set; }

        public decimal ValorComissao { get; set; }

        public decimal ValorServicos { get; set; }

        public decimal ValorTotal { get; set; }

        public int CdPlano { get; set; }
        public ICollection<OrcamentItenoPostDto> OrcamentoItens { get; set; } = new List<OrcamentItenoPostDto>();
        public ICollection<OrcamentoServicosPostDto> OrcamentoServicos { get; set; } = new List<OrcamentoServicosPostDto>();
    }

    public class OrcamentoPutDto
    {
        public Guid Id { get; set; }
        public long Sequencia { get; set; }
        public int Unity { get; set; }
        public int Empresa { get; set; }

        public int IdCliente { get; set; }

        public string Gerado { get; set; } = null!;

        public int IdFuncionario { get; set; }

        public decimal PercentualComissao { get; set; }

        public decimal ValorProdutos { get; set; }

        public decimal ValorAcrescimo { get; set; }

        public decimal ValorDesconto { get; set; }

        public decimal ValorComissao { get; set; }

        public decimal ValorServicos { get; set; }

        public decimal ValorTotal { get; set; }

        public int CdPlano { get; set; }
        public ICollection<OrcamentItenoPutDto> OrcamentoItens { get; set; } = new List<OrcamentItenoPutDto>();
        public ICollection<OrcamentoServicosPutDto> OrcamentoServicos { get; set; } = new List<OrcamentoServicosPutDto>();
    }
}
