using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SgxICIDropCopy.DropCopyMsg;
using SgxICIDropCopyAdapter.GlobalClass;

namespace SgxICIDropCopyAdapter.DataHandler
{
    public class DBHelper
    {
        string connstring;
        SqlConnection con;
        public DBHelper()
        {
            try
            {
                ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["mySqlConn"];
                connstring = settings.ConnectionString;
                con = new SqlConnection(connstring);
                con.Open();
            }
            catch (Exception ex)
            {
                AppGlobal.logerError.Error("DBHelper : " + ex.ToString());
            }

        }
        object lockInsertOrderdata = new object();
        public void InsertOrderdata(ExecutionReport report)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("InsertOrderTrade", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CFICode", report.CFICode == null ? String.Empty : report.CFICode);
                cmd.Parameters.AddWithValue("@Symbol", report.Symbol == null ? String.Empty : report.Symbol);
                cmd.Parameters.AddWithValue("@SecurityID", report.SecurityID == null ? String.Empty : report.SecurityID);
                cmd.Parameters.AddWithValue("@MaturityDate", report.MaturityDate == null ? String.Empty : report.MaturityDate);
                cmd.Parameters.AddWithValue("@StrikePrice", report.StrikePrice);
                cmd.Parameters.AddWithValue("@PutOrCall", report.PutOrCall);
                cmd.Parameters.AddWithValue("@SecurityType", report.SecurityType == null ? String.Empty : report.SecurityType);
                cmd.Parameters.AddWithValue("@Account", report.Account == null ? String.Empty : report.Account);
                cmd.Parameters.AddWithValue("@Side", report.Side == '\0' ? String.Empty : report.Side.ToString());
                cmd.Parameters.AddWithValue("@OrderID", report.OrderId == null ? String.Empty : report.OrderId);
                cmd.Parameters.AddWithValue("@LeavesQty", report.LeavesQty);
                cmd.Parameters.AddWithValue("@CumQty", report.CumQty);
                cmd.Parameters.AddWithValue("@OrderQty", report.OrderQty);
                cmd.Parameters.AddWithValue("@Price", report.Price);
                cmd.Parameters.AddWithValue("@StopPx", report.StopPx);
                cmd.Parameters.AddWithValue("@LastShares", report.LastQty);
                cmd.Parameters.AddWithValue("@LastPx", report.LastPx);
                cmd.Parameters.AddWithValue("@OrdType", report.OrdType == '\0' ? string.Empty : report.OrdType.ToString());
                cmd.Parameters.AddWithValue("@ExecID", report.ExecID == null ? String.Empty : report.ExecID);
                cmd.Parameters.AddWithValue("@TimeInForce", report.TimeInForce == '\0' ? String.Empty : report.TimeInForce.ToString());
                cmd.Parameters.AddWithValue("@PositionEffect", report.PositionEffect == null ? String.Empty : report.PositionEffect);
                cmd.Parameters.AddWithValue("@HandlInst", report.HandlInst);
                cmd.Parameters.AddWithValue("@IDSource", report.IDSource == null ? String.Empty : report.IDSource);
                cmd.Parameters.AddWithValue("@TransactTime", report.TransactTime == null ? String.Empty : report.TransactTime);
                cmd.Parameters.AddWithValue("@ExecType", report.ExecType == '\0' ? String.Empty : report.ExecType.ToString());

                string orderStatusOut = string.Empty;
                orderStatusOut = Enum.GetName(typeof(SgxICIDropCopy.API.FIXEnum.OrderStatus), report.OrdStatus);
                cmd.Parameters.AddWithValue("@OrdStatus", orderStatusOut);
                cmd.Parameters.AddWithValue("@ExecRestatementReason", report.ExecRestatementReason);
                cmd.Parameters.AddWithValue("@Text", report.Text == null ? String.Empty : report.Text);
                cmd.Parameters.AddWithValue("@SettlType", report.SettlType == null ? String.Empty : report.SettlType);

                cmd.Parameters.AddWithValue("@NoPartyIDs", report.NoPartyIDs);
                if (report.NoPartyIDs > 0)
                {
                    cmd.Parameters.AddWithValue("@PartyID", report.arrPartiesGroup[0].PartyID == null ? String.Empty : report.arrPartiesGroup[0].PartyID);
                    cmd.Parameters.AddWithValue("@PartyIDSource", report.arrPartiesGroup[0].PartyIdSource == '\0' ? string.Empty : report.arrPartiesGroup[0].PartyIdSource.ToString());
                    cmd.Parameters.AddWithValue("@PartyRole", report.arrPartiesGroup[0].PartyRole);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PartyID", string.Empty);
                    cmd.Parameters.AddWithValue("@PartyIDSource", string.Empty);
                    cmd.Parameters.AddWithValue("@PartyRole", string.Empty);
                }


                cmd.Parameters.AddWithValue("@customerinfo1", report.customerinfo1 == null ? String.Empty : report.customerinfo1);
                cmd.Parameters.AddWithValue("@customerinfo2", report.customerinfo2 == null ? String.Empty : report.customerinfo2);
                cmd.Parameters.AddWithValue("@DisplayQty", report.DisplayQty);

                cmd.Parameters.AddWithValue("@SendingTime", report.SendingTime == null ? String.Empty : report.SendingTime);
                cmd.Parameters.AddWithValue("@MsgType", report.MsgType == null ? String.Empty : report.MsgType);
                cmd.Parameters.AddWithValue("@MsgSeqNum", report.MsgSeqNum);

                cmd.Parameters.AddWithValue("@NoLegs", report.NoLegs);

                if (report.NoLegs >= 1)
                {
                    cmd.Parameters.AddWithValue("@LegSymbol1", report.legInstrumentGrps[0].LegSymbol == null ? String.Empty : report.legInstrumentGrps[0].LegSymbol);
                    cmd.Parameters.AddWithValue("@LegSecurityId1", report.legInstrumentGrps[0].LegSecurityId == null ? String.Empty : report.legInstrumentGrps[0].LegSecurityId);
                    cmd.Parameters.AddWithValue("@LegCFICode1", report.legInstrumentGrps[0].LegCFICode == null ? String.Empty : report.legInstrumentGrps[0].LegCFICode);
                    cmd.Parameters.AddWithValue("@LegSecurityType1", report.legInstrumentGrps[0].LegSecurityType == null ? String.Empty : report.legInstrumentGrps[0].LegSecurityType);
                    cmd.Parameters.AddWithValue("@LegMaturityDate1", report.legInstrumentGrps[0].LegMaturityDate == null ? String.Empty : report.legInstrumentGrps[0].LegMaturityDate);
                    cmd.Parameters.AddWithValue("@LegStrikePrice1", report.legInstrumentGrps[0].LegStrikePrice);
                    cmd.Parameters.AddWithValue("@LegSecurityExchange1", report.legInstrumentGrps[0].LegSecurityExchange == null ? String.Empty : report.legInstrumentGrps[0].LegSecurityExchange);
                    cmd.Parameters.AddWithValue("@LegSymbol1", report.legInstrumentGrps[0].LegSide == '\0' ? String.Empty : report.legInstrumentGrps[0].LegSide.ToString());
                    cmd.Parameters.AddWithValue("@LegPutOrCall1", report.legInstrumentGrps[0].LegPutOrCall);
                    cmd.Parameters.AddWithValue("@LegOrderQty1", report.legInstrumentGrps[0].LegOrderQty);
                    cmd.Parameters.AddWithValue("@LegRefID1", report.legInstrumentGrps[0].LegRefID == null ? String.Empty : report.legInstrumentGrps[0].LegRefID);
                }
                if (report.NoLegs >= 2)
                {
                    cmd.Parameters.AddWithValue("@LegSymbol2", report.legInstrumentGrps[1].LegSymbol == null ? String.Empty : report.legInstrumentGrps[1].LegSymbol);
                    cmd.Parameters.AddWithValue("@LegSecurityId2", report.legInstrumentGrps[1].LegSecurityId == null ? String.Empty : report.legInstrumentGrps[1].LegSecurityId);
                    cmd.Parameters.AddWithValue("@LegCFICode2", report.legInstrumentGrps[1].LegCFICode == null ? String.Empty : report.legInstrumentGrps[1].LegCFICode);
                    cmd.Parameters.AddWithValue("@LegSecurityType2", report.legInstrumentGrps[1].LegSecurityType == null ? String.Empty : report.legInstrumentGrps[1].LegSecurityType);
                    cmd.Parameters.AddWithValue("@LegMaturityDate2", report.legInstrumentGrps[1].LegMaturityDate == null ? String.Empty : report.legInstrumentGrps[1].LegMaturityDate);
                    cmd.Parameters.AddWithValue("@LegStrikePrice2", report.legInstrumentGrps[1].LegStrikePrice);
                    cmd.Parameters.AddWithValue("@LegSecurityExchange2", report.legInstrumentGrps[1].LegSecurityExchange == null ? String.Empty : report.legInstrumentGrps[1].LegSecurityExchange);
                    cmd.Parameters.AddWithValue("@LegSymbol2", report.legInstrumentGrps[1].LegSide == '\0' ? String.Empty : report.legInstrumentGrps[1].LegSide.ToString());
                    cmd.Parameters.AddWithValue("@LegPutOrCall2", report.legInstrumentGrps[1].LegPutOrCall);
                    cmd.Parameters.AddWithValue("@LegOrderQty2", report.legInstrumentGrps[1].LegOrderQty);
                    cmd.Parameters.AddWithValue("@LegRefID2", report.legInstrumentGrps[1].LegRefID == null ? String.Empty : report.legInstrumentGrps[1].LegRefID);
                }

                if (report.NoLegs >= 3)
                {
                    cmd.Parameters.AddWithValue("@LegSymbol3", report.legInstrumentGrps[2].LegSymbol == null ? String.Empty : report.legInstrumentGrps[2].LegSymbol);
                    cmd.Parameters.AddWithValue("@LegSecurityId3", report.legInstrumentGrps[2].LegSecurityId == null ? String.Empty : report.legInstrumentGrps[2].LegSecurityId);
                    cmd.Parameters.AddWithValue("@LegCFICode3", report.legInstrumentGrps[2].LegCFICode == null ? String.Empty : report.legInstrumentGrps[2].LegCFICode);
                    cmd.Parameters.AddWithValue("@LegSecurityType3", report.legInstrumentGrps[2].LegSecurityType == null ? String.Empty : report.legInstrumentGrps[2].LegSecurityType);
                    cmd.Parameters.AddWithValue("@LegMaturityDate3", report.legInstrumentGrps[2].LegMaturityDate == null ? String.Empty : report.legInstrumentGrps[2].LegMaturityDate);
                    cmd.Parameters.AddWithValue("@LegStrikePrice3", report.legInstrumentGrps[2].LegStrikePrice);
                    cmd.Parameters.AddWithValue("@LegSecurityExchange3", report.legInstrumentGrps[2].LegSecurityExchange == null ? String.Empty : report.legInstrumentGrps[2].LegSecurityExchange);
                    cmd.Parameters.AddWithValue("@LegSymbol3", report.legInstrumentGrps[2].LegSide == '\0' ? String.Empty : report.legInstrumentGrps[2].LegSide.ToString());
                    cmd.Parameters.AddWithValue("@LegPutOrCall3", report.legInstrumentGrps[2].LegPutOrCall);
                    cmd.Parameters.AddWithValue("@LegOrderQty3", report.legInstrumentGrps[2].LegOrderQty);
                    cmd.Parameters.AddWithValue("@LegRefID3", report.legInstrumentGrps[2].LegRefID == null ? String.Empty : report.legInstrumentGrps[2].LegRefID);
                }


                if (report.NoLegs >= 4)
                {
                    cmd.Parameters.AddWithValue("@LegSymbol4", report.legInstrumentGrps[3].LegSymbol == null ? String.Empty : report.legInstrumentGrps[3].LegSymbol);
                    cmd.Parameters.AddWithValue("@LegSecurityId4", report.legInstrumentGrps[3].LegSecurityId == null ? String.Empty : report.legInstrumentGrps[3].LegSecurityId);
                    cmd.Parameters.AddWithValue("@LegCFICode4", report.legInstrumentGrps[3].LegCFICode == null ? String.Empty : report.legInstrumentGrps[3].LegCFICode);
                    cmd.Parameters.AddWithValue("@LegSecurityType4", report.legInstrumentGrps[3].LegSecurityType == null ? String.Empty : report.legInstrumentGrps[3].LegSecurityType);
                    cmd.Parameters.AddWithValue("@LegMaturityDate4", report.legInstrumentGrps[3].LegMaturityDate == null ? String.Empty : report.legInstrumentGrps[3].LegMaturityDate);
                    cmd.Parameters.AddWithValue("@LegStrikePrice4", report.legInstrumentGrps[3].LegStrikePrice);
                    cmd.Parameters.AddWithValue("@LegSecurityExchange4", report.legInstrumentGrps[3].LegSecurityExchange == null ? String.Empty : report.legInstrumentGrps[3].LegSecurityExchange);
                    cmd.Parameters.AddWithValue("@LegSymbol4", report.legInstrumentGrps[3].LegSide == '\0' ? String.Empty : report.legInstrumentGrps[3].LegSide.ToString());
                    cmd.Parameters.AddWithValue("@LegPutOrCall4", report.legInstrumentGrps[3].LegPutOrCall);
                    cmd.Parameters.AddWithValue("@LegOrderQty4", report.legInstrumentGrps[3].LegOrderQty);
                    cmd.Parameters.AddWithValue("@LegRefID4", report.legInstrumentGrps[3].LegRefID == null ? String.Empty : report.legInstrumentGrps[3].LegRefID);
                }

                if (con.State == ConnectionState.Closed)
                    con.Open();

                int k = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                AppGlobal.logerError.Error("InsertOrderdata : " + ex.ToString());
            }
        }
    }
}
