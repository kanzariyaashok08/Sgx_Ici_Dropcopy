using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgxICIDropCopy.API
{
    public interface iDataInterface
    {

        string MsgType
        {
            get;
            set;
        }

        int MsgSeqNum
        {
            get;
            set;
        }

        bool PossDupFlag
        {
            get;
            set;
        }

        string OrigSendingTime
        {
            get;
            set;
        }

        string SendingTime
        {
            get;
            set;
        }

        string Data
        {
            get;
        }
    }
}
