using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgxICIDropCopy.RepeatingGrp
{
    public class LegInstrumentGrp
    {

        public string LegSecurityExchange { get; set; }


        public string LegExDestination { get; set; }


        public string LegSecurityId { get; set; }


        public string LegIDSource { get; set; }


        public string LegSymbol { get; set; }


        public string LegCFICode { get; set; }


        public string LegSecurityDesc { get; set; }


        public string LegProduct { get; set; }


        public string LegSecurityType { get; set; }


        public string LegSecuritySubType { get; set; }


        public string LegMaturityMonthYear { get; set; }


        public string LegMaturityDate { get; set; }


        public int LegMaturityDay { get; set; }


        public decimal LegStrikePrice { get; set; }


        public int LegPutOrCall { get; set; }


        public char LegSide { get; set; }


        public int LegRatioQty { get; set; }

        public string LegCurrency { get; set; }

        public decimal LegPrice { get; set; }

        public int LegOrderQty { get; set; }

        public string LegRefID { get; set; }

        public decimal LegLastPx { get; set; }

        public string LegContractYearMonth { get; set; }

        public char LegDeliveryTerm { get; set; }

        public string LegDeliveryDate { get; set; }

        public string LegAllocID { get; set; }

        public int NoLegSecurityAltID { get; set; }

    }
}
