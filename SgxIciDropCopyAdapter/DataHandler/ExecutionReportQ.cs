using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SgxICIDropCopy.DropCopyMsg;
using SgxICIDropCopyAdapter.GlobalClass;

namespace SgxICIDropCopyAdapter.DataHandler
{
    public class ExecutionReportQ
    {
        EventWaitHandle ExecutionReportEWH;
        Thread ExecutionReportTHR;
        ConcurrentQueue<ExecutionReport> ExecutionReportQueue;

        public ExecutionReportQ()
        {
            ExecutionReportEWH = new AutoResetEvent(false);
            ExecutionReportQueue = new ConcurrentQueue<ExecutionReport>();
            ExecutionReportTHR = new Thread(ProcessWork);
            ExecutionReportTHR.Start();
        }

        public void AddExecutionreport(ExecutionReport executionReport)
        {
            ExecutionReportQueue.Enqueue(executionReport);
            ExecutionReportEWH.Set();
        }

        public void Dispose()
        {
            ExecutionReportTHR.Join();
            ExecutionReportEWH.Close();
        }

        void ProcessWork()
        {
            try
            {
                while (true)
                {
                    if (ExecutionReportQueue.Count > 0)
                    {
                        ExecutionReport executionReport = null;
                        ExecutionReportQueue.TryDequeue(out executionReport);
                        AppGlobal.dBHelper.InsertOrderdata(executionReport);                        
                        //AppGlobal.loger.Info("Queue Counter : " + ExecutionReportQueue.Count + " Exe MsgSeqNo : " + executionReport.MsgSeqNum);

                    }
                    else
                        ExecutionReportEWH.WaitOne(1);
                }
            }
            catch (Exception ex)
            {
                AppGlobal.logerError.Error("ProcessWork : " + ex.ToString());
            }
        }
    }
}
