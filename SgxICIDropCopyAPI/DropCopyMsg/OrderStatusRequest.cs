using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SgxICIDropCopy.API;

namespace SgxICIDropCopy.DropCopyMsg
{
    public class OrderStatusRequest : MessageHeader, iDataInterface
    {
        #region Constructor

        public OrderStatusRequest(string msgData)
        {
            base.MsgType = FIXMessageType.OrderStatusRequest;
        }
        public OrderStatusRequest(MessageHeader msgHeader) : base(msgHeader)
        {
            base.MsgType = FIXMessageType.OrderStatusRequest;
        }

        #endregion

        #region Porperties

        #region Trader
        public string Account { get; set; }

        public string BrokerID { get; set; }

        public string CompanyID { get; set; }

        public int CustOrderCapacity { get; set; }

        #endregion

        public string ClOrdId { get; set; }

        public string OrderId { get; set; }
        public string ClearingAccountOverride { get; set; }

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

                if (!string.IsNullOrEmpty(Account))
                {
                    databuilder.Append(FIXTAG.Account + "=");
                    databuilder.Append(Account);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (!string.IsNullOrEmpty(BrokerID))
                {
                    databuilder.Append(FIXTAG.BrokerID + "=");
                    databuilder.Append(BrokerID);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (!string.IsNullOrEmpty(CompanyID))
                {
                    databuilder.Append(FIXTAG.CompanyID + "=");
                    databuilder.Append(CompanyID);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (CustOrderCapacity != 0)
                {
                    databuilder.Append(FIXTAG.CustOrderCapacity + "=");
                    databuilder.Append(CustOrderCapacity);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (!string.IsNullOrEmpty(ClOrdId))
                {
                    databuilder.Append(FIXTAG.ClOrdId + "=");
                    databuilder.Append(ClOrdId);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (!string.IsNullOrEmpty(OrderId))
                {
                    databuilder.Append(FIXTAG.OrderId + "=");
                    databuilder.Append(OrderId);
                    databuilder.Append(FixConst.Delimeter);
                }

                if (!string.IsNullOrEmpty(ClearingAccountOverride))
                {
                    databuilder.Append(FIXTAG.ClearingAccountOverride + "=");
                    databuilder.Append(ClearingAccountOverride);
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
