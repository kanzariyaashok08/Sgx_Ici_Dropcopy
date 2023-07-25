using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SgxICIDropCopy.API;

namespace SgxICIDropCopy.SessionMsg
{
    public class SessionLevelReject : MessageHeader
    {
        #region Constructor
        public SessionLevelReject(string MsgData)
        {
            base.MsgType = FIXMessageType.Reject;
            GenerateData(MsgData);
        }

        public SessionLevelReject(MessageHeader msgHeader) : base(msgHeader)
        {
            base.MsgType = FIXMessageType.Reject;
        }
        #endregion

        #region Properties      

        /// <summary>
        /// Reference message sequence number.
        /// Required Tag : Y
        /// </summary>
        public int RefSeqNum { get; set; }


        /// <summary>
        /// Reason for rejection.
        /// Required Tag : Y
        /// </summary>
        public string Text { get; set; }


        /// <summary>
        /// Tag number of the referenced FIX message.
        /// Required Tag : Y
        /// </summary>
        public int RefTagID { get; set; }


        /// <summary>
        /// Message type associated with the referenced FIX message.
        /// Required Tag : Y
        /// </summary>
        public string RefMsgType { get; set; }

        /// <summary>
        /// Reason for the rejection
        /// Required Tag : Y
        /// </summary>
        public int SessionRejectReason { get; set; }

        #endregion

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

                    case FIXTAG.RefSeqNum:
                        RefSeqNum = Convert.ToInt32(value);
                        break;

                    case FIXTAG.Text:
                        Text = value;
                        break;

                    case FIXTAG.RefTagID:
                        RefTagID = Convert.ToInt32(value);
                        break;

                    case FIXTAG.RefMsgType:
                        RefMsgType = value;
                        break;

                    case FIXTAG.SessionRejectReason:
                        SessionRejectReason = Convert.ToInt32(value);
                        break;
                        #endregion
                }

            }
        }
    }
}
