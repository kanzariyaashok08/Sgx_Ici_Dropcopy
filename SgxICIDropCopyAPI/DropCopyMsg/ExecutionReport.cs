using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SgxICIDropCopy.API;
using SgxICIDropCopy.RepeatingGrp;

namespace SgxICIDropCopy.DropCopyMsg
{
    public class ExecutionReport : MessageHeader
    {
        #region Constructor
        public ExecutionReport(string msgData)
        {
            base.MsgType = FIXMessageType.ExecutionReport;
            Generate(msgData);
        }
        public ExecutionReport(MessageHeader msgHeader) : base(msgHeader)
        {
            base.MsgType = FIXMessageType.ExecutionReport;
        }

        #endregion

        #region Properties
        public string CFICode { get; set; }

        public string Symbol { get; set; }

        public string SecurityID { get; set; }

        public string MaturityDate { get; set; }

        public decimal StrikePrice { get; set; }

        public int PutOrCall { get; set; }

        public string SecurityType { get; set; }

        public string Account { get; set; }

        public char Side { get; set; }

        public string OrderId { get; set; }

        public int LeavesQty { get; set; }

        public int CumQty { get; set; }

        public int OrderQty { get; set; }

        public decimal Price { get; set; }

        public decimal StopPx { get; set; }

        public int LastQty { get; set; }

        public decimal LastPx { get; set; }

        public char OrdType { get; set; }

        public string ExecID { get; set; }

        public char TimeInForce { get; set; }

        public string PositionEffect { get; set; }

        public int HandlInst { get; set; }

        public string IDSource { get; set; }

        public string TransactTime { get; set; }

        public char ExecType { get; set; }

        public char OrdStatus { get; set; }

        public int ExecRestatementReason { get; set; }

        public string SettlType { get; set; }

        public int NoPartyIDs { get; set; }

        public PartiesGroup[] arrPartiesGroup;

        public string customerinfo1 { get; set; }

        public string customerinfo2 { get; set; }

        public int DisplayQty { get; set; }

        public int NoLegs { get; set; }

        public LegInstrumentGrp[] legInstrumentGrps;

        public string Text { get; set; }

        #endregion
        public void Generate(string Msgdata)
        {
            try
            {
                string[] arrMsgString = Msgdata.Split(FixConst.Delimeter);

                int PartiesGroupCount = -1;
                int NoLegGroupCount = -1;
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

                        case FIXTAG.ExecID:
                            ExecID = value;
                            break;


                        case FIXTAG.OrderId:
                            OrderId = value;
                            break;

                        case FIXTAG.ExecType:
                            ExecType = Convert.ToChar(value);
                            break;

                        case FIXTAG.ExecRestatementReason:
                            ExecRestatementReason = Convert.ToInt32(value);
                            break;

                        case FIXTAG.OrdStatus:
                            OrdStatus = Convert.ToChar(value);
                            break;

                        case FIXTAG.LastShares:
                            LastQty = Convert.ToInt32(value);
                            break;

                        case FIXTAG.LastPx:
                            LastPx = Convert.ToDecimal(value);
                            break;

                        case FIXTAG.TransactTime:
                            TransactTime = value;
                            break;

                        case FIXTAG.LeavesQty:
                            LeavesQty = Convert.ToInt32(value);
                            break;

                        case FIXTAG.CumQty:
                            CumQty = Convert.ToInt32(value);
                            break;

                        case FIXTAG.SecurityID:
                            SecurityID = value;
                            break;

                        case FIXTAG.IDSource:
                            IDSource = value;
                            break;

                        case FIXTAG.Symbol:
                            Symbol = value;
                            break;

                        case FIXTAG.CFICode:
                            CFICode = value;
                            break;

                        case FIXTAG.SecurityType:
                            SecurityType = value;
                            break;

                        case FIXTAG.MaturityDate:
                            MaturityDate = value;
                            break;

                        case FIXTAG.PutOrCall:
                            PutOrCall = Convert.ToInt32(value);
                            break;

                        case FIXTAG.StrikePrice:
                            StrikePrice = Convert.ToDecimal(value);
                            break;

                        case FIXTAG.Account:
                            Account = value;
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

                        case FIXTAG.PartyRoleQualifier:
                            arrPartiesGroup[PartiesGroupCount].PartyRoleQualifier = Convert.ToInt32(value);
                            break;

                        case FIXTAG.PartyIdSource:
                            arrPartiesGroup[PartiesGroupCount].PartyIdSource = Convert.ToChar(value);
                            break;


                        case FIXTAG.Price:
                            Price = Convert.ToDecimal(value);
                            break;

                        case FIXTAG.StopPx:
                            StopPx = Convert.ToDecimal(value);
                            break;

                        case FIXTAG.DisplayQty:
                            DisplayQty = Convert.ToInt32(value);
                            break;


                        case FIXTAG.OrderQty:
                            OrderQty = Convert.ToInt32(value);
                            break;

                        case FIXTAG.Side:
                            Side = Convert.ToChar(value);
                            break;

                        case FIXTAG.OrdType:
                            OrdType = Convert.ToChar(value);
                            break;

                        case FIXTAG.TimeInForce:
                            TimeInForce = Convert.ToChar(value);
                            break;

                        case FIXTAG.PositionEffect:
                            PositionEffect = value;
                            break;

                        case FIXTAG.HandlInst:
                            HandlInst = Convert.ToInt32(value);
                            break;

                        case FIXTAG.NoLegs:
                            NoLegs = Convert.ToInt32(value);
                            legInstrumentGrps = new LegInstrumentGrp[NoLegs];
                            break;

                        case FIXTAG.LegSymbol:
                            NoLegGroupCount++;
                            legInstrumentGrps[NoLegGroupCount] = new LegInstrumentGrp();
                            legInstrumentGrps[NoLegGroupCount].LegSymbol = value;
                            break;

                        case FIXTAG.LegSecurityId:
                            legInstrumentGrps[NoLegGroupCount].LegSecurityId = value;
                            break;

                        case FIXTAG.LegCFICode:
                            legInstrumentGrps[NoLegGroupCount].LegCFICode = value;
                            break;

                        case FIXTAG.LegSecurityType:
                            legInstrumentGrps[NoLegGroupCount].LegSecurityType = value;
                            break;

                        case FIXTAG.LegMaturityDate:
                            legInstrumentGrps[NoLegGroupCount].LegMaturityDate = value;
                            break;

                        case FIXTAG.LegStrikePrice:
                            legInstrumentGrps[NoLegGroupCount].LegStrikePrice = Convert.ToDecimal(value);
                            break;

                        case FIXTAG.LegSecurityExchange:
                            legInstrumentGrps[NoLegGroupCount].LegSecurityExchange = value;
                            break;

                        case FIXTAG.LegSide:
                            legInstrumentGrps[NoLegGroupCount].LegSide = Convert.ToChar(value);
                            break;

                        case FIXTAG.LegPutOrCall:
                            legInstrumentGrps[NoLegGroupCount].LegPutOrCall = Convert.ToInt32(value);
                            break;

                        case FIXTAG.LegQty:
                            legInstrumentGrps[NoLegGroupCount].LegOrderQty = Convert.ToInt32(value);
                            break;

                        case FIXTAG.LegRefID:
                            legInstrumentGrps[NoLegGroupCount].LegRefID = value;
                            break;

                        case FIXTAG.Text:
                            Text = value;
                            break;

                        case FIXTAG.customerinfo1:
                            customerinfo1 = value;
                            break;

                        case FIXTAG.customerinfo2:
                            customerinfo2 = value;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}
