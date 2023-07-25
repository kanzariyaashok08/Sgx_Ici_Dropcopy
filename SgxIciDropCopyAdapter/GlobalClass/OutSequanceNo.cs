using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgxICIDropCopyAdapter.GlobalClass
{
    public class OutSequanceNo
    {
        string sFileName;
        internal int OutboundMessageSequence = 0;
        internal OutSequanceNo(string filename)
        {
            if (!Directory.Exists(AppGlobal.SequencePath)) Directory.CreateDirectory(AppGlobal.SequencePath);
            this.sFileName = AppGlobal.SequencePath + filename + "_Outbound.txt";
            FileInfo fi = new FileInfo(sFileName);
            if (fi.Exists && fi.LastWriteTime.Date != DateTime.Now.Date)
            {
                DayOfWeek day = DateTime.Now.DayOfWeek;
                int days = day - DayOfWeek.Monday;
                DateTime start = DateTime.Now.AddDays(-days);
                DateTime end = start.AddDays(5);

                if (start.Date == DateTime.Now.Date || (start.Date <= DateTime.Now.Date && end.Date < DateTime.Now.Date) || (DateTime.Now.Date.Day - fi.LastWriteTime.Date.Day) >= 7)
                    fi.Delete();
            }
        }

        /// <summary>
        /// Write Last Sequence No to File
        /// </summary>
        public void Write()
        {
            using (FileStream fs = new FileStream(sFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamWriter bw = new StreamWriter(fs))
                {
                    bw.Write(OutboundMessageSequence);
                }
            }
        }

        /// <summary>
        /// Read Last Sequence No from File
        /// </summary>
        internal void Read()
        {

            using (FileStream fs = new FileStream(sFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (StreamReader br = new StreamReader(fs))
                {
                    try
                    {
                        OutboundMessageSequence = Convert.ToInt32(br.ReadToEnd());
                    }
                    catch (Exception ex)
                    {
                        br.Close();
                        Write();
                    }
                }
            }

        }

    }
}
