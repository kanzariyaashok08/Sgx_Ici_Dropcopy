using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgxICIDropCopyAdapter.GlobalClass
{
    public class InSequanceNo
    {
        string sFileName;

        internal int InboundMessageSequence = 0;

        internal InSequanceNo(string filename)
        {
            if (!Directory.Exists(AppGlobal.SequencePath)) Directory.CreateDirectory(AppGlobal.SequencePath);
            this.sFileName = AppGlobal.SequencePath + filename + "_Inbound.txt";
            FileInfo fi = new FileInfo(sFileName);

            //Due to non persistant mode sequence reseat daily basis
            if (fi.Exists && fi.LastWriteTime.Date != DateTime.Now.Date)
            {
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
                    bw.Write(InboundMessageSequence);
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
                        InboundMessageSequence = Convert.ToInt32(br.ReadToEnd());
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
