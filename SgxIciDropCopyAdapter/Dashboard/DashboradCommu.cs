using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SgxICIDropCopyAdapter.GlobalClass;

namespace SgxICIDropCopyAdapter.Dashboard
{
    public class DashboradCommu
    {
        System.Timers.Timer sTimer;
        StringBuilder _dataBuilder;
        public WebClientSocketLib.ClientSocket Dashboardclientsocket;
        public DateTime _lastReceivedDataTime;
        public bool IsConnected = false;
        public DashboradCommu()
        {

            try
            {
                _dataBuilder = new StringBuilder();
                Dashboardclientsocket = new WebClientSocketLib.ClientSocket();
                Dashboardclientsocket.OnSocketConnect += DashboardClientsocket_OnSocketConnect;
                Dashboardclientsocket.OnSocketDataArrival += DashboardClientsocket_OnSocketDataArrival;
                Dashboardclientsocket.OnSocketDisconnect += DashboardClientsocket_OnSocketDisconnect;
                Dashboardclientsocket.OnSocketError += DashboardClientsocket_OnSocketError;
                Dashboardclientsocket.Connect(ConfigurationManager.AppSettings["DashboardDNS"], Convert.ToInt32(ConfigurationManager.AppSettings["DashboardPort"]));
                sTimer = new System.Timers.Timer();
                sTimer.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["HeartbeatInterval"]) * 1000;
                sTimer.Elapsed += STimer_Elapsed;
                sTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                AppGlobal.logerError.Error("DashboradCommu" + ex.ToString());
            }
        }

        private void DashboardClientsocket_OnSocketError(string error)
        {
            AppGlobal.loger.Info("Dashboard Socket error : " + error);
            IsConnected = false;
        }

        private void DashboardClientsocket_OnSocketDisconnect(WebClientSocketLib.StateObject state)
        {
            AppGlobal.loger.Info("Dashboard Socket is disconnected");
            IsConnected = false;
        }

        private void DashboardClientsocket_OnSocketDataArrival(WebClientSocketLib.StateObject state)
        {

            try
            {
                _lastReceivedDataTime = state.LastPacketTime = System.DateTime.Now;
                if (state.DataBuffer.Count <= 0) return;
                _dataBuilder.Append(Encoding.Default.GetString(state.DataBuffer.ToArray()));
                state.DataBuffer = new List<byte>();

                int total = 0;
                while (-1 != (total = _dataBuilder.ToString().IndexOf("<EOF>", StringComparison.Ordinal)))
                {
                    total = total + 5;
                    string data = _dataBuilder.ToString().Substring(0, total);
                    // 0 - Heartbeat
                    // 1 -Login Request
                    // 2 -Login Status
                    string[] arrdata = data.Split('|');

                    if (arrdata != null && arrdata.Length > 0)
                    {
                        if (arrdata[1] == "1")
                        {
                            SessionLogonRequest();
                        }
                    }

                    _dataBuilder.Remove(0, total);                    
                }
            }
            catch (Exception ex)
            {
                state.DataBuffer = new List<byte>();
                _dataBuilder.Clear();
            }
        }

        private void DashboardClientsocket_OnSocketConnect(WebClientSocketLib.StateObject state)
        {
            AppGlobal.loger.Info("Dashboard Socket is connected");
            IsConnected = true;
            if (AppGlobal.dropCopyInterractive != null)
                Dashboardclientsocket.Send(GetConnectionStatus());
        }

        private void STimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (DateTime.Now.Subtract(Dashboardclientsocket.clientsocketclass.LastPacketTime).Milliseconds >= Convert.ToInt32(ConfigurationManager.AppSettings["HeartbeatInterval"]) * 1000)
            {
                Dashboardclientsocket.CloseConnection();
                AppGlobal.loger.Info("Dashboard Socket connection close using Timer event");
            }
            else
                Dashboardclientsocket.Send(GetConnectionStatus());
        }

        public void SendConnectionStatustoDashboard()
        {
            Dashboardclientsocket.Send(GetConnectionStatus());
        }
        private string GetConnectionStatus()
        {
             return System.DateTime.Now + "|2|"+ ConfigurationManager.AppSettings["DashboardUserID"] + "|" + (AppGlobal.dropCopyInterractive.isLoggedIn == true ? "Session LogIn" : "Session LogOut") + "<EOF>";
           // return System.DateTime.Now + "|2|SGXICI|" + (AppGlobal.dropCopyInterractive.isLoggedIn == true ? "Session LogIn" : "Session LogOut") + "<EOF>";
        }

        private void SessionLogonRequest()
        {
            if (!AppGlobal.dropCopyInterractive.isLoggedIn)
            {
                AppGlobal.dropCopyInterractive = new FIXLayer.DropCopyInterractive();
                System.Threading.Thread.Sleep(1000);
            }
            else
            {
                SendConnectionStatustoDashboard();
            }
        }

    }
}
