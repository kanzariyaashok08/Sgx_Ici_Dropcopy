using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SgxICIDropCopy.API;
using System.Configuration;
using System.Reflection;
using SgxICIDropCopyAdapter.DataHandler;
using System.Timers;
using SgxICIDropCopyAdapter.Dashboard;

namespace SgxICIDropCopyAdapter.GlobalClass
{
    public static class AppGlobal
    {

        public static FIXLayer.DropCopyInterractive dropCopyInterractive;
        public static DashboradCommu DashboardCommunication;
        public static string SequencePath;
        public static string AssemblyProduct;
        public static DBHelper dBHelper;
        public static ExecutionReportQ executionReportQ;
        public static bool isFullDownload;
        public static ILog loger = LogManager.GetLogger("LogFileAppender");
        public static ILog logerError = LogManager.GetLogger("ErrorFileAppender");
        public static ILog logerFix = LogManager.GetLogger("FixFileAppender");
        public static Timer tmrAutologon;
        static AppGlobal()
        {
            var config = ConfigurationManager.ConnectionStrings["DropCopy_IP"];
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            if (attributes.Length == 0)
                AssemblyProduct = "SgxICIDropCopy";
            AssemblyProduct = ((AssemblyProductAttribute)attributes[0]).Product;
            SequencePath = "SgxICIDropCopy" + "\\Sequence\\";

            loger.Info("Application started");
            logerError.Error("Application started");
            logerFix.Debug("Application started");

            tmrAutologon = new Timer();
            tmrAutologon.Interval = 60000 * 5;
            tmrAutologon.Elapsed += TmrAutologon_Elapsed;
            tmrAutologon.Enabled = true;
        }

        private static void TmrAutologon_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Auto reconnect with Dropcopy server.
            if (DateTime.Now.Subtract(AppGlobal.dropCopyInterractive._lastReceivedDataTime).TotalMilliseconds > Convert.ToInt32(ConfigurationManager.AppSettings["HeartbeatInterval"]) * 30000 || !dropCopyInterractive.isLoggedIn)
            {
                dropCopyInterractive.clientSocket.ResetAllSocketFinal();
                dropCopyInterractive = new FIXLayer.DropCopyInterractive();
            }

            //Auto reconnect CRM Dashboard.
            if (DateTime.Now.Subtract(AppGlobal.DashboardCommunication._lastReceivedDataTime).TotalMilliseconds > Convert.ToInt32(ConfigurationManager.AppSettings["HeartbeatInterval"]) * 30000 || !DashboardCommunication.IsConnected)
            {
                DashboardCommunication.Dashboardclientsocket.CloseConnection();
                DashboardCommunication = new DashboradCommu();
            }


        }

        public static void UpdateAppSetting(string Key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;

                if (settings[Key] == null)
                    settings.Add(Key, value);
                else
                    settings[Key].Value = value;

                if (Key == "Password")
                    settings["NewPassword"].Value = string.Empty;

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (Exception ex)
            {
                AppGlobal.logerError.Error("UpdateAppSetting : " + ex.ToString());
            }
        }

    }
}
