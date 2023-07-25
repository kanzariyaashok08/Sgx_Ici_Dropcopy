using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SgxICIDropCopyAdapter.GlobalClass;
using SgxICIDropCopyAdapter.FIXLayer;

namespace SgxICIDropCopyAdapter
{
    class Program
    {
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        public static void Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;

            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);

            XmlConfigurator.Configure();
            Thread.Sleep(1000);
            AppGlobal.dropCopyInterractive = new DropCopyInterractive();
            AppGlobal.DashboardCommunication = new Dashboard.DashboradCommu();
            string choice = string.Empty;
            do
            {

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("******************************************************");

                Console.WriteLine("*    1 : Logon session");
                Console.WriteLine("*    2 : Logon with full downloading");
                Console.WriteLine("*    3 : Logout session");
                //Console.WriteLine("*    4 : OrderStatusRequest(Check Current Working Order)");
                Console.WriteLine("*    9 : CRM Dashboard Session logon)");
                Console.WriteLine("*    0 : Exit");

                Console.WriteLine("******************************************************");
                Console.Write("Enter Choice : ");
                Console.ForegroundColor = ConsoleColor.White;
                choice = Console.ReadLine();
                switch (choice)
                {

                    case "1": // Logon
                        AppGlobal.dropCopyInterractive = new DropCopyInterractive();
                        Thread.Sleep(1000);
                        break;

                    case "2": // Logon with full downloading
                        if (!AppGlobal.dropCopyInterractive.isLoggedIn)
                        {
                            AppGlobal.isFullDownload = true;
                            AppGlobal.dropCopyInterractive = new DropCopyInterractive();
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            Console.Write("Session already logon! ");
                        }
                        break;

                    case "3": // Logout
                        AppGlobal.dropCopyInterractive.SendLogout();
                        AppGlobal.loger.Info("Logout request send by Manual");
                        Thread.Sleep(1000);
                        break;

                    case "9": // CRM Dashboard Login
                        if (!AppGlobal.DashboardCommunication.IsConnected)
                        {
                            AppGlobal.DashboardCommunication = new Dashboard.DashboradCommu();
                        }
                        else
                            Console.Write("CRM Dashboard Session already logon! ");
                        break;



                    //case "4":
                    //    if (AppGlobal.dropCopyInterractive.isLoggedIn)
                    //    {
                    //        string Account, ClOrderID, OrderID = string.Empty;
                    //        Console.Write("Order Status Request : Ender Account or ClOrderID and OrderID");
                    //        Console.WriteLine();
                    //        Console.Write("Please enter Account  : ");
                    //        Account = Console.ReadLine();
                    //        Console.Write("Please enter ClOrderID  : ");
                    //        ClOrderID = Console.ReadLine();
                    //        Console.Write("Please enter OrderID  : ");
                    //        OrderID = Console.ReadLine();

                    //        AppGlobal.dropCopyInterractive.SendOrderStatusRequest(Account, ClOrderID, OrderID);
                    //    }
                    //    else
                    //    {
                    //        Console.WriteLine("Please logon session before OrderStatusRequest");
                    //    }
                    //    break;


                    case "0": // Logout and Exit
                        AppGlobal.dropCopyInterractive.SendLogout();
                        AppGlobal.loger.Info("Logout & Exit request send by Manual");
                        Thread.Sleep(1000);
                        Application.Exit();
                        Environment.Exit(1);
                        break;                   

                    //case "9": // Logout and Exit
                    //    int BSequanceNo, ESequanceNo = 0;
                    //    Console.Write("Send Resend Request ");
                    //    Console.WriteLine();
                    //    Console.Write("Begining Sequance number  : ");
                    //    int.TryParse(Console.ReadLine(), out BSequanceNo);
                    //    Console.Write("End Sequance Number  : ");
                    //    int.TryParse(Console.ReadLine(), out ESequanceNo);
                    //    if (BSequanceNo != ESequanceNo && BSequanceNo <= ESequanceNo)
                    //        AppGlobal.dropCopyInterractive.SendResendRequest(BSequanceNo, ESequanceNo);
                    //    break;

                    //case "7":
                    //    string line;
                    //    System.IO.StreamReader file = new System.IO.StreamReader(@"D:\Other\31-05-2021\tt\FixData_31.05.2021.log");
                    //    while ((line = file.ReadLine()) != null)
                    //    {
                    //        string[] ddata = line.Split('�');

                    //        if (ddata.Length >= 2)
                    //            AppGlobal.dropCopyInterractive.DataProcess(ddata[1]);
                    //        Thread.Sleep(20);
                    //    }
                    //    ////string ss = "8=FIX.4.29=0090735=849=TT_Order56=AA_DC234=15276142=IN52=20200429-04:53:00.926129=AAGARWAL37=eb97c6ec-b9d3-40e5-834a-f44db8cf1496198=64F82482:00460BAA526=1588113954125527=eb97c6ec-b9d3-40e5-834a-f44db8cf1496:610011=11453=1448=CHE-102F452=83447=D17=2a381047-1625-4a32-bfa3-5f5f85b748c620=219=100090976150=G18=239=11=TG01SG55=IU107=INR/USD460=1248=14256554093219186322167=FUT200=202006541=20200630205=30207=SGX100=XSIM461=F15=USD18211=M54=138=1040=244=130.6359=032=131=130.63151=614=46=130.6375=2020042960=20200429-04:52:48.51424777=O58=Text_TT field modified442=11028=Y18216=TCT2001KJ00110553=info@tgtfz.com880=72756032170680879518220=KGI18221=AARC18223=202006454=5455=IUM20456=98455=IU Jun20456=97455=SGXDB0107936456=4455=SDIUM0456=5455=IUM20456=816999=CHE-102F16561=20200429-04:52:48.51443016117=2416612=9D1XyZ8t2IbvK12lXnb5q18218=<DEFAULT>18226=010=123";
                    //    ////AppGlobal.dropCopyInterractive.DataProcess(ss);
                    //    break;
                    default:
                        break;
                }
            } while (true);

        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
