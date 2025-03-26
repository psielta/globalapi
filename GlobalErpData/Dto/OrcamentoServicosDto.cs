namespace GlobalErpData.Dto
{
    public class OrcamentoServicosDto
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
}