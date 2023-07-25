using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SgxICIDropCopy.API;

namespace SgxICIDropCopy.DropCopyMsg
{
    public class TradeCaptureReport : MessageHeader
    {
        #region Constructor
        public TradeCaptureReport(string MsgData)
        {
            Generate(MsgData);
        }
        #endregion

        #region Properties

        public string TradeReportID { get; set; }

        public string TradeReportRefId { get; set; }

        public string ExecID { get; set; }

        public char ExecType { get; set; }

        public int TradeReportTransType { get; set; }

        public int TradeReportType { get; set; }

        public char TradeHandlingInstr { get; set; }

        public int TrdType { get; set; }

        public int LastShares { get; set; }

        public int LeavesQty { get; set; }

        public decimal LastPx { get; set; }

        public decimal AvgPx { get; set; }

        public string TransactTime { get; set; }

        public string TradeDate { get; set; }

        public string OrigTradeDate { get; set; }

        public string TradeLinkId { get; set; }

        public string TradeId { get; set; }

        public string OrigTradeId { get; set; }

        public string TrdMatchId { get; set; }

        public decimal FutureReferencePrice { get; set; }

        public char MultiLegReportingType { get; set; }

        public string TransBkdTime { get; set; }

        #region Instrument

        public string SecurityID { get; set; }

        public string IDSource { get; set; }

        public string SecurityExchange { get; set; }

        public string ExDestination { get; set; }

        public string LastMkt { get; set; }

        public string Symbol { get; set; }

        public string CFICode { get; set; }

        public string SecurityType { get; set; }

        public int Product { get; set; }

        public string SecurityDesc { get; set; }

        public string MaturityMonthYear { get; set; }

        public string MaturityDate { get; set; }

        public int MaturityDay { get; set; }

        public string ContractYearMonth { get; set; }

        public char DeliveryTerm { get; set; }

        public string DeliveryDate { get; set; }

        public int PutOrCall { get; set; }

        public decimal StrikePrice { get; set; }

        public float DisplayFactor { get; set; }

        public string Currency { get; set; }

        public string AllocID { get; set; }

        public int NoSecurityAltID { get; set; }

        public string SecuritySubType { get; set; }

        #endregion

        public string AccountRiskGroup { get; set; }

        #endregion

