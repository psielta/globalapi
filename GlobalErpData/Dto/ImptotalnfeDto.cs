using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class ImptotalnfeDto
    {
        public string ChNfe { get; set; } = null!;
        public string? IcmsVbc { get; set; }
        public string? IcmsValor { get; set; }
        public string? IcmsVbcst { get; set; }
        public string? IcmsSt { get; set; }
        public string? IcmsVprod { get; set; }
        public string? IcmsFrete { get; set; }
        public string? IcmsSeg { get; set; }
        public string? IcmsDesc { get; set; }
        public string? IcmsVii { get; set; }
        public string? IcmsVipi { get; set; }
        public string? IcmsVpis { get; set; }
        public string? IcmsVconfins { get; set; }
        public string? IcmsOutros { get; set; }
        public string? IcmsVnf { get; set; }
        public string? RetPis { get; set; }
        public string? RetConfins { get; set; }
        public string? RetCsll { get; set; }
        public string? RetIrrf { get; set; }
        public string? RetBirrf { get; set; }
        public string? RetBcvprev { get; set; }
        public string? RetVprev { get; set; }
        public int? Vicmsdeson { get; set; }
        public int? IcmsVfcpst { get; set; }
        public int? IcmsVipidevol { get; set; }
    }
}
