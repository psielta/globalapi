using ACBrLib.Core.DFe;

namespace GlobalAPI_ACBrNFe.Lib.ACBr.NFe.Utils
{
    public class ResponseGerarDto
    {
        public EnvioRetornoResposta? envioRetornoResposta { get; set; } = null;
        public string pathPdf { get; set; } = "";
        public string xml { get; set; } = "";
    }

    
}
