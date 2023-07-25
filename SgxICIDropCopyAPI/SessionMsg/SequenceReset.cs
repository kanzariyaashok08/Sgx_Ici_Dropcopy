using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SgxICIDropCopy.API;

namespace SgxICIDropCopy.SessionMsg
{
    public class SequenceReset : MessageHeader, iDataInterface
    {
        #region Constructor
        public SequenceReset(string MsgData)
        {
            base.MsgType = FIXMessageType.SequenceReset;
            GenerateData(MsgData);
        }

        public SequenceReset(MessageHeader msgHeader) : base(msgHeader)
        {
            base.MsgType = FIXMessageType.SequenceReset;
            base.OrigSendingTime = DateTime.UtcNow.ToString(FixConst.TimeFormat);
        }

        #endregion

        #region Properties      

        /// <summary>
        /// Indicates that the Sequence Reset message replaces missing administrative or application messages
        /// Valid values include: 1) Y: Gap Fill message, Tag 34 (MsgSeqNum) field is valid 2) N: Sequence Reset, ignore Tag 34 (MsgSeqNum)
        /// Default: N
        /// Required Tag : N
        /// </summary>
        public bool GapFillFlag { get; set; }

        /// <summary>
        /// New sequence number
        /// Required Tag : Y
        /// </summary>
        public int NewSeqNo { get; set; }

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

                databuilder.Append(FIXTAG.GapFillFlag + "=");
                databuilder.Append(FIXMethods.GetBooleanValue(GapFillFlag));
                databuilder.Append(FixConst.Delimeter);

                databuilder.Append(FIXTAG.NewSeqNo + "=");
                databuilder.Append(NewSeqNo);
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

                    case FIXTAG.GapFillFlag:
                        GapFillFlag = FIXMethods.SetBooleanValue(value);
                        break;

                    case FIXTAG.NewSeqNo:
                        NewSeqNo = Convert.ToInt32(value);
                        break;
                        #endregion
                }

            }
        }
    }
}
