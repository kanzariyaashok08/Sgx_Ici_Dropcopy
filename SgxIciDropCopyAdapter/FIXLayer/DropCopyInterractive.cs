using AarnaNetworkSocket;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using SgxICIDropCopy.API;
using SgxICIDropCopy.DropCopyMsg;
using SgxICIDropCopy.SessionMsg;
using SgxICIDropCopyAdapter.DataHandler;
using SgxICIDropCopyAdapter.GlobalClass;

namespace SgxICIDropCopyAdapter.FIXLayer
{
    public class DropCopyInterractive
    {
        #region Variables
        public ClientSocket clientSocket;
        public DateTime _lastReceivedDataTime;
        StringBuilder _dataBuilder;
        Timer tmrHeartbeat;
        MessageHeader messageHeader;
        OutSequanceNo outSequanceNo;
        InSequanceNo inSequanceNo;
        bool isResendRequest;
        bool isHBTTimerStart;
        bool isTestRequest;
        public bool isLoggedIn;
        int hbtInterval = 0;
        string testrequestid = string.Empty;
        Heartbeat heartbeat;

        #endregion

        #region Constructor
        public DropCopyInterractive()
        {
            hbtInterval = Convert.ToInt32(ConfigurationManager.AppSettings["HeartbeatInterval"]);
            tmrHeartbeat = new Timer();
            tmrHeartbeat.Interval = hbtInterval * 1000 - 20;
            tmrHeartbeat.Elapsed += tmrHeartbeat_Elapsed;
            tmrHeartbeat.Enabled = true;

            GenerateMessageHeader();
            heartbeat = new Heartbeat(messageHeader);
            _dataBuilder = new StringBuilder();
            clientSocket = new ClientSocket();
            clientSocket.OnSocketConnect += ClientSocket_OnSocketConnect;
            clientSocket.OnSocketDisconnect += ClientSocket_OnSocketDisconnect;
            clientSocket.OnSocketError += ClientSocket_OnSocketError;
            clientSocket.OnSocketDataArrival += ClientSocket_OnSocketDataArrival;

            string ip = ConfigurationManager.AppSettings["DropCopy_IP"];
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["DropCopy_Port"]);
            clientSocket.Connect(ip, port);
            AppGlobal.dBHelper = new DBHelper();
            AppGlobal.executionReportQ = new ExecutionReportQ();
        }


        private void tmrHeartbeat_Elapsed(object sender, ElapsedEventArgs e)
        {
            tmrHeartbeat.Interval = hbtInterval * 1000 - 20;
            HeartBeatTimerAction();
        }
        #endregion

        #region Socket Events

        private void ClientSocket_OnSocketDataArrival(SocketState state)
        {

            try
            {
                _lastReceivedDataTime = DateTime.Now;
                if (state.DataBuffer.Count <= 0) return;
                _dataBuilder.Append(Encoding.Default.GetString(state.DataBuffer.ToArray()));
                state.DataBuffer = new List<byte>();

                int total = 0;
                while (-1 != (total = _dataBuilder.ToString().IndexOf(FixConst.FixEndString, StringComparison.Ordinal))
                    && _dataBuilder.Length >= (total + 8))
                {
                    total = total + 8;
                    string data = _dataBuilder.ToString().Substring(0, total);
                    DataProcess(data);
                    AppGlobal.logerFix.Debug(data);
                    _dataBuilder.Remove(0, total);
                }
            }
            catch (Exception ex)
            {
                state.DataBuffer = new List<byte>();
                _dataBuilder.Clear();
            }
        }

