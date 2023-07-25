using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgxICIDropCopy.RepeatingGrp
{
    public class LegFillsGrp
    {
        /// <summary>
        /// Unique identifier of leg execution as assigned by sell-side (broker, exchange, ECN). The ID must not overlap tag 17 (ExecID).
        /// Condition: Sent when tag 16120 > 0
        /// Required Tag : C
        /// </summary>
        public string LegFillExecID { get; set; }

        /// <summary>
        /// Price of this leg fill
        /// Condition: Sent when tag 16120 > 0
        /// Required Tag : C
        /// </summary>
        public decimal LegFillPx { get; set; }

        /// <summary>
        /// Quantity of this leg fill
        /// Condition: Sent when tag 16120 > 0
        /// Required Tag : C
        /// </summary>
        public int LegFillQty { get; set; }

        /// <summary>
        /// Trading Venue transaction identification code of this leg fill
        /// Condition: Sent if available when tag 16120 > 0
        /// Required Tag : C
        /// </summary>
        public string LegFillTradingVenueRegulatoryTradeID { get; set; }

        /// <summary>
        /// Whether this leg fill was a result of a liquidity provider providing or a liquidity taker taking the liquidity in this LegFillsGrp repeating group.
        /// Possible values include:
        /// 1: Added liquidity
        /// 2: Removed liquidity
        /// Required Tag : N
        /// </summary>
        public int LegFillLastLiquidityIndicator { get; set; }
    }
}
