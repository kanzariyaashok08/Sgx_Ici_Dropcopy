using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SgxICIDropCopy.API;

namespace SgxICIDropCopy.SessionMsg
{
    public class ResendRequest : MessageHeader, iDataInterface
    {
        #region Constructor
        public ResendRequest(string MsgData)
        {
            base.MsgType = FIXMessageType.ResendRequest;
            GenerateData(MsgData);
        }

        public ResendRequest(MessageHeader msgHeader) : base(msgHeader)
        {
            base.MsgType = FIXMessageType.ResendRequest;
        }

        #endregion

        #region Properties      

        /// <summary>
        /// Sequence number of the first message in the range to resend
        /// Use this value in Tag 34 (MsgSeqNum) of the first resent message.
        /// Required Tag : Y
        /// </summary>
        public int BeginSeqNo { get; set; }

        /// <summary>
        /// Sequence number of the last message in the range to resend
        /// Use this value in Tag 34 (MsgSeqNum) of the last message.
        /// If the request is for a single message, then Tag 7 (BeginSeqNo) must equal Tag 16 (EndSeqNo). If the request is for all messages subsequent to a particular message, then you must set 16=0 (representing infinity).
        /// Required Tag : Y
        /// </summary>
        public int EndSeqNo { get; set; }

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


                databuilder.Append(FIXTAG.BeginSeqNo + "=");
                databuilder.Append(BeginSeqNo);
                databuilder.Append(FixConst.Delimeter);

                databuilder.Append(FIXTAG.EndSeqNo + "=");
                databuilder.Append(EndSeqNo);
                databuilder.Append(FixConst.Delimeter);


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

        public void GenerateData(string Msgdata)
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

                    #region Msg Body                    

                    case FIXTAG.BeginSeqNo:
                        BeginSeqNo = Convert.ToInt32(value);
                        break;

                    case FIXTAG.EndSeqNo:
                        EndSeqNo = Convert.ToInt32(value);
                        break;

                        #endregion
                }

            }
        }
    }
}
