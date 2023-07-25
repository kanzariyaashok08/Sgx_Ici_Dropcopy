using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SgxICIDropCopy.API;

namespace SgxICIDropCopy.DropCopyMsg
{
    public class TradeCaptureReportAck : MessageHeader, iDataInterface
    {
        #region Constructor
        public TradeCaptureReportAck(string MsgData)
        {

        }

        public TradeCaptureReportAck(MessageHeader msgHeader) : base(msgHeader)
        {
            base.MsgType = FIXMessageType.TradeCaptureReportAck;
        }
        #endregion

        #region Properties

        public string TradeReportID { get; set; }

        public string TradeReportRefId { get; set; }

        public string ExecID { get; set; }

        public string TradeLinkId { get; set; }

        public int TradeReportTransType { get; set; }

        public int TradeReportType { get; set; }

        public char TradeRptStatus { get; set; }

        public int TradeReportRejectReason { get; set; }

        public string SecurityDesc { get; set; }

        public string TransactTime { get; set; }

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

        public string MaturityMonthYear { get; set; }

        public string MaturityDate { get; set; }

        public int MaturityDay { get; set; }

        public string ContractYearMonth { get; set; }

        public char DeliveryTerm { get; set; }

        public string DeliveryDate { get; set; }

        public int PutOrCall { get; set; }

        public decimal StrikePrice { get; set; }

        public char OptAttribute { get; set; }

        public float DisplayFactor { get; set; }

        public string Currency { get; set; }

        public string AllocID { get; set; }

        public string SecuritySubType { get; set; }

        #endregion

        #endregion

