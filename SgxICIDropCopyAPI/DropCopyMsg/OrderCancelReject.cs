using SgxICIDropCopy.API;
using SgxICIDropCopy.RepeatingGrp;
using System;

namespace SgxICIDropCopy.DropCopyMsg
{
    public class OrderCancelReject : MessageHeader
    {
        #region Constructor
        public OrderCancelReject(string msgData)
        {
            Generate(msgData);
        }

        public OrderCancelReject(MessageHeader msgHeader) : base(msgHeader)
        {
            base.MsgType = FIXMessageType.OrderCancelReject;
        }
        #endregion

        #region Properties

        public string OrderID { get; set; }

        public string ClOrdID { get; set; }

        public string OrigClOrdID { get; set; }

        public string SecondaryOrderID { get; set; }

        public char OrdStatus { get; set; }

        public string TransactTime { get; set; }

        public int CxlRejResponseTo { get; set; }

        public int NoPartyIDs { get; set; }

        public PartiesGroup[] arrPartiesGroup;

        public string Text { get; set; }

        public string Account { get; set; }

        #endregion
        public void Generate(string Msgdata)
        {
            string[] arrMsgString = Msgdata.Split(FixConst.Delimeter);
            int PartiesGroupCount = -1;
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

                    case FIXTAG.Account:
                        Account = value;
                        break;

                    case FIXTAG.OrderId:
                        OrderID = value;
                        break;

                    case FIXTAG.TransactTime:
                        TransactTime = value;
                        break;

                    case FIXTAG.OrdStatus:
                        OrdStatus = Convert.ToChar(value);
                        break;

                    case FIXTAG.Text:
                        Text = value;
                        break;

                    case FIXTAG.CxlRejResponseTo:
                        CxlRejResponseTo = Convert.ToInt32(value);
                        break;

                    case FIXTAG.NoPartyIDs:
                        NoPartyIDs = Convert.ToInt32(value);
                        arrPartiesGroup = new PartiesGroup[NoPartyIDs];
                        break;

                    case FIXTAG.PartyID:
                        PartiesGroupCount++;
                        arrPartiesGroup[PartiesGroupCount] = new PartiesGroup();
                        arrPartiesGroup[PartiesGroupCount].PartyID = value;
                        break;

                    case FIXTAG.PartyRole:
                        arrPartiesGroup[PartiesGroupCount].PartyRole = Convert.ToInt32(value);
                        break;

                    case FIXTAG.PartyIdSource:
                        arrPartiesGroup[PartiesGroupCount].PartyIdSource = Convert.ToChar(value);
                        break;






                        #endregion
                }

            }
        }
    }
}
