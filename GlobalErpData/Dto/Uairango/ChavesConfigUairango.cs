using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto.Uairango
{
    /// <summary>
    /// Contains constant keys used for configuration in the Uairango system.
    /// </summary>
    public static class ChavesConfigUairango
    {
        /// <summary>
        /// Represents the status of the establishment.
        /// 0 if the establishment is closed.
        /// 1 if the establishment is open.
        /// </summary>
        public const string STATUS_ESTABELECIMENTO = "status_estabelecimento";

        /// <summary>
        /// Represents the delivery status of the establishment.
        /// 0 if the establishment is not accepting delivery.
        /// 1 if the establishment is accepting delivery.
        /// </summary>
        public const string STATUS_DELIVERY = "status_delivery";

        /// <summary>
        /// Represents the delivery time ID.
        /// Must be a valid option from the /auth/info/prazos route.
        /// </summary>
        public const string ID_TEMPO_DELIVERY = "id_tempo_delivery";

        /// <summary>
        /// [Deprecated] Represents the time (in minutes) required to complete a delivery.
        /// </summary>
        public const string PRAZO_DELIVERY = "prazo_delivery";

        /// <summary>
        /// Represents the pickup status of the establishment.
        /// 0 if the establishment is not accepting pickup.
        /// 1 if the establishment is accepting pickup.
        /// </summary>
        public const string STATUS_RETIRADA = "status_retirada";

        /// <summary>
        /// Represents the pickup time ID.
        /// Must be a valid option from the /auth/info/prazos route.
        /// </summary>
        public const string ID_TEMPO_RETIRADA = "id_tempo_retirada";

        /// <summary>
        /// [Deprecated] Represents the time (in minutes) required to complete a pickup.
        /// </summary>
        public const string PRAZO_RETIRADA = "prazo_retirada";
    }
}
