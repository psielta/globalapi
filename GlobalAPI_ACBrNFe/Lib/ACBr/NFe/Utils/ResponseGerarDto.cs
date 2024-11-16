using ACBrLib.Core.DFe;
using GlobalErpData.Models;

namespace GlobalAPI_ACBrNFe.Lib.ACBr.NFe.Utils
{
    public class ResponseGerarDto
    {
        public EnvioRetornoResposta? envioRetornoResposta { get; set; } = null;
        public string pathPdf { get; set; } = "";
        public string xml { get; set; } = "";
        public bool success { get; set; } = true;
        public int StatusCode { get; set; } = 200;
        public string Message { get; set; } = "Success";
    }

    
}
