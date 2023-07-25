using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SgxICIDropCopy.API;

namespace SgxICIDropCopy.SessionMsg
{
    public class Logon : MessageHeader, iDataInterface
    {
        #region Constructor
        public Logon(string MsgData)
        {
            base.MsgType = FIXMessageType.Logon;
            GenerateData(MsgData);
        }

        public Logon(MessageHeader MsgHeader) : base(MsgHeader)
        {
            base.MsgType = FIXMessageType.Logon;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Method of encryption
        /// As  FIX does not support encrypted logons, you must set the value to 0 (None/Other).
        /// </summary>
        public int EncryptMethod { get; set; }

        /// <summary>
        /// Heartbeat interval (seconds)
        /// </summary>
        public int HeartBtInt { get; set; }

        public string DefaultAppVerID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
        
        public string NewPassword { get; set; }

        public string PwdExpireDate { get; set; }

        public int SessionStatus { get; set; }

        public string Text { get; set; }


        ///// <summary>
        ///// Whether to reset the sequence numbers on both sides of the FIX session
        ///// Valid values include Y and N (default).
        ///// Notes:
        ///// If a FIX client sends Y, it must also set tag 34 (MsgSeqNum) = 1.
        ///// For Security Definition sessions, FIX clients must always set 141=Y and 34=1.
        ///// </summary>
        //public bool ResetSeqNumFlag { get; set; }

        /// <summary>
        /// For internal  use only
        /// </summary>
        public bool ByPassSessionRecovery { get; set; }

        #endregion
        public virtual string Data
        {
            get
            {
                if (string.IsNullOrEmpty(BeginString) || string.IsNullOrEmpty(MsgType)
                    || string.IsNullOrEmpty(SenderCompID) || string.IsNullOrEmpty(TargetCompID)
                    || HeartBtInt < 1 || string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
                    return null;

                StringBuilder databuilder = new StringBuilder();
                StringBuilder prefixBuilder = new StringBuilder();

                GenerateMessageHeader(ref databuilder);

                databuilder.Append(FIXTAG.EncryptMethod + "=");
                databuilder.Append(EncryptMethod);
                databuilder.Append(FixConst.Delimeter);

                databuilder.Append(FIXTAG.HeartBtInt + "=");
                databuilder.Append(HeartBtInt);
                databuilder.Append(FixConst.Delimeter);

                databuilder.Append(FIXTAG.DefaultApplVerID + "=");
                databuilder.Append(DefaultAppVerID);
                databuilder.Append(FixConst.Delimeter);

                databuilder.Append(FIXTAG.UserName + "=");
                databuilder.Append(UserName);
                databuilder.Append(FixConst.Delimeter);

                databuilder.Append(FIXTAG.Password + "=");
                databuilder.Append(Password);
                databuilder.Append(FixConst.Delimeter);

                if (!string.IsNullOrEmpty(NewPassword))
                {
                    databuilder.Append(FIXTAG.NewPassword + "=");
                    databuilder.Append(NewPassword);
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


                    case FIXTAG.EncryptMethod:
                        EncryptMethod = Convert.ToInt32(value);
                        break;


                    case FIXTAG.HeartBtInt:
                        HeartBtInt = Convert.ToInt32(value);
                        break;

                    case FIXTAG.DefaultApplVerID:
                        DefaultAppVerID = value;
                        break;

                    case FIXTAG.PwdExpireDate:
                        PwdExpireDate = value;
                        break;

                    case FIXTAG.SessionStatus:
                        SessionStatus = Convert.ToInt32(value); break;

                        #endregion
                }

            }
        }
    }
}
