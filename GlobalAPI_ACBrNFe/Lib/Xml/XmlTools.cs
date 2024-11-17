using ACBrLib.NFe;
using System.Globalization;
using System.Text;
using System.Xml;

namespace GlobalAPI_ACBrNFe.Lib.Xml
{
    public static class XmlTools
    {
        public static string LinearizarXml(string xml)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            // Remove espaços em branco entre elementos
            xmlDoc.PreserveWhitespace = false;

            return xmlDoc.OuterXml;
        }

        private static string GetGrupoAutorizacao(ConsultaNFeResposta resposta)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true
            };

            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb, settings))
            {
                writer.WriteStartElement("protNFe", "http://www.portalfiscal.inf.br/nfe");
                writer.WriteAttributeString("versao", "4.00");

                writer.WriteStartElement("infProt");

                writer.WriteElementString("tpAmb", ((int)resposta.tpAmb).ToString());
                writer.WriteElementString("verAplic", resposta.VerAplic);
                writer.WriteElementString("chNFe", resposta.ChNFe);
                writer.WriteElementString("dhRecbto", resposta.DhRecbto.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture));
                writer.WriteElementString("nProt", resposta.NProt);
                writer.WriteElementString("digVal", resposta.DigVal);
                writer.WriteElementString("cStat", resposta.CStat.ToString());
                writer.WriteElementString("xMotivo", resposta.XMotivo);

                writer.WriteEndElement(); // Fecha infProt
                writer.WriteEndElement(); // Fecha protNFe
            }

            return sb.ToString();
        }

        public static string AdicionarGrupoAutorizacao(string xmlAntigo, ConsultaNFeResposta resposta)
        {
            if (string.IsNullOrEmpty(xmlAntigo))
                return string.Empty;

            if (xmlAntigo.Contains("protNFe"))
                return xmlAntigo;

            var xmlGrupoAutorizacao = GetGrupoAutorizacao(resposta);
            string xmlAntigoSemCabecalho = xmlAntigo.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");

            return LinearizarXml($@"<?xml version=""1.0"" encoding=""UTF-8""?>
                <nfeProc versao=""4.00"" xmlns=""http://www.portalfiscal.inf.br/nfe"">
                {xmlAntigoSemCabecalho}
                {xmlGrupoAutorizacao}
                </nfeProc>");
        }
    }

}