        public virtual string Data
        {
            get
            {
                if (string.IsNullOrEmpty(BeginString) || string.IsNullOrEmpty(MsgType)
                  || string.IsNullOrEmpty(SenderCompID) || string.IsNullOrEmpty(TargetCompID))
                    return null;

                StringBuilder databuilder = new StringBuilder();
                StringBuilder prefixBuilder = new StringBuilder();

                GenerateMessageHeader(ref databuilder);

                if (!string.IsNullOrEmpty(TradeReportID))
                {
                    databuilder.Append(FIXTAG.TradeReportID + "=");
                    databuilder.Append(TradeReportID);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (!string.IsNullOrEmpty(TradeReportRefId))
                {
                    databuilder.Append(FIXTAG.TradeReportRefId + "=");
                    databuilder.Append(TradeReportRefId);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (!string.IsNullOrEmpty(ExecID))
                {
                    databuilder.Append(FIXTAG.ExecID + "=");
                    databuilder.Append(ExecID);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (!string.IsNullOrEmpty(TradeLinkId))
                {
                    databuilder.Append(FIXTAG.TradeLinkId + "=");
                    databuilder.Append(TradeLinkId);
                    databuilder.Append(FixConst.Delimeter);
                }

                databuilder.Append(FIXTAG.TradeReportTransType + "=");
                databuilder.Append(TradeReportTransType);
                databuilder.Append(FixConst.Delimeter);

                databuilder.Append(FIXTAG.TradeReportType + "=");
                databuilder.Append(TradeReportType);
                databuilder.Append(FixConst.Delimeter);


                if (TradeRptStatus != '\0')
                {
                    databuilder.Append(FIXTAG.TradeRptStatus + "=");
                    databuilder.Append(TradeRptStatus);
                    databuilder.Append(FixConst.Delimeter);
                }

                databuilder.Append(FIXTAG.TradeReportRejectReason + "=");
                databuilder.Append(TradeReportRejectReason);
                databuilder.Append(FixConst.Delimeter);

                if (!string.IsNullOrEmpty(SecurityDesc))
                {
                    databuilder.Append(FIXTAG.SecurityDesc + "=");
                    databuilder.Append(SecurityDesc);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (!string.IsNullOrEmpty(TransactTime))
                {
                    databuilder.Append(FIXTAG.TransactTime + "=");
                    databuilder.Append(TransactTime);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (!string.IsNullOrEmpty(SecurityExchange))
                {
                    databuilder.Append(FIXTAG.SecurityExchange + "=");
                    databuilder.Append(SecurityExchange);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (!string.IsNullOrEmpty(ExDestination))
                {
                    databuilder.Append(FIXTAG.ExDestination + "=");
                    databuilder.Append(ExDestination);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (!string.IsNullOrEmpty(CFICode))
                {
                    databuilder.Append(FIXTAG.CFICode + "=");
                    databuilder.Append(CFICode);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (!string.IsNullOrEmpty(Symbol))
                {
                    databuilder.Append(FIXTAG.Symbol + "=");
                    databuilder.Append(Symbol);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (!string.IsNullOrEmpty(SecurityID))
                {
                    databuilder.Append(FIXTAG.SecurityID + "=");
                    databuilder.Append(SecurityID);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (!string.IsNullOrEmpty(IDSource))
                {
                    databuilder.Append(FIXTAG.IDSource + "=");
                    databuilder.Append(IDSource);
                    databuilder.Append(FixConst.Delimeter);
                }




                if (!string.IsNullOrEmpty(SecurityType))
                {
                    databuilder.Append(FIXTAG.SecurityType + "=");
                    databuilder.Append(SecurityType);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (!string.IsNullOrEmpty(SecurityDesc))
                {
                    databuilder.Append(FIXTAG.SecurityDesc + "=");
                    databuilder.Append(SecurityDesc);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (Product > 0)
                {
                    databuilder.Append(FIXTAG.Product + "=");
                    databuilder.Append(Product);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (!string.IsNullOrEmpty(MaturityMonthYear))
                {
                    databuilder.Append(FIXTAG.MaturityMonthYear + "=");
                    databuilder.Append(MaturityMonthYear);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (!string.IsNullOrEmpty(MaturityDate))
                {
                    databuilder.Append(FIXTAG.MaturityDate + "=");
                    databuilder.Append(MaturityDate);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (MaturityDay > 0 && MaturityDay < 32)
                {
                    databuilder.Append(FIXTAG.MaturityDay + "=");
                    databuilder.Append(MaturityDay);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (DeliveryTerm != '\0')
                {
                    databuilder.Append(FIXTAG.DeliveryTerm + "=");
                    databuilder.Append(DeliveryTerm);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (!string.IsNullOrEmpty(DeliveryDate))
                {
                    databuilder.Append(FIXTAG.DeliveryDate + "=");
                    databuilder.Append(DeliveryDate);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (StrikePrice != 0)
                {
                    databuilder.Append(FIXTAG.StrikePrice + "=");
                    databuilder.Append(StrikePrice);
                    databuilder.Append(FixConst.Delimeter);


                    databuilder.Append(FIXTAG.PutOrCall + "=");
                    databuilder.Append(PutOrCall);
                    databuilder.Append(FixConst.Delimeter);

                    databuilder.Append(FIXTAG.OptAttribute + "=");
                    databuilder.Append(OptAttribute);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (!string.IsNullOrEmpty(SecuritySubType))
                {
                    databuilder.Append(FIXTAG.SecuritySubType + "=");
                    databuilder.Append(SecuritySubType);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (!string.IsNullOrEmpty(Currency))
                {
                    databuilder.Append(FIXTAG.Currency + "=");
                    databuilder.Append(Currency);
                    databuilder.Append(FixConst.Delimeter);
                }

                prefixBuilder.Append(FIXTAG.BeginString + "=");
                prefixBuilder.Append(BeginString);
                prefixBuilder.Append(FixConst.Delimeter);

                prefixBuilder.Append(FIXTAG.BodyLength + "=");
                BodyLength = databuilder.Length;
                prefixBuilder.Append(BodyLength);
                prefixBuilder.Append(FixConst.Delimeter);

                prefixBuilder.Append(databuilder);

                string checksum = FIXMethods.GenerateCheckSum(prefixBuilder.ToString().ToCharArray());
                prefixBuilder.Append(FIXTAG.Checksum + "=");
                prefixBuilder.Append(checksum);
                prefixBuilder.Append(FixConst.Delimeter);


                return prefixBuilder.ToString();
            }
        }
    }
}