        public void DataProcess(string data)
        {
            try
            {

                DataProcess Processed = new DataProcess(data);

                switch (Processed.Header.MsgType)
                {
                    case FIXMessageType.Logon:
                        Logon logOn = new Logon(data);
                        tmrHeartbeat.Enabled = true;
                        isResendRequest = false;
                        isHBTTimerStart = true;
                        isLoggedIn = true;
                        CheckInMessageSequenceNo(logOn.MsgSeqNum, logOn.PossDupFlag);
                        Console.WriteLine("Logon successful.!");


                        //Password update in AppSetting after successful changed.
                        if (logOn.SessionStatus == 1)
                        {
                            Console.WriteLine("Password updated successful");

                            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["NewPassword"]))
                            {
                                AppGlobal.UpdateAppSetting("Password", ConfigurationManager.AppSettings["NewPassword"]);
                            }
                        }
                        Console.WriteLine($"Password expiry date : {logOn.PwdExpireDate}");
                        if (AppGlobal.DashboardCommunication != null)
                            AppGlobal.DashboardCommunication.SendConnectionStatustoDashboard();
                        break;

                    case FIXMessageType.Logout:
                        Logout logOut = new Logout(data);

                        //todo
                        if (logOut.Text != "Session is already connected ")
                            CheckInMessageSequenceNo(logOut.MsgSeqNum, logOut.PossDupFlag);

                        if (!string.IsNullOrEmpty(logOut.Text) && logOut.Text.Contains("MsgSeqNum too low"))
                        {
                            string[] sequence = logOut.Text.Split(' ');
                            if (sequence.Length >= 7)
                            {
                                var seq = sequence[7].Substring(0, sequence[7].Length - 1);
                                outSequanceNo.OutboundMessageSequence = Convert.ToInt32(seq) - 1;
                                outSequanceNo.Write();
                            }
                        }

                        string logoutmsg = $"Session logout : {logOut.Text} ";
                        Console.WriteLine(logoutmsg);
                        AppGlobal.loger.Info(logoutmsg);
                        CloseConnection();
                        break;

                    case FIXMessageType.Heartbeat:
                        Heartbeat heartbeat = new Heartbeat(data);
                        CheckInMessageSequenceNo(heartbeat.MsgSeqNum, heartbeat.PossDupFlag);
                        if (!string.IsNullOrEmpty(heartbeat.TestReqID))
                        {
                            if (isTestRequest && heartbeat.TestReqID == testrequestid)
                                isTestRequest = false;
                        }
                        break;

                    case FIXMessageType.TestRequest:
                        TestRequest testRequest = new TestRequest(data);
                        CheckInMessageSequenceNo(testRequest.MsgSeqNum, testRequest.PossDupFlag);
                        if (isHBTTimerStart)
                        {
                            SendHeartbeat(testRequest.TestReqID);
                        }
                        AppGlobal.loger.Info("TestRequest received from SGX");
                        break;

                    case FIXMessageType.ResendRequest:
                        ResendRequest resendRequest = new ResendRequest(data);
                        CheckInMessageSequenceNo(resendRequest.MsgSeqNum, resendRequest.PossDupFlag);
                        if (!resendRequest.PossDupFlag)
                            SendSequenceReset(resendRequest.BeginSeqNo, outSequanceNo.OutboundMessageSequence);
                        AppGlobal.loger.Info("ResendRequest received from SGX");
                        break;

                    case FIXMessageType.SequenceReset:
                        SequenceReset sequenceReset = new SequenceReset(data);

                        if (sequenceReset.GapFillFlag)
                        {
                            InSequenceReset(sequenceReset.NewSeqNo);
                        }
                        else
                        {
                            CheckInMessageSequenceNo(sequenceReset.MsgSeqNum, sequenceReset.PossDupFlag);
                        }
                        AppGlobal.loger.Info($"SequenceReset with NewSeqNo : {sequenceReset.NewSeqNo} received from SGX");
                        break;

                    case FIXMessageType.Reject:
                        SessionLevelReject sessionLevelReject = new SessionLevelReject(data);
                        CheckInMessageSequenceNo(sessionLevelReject.MsgSeqNum, sessionLevelReject.PossDupFlag);

                        string error = $"Session Rejected! Type: {FIXMethods.GetRejectionReason(sessionLevelReject.SessionRejectReason)} Reason: {sessionLevelReject.Text.Trim()}";
                        AppGlobal.loger.Info(error);
                        Console.WriteLine(error);
                        break;

                    case FIXMessageType.ExecutionReport:
                        ExecutionReport executionReport = new ExecutionReport(data);
                        CheckInMessageSequenceNo(executionReport.MsgSeqNum, executionReport.PossDupFlag);

                        AppGlobal.executionReportQ.AddExecutionreport(executionReport);
                        break;

                    case FIXMessageType.TradeCaptureReport:
                        TradeCaptureReport tradeCaptureReport = new TradeCaptureReport(data);
                        CheckInMessageSequenceNo(tradeCaptureReport.MsgSeqNum, tradeCaptureReport.PossDupFlag);
                        break;

                    case FIXMessageType.OrderCancelReject:
                        OrderCancelReject orderCancelReject = new OrderCancelReject(data);
                        CheckInMessageSequenceNo(orderCancelReject.MsgSeqNum, orderCancelReject.PossDupFlag);

                        if (!string.IsNullOrEmpty(orderCancelReject.Text))
                            Console.WriteLine($"Order rejection reson {orderCancelReject.Text}");
                        break;

                    default:
                        CheckInMessageSequenceNo(Processed.Header.MsgSeqNum, Processed.Header.PossDupFlag);
                        break;
                }

