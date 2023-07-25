using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SgxICIDropCopy.API
{
    public class DataProcess
    {
        public MessageHeader Header;
        public string Data = string.Empty;
        public string Trailer;
        public bool IsChecksumValid = false;
        private string Checksum;

        public DataProcess(string message)
        {
            string[] temp = message.Split(new[] { FixConst.Delimeter + "52=", FixConst.Delimeter + "10=" }, 3, StringSplitOptions.None);
            string[] mixdata = temp[1].Split(FixConst.DelimeterS, 2, StringSplitOptions.None);

            string head = string.Concat(new[] { temp[0], FixConst.Delimeter + "52=", mixdata[0] });
            Header = new MessageHeader(head);

        }

        /// <summary>
        /// generate Checksum
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public string GenerateCheckSum(char[] buffer)
        {
            uint checksum = 0;
            for (long index = 0L; index < buffer.Length; checksum += (uint)buffer[index++])
            { }

            return string.Format("{0:000}", checksum % 256);
        }

        /// <summary>
        /// Check for valid checksum
        /// </summary>
        /// <param name="message"></param>
        /// <returns>retun true is success</returns>
        public bool CalculateChecksum(string message)
        {
            try
            {
                //string messageString = message.Remove( message.IndexOf( FixCommon.FixConst.FixEndString, StringComparison.Ordinal ), 7 );
                string tempcheckSum = GenerateCheckSum(message.ToCharArray());
                //string tmpmsg = message.Trim().Substring( message.LastIndexOf( FixCommon.FixConst.FixEndString, StringComparison.Ordinal ), 7 );
                if (Convert.ToInt32(tempcheckSum) == Convert.ToInt32(Checksum.Trim()))
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }


        }
    }
}
