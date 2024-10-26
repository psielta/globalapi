using GlobalErpData.Models;

namespace GlobalAPI_ACBrNFe.Models
{
    public class ImpNFeTemp
    {
        public Impcabnfe? impcabnfe { get; set; }
        public Imptotalnfe? imptotalnfe { get; set; }
        public List<Impitensnfe>? impitensnves { get; set; }
        public List<Impdupnfe>? impdupnfe { get; set; }
        public List<Amarracao>? amarracoes { get; set; }
    }

    public class ImpNFeTemp2
    {
        public Impcabnfe? impcabnfe { get; set; }
        public Imptotalnfe? imptotalnfe { get; set; }
        public List<Impitensnfe>? impitensnves { get; set; }
        public List<Impdupnfe>? impdupnfe { get; set; }
        public List<Amarracao2>? amarracoes { get; set; }
    }
}
