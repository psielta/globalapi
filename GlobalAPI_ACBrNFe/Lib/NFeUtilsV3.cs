namespace GlobalAPI_ACBrNFe.Lib
{
    using System;

    public static class NFeUtilsV3
    {
        /// <summary>
        /// Obtém informações da chave de acesso da NFe.
        /// </summary>
        /// <param name="chaveAcesso">Chave de acesso da NFe (44 dígitos).</param>
        /// <returns>Informações extraídas da chave de acesso.</returns>
        public static NFeInfo ObterInformacoesDaChave(string chaveAcesso)
        {
            if (string.IsNullOrWhiteSpace(chaveAcesso) || chaveAcesso.Length != 44)
            {
                throw new ArgumentException("A chave de acesso deve conter exatamente 44 dígitos numéricos.", nameof(chaveAcesso));
            }

            return new NFeInfo
            {
                UF = ObterUF(chaveAcesso.Substring(0, 2)),
                Ano = $"20{chaveAcesso.Substring(2, 2)}",
                Mes = chaveAcesso.Substring(4, 2),
                CNPJEmitente = FormatarCNPJ(chaveAcesso.Substring(6, 14)),
                Modelo = chaveAcesso.Substring(20, 2),
                Serie = chaveAcesso.Substring(22, 3),
                Numero = chaveAcesso.Substring(25, 9),
                TipoEmissao = chaveAcesso.Substring(34, 1),
                CodigoNumerico = chaveAcesso.Substring(35, 8),
                DV = chaveAcesso.Substring(43, 1)
            };
        }

        private static string ObterUF(string codigoUF)
        {
            return codigoUF switch
            {
                "12" => "AC",
                "27" => "AL",
                "13" => "AM",
                "16" => "AP",
                "29" => "BA",
                "23" => "CE",
                "53" => "DF",
                "32" => "ES",
                "52" => "GO",
                "21" => "MA",
                "31" => "MG",
                "50" => "MS",
                "51" => "MT",
                "15" => "PA",
                "25" => "PB",
                "26" => "PE",
                "22" => "PI",
                "41" => "PR",
                "33" => "RJ",
                "24" => "RN",
                "43" => "RS",
                "11" => "RO",
                "14" => "RR",
                "42" => "SC",
                "35" => "SP",
                "28" => "SE",
                "17" => "TO",
                _ => "UF Desconhecida"
            };
        }

        private static string FormatarCNPJ(string cnpj)
        {
            if (cnpj.Length != 14)
            {
                return cnpj;
            }

            return $"{cnpj.Substring(0, 2)}.{cnpj.Substring(2, 3)}.{cnpj.Substring(5, 3)}/{cnpj.Substring(8, 4)}-{cnpj.Substring(12, 2)}";
        }
    }

    /// <summary>
    /// Classe para armazenar informações da chave de acesso da NFe.
    /// </summary>
    public class NFeInfo
    {
        public string UF { get; set; }
        public string Ano { get; set; }
        public string Mes { get; set; }
        public string CNPJEmitente { get; set; }
        public string Modelo { get; set; }
        public string Serie { get; set; }
        public string Numero { get; set; }
        public string TipoEmissao { get; set; }
        public string CodigoNumerico { get; set; }
        public string DV { get; set; }

        public override string ToString()
        {
            return $"UF: {UF}, Ano: {Ano}, Mês: {Mes}, CNPJ Emitente: {CNPJEmitente}, Modelo: {Modelo}, Série: {Serie}, Número: {Numero}, Tipo de Emissão: {TipoEmissao}, Código Numérico: {CodigoNumerico}, DV: {DV}";
        }
    }

}