        public void Generate(string Msgdata)
        {
            string[] arrMsgString = Msgdata.Split(FixConst.Delimeter);
            for (int i = 0; i < arrMsgString.Length; i++)
            {
                if (string.IsNullOrEmpty(arrMsgString[i])) continue;
                string[] arrTagValue = arrMsgString[i].Split(FixConst.Equalto);

                int Tag = Convert.ToInt32(arrTagValue[0]);
                string value = arrTagValue[1];

                switch (Tag)
                {
                    #region  MessageHeaderRequest

                    case FIXTAG.BeginString:
                        BeginString = value;
                        break;

                    case FIXTAG.BodyLength:
                        BodyLength = Convert.ToInt32(value);
                        break;

                    case FIXTAG.MsgType:
                        MsgType = value;
                        break;

                    case FIXTAG.SenderCompID:
                        SenderCompID = value;
                        break;

                    case FIXTAG.TargetCompID:
                        TargetCompID = value;
                        break;


                    case FIXTAG.MsgSeqNum:
                        MsgSeqNum = Convert.ToInt32(value);
                        break;

                    case FIXTAG.PossDupFlag:
                        PossDupFlag = FIXMethods.SetBooleanValue(value);
                        break;

                    case FIXTAG.OrigSendingTime:
                        OrigSendingTime = value;
                        break;

                    case FIXTAG.SendingTime:
                        SendingTime = value;
                        break;

                    case FIXTAG.Checksum:
                        Checksum = value;
                        break;
                    #endregion

                    #region MsgBody

                    case FIXTAG.TradeReportID:
                        TradeReportID = value;
                        break;

                    case FIXTAG.TradeReportRefId:
                        TradeReportRefId = value;
                        break;

                    case FIXTAG.ExecID:
                        ExecID = value;
                        break;

                    case FIXTAG.ExecType:
                        ExecType = Convert.ToChar(value);
                        break;

                    case FIXTAG.TradeReportTransType:
                        TradeReportTransType = Convert.ToInt32(value);
                        break;

                    case FIXTAG.TradeReportType:
                        TradeReportType = Convert.ToInt32(value);
                        break;

                    case FIXTAG.TradeHandlingInstr:
                        TradeHandlingInstr = Convert.ToChar(value);
                        break;

                    case FIXTAG.TrdType:
                        TrdType = Convert.ToInt32(value);
                        break;

                    case FIXTAG.LastShares:
                        LastShares = Convert.ToInt32(value);
                        break;

                    case FIXTAG.LeavesQty:
                        LeavesQty = Convert.ToInt32(value);
                        break;

                    case FIXTAG.LastPx:
                        LastPx = Convert.ToDecimal(value);
                        break;

                    case FIXTAG.AvgPx:
                        AvgPx = Convert.ToDecimal(value);
                        break;

                    case FIXTAG.TransactTime:
                        TransactTime = value;
                        break;

                    case FIXTAG.TradeDate:
                        TradeDate = value;
                        break;

                    case FIXTAG.OrigTradeDate:
                        OrigTradeDate = value;
                        break;

                    case FIXTAG.TradeLinkId:
                        TradeLinkId = value;
                        break;

                    case FIXTAG.TradeId:
                        TradeId = value;
                        break;

                    case FIXTAG.OrigTradeId:
                        OrigTradeId = value;
                        break;

                    case FIXTAG.TrdMatchID:
                        TrdMatchId = value;
                        break;

                    case FIXTAG.FutureReferencePrice:
                        FutureReferencePrice = Convert.ToDecimal(value);
                        break;

                    case FIXTAG.MultiLegReportingType:
                        MultiLegReportingType = Convert.ToChar(value);
                        break;

                    case FIXTAG.TransBkdTime:
                        TransBkdTime = value;
                        break;

                    #region Instrument

                    case FIXTAG.SecurityID:
                        SecurityID = value;
                        break;

                    case FIXTAG.IDSource:
                        IDSource = value;
                        break;

                    case FIXTAG.SecurityExchange:
                        SecurityExchange = value;
                        break;

                    case FIXTAG.ExDestination:
                        ExDestination = value;
                        break;

                    case FIXTAG.LastMkt:
                        LastMkt = value;
                        break;

                    case FIXTAG.Symbol:
                        Symbol = value;
                        break;

                    case FIXTAG.CFICode:
                        CFICode = value;
                        break;

                    case FIXTAG.SecurityType:
                        SecurityType = value;
                        break;

                    case FIXTAG.Product:
                        Product = Convert.ToInt32(value);
                        break;

                    case FIXTAG.SecurityDesc:
                        SecurityDesc = value;
                        break;

                    case FIXTAG.MaturityMonthYear:
                        MaturityMonthYear = value;
                        break;

                    case FIXTAG.MaturityDate:
                        MaturityDate = value;
                        break;

                    case FIXTAG.MaturityDay:
                        MaturityDay = Convert.ToInt32(value);
                        break;

                    case FIXTAG.ContractYearMonth:
                        ContractYearMonth = value;
                        break;

                    case FIXTAG.DeliveryTerm:
                        DeliveryTerm = Convert.ToChar(value);
                        break;

                    case FIXTAG.DeliveryDate:
                        DeliveryDate = value;
                        break;

                    case FIXTAG.PutOrCall:
                        PutOrCall = Convert.ToInt32(value);
                        break;

                    case FIXTAG.StrikePrice:
                        StrikePrice = Convert.ToDecimal(value);
                        break;

                    case FIXTAG.DisplayFactor:
                        DisplayFactor = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
                        break;

                    case FIXTAG.Currency:
                        Currency = value;
                        break;

                    case FIXTAG.AllocID:
                        AllocID = value;
                        break;


                    case FIXTAG.SecuritySubType:
                        SecuritySubType = value;
                        break;

                    #endregion
                    case FIXTAG.AccountRiskGroup:
                        AccountRiskGroup = value;
                        break;



                        #endregion
                }
            }
        }
    }
}