                Console.WriteLine($"{DateTime.Now.ToString("dd-MMM-yyyy HH mm ss ttt")}  << {SgxICIDropCopy.API.FIXMessageType.GetMsgType(Processed.Header.MsgType)} from Exchange.");
            }
            catch (Exception ex)
            {
                AppGlobal.logerError.Error("DataProcess : " + ex.ToString());
            }
        }

        private void ClientSocket_OnSocketError(string error)
        {
            AppGlobal.logerError.Error("ClientSocket_OnSocketError : " + error);
            CloseConnection();
        }

        private void ClientSocket_OnSocketDisconnect(SocketState state)
        {
            AppGlobal.logerError.Error("ClientSocket_OnSocketDisconnect : " + state.UniqueName);
            CloseConnection();
        }

        private void ClientSocket_OnSocketConnect(SocketState state)
        {
            AppGlobal.logerError.Error("Exchange DropCopy Socket connected successful.");
            SendLogonRequest();


        }
        #endregion

        private void GenerateMessageHeader()
        {

            try
            {
                messageHeader = new MessageHeader();
                messageHeader.BeginString = ConfigurationManager.AppSettings["BeginString"];
                messageHeader.SenderCompID = ConfigurationManager.AppSettings["SenderCompId"];
                messageHeader.TargetCompID = ConfigurationManager.AppSettings["TargetCompId"];
            }
            catch (Exception ex)
            {
                AppGlobal.logerError.Error("GenerateMessageHeader : " + ex.ToString());
            }
        }

        private void SendLogonRequest()
        {
            try
            {
                inSequanceNo = new InSequanceNo(ConfigurationManager.AppSettings["SenderCompId"]);
                inSequanceNo.Read();

                outSequanceNo = new OutSequanceNo(ConfigurationManager.AppSettings["SenderCompId"]);
                outSequanceNo.Read();

                if (AppGlobal.isFullDownload)
                {
                    inSequanceNo.InboundMessageSequence = 1;
                    inSequanceNo.Write();
                    AppGlobal.isFullDownload = false;
                }

                Logon logon = new Logon(messageHeader);
                logon.EncryptMethod = 0;
                logon.HeartBtInt = Convert.ToInt32(ConfigurationManager.AppSettings["HeartbeatInterval"]);
                logon.UserName = ConfigurationManager.AppSettings["UserName"];
                logon.Password = ConfigurationManager.AppSettings["Password"];
                logon.NewPassword = ConfigurationManager.AppSettings["NewPassword"];
                logon.DefaultAppVerID = ConfigurationManager.AppSettings["DefaultApplVerID"];

                if (ConfigurationManager.AppSettings["IsPersistant"].ToLower() == "false")
                {
                    inSequanceNo.InboundMessageSequence = 0;
                    inSequanceNo.Write();
                    outSequanceNo.OutboundMessageSequence = 0;//
                    outSequanceNo.Write();
                }

                

                SendData(logon);
            }
            catch (Exception ex)
            {
                AppGlobal.logerError.Error("SendLogonRequest : " + ex.ToString());
            }
        }

        public void SendLogout()
        {
            if (isLoggedIn)
            {
                Logout logout = new Logout(messageHeader);
                logout.Text = "Logout by User";
                SendData(logout);
            }
            else
            {
                AppGlobal.logerError.Error("Session is allready logged out");
                Console.WriteLine("Session is allready logged out");
            }
        }
        private void SendHeartbeat()
        {
            try
            {
                heartbeat.TestReqID = string.Empty;
                SendData(heartbeat);
            }
            catch (Exception ex)
            {
                AppGlobal.logerError.Error("SendHeartbeat : " + ex.ToString());
            }
        }

        private void SendHeartbeat(string TestRequestID)
        {
            try
            {
                heartbeat.TestReqID = TestRequestID;
                SendData(heartbeat);
            }
            catch (Exception ex)
            {
                AppGlobal.logerError.Error("SendHeartbeat : " + ex.ToString());
            }
        }

        public void SendResendRequest(int BeginSeqNo, int EndSeqNo)
        {
            try
            {
                isResendRequest = true;
                ResendRequest resendrequest = new ResendRequest(messageHeader);
                resendrequest.BeginSeqNo = BeginSeqNo;
                resendrequest.EndSeqNo = EndSeqNo;
                SendData(resendrequest);
                AppGlobal.loger.Info($"Send ResendRequest to SGX with BeginSeqNo : {BeginSeqNo} EndSeqNo {EndSeqNo}");
            }
            catch (Exception ex)
            {
                AppGlobal.logerError.Error("SendResendRequest : " + ex.ToString());
            }
        }

        private void SendTestRequest()
        {
            try
            {
                TestRequest testRequest = new TestRequest(messageHeader);
                testrequestid = testRequest.TestReqID = new Random().Next(1, 9999).ToString();
                SendData(testRequest);
                AppGlobal.loger.Info("Send TestRequest to SGX");
            }
            catch (Exception ex)
            {
                AppGlobal.logerError.Error("SendTestRequest : " + ex.ToString());
            }
        }

        private void SendSequenceReset(int startMsgSeqNo, int nextmsgSeqNo, bool GapFillFlag = true)
        {
            SequenceReset sequenceResetRequest = new SequenceReset(messageHeader);
            sequenceResetRequest.GapFillFlag = GapFillFlag;
            sequenceResetRequest.PossDupFlag = true;
            sequenceResetRequest.MsgSeqNum = startMsgSeqNo;
            sequenceResetRequest.NewSeqNo = ++nextmsgSeqNo;
            SendData(sequenceResetRequest);
            AppGlobal.loger.Info("Send SequenceReset to SGX");
        }

        private object locksend = new object();
        private void SendData(iDataInterface data)
        {
            try
            {
                lock (locksend)
                {
                    if (!data.PossDupFlag)
                    {
                        data.MsgSeqNum = ++outSequanceNo.OutboundMessageSequence;
                        outSequanceNo.Write();
                    }
                    clientSocket.Send(Encoding.ASCII.GetBytes(data.Data.ToCharArray()));
                    AppGlobal.logerFix.Debug(data.Data);
                    Console.WriteLine($"{DateTime.Now.ToString("dd-MMM-yyyy HH mm ss ttt")} >> {SgxICIDropCopy.API.FIXMessageType.GetMsgType(data.MsgType)} to Exchange.");
                }
            }
            catch (Exception ex)
            {
                AppGlobal.logerError.Error("SendData : " + ex.ToString());
            }
        }

        private void CheckInMessageSequenceNo(int MessageSequenceNo, bool PossDupFlag)
        {
            try
            {
                if (++inSequanceNo.InboundMessageSequence < MessageSequenceNo && !PossDupFlag && !isResendRequest)
                {
                    SendResendRequest(inSequanceNo.InboundMessageSequence - 1, 0);
                }
                else if (inSequanceNo.InboundMessageSequence > MessageSequenceNo && !PossDupFlag)
                {
                    InSequenceReset(MessageSequenceNo + 1);
                }
                else
                {
                    inSequanceNo.Write();
                }
            }
            catch (Exception ex)
            {
                AppGlobal.logerError.Error("CheckInMessageSequenceNo : " + ex.ToString());
            }

        }

        private void InSequenceReset(int NewSeqNo)
        {
            try
            {
                inSequanceNo.InboundMessageSequence = NewSeqNo;
                inSequanceNo.Write();
            }
            catch (Exception ex)
            {
                AppGlobal.logerError.Error("InSequenceReset : " + ex.ToString());
            }
        }

        private void HeartBeatTimerAction()
        {
            try
            {
                if (!isHBTTimerStart)
                {
                    clientSocket.ResetSocket();
                    return;
                }

                if (DateTime.Now.Subtract(_lastReceivedDataTime).TotalMilliseconds <= hbtInterval * 1000)
                {
                    if (!isTestRequest)
                    {
                        SendHeartbeat();
                    }
                }
                else
                {
                    if (!isTestRequest)
                    {
                        isTestRequest = true;
                        SendTestRequest();
                    }
                    else
                    {
                        SendHeartbeat();
                        isHBTTimerStart = false;
                    }
                }
            }
            catch (Exception ex)
            {
                AppGlobal.logerError.Error("HeartBeatTimerAction : " + ex.ToString());
            }
        }

        public void CloseConnection()
        {
            try
            {
                //AppGlobal.tmrAutologon.Enabled = true;
                tmrHeartbeat.Enabled = false;
                isResendRequest = false;
                isHBTTimerStart = false;
                isLoggedIn = false;
                clientSocket.ResetSocket();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine();
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("LOGON DETAILS");
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("Date : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff"));
                Console.WriteLine("Status : logged out");
                Console.WriteLine("------------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
                AppGlobal.loger.Info(" Market connection successfully closed!");

                if (AppGlobal.DashboardCommunication != null)
                    AppGlobal.DashboardCommunication.SendConnectionStatustoDashboard();
            }
            catch (Exception ex)
            {
                AppGlobal.logerError.Error("CloseConnection : " + ex.ToString());
            }
        }

    }
}
