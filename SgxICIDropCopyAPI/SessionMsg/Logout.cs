using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SgxICIDropCopy.API;

namespace SgxICIDropCopy.SessionMsg
{
    public class Logout : MessageHeader, iDataInterface
    {
        #region Constructor
        public Logout(string MsgData)
        {
            base.MsgType = FIXMessageType.Logout;
            GenerateData(MsgData);
        }
        public Logout(MessageHeader msgHeader) : base(msgHeader)
        {
            base.MsgType = FIXMessageType.Logout;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Indicates whether the logout is forced
        /// Possible values include:
        /// 0: Unforced logout
        /// 1: Forced logout
        /// </summary>
        public int SessionStatus { get; set; }

        /// <summary>
        /// Information to include with the message
        /// </summary>
        public string Text { get; set; }

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

                databuilder.Append(FIXTAG.SessionStatus + "=");
                databuilder.Append(SessionStatus);
                databuilder.Append(FixConst.Delimeter);

                if (!string.IsNullOrEmpty(Text))
                {
                    databuilder.Append(FIXTAG.Text + "=");
                    databuilder.Append(Text);
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
                    case FIXTAG.SessionStatus:
                        SessionStatus = Convert.ToInt32(value);
                        break;

                    case FIXTAG.Text:
                        Text = value;
                        break;

                        #endregion
                }

            }
        }
    }
}
