using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SgxICIDropCopy.API;

namespace SgxICIDropCopy.API
{
    public class MessageHeader
    {
        #region Constructor

        public MessageHeader()
        {
        }

        public MessageHeader(string message)
        {
            Generate(message);
        }
        public MessageHeader(MessageHeader MsgHeader)
        {
            BeginString = MsgHeader.BeginString;
            BodyLength = MsgHeader.BodyLength;
            MsgSeqNum = MsgHeader.MsgSeqNum;
            MsgType = MsgHeader.MsgType;
            OrigSendingTime = MsgHeader.OrigSendingTime;
            PossDupFlag = MsgHeader.PossDupFlag;
            SenderCompID = MsgHeader.SenderCompID;
            SendingTime = MsgHeader.SendingTime;
            TargetCompID = MsgHeader.TargetCompID;
        }
        #endregion

        private void Generate(string message)
        {
            string[] messageString = message.Split(FixConst.Delimeter);

            for (int i = 0; i < messageString.Length - 1; i++)
            {
                if (string.IsNullOrEmpty(messageString[i])) continue;

                string[] temp = messageString[i].Split(FixConst.Equalto);

                int tag = Convert.ToInt32(temp[0]);
                string value = temp[1];

                switch (tag)
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

                    case FIXTAG.PossResend:
                        PossResend = FIXMethods.SetBooleanValue(value);
                        break;

                    case FIXTAG.OrigSendingTime:
                        OrigSendingTime = value;
                        break;

                    case FIXTAG.SendingTime:
                        SendingTime = value;
                        break;
                        #endregion
                }
            }
        }

        #region Properties
        /// <summary>
        /// FIX protocol version
        /// The tag indicates the beginning of a new message. This tag must be the first tag in the message.
        /// You must set the value to FIX.4.2.
        /// Required Tag : Y
        /// </summary>
        public string BeginString { get; set; }

        /// <summary>
        /// Message length (in characters)
        /// he value represents number of characters in the message following this tag up to, and including, the delimiter immediately preceding Tag 10 (CheckSum). This tag must be the second field in a message.
        /// Required Tag : Y
        /// </summary>
        public int BodyLength { get; set; }

        /// <summary>
        /// Type of message contained in the message body
        /// This tag must appear third in the list of header tags.
        /// Required Tag : Y
        /// </summary>
        public string MsgType { get; set; }

        /// <summary>
        /// ID for the FIX client, corresponding to the RemoteCompID specified for the user in  User Setup
        /// Required Tag : Y
        /// </summary>
        public string SenderCompID { get; set; }

        /// <summary>
        ///  session identity
        ///  FIX does not validate this field. To guarantee session persistence, the FIX client must maintain the same value of this field for the life of the session.
        /// You can use any value in this tag to identify the  session for the FIX client.  FIX will return this value in tag 49 (SenderCompID) in its responses.
        /// Required Tag : Y
        /// </summary>
        public string TargetCompID { get; set; }

        
        /// <summary>
        /// Message sequence number
        /// Required Tag : Y
        /// </summary>
        public int MsgSeqNum { get; set; }

        /// <summary>
        /// Whether the sequence number for this message is already used
        /// Possible values include: 1) Y: Possible duplicate 2) N: Original transmission
        /// Condition: Must send when a FIX client resends messages
        /// Note: If 43=Y is present,  will reject New Order Single (D) and Order Cancel/Replace Request (G) messages with a Business Message Reject (j) message.
        /// Required Tag : C
        /// </summary>
        public bool PossDupFlag { get; set; }

        /// <summary>
        /// Whether the message might contain information that has been sent under another sequence numbe
        /// Possible values include: 1)Y: Possible resend 2)Original transmission
        /// Condition: Sent when  FIX restarts after encountering a corrupt FIX message cache, and only until it completes the initial download.
        /// Required Tag : C
        /// </summary>
        public bool PossResend { get; set; }

        /// <summary>
        /// Original time of message transmission, when transmitting orders as the result of a resend request
        /// Always expressed in UTC.
        /// Condition: Required for resent messages
        /// Required Tag : C
        /// </summary>
        public string OrigSendingTime { get; set; }
        //{
        //    get { return OrigSendingTime = DateTime.UtcNow.ToString(FixConst.TimeFormat); }
        //    set {; }
        //}


        /// <summary>
        /// Time, in UTC, the message was sent.
        /// Required Tag : Y
        /// </summary>
        public string SendingTime
        {
            get { return SendingTime = DateTime.UtcNow.ToString(FixConst.TimeFormat); }
            set {; }
        }

        /// <summary>
        /// Unencrypted three-character checksum
        /// This tag must always be the last field in a message (i.e. it serves, with the trailing <SOH>, as the end-of-message delimiter).
        /// Required Tag : Y
        /// </summary>
        public string Checksum { get; set; }

        #endregion

        public void GenerateMessageHeader(ref StringBuilder databuilder)
        {
            databuilder.Append(FIXTAG.MsgType + "=");
            databuilder.Append(MsgType);
            databuilder.Append(FixConst.Delimeter);

            databuilder.Append(FIXTAG.SenderCompID + "=");
            databuilder.Append(SenderCompID);
            databuilder.Append(FixConst.Delimeter);

            databuilder.Append(FIXTAG.TargetCompID + "=");
            databuilder.Append(TargetCompID);
            databuilder.Append(FixConst.Delimeter);

            databuilder.Append(FIXTAG.MsgSeqNum + "=");
            databuilder.Append(MsgSeqNum);
            databuilder.Append(FixConst.Delimeter);

            if (PossDupFlag)
            {
                databuilder.Append(FIXTAG.PossDupFlag + "=");
                databuilder.Append(FIXMethods.GetBooleanValue(PossDupFlag));
                databuilder.Append(FixConst.Delimeter);
            }

            if (!string.IsNullOrEmpty(OrigSendingTime))
            {
                databuilder.Append(FIXTAG.OrigSendingTime + "=");
                databuilder.Append(OrigSendingTime);
                databuilder.Append(FixConst.Delimeter);
            }

            databuilder.Append(FIXTAG.SendingTime + "=");
            databuilder.Append(SendingTime);
            databuilder.Append(FixConst.Delimeter);
        }


    }
}
