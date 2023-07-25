using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgxICIDropCopy.API
{
    public class FIXEnum
    {
        public enum ExecTransType
        {
            New = '0',
            Cancel = '1',
            Correct = '2',
            Status = '3'
        }

        public enum ExecType
        {
            New = '0',
            PartialFilled = '1',
            Filled = '2',
            DoneforDay = '3',
            Cancelled = '4',
            Replaced = '5',
            PendingCancel = '6',
            Stopped = '7',
            Rejected = '8',
            PendingNew = 'A',
            Calculated = 'B',
            Expired = 'C',
            Restated = 'D',
            PendingReplace = 'E',
            Trade = 'F',
            TradeCorrection = 'G',
            TradeCancel = 'H',
            OrderStatus = 'I',
            TradeInAClearingHold = 'J',
            TradeHasBeenReleasedToClearing = 'K',
            TriggeredOrActivatedBySystem = 'L'
        }

        public enum OrderStatus
        {
            New = '0',
            PartialFilled = '1',
            Filled = '2',
            DoneforDay = '3',
            Cancelled = '4',
            Replaced = '5',
            PendingCancel = '6',
            Stopped = '7',
            Rejected = '8',
            Suspended = '9',
            Pendingnew = 'A',
            Calculated = 'B',
            Expired = 'C',
            Restated = 'D',
            Pendingreplace = 'E'
        }
    }
}
