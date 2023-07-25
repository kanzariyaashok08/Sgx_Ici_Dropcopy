using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgxICIDropCopy.API
{
    public class FIXMessageType
    {
        public const string Heartbeat = "0";
        public const string TestRequest = "1";
        public const string ResendRequest = "2";
        public const string Reject = "3";
        public const string SequenceReset = "4";
        public const string Logout = "5";
        public const string ExecutionReport = "8";
        public const string OrderCancelReject = "9";
        public const string Logon = "A";
        public const string News = "B";
        public const string SecurityDefinitionRequest = "c";
        public const string OrderSingle = "D";
        public const string SecurityDefinition = "d";
        public const string SecurityStatusRequest = "e";
        public const string SecurityStatus = "f";
        public const string OrderCancelRequest = "F";
        public const string OrderCancelReplaceRequest = "G";
        public const string OrderStatusRequest = "H";
        public const string MarketDataRequest = "V";
        public const string MarketDataSnapshotFullRefresh = "W";
        public const string MarketDataIncrementalRefresh = "X";
        public const string MarketDataRequestReject = "Y";
        public const string TradeCaptureReport = "AE";
        public const string TradeCaptureReportAck = "AR";


        public static string GetMsgType(string MsgType)
        {
            string StrMsgType = string.Empty;
            switch (MsgType)
            {
                case "0":
                    StrMsgType = "Heartbeat";
                    break;

                case "1":
                    StrMsgType = "TestRequest";
                    break;

                case "2":
                    StrMsgType = "ResendRequest";
                    break;

                case "3":
                    StrMsgType = "Reject";
                    break;

                case "4":
                    StrMsgType = "SequenceReset";
                    break;

                case "5":
                    StrMsgType = "Logout";
                    break;

                case "8":
                    StrMsgType = "ExecutionReport";
                    break;

                case "9":
                    StrMsgType = "OrderCancelReject";
                    break;

                case "A":
                    StrMsgType = "Logon";
                    break;

                case "B":
                    StrMsgType = "News";
                    break;

                case "D":
                    StrMsgType = "OrderSingle";
                    break;

                case "G":
                    StrMsgType = "OrderCancelReplaceRequest";
                    break;

                case "F":
                    StrMsgType = "OrderCancelRequest";
                    break;

                case "AR":
                    StrMsgType = "TradeCaptureReportAck";
                    break;

                case "AE":
                    StrMsgType = "TradeCaptureReport";
                    break;

                default:
                    break;
            }

            return StrMsgType;
        }
    }


}
