using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SgxICIDropCopy.API;

namespace SgxICIDropCopy.SessionMsg
{
    public class News : MessageHeader
    {
        #region Constructor
        public News(string MsgData)
        {
            base.MsgType = FIXMessageType.News;
            GenerateData(MsgData);
        }

        public News(MessageHeader msgHeader) : base(msgHeader)
        {
            base.MsgType = FIXMessageType.News;
        }
        #endregion

        #region Properties      

        /// <summary>
        /// Specifies the headline text
        ///  FIX always sends "Recovery Complete".
        /// Required Tag : Y
        /// </summary>
        public string Headline { get; set; }

        /// <summary>
        ///Specifies the number of repeating lines of text specified
        /// FIX always sets the value to 1.
        /// Required Tag : Y
        /// </summary>
        public int NoLinesOfText { get; set; }

        /// <summary>
        ///One line of text in the message
        /// FIX always sets the value to "Recovery is complete".
        /// Required Tag : Y
        /// </summary>
        public string Text { get; set; }

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

                    case FIXTAG.Headline:
                        Headline = value;
                        break;

                    case FIXTAG.NoLinesOfText:
                        NoLinesOfText = Convert.ToInt32(value);
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
